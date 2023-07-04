using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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

    bool Firing = false;

    private void OnEnable()
    {
        if (!photonView.IsMine)
        {
            virtualCamera.SetActive(false);
            return;
        }

        NetworkManager.Instance.ActivateOfflineMode();

        input = GameManager.Instance.gameInput;
        input.Gameplay.Enable();

        input.Gameplay.Jump.performed += Jump;
        input.Gameplay.Run.performed += Run;
        input.Gameplay.Run.canceled += Run;
        input.Gameplay.Shooting.performed += Shoot;
        input.Gameplay.Shooting.canceled += Shoot;

        OnGroundInputHandler = new CharacterMovementGroundInputHandler(characterMovement, characterAnimation, this, input, mouseSencitivity);
        OnAirInputHandler = new CharacterMovementAirInputHandler(characterMovement, characterAnimation, this, input, mouseSencitivity);
        movementInputHandler = OnGroundInputHandler;
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
        }
    }
    private void Update()
    {
        if (!photonView.IsMine)
            return;

        movementInputHandler = movementInputHandler.HandleInput();
        characterAim.Firing = Firing;
        if (Firing)
            characterAim.TargetAimRigWeight = 1;
        else
            characterAim.TargetAimRigWeight = 0;

        characterAim.TargetPos = characterAim.FindTargetPos();
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
        Firing = context.phase == InputActionPhase.Performed;
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
}
