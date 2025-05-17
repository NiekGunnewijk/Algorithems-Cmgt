using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DungeonGenerator : MonoBehaviour
{
    List<RectInt> rooms = new List<RectInt>();
    List<RectInt> doors = new List<RectInt>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int dungeonHeight;
    [SerializeField] int dungeonWidth;
    [SerializeField] int maxRoomAmount;
    [SerializeField] int minRoomSize;
    [SerializeField] int doorWidth;
    [SerializeField] int RandomSeed;
    //[SerializeField] int maxRoomsize;
    
    public bool allRoomsGenerated;
    private IEnumerator roomCoroutine;
    private IEnumerator doorCoroutine;
    private int lastRoomCount;


    void Start()
    {
        roomCoroutine = Generate();
        doorCoroutine = PlaceDoors();
        Random.InitState(RandomSeed);
        rooms.Add(new RectInt(new Vector2Int(0,0), new Vector2Int(dungeonWidth, dungeonHeight)));
        StartCoroutine(roomCoroutine);
        //GenerateDungeon();
        //AlgorithmsUtils.DebugRectInt(dungeon, Color.blue, 100f);
    }

    private void Update()
    {
        DrawRooms();
        DrawDoors();
    }
    


    void GenerateDungeon()
    {
        
       for (int i = rooms.Count - 1; i >= 0; i--)
       {
            if (rooms.Count == maxRoomAmount)
            {
                allRoomsGenerated = true;
                print("maxxed");
                return;
            }

            if (rooms[i].width < minRoomSize && rooms[i].height < minRoomSize)
                continue;

            RectInt newRoom = new RectInt(Vector2Int.zero, Vector2Int.zero);
            print("made Room");
            newRoom.x = rooms[i].x;
            newRoom.y = rooms[i].y;

            if (rooms[i].width < rooms[i].height)
            {
                newRoom.width = rooms[i].width;
                newRoom.height = Random.Range(minRoomSize / 2, rooms[i].height - minRoomSize / 2);
                rooms[i] = new RectInt(new Vector2Int(rooms[i].x, rooms[i].y + newRoom.height), new Vector2Int(rooms[i].width, rooms[i].height - newRoom.height));
            }
            else
            {
                newRoom.height = rooms[i].height;
                newRoom.width = Random.Range(minRoomSize / 2, rooms[i].width - minRoomSize / 2);
                rooms[i] = new RectInt(new Vector2Int(rooms[i].x + newRoom.width, rooms[i].y), new Vector2Int(rooms[i].width - newRoom.width, rooms[i].height));
            }

            rooms.Add(newRoom);
       }
       
        print("lastroomcount: " + lastRoomCount);
        print("roomcount " + rooms.Count);

        if (lastRoomCount == rooms.Count)
        {
            allRoomsGenerated = true;
        }
        
        lastRoomCount = rooms.Count;
    }

    IEnumerator Generate()
    {
        while (!allRoomsGenerated)
        {
            GenerateDungeon();
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(doorCoroutine);
    }
    IEnumerator PlaceDoors()
    {
        for (int i = 0; i <= rooms.Count - 1; i++)
        {
            for (int n = 0; n <= rooms.Count - 1; n++)
            {
                if (AlgorithmsUtils.Intersects(rooms[i], rooms[n]))
                {
                    if (rooms[i].y == rooms[n].y + rooms[n].height || rooms[i].x == rooms[n].x + rooms[n].width)
                       continue;

                    if (rooms[i].x + rooms[i].width == rooms[n].x)
                    {
                        if (rooms[i].y + rooms[i].height == rooms[n].y)
                            continue;

                        int yMax = Mathf.Max(rooms[i].y, rooms[n].y);
                        int yMin = Mathf.Min(rooms[i].y + rooms[i].height, rooms[n].y + rooms[n].height);

                        int doorY = Random.Range(yMax + doorWidth / 2, yMin - doorWidth / 2);
                        doors.Add(new RectInt(rooms[n].x, doorY, 0, doorWidth));
                        print(rooms[i]);
                        print(rooms[n]);
                        yield return new WaitForSeconds(1);
                    }
                    else if (rooms[i].y + rooms[i].height == rooms[n].y)
                    {

                        int xMax = Mathf.Max(rooms[i].x, rooms[n].x);
                        int xMin = Mathf.Min(rooms[i].x + rooms[i].width, rooms[n].x + rooms[n].width);

                        int doorX = Random.Range(xMax + doorWidth / 2, xMin - doorWidth / 2);
                        doors.Add(new RectInt(doorX, rooms[n].y, doorWidth, 0));
                        print(rooms[i]);
                        print(rooms[n]);
                        yield return new WaitForSeconds(1);
                    }
                }
            }
            
        }
            
        print(doors.Count);
        print(rooms.Count);
    }
    

    void DrawRooms()
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(rooms[i], new Color(rooms[i].width, rooms[i].width, rooms[i].width));
        }
    }

    void DrawDoors()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doors[i], Color.black);
        }
    }
}


