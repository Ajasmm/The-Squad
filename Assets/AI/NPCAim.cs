using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCAim : MonoBehaviour, Photon.Pun.IPunObservable
{
    [Header("Animation and Rigging")]
    [SerializeField] Animator animator;
    [SerializeField] string Firing_ParameterName = "Firing";
    [SerializeField] string Reload_ParameterName = "Reload";
    [SerializeField] Rig AimRig;
    [SerializeField] Rig HoldRig;
    [SerializeField] Transform aimTarget;

    [Space(10), Header("Shooting")]
    [SerializeField] Gun gun;
    [SerializeField] Transform GunTip;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Vector3 NotHitPos;

    [Space(10), Header("Other")]
    [SerializeField] PhotonView photonView;


    RaycastHit hit = new RaycastHit();
    public Vector3 TargetPos;
    public float TargetAimRigWeight;
    public float TargetHoldRigWeight;
    public bool Firing = false;

    private bool preFireState = false;

    private int FiringHash;
    private int ReloadHash;

    private void Start()
    {
        FiringHash = Animator.StringToHash(Firing_ParameterName);
        ReloadHash = Animator.StringToHash(Reload_ParameterName);
        animator.SetBool(FiringHash, false);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        aimTarget.position = TargetPos;

        AimRig.weight = Mathf.MoveTowards(AimRig.weight, TargetAimRigWeight, deltaTime * 4);
        HoldRig.weight = Mathf.MoveTowards(HoldRig.weight, TargetHoldRigWeight, deltaTime * 4);

        if (preFireState != Firing)
        {
            animator.SetBool(FiringHash, Firing);
            preFireState = Firing;
        }
    }

    private bool Fire(out RaycastHit hit)
    {
        return Physics.Raycast(GunTip.position, (TargetPos - GunTip.position).normalized, out hit, Mathf.Infinity, layerMask.value);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Firing);
            stream.SendNext(TargetPos);
            stream.SendNext(TargetAimRigWeight);
            stream.SendNext(TargetHoldRigWeight);
        }
        else
        {
            Firing = (bool)stream.ReceiveNext();
            TargetPos = (Vector3)stream.ReceiveNext();
            TargetAimRigWeight = (float)stream.ReceiveNext();
            TargetHoldRigWeight = (float)stream.ReceiveNext();
        }
    }
    public void Reload()
    {
        animator.SetTrigger(ReloadHash);
    }
    public void Fire_Animation(AnimationEvent eventData)
    {
        if (!photonView.IsMine)
            return;

        if (eventData.animatorClipInfo.weight < 1)
            return;

        Debug.Log("In animation call back fire");

        if (gun.PeekShoot())
        {
            RaycastHit hit;
            if (Fire(out hit))
            {
                // get the health component from the hitted object and do call the PUNRPC function
                CharacterHealth characterHealth;
                if (hit.collider.gameObject.TryGetComponent(out characterHealth))
                {
                    characterHealth.AddDamage(75, photonView.Owner);
                }
                Debug.Log(hit.collider.name);
                gun.Shoot();
            }
        }
    }
    public void ReloadFinished()
    {
        gun.Reload();
    }

}
