using System;
using Unity.Mathematics;
using UnityEngine;

public class IdleWeapon : WeaponInputHandler
{
    public IdleWeapon(CharacterAim characterAim, CharacterInput characterInput) :
        base(characterAim, characterInput)
    {
    }

    public override WeaponInputHandler Fire(bool Firing)
    {
        if (Firing)
        {
            characterAim.Firing = true;
            return characterInput.FiringWeapon;
        }

        characterAim.Firing = false;
        return this;
    }

    public override WeaponInputHandler HandleInput()
    {
        characterAim.TargetAimRigWeight = 0;
        characterAim.GunHoldRigWeight = 1;

        return this;
    }

    public override WeaponInputHandler Reload()
    {
        characterAim.Firing = false;
        characterInput.Reload();
        return characterInput.ReloadWeapon;
    }
}
