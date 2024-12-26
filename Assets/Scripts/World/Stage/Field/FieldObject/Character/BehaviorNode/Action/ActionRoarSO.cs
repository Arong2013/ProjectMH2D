using UnityEngine;

[CreateAssetMenu(fileName = "ActionRoar", menuName = "Behavior/Actions/Roar")]
public class ActionRoarSO : BehaviorActionSO
{
    public override BehaviorAction CreateAction()
    {
        return new ActionRoar();
    }
}
public class ActionRoar : BehaviorAction
{
    Transform target;
    bool canRoar;

    public override BehaviorState Execute()
    {
        if (target == null)
            this.target = actionPhase.GetData("target") as Transform;

        if (character.GetCharacterStateType() == typeof(RoarState))
        {
            canRoar = true;
            character.SetAnimatorValue(CharacterAnimeIntName.RoarType, 0);
            return BehaviorState.RUNNING;
        }
        else
        {
            if (canRoar)
                return BehaviorState.SUCCESS;
            else
            {
                character.SetAnimatorValue(CharacterAnimeIntName.RoarType, 1);
                return BehaviorState.RUNNING;
            }
        }
    }
}
