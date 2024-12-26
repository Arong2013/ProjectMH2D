using UnityEngine;

[CreateAssetMenu(fileName = "ActionAttack", menuName = "Behavior/Actions/Attack")]
public class ActionAttackSO : BehaviorActionSO
{
    [SerializeField] int attackType = 0; // 공격 타입

    public override BehaviorAction CreateAction()
    {
        return new ActionAttack(attackType);
    }
}

public class ActionAttack : BehaviorAction
{
    Transform target;
    int attackType;
    bool canAttack = true;
    public ActionAttack(int attackType)
    {
        this.attackType = attackType;
    }

    public override BehaviorState Execute()
    {
        if (target == null)
            this.target = actionPhase.GetData("target") as Transform;
        if (character.GetCharacterStateType() == typeof(NormalAttackState))
        {
            character.SetAnimatorValue(CharacterAnimeIntName.AttackType, 0);
            return BehaviorState.RUNNING;
        }
        else
        {
            if(character.GetAnimatorValue<CharacterAnimeIntName,int>(CharacterAnimeIntName.AttackType) == 0)
            {
                character.SetAnimatorValue(CharacterAnimeIntName.AttackType, attackType);
                return BehaviorState.RUNNING;
            }
            else
            {
                return BehaviorState.SUCCESS;
            }
        }
    }
}
