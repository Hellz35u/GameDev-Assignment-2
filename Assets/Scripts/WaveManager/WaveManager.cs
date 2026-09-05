using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private WaveTextAnimator waveTextAnimator;
    [SerializeField] private Timer timer;
    private HashSet<GameObject> currentEnemiesInScene = new HashSet<GameObject>();
    private List<GameObject> enemiesNeedToSpwan = new List<GameObject>();
    [SerializeField] private PositionToSpawnEnemie spawnPositions;
    private bool waveEnded;
    private bool finishedSpawning;
    private Coroutine currentWaveCoroutine;

    private void Start()
    {
        if (spawnPositions == null)
        {
            Debug.LogError("Spawn position is not assigned!");
            return;
        }

        if (waves.Count == 0)
        {
            Debug.LogError("No waves configured!");
            return;
        }

        currentWaveCoroutine =
            StartCoroutine(RunWave(waves.First().GetSpawnDuration()));
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
    private IEnumerator RunWave(float spawnDuration)
    {
        StartWave();

        int enemiesToSpawnCount = enemiesNeedToSpwan.Count;

        for (int i = 0; i < enemiesToSpawnCount; i++)
        {
            if (waveEnded)
                yield break;

            Vector3 randomPosition = spawnPositions.GetRandomPosition();

            Debug.Log("Spawning enemy at: " + randomPosition);

            SpawnEnemy(randomPosition);

            yield return new WaitForSeconds(spawnDuration);
        }

        finishedSpawning = true;

        CheckIfAllEnemiesKilled();
    }
    private void StartWave()
    {
        if (waves.Count == 0)
            return;
        
        waveEnded = false;
        finishedSpawning = false;

        WaveData wave = waves.First<WaveData>();
        
        enemiesNeedToSpwan.Clear();
        enemiesNeedToSpwan.AddRange(wave.GetEnemiesList());
        waveTextAnimator.DisplayWaveText(wave.GetWaveName());
        timer.StartTimer(wave.GetWaveDuration(), EndWaveByTime);
        
        
    }
    private void EndWaveByTime()
    {
        if (waveEnded)
            return;
        
        waveEnded = true;
        
        timer.StopTimer();

        if (currentWaveCoroutine != null)
        {
            StopCoroutine(currentWaveCoroutine);
            currentWaveCoroutine = null;
        }

        foreach (GameObject enemy in currentEnemiesInScene)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        currentEnemiesInScene.Clear();
        if(waves.Count > 0)
        {
            waves.RemoveAt(0);
        }
        if (waves.Count > 0)
        {
            currentWaveCoroutine = StartCoroutine(RunWave(waves.First().GetSpawnDuration()));
        }
        else
        {
            Debug.Log("All waves are finished");
        }

    }
    private void EndWaveByEnemiesKilled()
    {
        if (waveEnded)
            return;

        waveEnded = true;

        timer.StopTimer();
        currentEnemiesInScene.Clear();

        if(waves.Count > 0)
        {
            waves.RemoveAt(0);
        }
        if(waves.Count > 0)
        {
            currentWaveCoroutine = StartCoroutine(RunWave(waves.First().GetSpawnDuration()));
        }
        else
        {
            Debug.Log("All waves are finished");
        }
    }
    public void CharacterDeathListiner(GameObject other)
    {
        if (currentEnemiesInScene.Contains(other))
        {
            currentEnemiesInScene.Remove(other);
        }

        CheckIfAllEnemiesKilled();
    }
    private void CheckIfAllEnemiesKilled()
    {
        if(finishedSpawning && currentEnemiesInScene.Count == 0)
        {
            EndWaveByEnemiesKilled();
        }
    }
   
}