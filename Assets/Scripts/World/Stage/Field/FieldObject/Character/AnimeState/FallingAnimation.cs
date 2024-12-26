using UnityEngine;

public class FallingAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new FallingState(character));
    }
}
public class FallingState: CharacterState
{
    public FallingState(CharacterMarcine character) : base(character) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Execute()
    {
        character.Move();
        if(character.isGround)
            character.SetAnimatorValue(CharacterAnimeIntName.MovementType, 0);
    }
}
