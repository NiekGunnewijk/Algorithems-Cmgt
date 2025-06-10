using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class BuildDungeon : MonoBehaviour
{
    private DungeonGenerator roomGenerator;
    private PlaceDoors doorGenerator;

    private List<RectInt> rooms = new List<RectInt>();
    private List<RectInt> doors = new List<RectInt>();
    private List<GameObject> walls = new List<GameObject>();

    private IEnumerator constructCoroutine;

    [SerializeField] GameObject Wall;
    [SerializeField] GameObject WreckingBall;
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
        constructCoroutine = ConstructDungeon();
        StartCoroutine(constructCoroutine);
    }


    IEnumerator ConstructDungeon()
    {
        // make outer 2 walls
        MakeBackWalls();

        yield return new WaitForSeconds(GenerationDelay);


        // make walls
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int n = 0; rooms[i].width > n; n++)
            {
                walls.Add(Instantiate(Wall, new Vector3(rooms[i].x + n, 0, rooms[i].y), Quaternion.identity));
            }

            for (int n = 1; rooms[i].height > n; n++)
            {
                walls.Add(Instantiate(Wall, new Vector3(rooms[i].x, 0, rooms[i].y + n), new Quaternion()));
            }
            yield return new WaitForSeconds(GenerationDelay);
        }

        // remove the doors



        /*
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i].width < doors[i].height)
            {
                for (int n = 1; doors[i].height >= n; n++)
                {
                    Instantiate(WreckingBall, new Vector3(doors[i].x, 10, doors[i].y + n), new Quaternion());
                }
            }
            else
            {
                for (int n = 1; doors[i].width >= n; n++)
                {
                    walls.Add(Instantiate(WreckingBall, new Vector3(doors[i].x + n, 0, doors[i].y), new Quaternion()));
                }
            }
        }
        */

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

}
