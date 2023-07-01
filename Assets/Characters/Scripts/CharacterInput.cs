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
    [SerializeField] PhotonView photonView;

    [Header("Parameters")]
    [SerializeField] float mouseSencitivity = 1;

    GameInput input;

    CharacterMovementInputHandler movementInputHandler;

    public CharacterMovementGroundInputHandler OnGroundInputHandler { get; private set; }
    public CharacterMovementAirInputHandler OnAirInputHandler { get; private set; }

    

    private void OnEnable()
    {
        if (!photonView.IsMine)
            return;

        NetworkManager.Instance.ActivateOfflineMode();

        input = GameManager.Instance.gameInput;
        input.Gameplay.Enable();

        input.Gameplay.Jump.performed += Jump;
        input.Gameplay.Run.performed += Run;
        input.Gameplay.Run.canceled += Run;

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
        }
    }
    private void Update()
    {
        movementInputHandler = movementInputHandler.HandleInput();
    }


    private void Jump(InputAction.CallbackContext context)
    {
        movementInputHandler = movementInputHandler.Jump(context);
    }
    private void Run(InputAction.CallbackContext context)
    {
        movementInputHandler.Run(context);
    }
    public void JumpUp()
    { 
        photonView.RPC("JumpUpRPC", RpcTarget.All);
    }
    public void JumpDown()
    {
        photonView.RPC("JumpDownRPC", RpcTarget.All);
    }

    [PunRPC]
    private void JumpUpRPC()
    {
        characterMovement.Jump();
        characterAnimation.JumpUp();
    }
    [PunRPC]
    private void JumpDownRPC()
    {
        characterAnimation.JumpDown();
    }
}
