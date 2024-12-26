using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ConditionParabolicMover", menuName = "Behavior/Conditions/ConditionParabolicMover")]
public class ConditionParabolicMoverSO : BehaviorConditionSO
{
    [Header("Detection Settings")]
    [SerializeField] private float flightTime = 2f;
    [SerializeField] private Vector3 gravity = new Vector3(0, -9.81f, 0);
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float positionCheckInterval = 0.1f;
    [SerializeField] private float checkRadius = 0.5f;

    public override BehaviorCondition CreateCondition()
    {
        return new ConditionParabolicMover(flightTime, gravity, obstacleLayer, positionCheckInterval, checkRadius);
    }
}

public class ConditionParabolicMover : BehaviorCondition
{
    private float flightTime;
    private Vector3 gravity;
    private LayerMask obstacleLayer;
    private float positionCheckInterval;
    private float checkRadius;

    public ConditionParabolicMover(float flightTime, Vector3 gravity, LayerMask obstacleLayer, float positionCheckInterval, float checkRadius)
    {
        this.flightTime = flightTime;
        this.gravity = gravity;
        this.obstacleLayer = obstacleLayer;
        this.positionCheckInterval = positionCheckInterval;
        this.checkRadius = checkRadius;
    }

    public override BehaviorState Execute()
    {
        if (actionPhase.GetData("target") == null)
            return BehaviorState.FAILURE;
        if(actionPhase.GetData("pathPoints") != null)
            return BehaviorState.SUCCESS;
        var  target = actionPhase.GetData("target") as Transform;
        List<Vector3> pathPoints = ValidateAndCalculateParabolicPath(character.transform.position, target.position, flightTime);
        if (pathPoints != null)
        {
            Debug.Log(pathPoints.Count);    
            actionPhase.SetData("pathPoints", pathPoints); // 성공 시 경로를 저장
            return BehaviorState.SUCCESS;
        }
        return BehaviorState.FAILURE;
    }
    /// <summary>
    /// 포물선 경로를 미리 계산하고 장애물이 없는지 검증
    /// </summary>
    private List<Vector3> ValidateAndCalculateParabolicPath(Vector3 start, Vector3 target, float time)
    {
        Vector3 initialVelocity = CalculateLaunchVelocity(start, target, time);
        List<Vector3> pathPoints = new List<Vector3>();

        int steps = Mathf.CeilToInt(time / positionCheckInterval); // 계산할 스텝 개수
        for (int i = 0; i <= steps; i++)
        {
            float t = i * positionCheckInterval;
            Vector3 point = start + initialVelocity * t + 0.5f * gravity * t * t; // 포물선 위치 공식
            pathPoints.Add(point);

            // 경로 상의 점에서 충돌 체크
            if (Physics.CheckSphere(point, checkRadius, obstacleLayer))
            {
                Debug.Log($"Obstacle detected at {point}");
                return null; // 장애물이 하나라도 있으면 null 반환
            }
        }

        return pathPoints; // 경로 상에 장애물이 없으면 경로 반환
    }

    /// <summary>
    /// 포물선 초기 속도 계산
    /// </summary>
    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float time)
    {
        Vector3 displacement = target - start;          // 목표 위치와 시작 위치 간의 거리
        Vector3 horizontalDisplacement = new Vector3(displacement.x, 0, displacement.z); // 수평 거리
        float verticalDisplacement = displacement.y;    // 수직 거리

        // 수평 속도
        Vector3 horizontalVelocity = horizontalDisplacement / time;

        // 수직 속도 (SUVAT 공식: v = (s - 0.5 * g * t^2) / t)
        float verticalVelocity = (verticalDisplacement - 0.5f * gravity.y * time * time) / time;

        return horizontalVelocity + Vector3.up * verticalVelocity; // 수평 + 수직 속도 합
    }
}
