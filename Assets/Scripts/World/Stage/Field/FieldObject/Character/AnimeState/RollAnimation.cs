using UnityEngine;
public class RollAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new RollState(character));
    }
}
public class RollState : CharacterState
{
    float rolledDistance = 0f;

    public RollState(CharacterMarcine character) : base(character)
    {

    }
    public override void Enter()
    {
        base.Enter();
        character.Roll();
    }
    public override void Exit()
    {
        base.Exit();
    }
}