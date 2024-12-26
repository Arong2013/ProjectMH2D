using UnityEngine;

[CreateAssetMenu(fileName = "ConditionForwardTarget", menuName = "Behavior/Conditions/ForwardTarget")]
public class ConditionForwardTargetSO : BehaviorConditionSO
{
    [SerializeField] LayerMask EnemyLayer;
    [Header("Raycast Settings")]
    [SerializeField] float distance = 10f; // 레이캐스트 거리
    public override BehaviorCondition CreateCondition()
    {
        return new ConditionForwardTarget(distance, EnemyLayer);
    }
}
public class ConditionForwardTarget : BehaviorCondition
{
    float distance;
    LayerMask EnemyLayer;
    public ConditionForwardTarget(float distance, LayerMask layerMask)
    {
        this.distance = distance;
        EnemyLayer = layerMask; 
    }
    public override BehaviorState Execute()
    {
        RaycastHit hit;
        if (Physics.Raycast(character.transform.position, character.transform.forward, out hit, distance, EnemyLayer))
        {
            actionPhase.SetData("target", hit.transform);
            return BehaviorState.SUCCESS;
        }

        return BehaviorState.FAILURE;
    }
}