using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class BuildDungeon : MonoBehaviour
{
    private DungeonGenerator roomGenerator;
    private PlaceDoors doorGenerator;

    private List<RectInt> rooms = new List<RectInt>();
    private List<RectInt> doors = new List<RectInt>();

    private IEnumerator constructCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomGenerator = GetComponent<DungeonGenerator>();
        doorGenerator = GetComponent<PlaceDoors>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
