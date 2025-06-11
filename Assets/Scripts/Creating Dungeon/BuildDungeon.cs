using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.AI.Navigation;
public class BuildDungeon : MonoBehaviour
{
    private DungeonGenerator roomGenerator;
    private PlaceDoors doorGenerator;

    private List<RectInt> rooms = new List<RectInt>();
    private List<RectInt> doors = new List<RectInt>();
    private List<GameObject> walls = new List<GameObject>();

    private IEnumerator constructCoroutine;

    [SerializeField] GameObject Floor;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] GameObject Wall;
    [SerializeField] int GenerationDelay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomGenerator = GetComponent<DungeonGenerator>();
        doorGenerator = GetComponent<PlaceDoors>();
    }
    private void OnEnable()
    {
        GenerationManager.Instance.OnFinishedDoorGeneration += StartConstructing;
    }

    private void OnDisable()
    {
        GenerationManager.Instance.OnFinishedDoorGeneration -= StartConstructing;
    }


    private void StartConstructing()
    {
        rooms = roomGenerator.Rooms;
        doors = doorGenerator.Doors;
        
        navMeshSurface.transform.localScale = new Vector3(roomGenerator.dungeonWidth, 1, roomGenerator.dungeonHeight);
        
        constructCoroutine = ConstructDungeon();
        StartCoroutine(constructCoroutine);
    }


    IEnumerator ConstructDungeon()
    {
        SetFloor();
        yield return new WaitForSeconds(GenerationDelay);

        MakeBackWalls();
        yield return new WaitForSeconds(GenerationDelay);

        // make walls
        for (int i = 0; i < rooms.Count; i++)
        {
            //check for colliding doors to limit the checks, make a list of em and check when placing

            List<Vector2> doorpoints = new List<Vector2>();


            foreach (var door in doors)
            {
                if (AlgorithmsUtils.Intersects(rooms[i], door))
                {
                    if (door.width > door.height)
                    {
                        for (int n = 0; n < door.width; n++)
                        {
                            doorpoints.Add(new Vector2(door.x + n, door.y));
                        }
                    }
                    else
                    {
                        for (int n = 0; n < door.height; n++)
                        {
                            doorpoints.Add(new Vector2(door.x, door.y + n));
                        }
                    }
                }
            }
            // Place Walls on the left and bottom of the room
            for (int n = 0; rooms[i].width > n; n++)
            {
                if (!doorpoints.Contains(new Vector2(rooms[i].x + n, rooms[i].y)))
                    walls.Add(Instantiate(Wall, new Vector3(rooms[i].x + n, 0, rooms[i].y), Quaternion.identity));
            }

            for (int n = 1; rooms[i].height > n; n++)
            {
                if (!doorpoints.Contains(new Vector2(rooms[i].x, rooms[i].y + n)))
                    walls.Add(Instantiate(Wall, new Vector3(rooms[i].x, 0, rooms[i].y + n), new Quaternion()));
            }
            yield return new WaitForSeconds(GenerationDelay);
        }


        BakeNavMesh();
    }


    private void MakeBackWalls()
    {
        int width = roomGenerator.dungeonWidth;
        int height = roomGenerator.dungeonHeight;

        for (int i = 0; width + 1 > i; i++)
        {
            Instantiate(Wall, new Vector3(i, 0, height), new Quaternion());
        }

        for (int i = 0; height > i; i++)
        {
            Instantiate(Wall, new Vector3(width, 0, i), new Quaternion());
        }
    }

    private void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
    private void SetFloor()
    {
        Floor.transform.localScale = new Vector3(roomGenerator.dungeonWidth, roomGenerator.dungeonHeight, 1);
        Floor.transform.localPosition = new Vector3(roomGenerator.dungeonWidth / 2, 0, roomGenerator.dungeonHeight / 2);
    }

}
