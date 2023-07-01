using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public abstract class CharacterMovementInputHandler
{
    protected CharacterMovement characterMovement;
    protected CharacterAnimation characterAnimation;
    protected CharacterInput characterInput;
    protected GameInput input;

    public float SidewaysInput = 0;
    public float ForwardInput = 0;
    public bool RunningInput = false;
    public Vector2 Look = Vector2.zero;

    protected float _xMovement;
    protected float _yMovement;

    public CharacterMovementInputHandler(
        CharacterMovement characterMovement,
        CharacterAnimation characterAnimation,
        CharacterInput characterInput,
        GameInput input
        )
    {
        this.characterMovement = characterMovement;
        this.characterAnimation = characterAnimation;
        this.characterInput = characterInput;
        this.input = input;
    }

    public abstract CharacterMovementInputHandler HandleInput();

    public virtual void Run(InputAction.CallbackContext context) 
    {
        
    }
    public virtual CharacterMovementInputHandler Jump(InputAction.CallbackContext context)
    {
        return this;
    }
}
