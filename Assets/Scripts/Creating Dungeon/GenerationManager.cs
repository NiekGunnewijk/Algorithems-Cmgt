using UnityEngine;
using System;

public class GenerationManager : MonoBehaviour
{
    public static GenerationManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public event Action OnFinishedRoomGeneration;
    public void FinishedRoomGeneration()
    {
        if (OnFinishedRoomGeneration != null)
        {
            OnFinishedRoomGeneration();
        }
    }
    public event Action OnFinishedDoorGeneration;
    public void FinishedDoorGeneration()
    {
        if (OnFinishedDoorGeneration != null)
        {
            OnFinishedDoorGeneration();
        }
    }
}
