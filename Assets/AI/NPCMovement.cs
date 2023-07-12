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
    [SerializeField] PhotonView photonView;

    private Vector3 Target;
    public float stoppingDistance { set { StopDistance = value; } }
    private float StopDistance;

    Transform m_Transform;
    Vector3 prevPos;
    public float forwardVelocity { get; private set; }  

    private void Start()
    {
        m_Transform = transform;
        navMeshAgent.speed = WalkSpeed;
        navMeshAgent.stoppingDistance = 0;
    }
    private void Update()
    {
        if (!photonView.IsMine) return;

        Vector3 velocity = m_Transform.position - prevPos;
        forwardVelocity = m_Transform.InverseTransformDirection(velocity).z * (1 / Time.deltaTime);

        if (navMeshAgent.remainingDistance < StopDistance)
        {
            navMeshAgent.updatePosition = false;
            navMeshAgent.nextPosition = m_Transform.position;
        }
        else
        {
            navMeshAgent.updatePosition = true;
        }
        navMeshAgent.SetDestination(Target);
        prevPos = m_Transform.position;
    }
    public void SetDestination(Vector3 Target)
    {
        if (this.Target == Target)
            return;

        navMeshAgent.SetDestination(Target);
        this.Target = Target;
    }
    public float GetVelocity()
    {
        return m_Transform.InverseTransformDirection(navMeshAgent.velocity).z;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(forwardVelocity);
        }
        else
        {
            forwardVelocity = (float)stream.ReceiveNext();
        }
    }
}
