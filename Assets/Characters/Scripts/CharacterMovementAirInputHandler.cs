using System;
using UnityEngine;

[Serializable]
public class CharacterMovementAirInputHandler : CharacterMovementInputHandler
{
    float mouseSencitivity;

    public CharacterMovementAirInputHandler(CharacterMovement characterMovement,
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

        characterMovement.Y_Look = Look.x * mouseSencitivity;

        if (characterMovement.IsGrounded())
        {
            characterInput.JumpDown();
            return characterInput.OnGroundInputHandler;
        }

        return this;
    }
    private void GetInput()
    {
        Look = input.Gameplay.Look.ReadValue<Vector2>();
    }
}
