using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2Int gridWorldSize;
    [SerializeField] LayerMask unwalkableMask;  // ��ֹ��� Ȯ���� ���̾� ����ũ

    public AstarNode[,] CreateGrid()
    {
        // Grid ũ�� �ʱ�ȭ
        var grid = new AstarNode[gridWorldSize.x * 2 + 1, gridWorldSize.y * 2 + 1];

        for (int x = -gridWorldSize.x; x <= gridWorldSize.x; x++)
        {
            for (int y = -gridWorldSize.y; y <= gridWorldSize.y; y++)
            {
                Vector2Int nodePosition = new Vector2Int(x, y);
                bool isWalkable = IsWalkable(nodePosition);
                grid[x + gridWorldSize.x, y + gridWorldSize.y] = new AstarNode(isWalkable, nodePosition, x + gridWorldSize.x, y + gridWorldSize.y);
            }
        }
        return grid;
    }
    private bool IsWalkable(Vector2Int nodePosition)
    {
        Vector2 worldPosition = new Vector2(nodePosition.x, nodePosition.y); // ����� ���� ��ǥ
        if (Physics2D.Raycast(worldPosition, Vector2.zero, 0f, unwalkableMask))  // Physics2D.Raycast�� 2D ���� ���� ���
        {
            return false;  
        }
        return true;  
    }
}
