using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid grid;
    [SerializeField] List<AstarNode> nodes = new List<AstarNode>();

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        nodes = PathFinding(new Vector2Int(0, 0), new Vector2Int(2, 2), true);
    }

    public List<AstarNode> PathFinding(Vector2Int startPos, Vector2Int targetPos, bool allowDiagonal)
    {
        var nodeArray = grid.CreateGrid();
        var bottomLeft = new Vector2Int(startPos.x - grid.gridWorldSize.x, startPos.y - grid.gridWorldSize.y);
        var topRight = new Vector2Int(startPos.x + grid.gridWorldSize.x, startPos.y + grid.gridWorldSize.y);

        if (IsOutOfBounds(startPos, bottomLeft, topRight) || IsOutOfBounds(targetPos, bottomLeft, topRight))
        {
            Debug.LogWarning("Start or target position is out of the grid bounds.");
            return new List<AstarNode>();
        }

        var startNode = nodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        AstarNode targetNode;
        try
        {
            targetNode = nodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];
        }
        catch (IndexOutOfRangeException)
        {
            return new List<AstarNode>();
        }

        var openList = new SortedSet<AstarNode>(new NodeComparer());
        openList.Add(startNode);
        var closedSet = new HashSet<AstarNode>();
        var finalNodeList = new List<AstarNode>();

        while (openList.Count > 0)
        {
            var curNode = openList.Min;  // fCost가 가장 작은 노드 선택
            openList.Remove(curNode);
            closedSet.Add(curNode);

            if (curNode == targetNode)
            {
                AstarNode targetCurNode = targetNode;
                while (targetCurNode != startNode)
                {
                    finalNodeList.Add(targetCurNode);
                    targetCurNode = targetCurNode.parent;
                }
                finalNodeList.Add(startNode);
                finalNodeList.Reverse();

                return finalNodeList;
            }

            // 대각선 허용시 추가
            if (allowDiagonal)
            {
                AddNeighborNodes(curNode, nodeArray, openList, closedSet, bottomLeft, topRight, targetNode, allowDiagonal);
            }

            // 상하좌우
            AddNeighborNodes(curNode, nodeArray, openList, closedSet, bottomLeft, topRight, targetNode, allowDiagonal);
        }

        Debug.LogWarning("경로를 찾을 수 없습니다.");
        return finalNodeList;
    }

    private bool IsOutOfBounds(Vector2Int position, Vector2Int bottomLeft, Vector2Int topRight)
    {
        return position.x < bottomLeft.x || position.y < bottomLeft.y || position.x >= topRight.x || position.y >= topRight.y;
    }

    private void AddNeighborNodes(AstarNode curNode, AstarNode[,] nodeArray, SortedSet<AstarNode> openList, HashSet<AstarNode> closedSet,
                                  Vector2Int bottomLeft, Vector2Int topRight, AstarNode targetNode, bool allowDiagonal)
    {
        int[] dx = { 1, 0, -1, 0 };  // 우, 상, 좌, 하
        int[] dy = { 0, 1, 0, -1 };
        for (int i = 0; i < 4; i++)
        {
            int checkX = curNode.worldPosition.x + dx[i];
            int checkY = curNode.worldPosition.y + dy[i];

            if (checkX >= bottomLeft.x && checkX < topRight.x && checkY >= bottomLeft.y && checkY < topRight.y)
            {
                AstarNode neighborNode = nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
                if (neighborNode.walkable && !closedSet.Contains(neighborNode))
                {
                    int moveCost = curNode.gCost + ((curNode.worldPosition.x == checkX || curNode.worldPosition.y == checkY) ? 10 : 14);
                    if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode))
                    {
                        neighborNode.gCost = moveCost;
                        neighborNode.hCost = (Mathf.Abs(neighborNode.worldPosition.x - targetNode.worldPosition.x) +
                                               Mathf.Abs(neighborNode.worldPosition.y - targetNode.worldPosition.y)) * 10;
                        neighborNode.parent = curNode;

                        if (!openList.Contains(neighborNode))
                            openList.Add(neighborNode);
                    }
                }
            }
        }
        if (allowDiagonal)
        {
            AddDiagonalNeighbors(curNode, nodeArray, openList, closedSet, bottomLeft, topRight, targetNode);
        }
    }

    private void AddDiagonalNeighbors(AstarNode curNode, AstarNode[,] nodeArray, SortedSet<AstarNode> openList, HashSet<AstarNode> closedSet,
                                       Vector2Int bottomLeft, Vector2Int topRight, AstarNode targetNode)
    {
        int[] dx = { 1, 1, -1, -1 };
        int[] dy = { 1, -1, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int checkX = curNode.worldPosition.x + dx[i];
            int checkY = curNode.worldPosition.y + dy[i];

            if (checkX >= bottomLeft.x && checkX < topRight.x && checkY >= bottomLeft.y && checkY < topRight.y)
            {
                AstarNode neighborNode = nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
                if (neighborNode.walkable && !closedSet.Contains(neighborNode))
                {
                    if (!nodeArray[curNode.worldPosition.x - bottomLeft.x, checkY - bottomLeft.y].walkable ||
                        !nodeArray[checkX - bottomLeft.x, curNode.worldPosition.y - bottomLeft.y].walkable)
                        continue;

                    int moveCost = curNode.gCost + 14;  // 대각선은 14
                    if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode))
                    {
                        neighborNode.gCost = moveCost;
                        neighborNode.hCost = (Mathf.Abs(neighborNode.worldPosition.x - targetNode.worldPosition.x) +
                                               Mathf.Abs(neighborNode.worldPosition.y - targetNode.worldPosition.y)) * 10;
                        neighborNode.parent = curNode;

                        if (!openList.Contains(neighborNode))
                            openList.Add(neighborNode);
                    }
                }
            }
        }
    }
    private class NodeComparer : IComparer<AstarNode>
    {
        public int Compare(AstarNode x, AstarNode y)
        {
            if (x.fCost != y.fCost)
                return x.fCost.CompareTo(y.fCost);
            return x.hCost.CompareTo(y.hCost);
        }
    }
}
