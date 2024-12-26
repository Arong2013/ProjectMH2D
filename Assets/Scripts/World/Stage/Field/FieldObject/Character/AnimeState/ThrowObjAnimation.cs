using UnityEngine;
public class ThrowObjAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new ThrowObjState(character));
    }
}
public class ThrowObjState : CharacterState
{
    public ThrowObjState(CharacterMarcine character) : base(character)
    {

    }
    public override void Enter()
    {
        character.ThrowObj();
        character.SetAnimatorValue(CharacterAnimeIntName.InteractionType, 0);
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
}