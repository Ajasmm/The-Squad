using UnityEngine;

public class FiringWeapon : WeaponInputHandler
{
    public FiringWeapon(CharacterAim characterAim, CharacterInput characterInput) :
        base(characterAim, characterInput)
    {
    }

    public override WeaponInputHandler Fire(bool Firing)
    {
        if (!Firing)
        {
            characterAim.Firing = false;
            return characterInput.IdleWeapon;
        }

        characterAim.Firing = true;
        return this;
    }

    public override WeaponInputHandler HandleInput()
    {
        characterAim.TargetAimRigWeight = 1;
        characterAim.TargetHoldRigWeight = 1;

        characterAim.TargetPos = characterAim.FindTargetPos();
        return this;
    }

    public override WeaponInputHandler Reload()
    {
        characterAim.Firing = false;
        characterInput.Reload();
        return characterInput.ReloadWeapon;
    }
}
