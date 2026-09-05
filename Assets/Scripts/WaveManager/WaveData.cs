using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WaveData : MonoBehaviour
{
    [SerializeField] private float waveDuration;
    [SerializeField] private int maxEnemiesInScene;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private string waveName;
    [SerializeField] private float spawnDuration;

    public float GetWaveDuration()
    {
        return waveDuration;
    }
    public int GetMaxEnemiesInScene()
    {
        return maxEnemiesInScene;
    }
    public List<GameObject> GetEnemiesList()
    {
        return enemies;
    }
    public string GetWaveName()
    {
        return waveName;
    }
    public float GetSpawnDuration()
    {
        return spawnDuration;
    }
}
