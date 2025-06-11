using UnityEngine;
using System.Collections.Generic;


public class DrawingManager : MonoBehaviour
{
    private DungeonGenerator roomGenerator;
    private PlaceDoors doorGenerator;
    private PlaceGraph nodeGenerator;

    private List<Vector2> nodeList = new List<Vector2>();

    [SerializeField] bool showRooms = true;
    [SerializeField] bool showDoors = true;
    [SerializeField] bool showNodes = true;
    [SerializeField] bool showEdges = true;

    [SerializeField] int nodeRadius = 1;
    private void Start()
    {
        roomGenerator = GetComponent<DungeonGenerator>();
        doorGenerator = GetComponent<PlaceDoors>();
        nodeGenerator = GetComponent<PlaceGraph>();
    }

    // Update is called once per frame
    void Update()
    {
        nodeList = nodeGenerator.Graph.GetNodes();

        if (showRooms)
            DrawRooms();

        if (showDoors)
            DrawDoors();

        if (showNodes)
            DrawNodes();

        if (showEdges)
            DrawEdges();
    }

    void DrawDoors()
    {
        for (int i = 0; i < doorGenerator.Doors.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doorGenerator.Doors[i], Color.black);
        }
    }

    void DrawRooms()
    {
        for (int i = 0; i < roomGenerator.Rooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(roomGenerator.Rooms[i], Color.gray);
        }
    }
    void DrawNodes()
    {
        foreach (var node in nodeList)
        DebugExtension.DebugCircle(new Vector3(node.x, 0 ,node.y), Vector3.up, Color.green, nodeRadius);
    }

    void DrawEdges()
    {
        foreach(var node in nodeList)
        {
            foreach(var neighbor in nodeGenerator.Graph.GetNeighbors(node))
            {
                Debug.DrawLine(new Vector3(node.x, 0, node.y), new Vector3(neighbor.x, 0, neighbor.y));
            }
        }
    }
}
