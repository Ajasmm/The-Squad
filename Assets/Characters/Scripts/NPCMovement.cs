using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PhotonView))]
public class NPCMovement : MonoBehaviour,IPunObservable
{
    [Header("Parameters")]
    [SerializeField] float WalkSpeed = 2;

    [Header("Component References")]
    [SerializeField] NavMeshAgent navMeshAgent;

    public Transform Target { set { target = value; navMeshAgent.SetDestination(target.position); } }
    private Transform target;

    private void Start()
    {
        navMeshAgent.stoppingDistance = 0.1F;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(target);
        }
        else
        {
            Target = (Transform) stream.ReceiveNext();
        }
    }
}
