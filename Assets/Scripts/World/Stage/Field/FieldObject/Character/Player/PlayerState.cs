public abstract class CharacterState
{
    protected CharacterMarcine character;

    public CharacterState(CharacterMarcine character)
    {
        this.character = character;
    }
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
