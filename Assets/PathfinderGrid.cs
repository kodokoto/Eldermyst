using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct PathfinderNode
{
    public Vector3 position;
    public bool isWalkable;
    public int gCost;
    public int hCost;
    public int fCost;

    public PathfinderNode(Vector3 position, bool isWalkable)
    {
        // // create a gameobject at the position of the node
        // GameObject nodeObject = new GameObject("Node");
        // // set the parent of the node to the grid
        // nodeObject.transform.parent = GameObject.Find("caves").transform;
        // // set the position of the node
        // nodeObject.transform.position = position;

        this.position = position;
        this.isWalkable = isWalkable;
        gCost = 0;
        hCost = 0;
        fCost = 0;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    // dispay single dot for gizmo
    public void DisplayGizmo()
    {
        // red if not walkable
        if (!isWalkable)
        {
            Gizmos.color = Color.red;
        }
        // green if walkable
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawSphere(position, 0.1f);
    }
}

[InitializeOnLoad]
public class PathfinderGrid : MonoBehaviour
{
    private List<PathfinderNode> nodes;
    private Mesh mesh;

    void Awake()
    {
        nodes = new List<PathfinderNode>();
        mesh = GetComponent<MeshFilter>().mesh;
        GenerateGrid();
        Debug.Log("PathfinderGrid awake");
        Debug.Log("Number of nodes: " + nodes.Count);
    }

    static PathfinderGrid()
    {
        Debug.Log("PathfinderGrid loaded");
    }

    private void GenerateGrid()
    {
        // get size of grid based on mesh bounds
        Vector3 size = mesh.bounds.size;
        Debug.Log("Grid size: " + size);
        // get center of grid based on mesh bounds
        Vector3 center = mesh.bounds.center;

        // get the number of nodes in each direction
        int xNodes = Mathf.RoundToInt(size.x);
        int yNodes = Mathf.RoundToInt(size.y);

        Debug.Log("Number of nodes in each direction: " + xNodes + " " + yNodes);
        

        for (int x = 0; x < xNodes; x++)
        {
            for (int y = 0; y < yNodes; y++)
            {
                // get the position of the node
                Vector3 nodePosition = new Vector3(x, y, 0) + center;
                // check if the node is walkable
                bool isWalkable = IsNodeWalkable(nodePosition);
                // create the node
                PathfinderNode node = new PathfinderNode(nodePosition, isWalkable);
                // add the node to the list
                nodes.Add(node);
            }
        }
    }

    private bool IsNodeWalkable(Vector3 nodePosition)
    {
        // get the position of the node in world space
        Vector3 worldPosition = transform.TransformPoint(nodePosition);
        // overlap sphere at the position of the node
        
        Collider[] colliders = Physics.OverlapSphere(worldPosition, 0.1f, LayerMask.GetMask("Ground", "StickyWall"));

        if (colliders.Length > 0)
        {
            // if the node is not walkable, return false
            return false;
        }
        // if the raycast does not hit anything, return true
        return true;
    }

    // OnDrawGizmos
    //     - draw the grid  
    void OnDrawGizmos()
    {
        if (nodes != null)
        {
            foreach (PathfinderNode node in nodes)
            {
                node.DisplayGizmo();
            }
        }
    }

}


