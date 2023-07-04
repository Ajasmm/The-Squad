public abstract class WeaponInputHandler
{
    protected CharacterAim characterAim;
    protected CharacterInput characterInput;
    public WeaponInputHandler(CharacterAim characterAim, CharacterInput characterInput)
    {
        this.characterAim = characterAim;
        this.characterInput = characterInput;
    }
    public abstract WeaponInputHandler HandleInput();
    public abstract WeaponInputHandler Fire(bool FireValue);
    public abstract WeaponInputHandler Reload();
}
