using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PlaceDoors : MonoBehaviour
{
    private List<RectInt> rooms = new List<RectInt>();
    public List<RectInt> Doors = new List<RectInt>();

    [SerializeField] DungeonGenerator generator;
    [SerializeField] int doorWidth;
    [SerializeField] int GenerationDelay;


    private IEnumerator doorCoroutine;

    private void OnEnable()
    {
        GenerationManager.Instance.OnFinishedRoomGeneration += StartGenerating;
    }
    private void OnDisable()
    {
        GenerationManager.Instance.OnFinishedRoomGeneration -= StartGenerating;
    }

    void Start()
    {
        doorCoroutine = CreateDoors();
    }


    void StartGenerating()
    {
        rooms = generator.Rooms;
        StartCoroutine(doorCoroutine);
    }
    

    IEnumerator CreateDoors()
    {
        for (int i = 0; i <= rooms.Count - 1; i++)
        {
            for (int n = 0; n <= rooms.Count - 1; n++)
            {
                if (AlgorithmsUtils.Intersects(rooms[i], rooms[n]))
                {
                    // stop corner collision
                    if (rooms[i].y == rooms[n].y + rooms[n].height || rooms[i].x == rooms[n].x + rooms[n].width)
                        continue;

                    if (rooms[i].x + rooms[i].width == rooms[n].x)
                    {
                        //corner collision
                        if (rooms[i].y + rooms[i].height == rooms[n].y)
                            continue;

                        int yMax = Mathf.Max(rooms[i].y, rooms[n].y);
                        int yMin = Mathf.Min(rooms[i].y + rooms[i].height, rooms[n].y + rooms[n].height);

                        int doorY;
                        if ((yMin - yMax) < doorWidth)
                            continue;
                        if (yMin - yMax == doorWidth)
                            doorY = yMax;    
                        else
                        doorY = UnityEngine.Random.Range(yMax + doorWidth / 2, yMin - doorWidth / 2);

                        Doors.Add(new RectInt(rooms[n].x, doorY, 0, doorWidth));
                        
                        yield return new WaitForSeconds(GenerationDelay);
                    }
                    else if (rooms[i].y + rooms[i].height == rooms[n].y)        
                    {

                        int xMax = Mathf.Max(rooms[i].x, rooms[n].x);
                        int xMin = Mathf.Min(rooms[i].x + rooms[i].width, rooms[n].x + rooms[n].width);
                        int doorX;

                        if ((xMin - xMax) < doorWidth)
                            continue;
                        if (xMin - xMax == doorWidth)
                            doorX = xMax;
                        else
                        doorX = UnityEngine.Random.Range(xMax + doorWidth / 2, xMin - doorWidth / 2);
                        
                        Doors.Add(new RectInt(doorX, rooms[n].y, doorWidth, 0));
                        
                        yield return new WaitForSeconds(GenerationDelay);
                    }
                }
            }
        }

        GenerationManager.Instance.FinishedDoorGeneration();
    }

    

}
