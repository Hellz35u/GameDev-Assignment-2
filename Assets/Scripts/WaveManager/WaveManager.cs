using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private List<GameObject> currentEnemiesInScene;
   
    private List<GameObject> enemiesNeedToSpwan = new List<GameObject>();
    private List<Vector3> spawnPositions = new List<Vector3>();
    private bool waveEnded;
    private WaveTextAnimator waveTextAnimator;
    private Timer timer;

    private void Start()
    {
        PositionToSpawnEnemie[] spawnPositionArray = FindObjectsByType<PositionToSpawnEnemie>(FindObjectsSortMode.None);
        foreach (PositionToSpawnEnemie sp in spawnPositionArray)
        {
            spawnPositions.Add(sp.GetPositon());
        }

    }
    

    private void SpawnEnemy(Vector3 SpawnPosition)
    {
        if (enemiesNeedToSpwan.Count == 0)
            return;

        GameObject prefab = enemiesNeedToSpwan.First<GameObject>();
        GameObject spawnedEnemy = Instantiate(prefab,SpawnPosition, Quaternion.identity);
        enemiesNeedToSpwan.Remove(prefab);
        currentEnemiesInScene.Add(spawnedEnemy);
    }
    private int GetPositionsRandomIndex()
    {
        return UnityEngine.Random.Range(0, spawnPositions.Count);
    }
    private IEnumerator RunWave(float spawnDuration)
    {
        StartWave();
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            int randomIndex = GetPositionsRandomIndex();
            SpawnEnemy(spawnPositions[randomIndex]);
            yield return new WaitForSeconds(spawnDuration);
        }
        if (waves.Count == 0)
            yield break;

        
        
    }
    private void StartWave()
    {
        if(waves.Count != 0)
        {
            waveEnded = false;
            WaveData wave = waves.First<WaveData>();
            waveTextAnimator.DisplayWaveText(wave.GetWaveName());
            timer.StartTimer(wave.GetTimerDuration(), EndWaveByTime);
        }
        return;
    }
    private void EndWaveByTime()
    {
        if (waveEnded)
            return;
        
        waveEnded = true;

        timer.StopTimer();

        foreach (GameObject enemy in currentEnemiesInScene)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        currentEnemiesInScene.Clear();
        waves.RemoveAt(0);
    }
   
}