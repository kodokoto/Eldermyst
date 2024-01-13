using System.Collections.Generic;
using UnityEngine;

public enum PathfindingMode
{
    Walking,
    Flying
}
public class PathfinderNode
{
    public Vector3 worldPosition;
    public bool isFlyable;
    public bool isWalkable;
    public Vector2Int gridPosition;
    public int gCost;
    public int hCost;
    public int FCost
    {
        get { return gCost + hCost; }
    }

    public PathfinderNode? parent;

    public PathfinderNode(Vector2Int gridPosition, Vector3 worldPosition, bool isWalkable, bool isFlyable, PathfinderNode? parent = null)
    {
        // // create a gameobject at the position of the node
        // GameObject nodeObject = new GameObject("Node");
        // // set the parent of the node to the grid
        // nodeObject.transform.parent = GameObject.Find("caves").transform;
        // // set the position of the node
        // nodeObject.transform.position = position;
        this.gridPosition = gridPosition;
        this.worldPosition = worldPosition;
        this.isFlyable = isFlyable;
        this.isWalkable = isWalkable;
        this.parent = parent;
    }
}

public class PathfinderGrid : MonoBehaviour
{
    // grid size
    [SerializeField] private int gridSizeX = 200;
    [SerializeField] private int gridSizeY = 150;
    private float nodeRadius = 1.2f;
    [SerializeField] private LayerMask SolidLayer;


    // grid offset transform matrix
    // - negate the y value and shift the grid up by 1

    [SerializeField] private Vector3 gridOffset = new Vector3(-11, 13, 0);
    
    public PathfinderNode[,] nodes;

    // Start is called before the first frame update
    void Start()
    {
        SolidLayer = LayerMask.GetMask("Ground", "StickyWall");
        // create the grid
        CreateGrid();
    }

    void CreateGrid()
    {
        // initialize the nodes array
        nodes = new PathfinderNode[gridSizeX, gridSizeY];

        // loop through the grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // calculate the position of the node
                Vector3 nodePosition = new Vector3(x * nodeRadius, y * nodeRadius * -1, 0) + gridOffset;
                // check if the node is walkable
                bool isFlyable = IsNodeFlyable(nodePosition + new Vector3(0, 0, 0));
                // bool isWalkable = isFlyable && IsNodeWalkable(nodePosition);
                // create the node
                nodes[x, y] = new PathfinderNode(new Vector2Int(x, y), nodePosition, false, isFlyable);
            }
        }
    
        Debug.Log("Size of nodes array: " + nodes.Length);

        // this is ugly but time is forcing my hand
        for (int x = 1; x < gridSizeX -1; x++)
        {
            for (int y = 1; y < gridSizeY-1; y++)
            {
                CheckIfWalkable(nodes[x, y]);
            }
        }
    }

    // This method naively assumes that there will be no index out of bounds errors,
    // ugly but it is an evil that must be dealth with for now
    private void CheckIfWalkable(PathfinderNode node)
    {
        PathfinderNode nodeBellow = nodes[node.gridPosition.x, node.gridPosition.y + 1];
        PathfinderNode nodeLeft = nodes[node.gridPosition.x - 1, node.gridPosition.y];
        PathfinderNode nodeRight = nodes[node.gridPosition.x + 1, node.gridPosition.y];
        if (node.isFlyable && (!nodeBellow.isFlyable | (!nodeLeft.isFlyable ^ !nodeRight.isFlyable)))
        {
            node.isWalkable = true;
        }
    }

    private bool IsNodeFlyable(Vector3 nodePosition)
    {
        // get the position of the node in world space
        // Vector3 worldPosition = transform.TransformPoint(nodePosition);
        // overlap sphere at the position of the node
        
        Collider[] colliders = Physics.OverlapSphere(nodePosition, .9f, SolidLayer);

        if (colliders.Length > 0)
        {
            // if the node is not walkable, return false
            return false;
        }
        // if the raycast does not hit anything, return true
        return true;
    }

    private bool IsNodeWalkable(Vector3 nodePosition)
    {
        Collider[] c = Physics.OverlapBox(new Vector3(nodePosition.x + (nodeRadius*2), nodePosition.y), new Vector3(0.1f, nodeRadius, 0));
        if (c.Length > 0)
        {
            return true;
        }
        // bool isTouchingGround = Physics.Raycast(nodePosition, Vector2.down, 0.00001f, SolidLayer);
        // bool isTouchingWall = Physics.Raycast(nodePosition, Vector2.left, nodeRadius, SolidLayer) || Physics.Raycast(nodePosition, Vector2.right, nodeRadius, SolidLayer);

        // if (isTouchingGround || isTouchingWall)
        // {
        //     return true;
        // }
        return false;
    }

    // OnDrawGizmos
    //     - draw the grid  
    void OnDrawGizmos()
    {
        if (nodes == null) return;

        foreach (PathfinderNode node in nodes)
        {
            
            // Gizmos.DrawSphere(node.worldPosition, 0.1f);

            if (node.isWalkable)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(node.worldPosition, 0.1f);
            }
            else if (node.isFlyable)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(node.worldPosition, 0.1f);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(node.worldPosition, 0.1f);
            }
        }
    }

    private int GetDistance(PathfinderNode nodeA, PathfinderNode nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int distanceY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        int remaining = Mathf.Abs(distanceX - distanceY);

        return Mathf.Min(distanceX, distanceY) * 14 + remaining * 10;
    }

    private List<Vector3> RetracePath(PathfinderNode startNode, PathfinderNode endNode)
    {
        List<Vector3> path = new List<Vector3>();
        PathfinderNode currentNode = endNode;

        while (currentNode.gridPosition != startNode.gridPosition)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    private PathfinderNode GetNearestNode(Vector3 position)
    {
        // convert the position to node indices
        Vector2Int nodeIndices = PositionToNodeIndex(position);
        // return the node at the node indices
        return nodes[nodeIndices.x, nodeIndices.y];
    }

    private Vector3 NodeIndexToPosition(int x, int y)
    {
        return new Vector3(x * nodeRadius, y * nodeRadius * -1) + gridOffset;
    }

    // inverse of NodeIndexToPosition
    private Vector2Int PositionToNodeIndex(Vector3 position)
    {
        // subtract the grid offset
        position -= gridOffset;

        // divide by the node radius
        position /= nodeRadius;

        // round the x and y values
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y) * -1;


        // ensure the x and y values are within the grid incase of rounding errors
        if (x == gridSizeX)
            x--;
        else if (x > gridSizeX)
            Debug.LogError("Position.x value is greater than grid size");
        if (y == gridSizeY)
            y--;
        else if (y > gridSizeY)
            Debug.LogError("Position.y value is greater than grid size");

        return new Vector2Int(x, y);
    }
    private List<PathfinderNode> GetNeighboringNodes(PathfinderNode node)
    {
        // create a list of neighboring nodes
        List<PathfinderNode> neighboringNodes = new List<PathfinderNode>();
        // get the x and y index of the node
        int xIndex = node.gridPosition.x;
        int yIndex = node.gridPosition.y;
        // loop through the neighboring nodes
        for (int x = xIndex - 1; x <= xIndex + 1; x++)
        {
            for (int y = yIndex - 1; y <= yIndex + 1; y++)
            {
                // check if the node is within the grid
                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
                {
                    // check if the node is not the current node
                    if (x != xIndex || y != yIndex)
                    {
                        // add the node to the list
                        neighboringNodes.Add(nodes[x, y]);
                    }
                }
            }
        }
        // return the list of neighboring nodes
        return neighboringNodes;
    }

    // A* Pathfinding

    public void ResetGrid()
    {
        foreach (PathfinderNode node in nodes)
        {
            node.gCost = int.MaxValue;
            node.parent = null;
        }
    }

    public PathfinderNode GetLowestFCostNode(List<PathfinderNode> nodes)
    {
        PathfinderNode lowestFCostNode = nodes[0];

        foreach (PathfinderNode node in nodes)
        {
            if (node.FCost < lowestFCostNode.FCost)
            {
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode;
    }

    public Vector3 GetRandomWalkablePosition()
    {
        PathfinderNode node = nodes[Random.Range(0, gridSizeX), Random.Range(0, gridSizeY)];
        while (!node.isWalkable)
        {
            node = nodes[Random.Range(0, gridSizeX), Random.Range(0, gridSizeY)];
        }
        return node.worldPosition;
    }
    
    public List<Vector3> GetPath(Vector3 startPosition, Vector3 endPosition, PathfindingMode mode = PathfindingMode.Flying)
    {
        PathfinderNode startNode = GetNearestNode(startPosition);
        PathfinderNode endNode = GetNearestNode(endPosition);

        List<PathfinderNode> openNodes = new List<PathfinderNode>();
        HashSet<PathfinderNode> closedNodes = new HashSet<PathfinderNode>();

        openNodes.Add(startNode);

        ResetGrid();

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, endNode);

        while (openNodes.Count > 0)
        {
            PathfinderNode currentNode = GetLowestFCostNode(openNodes);

            if (currentNode.gridPosition == endNode.gridPosition)
            {
                var res = RetracePath(startNode, endNode);
                res.Add(endNode.worldPosition);
                return res;
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            foreach (PathfinderNode neighbor in GetNeighboringNodes(currentNode))
            {
                if (mode == PathfindingMode.Flying && !neighbor.isFlyable) continue;
                if (mode == PathfindingMode.Walking && !neighbor.isWalkable) continue;
                if (closedNodes.Contains(neighbor)) continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                if (newMovementCostToNeighbor < neighbor.gCost)
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openNodes.Contains(neighbor))
                    {
                        openNodes.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }
}