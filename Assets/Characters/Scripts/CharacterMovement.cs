using Photon.Pun;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PhotonView))]
public class CharacterMovement : MonoBehaviour, Photon.Pun.IPunObservable
{
    [Header("Parameters")]
    [SerializeField] float WalkSpeed = 2;
    [SerializeField] float RunSpeed = 6;
    [SerializeField] float JumpHeight = 1.5F;

    [Header("Component References")]
    [SerializeField] CharacterController characterController;

    public float ForwardInput;
    public float SidewaysInput;
    public bool RunningInput;
    public float Y_Look;

    const float GRAVITY = 9.8F;

    Vector3 localMovement;

    Transform m_Transform;

    private void Awake()
    {
        localMovement = Vector3.zero;
    }
    private void Start()
    {
        m_Transform = transform;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        localMovement.z = ForwardInput;
        localMovement.x = SidewaysInput;

        // Update gravity
        if (localMovement.y > 0 || !characterController.isGrounded)
            localMovement.y -= GRAVITY * deltaTime;
        else
            localMovement.y = -GRAVITY * 0.25F;

        // Only run in forward motion
        if (RunningInput && localMovement.z > 0)
            localMovement.z *= RunSpeed;
        else
            localMovement.z *= WalkSpeed;

        // apply movement to character controller
        m_Transform.Rotate(m_Transform.up, Y_Look, Space.Self);
        localMovement = m_Transform.TransformVector(localMovement);
        characterController.Move(localMovement * deltaTime);
    }

    public void Jump()
    {
        float JumpVelocity = Mathf.Sqrt(2 * JumpHeight * GRAVITY);
        localMovement.y = JumpVelocity;
    }
    public bool IsGrounded() => characterController.isGrounded;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ForwardInput);
            stream.SendNext(ForwardInput);
            stream.SendNext(RunningInput);
            stream.SendNext(Y_Look);
        }
        else
        {
            ForwardInput = (float)stream.ReceiveNext();
            SidewaysInput = (float)stream.ReceiveNext();
            RunningInput = (bool)stream.ReceiveNext();
            Y_Look = (float)stream.ReceiveNext();
        }
    }
}
