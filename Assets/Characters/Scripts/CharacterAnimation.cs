using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]
public class CharacterAnimation : MonoBehaviour, Photon.Pun.IPunObservable
{
    [Header("Parameters")]
    [SerializeField] string ForwardParameter_Name = "Forward";
    [SerializeField] string SidewaysParameter_Name = "Sideways";
    [SerializeField] string JumpUpParameter_Name = "JumpUp";
    [SerializeField] string JumpDownParameter_Name = "JumpDown";

    [Header("Component References")]
    [SerializeField] Animator animator;
    [SerializeField] PhotonView photonView;

    public float ForwardInput;
    public float SidewaysInput;
    public bool RunningInput;

    private int forwardHash;
    private int sidewaysHash;
    public int JumpUpHash { get; private set; }
    public int JumpDownHash { get; private set; }

    private void Awake()
    {
        forwardHash = Animator.StringToHash(ForwardParameter_Name);
        sidewaysHash = Animator.StringToHash(SidewaysParameter_Name);
        JumpUpHash = Animator.StringToHash(JumpUpParameter_Name);
        JumpDownHash = Animator.StringToHash(JumpDownParameter_Name);
    }

    private void Update()
    {
        // Running animation only in forward movement.
        float forward = ForwardInput;
        if (RunningInput && forward > 0)
            forward *= 2;

        animator.SetFloat(forwardHash, forward);
        animator.SetFloat(sidewaysHash, SidewaysInput);
    }

    public void JumpUp()
    {
        animator.SetTrigger(JumpUpHash);
    }
    public void JumpDown()
    {
        animator.SetTrigger(JumpDownHash);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ForwardInput);
            stream.SendNext(SidewaysInput);
            stream.SendNext(RunningInput);
        }
        else
        {
            ForwardInput = (float)stream.ReceiveNext();
            SidewaysInput = (float)stream.ReceiveNext();
            RunningInput = (bool)stream.ReceiveNext();
        }
    }
}
