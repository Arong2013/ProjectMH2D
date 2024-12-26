using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ActionMoveAlongPath", menuName = "Behavior/Actions/MoveAlongPath")]
public class ActionMoveAlongPathSO : BehaviorActionSO
{
    public float moveSpeed = 5f; // 이동 속도
    public override BehaviorAction CreateAction()
    {
        return new ActionMoveAlongPath(moveSpeed);
    }
}

public class ActionMoveAlongPath : BehaviorAction
{
    private List<Vector3> pathPoints; // 이동 경로 데이터
    private int currentPointIndex = 0; // 현재 이동 중인 경로 점 인덱스
    private float moveSpeed;

    public ActionMoveAlongPath(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public override BehaviorState Execute()
    {
        if (pathPoints == null || pathPoints.Count == 0)
        {
            pathPoints = actionPhase.GetData("pathPoints") as List<Vector3>;
            if (pathPoints == null || pathPoints.Count == 0)
            {
                Debug.LogWarning("Path points are missing or invalid.");
                return BehaviorState.FAILURE;
            }
        }

        Vector3 targetPosition = pathPoints[currentPointIndex];

        Vector3 direction = (targetPosition - character.transform.position).normalized;
        character.transform.position += direction * moveSpeed * Time.deltaTime;


        if (Vector3.Distance(character.transform.position, targetPosition) < 0.1f)
        {
            currentPointIndex++; // 다음 경로 점으로 이동

            // 마지막 점에 도달했는지 확인
            if (currentPointIndex >= pathPoints.Count)
            {
                Debug.Log("Reached the end of the path.");
                return BehaviorState.SUCCESS;
            }
        }

        return BehaviorState.RUNNING;
    }
}
