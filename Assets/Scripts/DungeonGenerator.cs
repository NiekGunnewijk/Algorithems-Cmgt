using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DungeonGenerator : MonoBehaviour
{
    public List<RectInt> rooms = new List<RectInt>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int dungeonHeight;
    [SerializeField] int dungeonWidth;
    [SerializeField] int maxRoomAmount;
    [SerializeField] int minRoomSize;
    [SerializeField] int RandomSeed;
    [SerializeField] int GenerationDelay;
    
    bool allRoomsGenerated;
    private IEnumerator roomCoroutine;
    private int lastRoomCount;


    void Start()
    {
        roomCoroutine = Generate();
        Random.InitState(RandomSeed);
        rooms.Add(new RectInt(new Vector2Int(0,0), new Vector2Int(dungeonWidth, dungeonHeight)));
        StartCoroutine(roomCoroutine);
        //GenerateDungeon();
        //AlgorithmsUtils.DebugRectInt(dungeon, Color.blue, 100f);
    }

    private void Update()
    {
        DrawRooms();
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
            yield return new WaitForSeconds(GenerationDelay);
        }
    }
    

    void DrawRooms()
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(rooms[i], Color.gray);
        }
    }

}


