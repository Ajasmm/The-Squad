using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;

public class CharacterAim : MonoBehaviour,IPunObservable
{
    [Header("Animation and Rigging")]
    [SerializeField] Animator animator;
    [SerializeField] string Firing_ParameterName = "Firing";
    [SerializeField] Rig AimRig;
    [SerializeField] Transform aimTarger;

    [Space(10), Header("Shooting")]
    [SerializeField] Gun gun;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Vector3 NotHitPos;

    [Space(10), Header("Other")]
    [SerializeField] PhotonView photonView;

    Transform mainCamera_T;

    public Vector3 TargetPos;
    public float TargetAimRigWeight;
    public bool Firing = false;

    private bool preFireState = false;

    private int FiringHash;

    private void Start()
    {
        mainCamera_T = Camera.main.transform;
        FiringHash = Animator.StringToHash(Firing_ParameterName);
        animator.SetBool(FiringHash, false);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        aimTarger.position = TargetPos;
        AimRig.weight = Mathf.MoveTowards(AimRig.weight, TargetAimRigWeight, deltaTime * 4);

        if(preFireState != Firing)
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

        RaycastHit hitInfo;
        if (Physics.Raycast(mainCamera_T.position, mainCamera_T.forward, out hitInfo, Mathf.Infinity, layerMask.value))
                return hitInfo.point;
        else
            return NotHitPos;
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
}

public class Gun : MonoBehaviour
{
    [SerializeField] GunInfo gunInfo;
    [SerializeField] ParticleSystem muzleFlash;
    [SerializeField] AudioSource shootSound;

    #region BulletInventory
    private int bulletInMag = 30;
    private int bulletInHand = 250;
    #endregion

    private void Start()
    {
        UpdateBulletInfo();
    }

    public bool PeekShoot() => bulletInMag > 0;
    public bool PeekReload()
    {
        if (bulletInHand == 0)
            return false;

        return true;
    }

    public bool Shoot()
    {
        if(bulletInMag == 0)
            return false;

        bulletInMag--;

        if(muzleFlash) 
            muzleFlash.Play();

        if(shootSound) 
            shootSound.Play();
        
        return true;
    }
    public void Reload()
    {
        if(bulletInHand == 0)
            return;

        int requiredBullets = 30 - bulletInMag;
        int GottenBullets = (bulletInHand >= requiredBullets) ? requiredBullets : bulletInHand;
        bulletInMag += GottenBullets;
        bulletInHand -= GottenBullets;
    }

    private void UpdateBulletInfo()
    {
        gunInfo.BulletInMag = bulletInMag;
        gunInfo.BulletInHand = bulletInHand;
    }
}

public class GunInfo : ScriptableObject
{
    public int BulletInMag
    {
        set 
        { 
            bulletInMag = value;
            OnValueChange.Invoke(bulletInMag, bulletInHand);
        }
    }
    public int BulletInHand
    {
        set
        {
            bulletInHand = value;
            OnValueChange.Invoke(bulletInMag, bulletInHand);
        }
    }

    private int bulletInMag;
    private int bulletInHand;

    Action<int, int> OnValueChange;

    
    public void AddChangeListener(Action<int, int> listener)
    {
        listener(bulletInMag, bulletInHand);
        OnValueChange += listener;
    }
    public void RemoveChangeListener(Action<int, int> listener)
    {
        OnValueChange -= listener;
    }
}
