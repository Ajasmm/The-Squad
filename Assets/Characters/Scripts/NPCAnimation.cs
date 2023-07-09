using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]
public class NPCAnimation : MonoBehaviour, IPunObservable
{
    [Header("Parameters")]
    [SerializeField] string ForwardParameter_Name = "Forward";

    [Header("Component References")]
    [SerializeField] Animator animator;
    [SerializeField] PhotonView photonView;

    public float ForwardInput;

    private int forwardHash;

    private void Awake()
    {
        forwardHash = Animator.StringToHash(ForwardParameter_Name);
    }

    private void Update()
    {
        float forward = Mathf.Clamp(ForwardInput, 0f, 1f);

        animator.SetFloat(forwardHash, forward);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ForwardInput);
        }
        else
        {
            ForwardInput = (float)stream.ReceiveNext();
        }
    }
}