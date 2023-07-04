using Photon.Pun;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;

public class CharacterAim : MonoBehaviour,IPunObservable
{
    [Header("Animation and Rigging")]
    [SerializeField] Animator animator;
    [SerializeField] string Firing_ParameterName = "Firing";
    [SerializeField] string Reload_ParameterName = "Reload";
    [SerializeField] Rig AimRig;
    [SerializeField] Rig HoldRig;
    [SerializeField] Transform aimTarger;

    [Space(10), Header("Shooting")]
    [SerializeField] Gun gun;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Vector3 NotHitPos;

    [Space(10), Header("Other")]
    [SerializeField] PhotonView photonView;

    Transform mainCamera_T;

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
        mainCamera_T = Camera.main.transform;
        FiringHash = Animator.StringToHash(Firing_ParameterName);
        ReloadHash = Animator.StringToHash(Reload_ParameterName);
        animator.SetBool(FiringHash, false);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        aimTarger.position = TargetPos;
        AimRig.weight = Mathf.MoveTowards(AimRig.weight, TargetAimRigWeight, deltaTime * 4);
        HoldRig.weight = Mathf.MoveTowards(HoldRig.weight, TargetHoldRigWeight, deltaTime * 4);

        if (preFireState != Firing)
        {
            animator.SetBool(FiringHash, Firing);
            preFireState = Firing;
        }

        if (mainCamera_T == null)
            mainCamera_T = Camera.main.transform;
    }

    // Only to find the target Pos;
    public Vector3 FindTargetPos()
    {
        if (mainCamera_T == null)
            return NotHitPos;

        return Fire(out hit) ? hit.point : NotHitPos;
    }

    private bool Fire(out RaycastHit hit)
    {
        return Physics.Raycast(mainCamera_T.position, mainCamera_T.forward, out hit, Mathf.Infinity, layerMask.value);
    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Firing);
            stream.SendNext(TargetPos);
            stream.SendNext(TargetAimRigWeight);
        }
        else
        {
            Firing = (bool) stream.ReceiveNext();
            TargetPos = (Vector3) stream.ReceiveNext();
            TargetAimRigWeight = (float) stream.ReceiveNext();
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

        if (gun.PeekShoot())
        {
            RaycastHit hit;
            if (Fire(out hit))
            {
                // get the health component from the hitted object and do call the PUNRPC function
                CharacterHealth characterHealth;
                if(hit.collider.gameObject.TryGetComponent(out characterHealth))
                {
                    characterHealth.AddDamage(100, photonView.Owner);
                }

                gun.Shoot();
            }
        }
    }
    public void ReloadFinished()
    {
        gun.Reload();
    }
    
}
