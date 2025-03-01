using UnityEngine;
using System.Collections.Generic;
public class DungeonGenerator : MonoBehaviour
{
    List<RectInt> rooms = new List<RectInt>();

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int dungeonHeight;
    [SerializeField] int dungeonWidth;
    [SerializeField] int maxRoomAmount;
    [SerializeField] int minRoomSize;
    //[SerializeField] int maxRoomsize;
    bool allRoomsGenerated;

    void Start()
    {
        GenerateDungeon();
        //AlgorithmsUtils.DebugRectInt(dungeon, Color.blue, 100f);
    }

    private void Update()
    {
        DrawRooms();
    }


    void GenerateDungeon()
    {
        //outline
        rooms.Add(new RectInt(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonHeight)));

        while (!allRoomsGenerated)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                
                int xOffset = 0;
                int yOffset = 0;

                RectInt room = rooms[i];
                Debug.Log("current " + room.ToString());

                if (room.height <= room.width && room.width > minRoomSize * 2 + 1)
                {
                    xOffset = Random.Range(minRoomSize, room.width - minRoomSize);

                    room.width -= xOffset;

                    rooms.Add(new RectInt(new Vector2Int((room.x + room.width) - 1, room.y), new Vector2Int(xOffset + 1, room.height)));
                }
                else if (room.height > minRoomSize * 2 + 1)
                {
                    yOffset = Random.Range(minRoomSize, room.height - minRoomSize);

                    room.height -= yOffset;
                    rooms.Add(new RectInt(new Vector2Int(room.x, (room.y + room.height) - 1), new Vector2Int(room.width, yOffset + 1)));
                }
                //if (rooms.Count >= maxRoomAmount)
                //{
                //    break;
                //}
            }

            allRoomsGenerated = true;

            //if (rooms.Count >= maxRoomAmount)
            //{
            //    break;
            //}

            for (int j = 0; j < rooms.Count; j++)
            {
                if (rooms[j].height > minRoomSize * 2 + 1 || rooms[j].width > minRoomSize * 2 + 1)
                {
                    allRoomsGenerated = false;
                    break;
                }
            }


        }
        allRoomsGenerated = true;
        return;
    }


    void DrawRooms()
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(rooms[i], Color.blue);
        }
    }
}


