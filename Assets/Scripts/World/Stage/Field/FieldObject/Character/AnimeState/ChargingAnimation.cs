using UnityEngine;
public class ChargingAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new ChargingState(character));
    }
}
public class ChargingState : AttackState
{
    float ChargingCount;
    public ChargingState(CharacterMarcine character) : base(character)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        character.SetAnimatorValue(CharacterAnimeBoolName.CanCharging, true);
        character.SetAnimatorValue(CharacterAnimeFloatName.ChargingCount, 0);
        ChargingCount = 0;
    }
    public override void Execute()
    {
        base.Execute();
        ChargingCount += Time.deltaTime;
        character.SetAnimatorValue(CharacterAnimeFloatName.ChargingCount, ChargingCount);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void BtnUp()
    {
        character.SetAnimatorValue(CharacterAnimeBoolName.CanCharging, false);
    }
}