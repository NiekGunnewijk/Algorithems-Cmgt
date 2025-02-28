using UnityEngine;
using System.Collections.Generic;
public class DungeonGenerator : MonoBehaviour
{
    List<RectInt> rooms = new List<RectInt>();

    RectInt rectInt = new RectInt(new Vector2Int(0, 0), new Vector2Int(100, 50));
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int dungeonHeight;
    [SerializeField] int dungeonWidth;
    [SerializeField] int maxRoomAmount;
    [SerializeField] int minRoomSize;
    [SerializeField] int maxRoomsize;


    void Start()
    {
        AlgorithmsUtils.DebugRectInt(rectInt, Color.blue, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
