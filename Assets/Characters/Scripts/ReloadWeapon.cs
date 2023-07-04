using UnityEngine;

public class ReloadWeapon : WeaponInputHandler
{
    public ReloadWeapon(CharacterAim characterAim, CharacterInput characterInput) :
        base(characterAim, characterInput)
    {
    }

    public override WeaponInputHandler Fire(bool Firing)
    {
        return this;
    }
    public override WeaponInputHandler HandleInput()
    {
        characterAim.TargetAimRigWeight = 0;
        characterAim.TargetHoldRigWeight = 0;

        return this;
    }
    public override WeaponInputHandler Reload()
    {
        return this;
    }
}