using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;

[RequireComponent(typeof(PhotonView))]
public class CharacterInput : MonoBehaviour
{ 
    [Header("Component References")]
    [SerializeField] CharacterMovement characterMovement;
    [SerializeField] CharacterAnimation characterAnimation;
    [SerializeField] CharacterAim characterAim;
    [SerializeField] PhotonView photonView;

    [Space(10), Header("Parameters")]
    [SerializeField] float mouseSencitivity = 1;

    [Space(10), Header("Other")]
    [SerializeField] GameObject virtualCamera;

    GameInput input;

    CharacterMovementInputHandler movementInputHandler;

    public CharacterMovementGroundInputHandler OnGroundInputHandler { get; private set; }
    public CharacterMovementAirInputHandler OnAirInputHandler { get; private set; }


    WeaponInputHandler weaponInputHandler;

    public IdleWeapon IdleWeapon { get; private set; }
    public FiringWeapon FiringWeapon { get; private set; }
    public ReloadWeapon ReloadWeapon { get; private set; }

    private void OnEnable()
    {
        if (!photonView.IsMine)
        {
            virtualCamera.SetActive(false);
            return;
        }

        input = GameManager.Instance.gameInput;
        input.Gameplay.Enable();

        input.Gameplay.Jump.performed += Jump;
        input.Gameplay.Run.performed += Run;
        input.Gameplay.Run.canceled += Run;
        input.Gameplay.Shooting.performed += Shoot;
        input.Gameplay.Shooting.canceled += Shoot;
        input.Gameplay.Reload.performed += Reload;

        OnGroundInputHandler = new CharacterMovementGroundInputHandler(characterMovement, characterAnimation, this, input, mouseSencitivity);
        OnAirInputHandler = new CharacterMovementAirInputHandler(characterMovement, characterAnimation, this, input, mouseSencitivity);

        IdleWeapon = new IdleWeapon(characterAim, this);
        FiringWeapon = new FiringWeapon(characterAim, this);
        ReloadWeapon = new ReloadWeapon(characterAim, this);

        movementInputHandler = OnGroundInputHandler;
        weaponInputHandler = IdleWeapon;
    }
    private void OnDisable()
    {
        if(input != null)
        {
            input.Gameplay.Jump.performed -= Jump;
            input.Gameplay.Run.performed -= Run;
            input.Gameplay.Run.canceled -= Run;
            input.Gameplay.Shooting.performed -= Shoot;
            input.Gameplay.Shooting.canceled -= Shoot;
            input.Gameplay.Reload.performed -= Reload;
        }
    }
    private void Start()
    {

        Hashtable customProperties = new Hashtable();
        int actorId = PhotonNetwork.LocalPlayer.ActorNumber;
        customProperties.TryAdd(actorId.ToString(), 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);

        if (PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties))
            Debug.LogWarning("Property set from player");
        else
            Debug.LogWarning("Failed to set custom property");

        if (GameManager.Instance.GamePlayMode)
            GameManager.Instance.GamePlayMode.Player = photonView.Owner;
    }
    private void Update()
    {
        if (!photonView.IsMine)
            return;

        movementInputHandler = movementInputHandler.HandleInput();
        weaponInputHandler = weaponInputHandler.HandleInput();
    }


    private void Jump(InputAction.CallbackContext context)
    {
        movementInputHandler = movementInputHandler.Jump(context);
    }
    private void Run(InputAction.CallbackContext context)
    {
        movementInputHandler.Run(context);
    }
    private void Shoot(InputAction.CallbackContext context)
    {
        weaponInputHandler = weaponInputHandler.Fire(context.phase == InputActionPhase.Performed);
    }
    private void Reload(InputAction.CallbackContext context)
    {
        weaponInputHandler = weaponInputHandler.Reload();
    }



    public void JumpUp()
    { 
        photonView.RPC("JumpUpRPC", RpcTarget.All);
    }
    public void JumpDown()
    {
        photonView.RPC("JumpDownRPC", RpcTarget.All);
    }
    public void JumpUpAnimation()
    {
        photonView.RPC("JumpUpAnimationOnlyRPC", RpcTarget.All);
    }
    public void Reload()
    {
        photonView.RPC("ReloadAnimationOnlyRPC", RpcTarget.All);
    }
    public void ReloadAnimationFinished()
    {
        if (!photonView.IsMine) return;

        weaponInputHandler = input.Gameplay.Shooting.phase == InputActionPhase.Performed ? FiringWeapon : IdleWeapon;
        weaponInputHandler.HandleInput();
        characterAim.ReloadFinished();
    }

    [PunRPC]
    private void JumpDownRPC()
    {
        characterAnimation.JumpDown();
    }
    [PunRPC]
    private void JumpUpRPC()
    {
        characterMovement.Jump();
        characterAnimation.JumpUp();
    }
    [PunRPC]
    private void JumpUpAnimationOnlyRPC()
    {
        characterAnimation.JumpUp();
    }
    [PunRPC]
    private void ReloadAnimationOnlyRPC()
    {
        characterAim.Reload();
    }

}
