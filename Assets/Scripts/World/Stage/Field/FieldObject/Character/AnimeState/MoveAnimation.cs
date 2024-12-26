using UnityEngine;

public class MoveAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new MoveState(character));
    }
}
public class MoveState : CharacterState
{
    public MoveState(CharacterMarcine character) : base(character) {  }
    public override void Execute()
    {
        character.Move();
        character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, character.currentDir.magnitude);
        if (character.currentDir.magnitude < 0.1)
            character.SetAnimatorValue(CharacterAnimeIntName.MovementType, 0);
    }
}
