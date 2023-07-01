using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class CharacterMovementGroundInputHandler : CharacterMovementInputHandler
{
    float mouseSencitivity;

    public CharacterMovementGroundInputHandler(CharacterMovement characterMovement,
        CharacterAnimation characterAnimation,
        CharacterInput characterInput,
        GameInput input, 
        float mouseSencitivity) : 
        base(characterMovement,
            characterAnimation,
            characterInput,
            input)
    {
        this.mouseSencitivity = mouseSencitivity;
    }

    public override CharacterMovementInputHandler HandleInput()
    {
        GetInput();
        UpdateVariables();

        characterMovement.ForwardInput = ForwardInput;
        characterMovement.SidewaysInput = SidewaysInput;
        characterMovement.RunningInput = RunningInput;
        characterMovement.Y_Look = Look.x * mouseSencitivity;

        characterAnimation.ForwardInput = ForwardInput;
        characterAnimation.SidewaysInput = SidewaysInput;
        characterAnimation.RunningInput = RunningInput;

        if (!characterMovement.IsGrounded())
        {
            characterInput.JumpUpAnimation();
            return characterInput.OnAirInputHandler;
        }

        return this;
    }
    private void GetInput()
    {
        Vector2 movement = input.Gameplay.Movement.ReadValue<Vector2>();
        _xMovement = movement.x;
        _yMovement = movement.y;
        Look = input.Gameplay.Look.ReadValue<Vector2>();
    }
    private void UpdateVariables()
    {
        float deltaTime = Time.deltaTime;
        SidewaysInput = Mathf.MoveTowards(SidewaysInput, _xMovement, deltaTime * 4);
        ForwardInput = Mathf.MoveTowards(ForwardInput, _yMovement, deltaTime * 4);
    }

    public override CharacterMovementInputHandler Jump(InputAction.CallbackContext context)
    {
        characterInput.JumpUp();
        return characterInput.OnGroundInputHandler;
    }

    public override void Run(InputAction.CallbackContext context)
    {
        RunningInput = context.phase == InputActionPhase.Performed;
    }

}
