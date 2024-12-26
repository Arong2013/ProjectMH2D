using UnityEngine;

[CreateAssetMenu(fileName = "ConditionCloseTarget", menuName = "Behavior/Conditions/ConditionCloseTarget")]
public class ConditionCloseTargetSO : BehaviorConditionSO
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 10f; // 감지 반경
    [SerializeField] private LayerMask enemyLayer; // 감지할 적 레이어

    public override BehaviorCondition CreateCondition()
    {
        return new ConditionCloseTarget(detectionRadius, enemyLayer);
    }
}
public class ConditionCloseTarget : BehaviorCondition
{
    private float detectionRadius;
    private LayerMask enemyLayer;

    public ConditionCloseTarget(float detectionRadius, LayerMask enemyLayer)
    {
        this.detectionRadius = detectionRadius;
        this.enemyLayer = enemyLayer;
    }
    public override BehaviorState Execute()
    {
        // 자기 위치를 기준으로 감지
        Collider[] detectedEnemies = Physics.OverlapSphere(character.transform.position, detectionRadius, enemyLayer);

        if (detectedEnemies.Length > 0)
        {
            Transform closestEnemy = GetClosestEnemy(detectedEnemies);
            actionPhase.SetData("target", closestEnemy);
            return BehaviorState.SUCCESS;
        }
        return BehaviorState.FAILURE;
    }

    private Transform GetClosestEnemy(Collider[] enemies)
    {
        Transform closest = null;
        float closestDistance = float.MaxValue;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(character.transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy.transform;
            }
        }
        return closest;
    }
}
