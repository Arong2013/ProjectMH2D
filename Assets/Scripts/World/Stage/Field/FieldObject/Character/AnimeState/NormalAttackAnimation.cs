using UnityEngine;

public class NormalAttackAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new NormalAttackState(character));
    }
}
public class NormalAttackState : AttackState
{
    public NormalAttackState(CharacterMarcine character) : base(character)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void BtnUp()
    {
;
    }
    public override void Exit()
    {
        base.Exit();
    }
}