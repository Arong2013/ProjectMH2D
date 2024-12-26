using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParabolicMover : MonoBehaviour
{
    public float distanceMagnitude;
    public float flightTime = 2f;            // 비행 시간 (초 단위)
    public Vector3 gravity = new Vector3(0, -9.81f, 0); // 중력 가속도
    public LayerMask obstacleLayer;          // 체크할 레이어 (특정 레이어 설정)
    public float positionCheckInterval = 0.1f; // 경로 샘플링 간격
    public float checkRadius = 0.5f;         // 충돌 체크 반지름

    private Rigidbody rb;
    private List<Vector3> calculatedPath = new List<Vector3>(); // 미리 계산된 경로

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 targetPosition = transform.position + transform.forward * distanceMagnitude;

        if (ValidateParabolicPath(transform.position, targetPosition, flightTime))
        {
            Debug.Log("No obstacles detected. Launching!");
            LaunchToTarget(targetPosition);
        }
        else
        {
            Debug.Log("Obstacles detected in path. Cannot move!");
        }
    }

    /// <summary>
    /// 포물선 경로를 미리 계산하고 장애물이 없는지 검증합니다.
    /// </summary>
    private bool ValidateParabolicPath(Vector3 start, Vector3 target, float time)
    {
        calculatedPath.Clear();
        Vector3 initialVelocity = CalculateLaunchVelocity(start, target, time);

        int steps = Mathf.CeilToInt(time / positionCheckInterval); // 계산할 스텝 개수
        for (int i = 0; i <= steps; i++)
        {
            float t = i * positionCheckInterval;
            Vector3 point = start + initialVelocity * t + 0.5f * gravity * t * t; // 포물선 위치 공식
            calculatedPath.Add(point);

            // 경로 상의 점에서 충돌 체크
            if (Physics.CheckSphere(point, checkRadius, obstacleLayer))
            {
                Debug.Log($"Obstacle detected at {point}");
                return false; // 장애물이 하나라도 있으면 경로가 유효하지 않음
            }
        }

        return true; // 장애물이 없음
    }

    /// <summary>
    /// 목표 위치로 이동합니다.
    /// </summary>
    public void LaunchToTarget(Vector3 targetPosition)
    {
        Vector3 velocity = CalculateLaunchVelocity(transform.position, targetPosition, flightTime);
        rb.useGravity = false; // Rigidbody의 기본 중력을 끄고 사용자 정의 중력을 적용
        rb.velocity = velocity;

        // 사용자 정의 중력 적용 시작
        StartCoroutine(ApplyCustomGravity());
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

    /// <summary>
    /// 사용자 정의 중력을 적용합니다.
    /// </summary>
    private IEnumerator ApplyCustomGravity()
    {
        while (true)
        {
            rb.velocity += gravity * Time.fixedDeltaTime; // 사용자 정의 중력 적용
            yield return new WaitForFixedUpdate();
        }
    }
}
