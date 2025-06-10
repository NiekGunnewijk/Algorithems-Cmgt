using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PlaceGraph : MonoBehaviour
{
    private DungeonGenerator roomGenerator;
    private PlaceDoors doorGenerator;

    public Graph<Vector2> Graph = new Graph<Vector2>();         
    private List<RectInt> rooms = new List<RectInt>();
    private List<RectInt> doors = new List<RectInt>();
    
    [SerializeField] int GenerationDelay;

    private IEnumerator nodeCoroutine;

    private void OnEnable()
    {
        GenerationManager.Instance.OnFinishedDoorGeneration += StartPlacing;
    }
    private void OnDisable()
    {
        GenerationManager.Instance.OnFinishedDoorGeneration -= StartPlacing;
    }

    

    private void StartPlacing()
    {
        roomGenerator = GetComponent<DungeonGenerator>();
        doorGenerator = GetComponent<PlaceDoors>();

        rooms = roomGenerator.Rooms;
        doors = doorGenerator.Doors;

        nodeCoroutine = PlaceNodes();
        StartCoroutine(nodeCoroutine);
    }


    private IEnumerator PlaceNodes() 
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            Vector2 roomNode = new Vector2((rooms[i].x + rooms[i].width / 2), (rooms[i].y + rooms[i].height / 2));
            Graph.AddNode(roomNode);
            yield return new WaitForSeconds(GenerationDelay);

            for (int n = 0; n < doors.Count; n++)
            {
                if (AlgorithmsUtils.Intersects(rooms[i], doors[n]))
                {
                    if (doors[n].y < rooms[i].y || doors[n].y > rooms[i].y + rooms[i].height)
                        continue;
                    if (doors[n].x < rooms[i].x || doors[n].x > rooms[i].x + rooms[i].width)
                        continue;

                    Vector2 doorNode = new Vector2((doors[n].x + doors[n].width / 2), (doors[n].y + doors[n].height / 2));
                    Graph.AddNode(doorNode);
                    Graph.AddEdge(roomNode, doorNode);
                } 
            }

        }
        
    }


}
