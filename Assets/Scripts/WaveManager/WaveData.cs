using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public List<GameObject> enemies;
    public float spawnInterval = 2f;
    public float waveDuration = 60f;
}