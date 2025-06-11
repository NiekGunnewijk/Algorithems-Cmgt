using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DungeonGenerator : MonoBehaviour
{
    public List<RectInt> Rooms = new List<RectInt>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int dungeonHeight;
    public int dungeonWidth;
    [SerializeField] int maxRoomAmount;
    [SerializeField] int minRoomSize;       // Todo <--------------------------------
    [SerializeField] int RandomSeed;
    [SerializeField] int GenerationDelay;
    
    bool allRoomsGenerated;
    private int lastRoomCount;

    private IEnumerator roomCoroutine;

    void Start()
    {
        roomCoroutine = Generate();
        Random.InitState(RandomSeed);
        Rooms.Add(new RectInt(new Vector2Int(0,0), new Vector2Int(dungeonWidth, dungeonHeight)));
        StartCoroutine(roomCoroutine);
    }


    void GenerateDungeon()
    {
       for (int i = Rooms.Count - 1; i >= 0; i--)
       {
            if (Rooms.Count == maxRoomAmount)
            {
                allRoomsGenerated = true;
                return;
            }


            if (Rooms[i].width < minRoomSize * 2 || Rooms[i].height < minRoomSize * 2)
                continue;


            RectInt newRoom = new RectInt(Vector2Int.zero, Vector2Int.zero);
            newRoom.x = Rooms[i].x;
            newRoom.y = Rooms[i].y;

            if (Rooms[i].width < Rooms[i].height)
            {
                newRoom.width = Rooms[i].width;
                newRoom.height = Random.Range(minRoomSize, Rooms[i].height - minRoomSize);

                Rooms[i] = new RectInt(new Vector2Int(Rooms[i].x, Rooms[i].y + newRoom.height), new Vector2Int(Rooms[i].width, Rooms[i].height - newRoom.height));
            }
            else
            {
                newRoom.height = Rooms[i].height;
                newRoom.width = Random.Range(minRoomSize, Rooms[i].width - minRoomSize);

                Rooms[i] = new RectInt(new Vector2Int(Rooms[i].x + newRoom.width, Rooms[i].y), new Vector2Int(Rooms[i].width - newRoom.width, Rooms[i].height));
            }

            Rooms.Add(newRoom);
       }

       //Check if there was a change
        if (lastRoomCount == Rooms.Count)
        {
            allRoomsGenerated = true;
        }
        
        lastRoomCount = Rooms.Count;
    }

    IEnumerator Generate()
    {
        while (!allRoomsGenerated)
        {
            GenerateDungeon();
            yield return new WaitForSeconds(GenerationDelay);
        }
        GenerationManager.Instance.FinishedRoomGeneration();
    }
    



}


