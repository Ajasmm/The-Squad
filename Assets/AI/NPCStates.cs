using ExitGames.Client.Photon;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPCStates : MonoBehaviourPunCallbacks,IOnPhotonViewOwnerChange,IPunObservable
{
    [Header("Patrolling Parameters")]
    public List<Transform> patrollingPoints;
    public int CurrentPatrollingPointIndex = 0;


    [Header("Components")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] NPCMovement NPCMovement;
    [SerializeField] NPCAnimation NPCAnimation;
    [SerializeField] NPCAim NPCAim;
    [SerializeField] StateMachine StateMachine;

    Transform m_Transform;

    const string PATROLLINGPOINTS_kEY = "PATROLLIGN POINTS";

    private void Awake()
    {
        playersInRange = new HashSet<Transform>();
        m_Transform = transform;
    }
    private void Start()
    {
        photonView.AddCallback<IOnPhotonViewOwnerChange>(this);
        PhotonNetwork.AddCallbackTarget(this);
        if (!photonView.IsMine)
        {
            StateMachine.enabled = false;
            agent.enabled = false;
            return;
        }
        
    }
    private void OnDestroy()
    {
        photonView.RemoveCallback<IOnPhotonViewOwnerChange>(this);
    }

    private void Update()
    {
        NPCAnimation.ForwardInput = NPCMovement.forwardVelocity;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (!photonView.IsMine) return;

        /*
        string[] content = new string[patrollingPoints.Count];
        for (int i = 0; i < patrollingPoints.Count; i++)
            content[i] = patrollingPoints[i].name;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
        {
            CachingOption = EventCaching.AddToRoomCache,
            Receivers = ReceiverGroup.All
        };
        SendOptions sendOptions = new SendOptions()
        {
            DeliveryMode = DeliveryMode.Reliable
        };

        PhotonNetwork.RaiseEvent(2, content, raiseEventOptions, sendOptions);
*/
    }

    #region Patrolling
    public void OnPatrollingStart()
    {
        NPCMovement.SetDestination(patrollingPoints[CurrentPatrollingPointIndex].position);
        NPCMovement.stoppingDistance = 0.1F;

        // Updating Animation
        if (NPCAim == null)
            Debug.Log("Npcaim is null");
        if (m_Transform == null)
            Debug.Log("Transform is null");

        NPCAim.TargetAimRigWeight = 0;
        NPCAim.TargetHoldRigWeight = 1;
        NPCAim.Firing = false;
    }
    public void OnPatrollingUpdate()
    {
        if (agent.remainingDistance < 0.5F)
        {
            CurrentPatrollingPointIndex++;
            if (CurrentPatrollingPointIndex >= patrollingPoints.Count)
                CurrentPatrollingPointIndex = 0;

            StateMachine.TriggerUnityEvent("ToWait");
        }

    }
    public void OnPatrollingStop()
    {

    }
    #endregion

    #region Waiting
    Coroutine waitCoroutine;
    public void OnWaitStart()
    {
        if (waitCoroutine != null)
            StopCoroutine(waitCoroutine);
        waitCoroutine = StartCoroutine(WaitWhilePatrilling(5));

        // Update Animations
        NPCAim.TargetPos = m_Transform.position;
        NPCAim.TargetAimRigWeight = 0;
        NPCAim.TargetHoldRigWeight = 1;
        NPCAim.Firing = false;
    }
    public void OnWaitUpdate()
    {

    }
    public void OnWaitStop()
    {
        if (waitCoroutine != null)
            StopCoroutine(waitCoroutine);
    }
    IEnumerator WaitWhilePatrilling(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StateMachine.TriggerUnityEvent("ToPatrolling");
    }
    #endregion

    #region FollowPalyer
    [Header("FollowPlayer Parameters")]
    public Transform playerToFollow;
    private HashSet<Transform> playersInRange;

    public void OnFollowPlayerStart()
    {
        NPCMovement.stoppingDistance = 5F;

        if (playersInRange.Count > 0)
            playerToFollow = playersInRange.First();

        // Updating animation
        NPCAim.TargetAimRigWeight = 1;
        NPCAim.TargetHoldRigWeight = 1;
        NPCAim.Firing = false;

    }
    public void OnFollowPlayerUpdate()
    {
        if (playerToFollow != null)
            NPCMovement.SetDestination(playerToFollow.position);

        if (IsInFiringRange())
            StateMachine.TriggerUnityEvent("ToFiring");


        // Updating Animation
        if (playerToFollow != null)
            NPCAim.TargetPos = playerToFollow.position + Vector3.up;
    }
    public void OnFollowPlayerStop()
    {
        NPCMovement.SetDestination(m_Transform.position);
        playerToFollow = null;
    }

    private bool IsInFiringRange()
    {
        if (agent.remainingDistance < 6)
            return true;

        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange.Add(other.transform);

            if (playerToFollow == null)
                playerToFollow = other.transform;

            StateMachine.TriggerUnityEvent("ToFollow");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange.Remove(other.transform);

            if (playerToFollow == other.transform)
                playerToFollow = null;

            UpdateFollowPlayer();
        }
    }
    #endregion

    #region ShootPlayer
    Coroutine firingCoroutine;
    public void OnFiringStarted()
    {
        // Updating Animation
        NPCAim.TargetAimRigWeight = 1;
        NPCAim.TargetHoldRigWeight = 1;
        NPCAim.Firing = true;

        firingCoroutine = StartCoroutine(WaitWhileFiring(1));

        if (playersInRange.Count > 0)
            playerToFollow = playersInRange.First();
    }
    public void OnFiringUpdte()
    {
        // Updating Animation
        if (playerToFollow == null)
            return;

        NPCAim.TargetPos = playerToFollow.position + Vector3.up;
        if (playerToFollow != null)
            NPCMovement.SetDestination(playerToFollow.position);
    }
    public void OnFiringStop()
    {
        if (firingCoroutine != null)
            StopCoroutine(firingCoroutine);
        playerToFollow = null;
    }
    IEnumerator WaitWhileFiring(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StateMachine.TriggerUnityEvent("ToFiringWait");
    }

    #endregion

    #region ShootinWait
    Coroutine firingWaitCoroutine;
    public void OnFiringWaitStart()
    {
        NPCAim.Firing = false;
        NPCAim.TargetAimRigWeight = 1;
        NPCAim.TargetHoldRigWeight = 1;
        firingWaitCoroutine = StartCoroutine(FiringWait(2));
        playerToFollow = playersInRange.First();
    }
    public void OnFiringWaitUpdate()
    {
        if(playerToFollow == null) return;

        NPCAim.TargetPos = playerToFollow.position + Vector3.up;
        if (playerToFollow != null)
            NPCMovement.SetDestination(playerToFollow.position);
    }
    public void OnFiringWaitStop()
    {
        if (firingWaitCoroutine != null)
            StopCoroutine(firingWaitCoroutine);
        playerToFollow = null;
    }
    public IEnumerator FiringWait(float second)
    {
        yield return new WaitForSeconds(second);
        StateMachine.TriggerUnityEvent("ToFiring");
    }
    #endregion

    public void UpdateFollowPlayer()
    {
        if (playerToFollow != null)
            return;

        playersInRange.RemoveWhere((Transform t) => t == null);
        if (playersInRange.Count > 0)
            playerToFollow = playersInRange.First();
        else
            StateMachine.TriggerUnityEvent("ToWait");
    }
    public void CheckForFollowPlayer()
    {
        UpdateFollowPlayer();
        if (playerToFollow == null)
            return;

        StateMachine.TriggerUnityEvent("ToFollow");
    }

    public void OnOwnerChange(Player newOwner, Player previousOwner)
    {
        if (newOwner.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            return;

        agent.enabled = true;
        StateMachine.enabled = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(CurrentPatrollingPointIndex);
        }
        else
        {
            CurrentPatrollingPointIndex = (int) stream.ReceiveNext();
        }
    }

}
