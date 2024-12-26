using UnityEngine;

[CreateAssetMenu(fileName = "ActionChessTarget", menuName = "Behavior/Actions/ChessTarget")]
public class ActionChessTargetSO : BehaviorActionSO
{
    [SerializeField] float stopDistance = 4.0f; // 멈출 거리

    public override BehaviorAction CreateAction()
    {
        return new ActionChessTarget(stopDistance);
    }
}

public class ActionChessTarget : BehaviorAction
{
    Transform target;
    float stopDistance;

    public ActionChessTarget(float stopDistance)
    {
        this.stopDistance = stopDistance;
    }

    public override BehaviorState Execute()
    {
        if (target == null)
            this.target = actionPhase.GetData("target") as Transform;

        float distanceToTarget = Vector3.Distance(character.transform.position, target.position);
        if (distanceToTarget <= stopDistance)
        {
            character.SetDir(new Vector2(0, 0));
            character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, 0f);
            return BehaviorState.SUCCESS; // 행동 완료 상태 반환
        }
        else
        {
            character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, 0.1f);
            Vector3 direction = (target.position - character.transform.position).normalized;
            character.SetDir(new Vector2(direction.x, 0));
            return BehaviorState.RUNNING; // 행동 실행 중 상태 반환
        }
    }
}
