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
    [SerializeField] private PositionToSpawnEnemie spawnPositions;

    private HashSet<GameObject> currentEnemiesInScene = new HashSet<GameObject>();
    private List<GameObject> enemiesNeedToSpwan = new List<GameObject>();
    
    private bool waveEnded;
    private bool finishedSpawning;
    private Coroutine currentWaveCoroutine;

    private void OnEnable()
    {
        GameEvents.CharacterDeath += CharacterDeathListiner;
    }

    private void OnDisable()
    {
        GameEvents.CharacterDeath -= CharacterDeathListiner;
    }

    private void Start()
    {
        if (spawnPositions == null)
        {
            Debug.LogError("WaveManager: Spawn position is not assigned!");
            return;
        }
        if (waves == null || waves.Count == 0)
        {
            Debug.LogError("WaveManager: No waves configured!");
            return;
        }
        if (timer == null)
        {
            Debug.LogError("WaveManager: Timer is not assigned!");
            return;
        }
        if (waveTextAnimator == null)
        {
            Debug.LogError("WaveManager: WaveTextAnimator is not assigned!");
            return;
        }
        if (waves.First() == null)
        {
            Debug.LogError("WaveManager: First wave is null!");
            return;
        }

        currentWaveCoroutine = StartCoroutine(RunWave(waves.First().GetSpawnDuration()));
    }

    private void SpawnEnemy(Vector3 SpawnPosition)
    {
        if (enemiesNeedToSpwan == null || enemiesNeedToSpwan.Count == 0)
        {
            Debug.LogWarning("WaveManager: No enemies left to spawn.");
            return;
        }

        GameObject prefab = enemiesNeedToSpwan.First<GameObject>();

        if (prefab == null)
        {
            Debug.LogWarning("WaveManager: Enemy prefab is null.");
            enemiesNeedToSpwan.RemoveAt(0);
            return;
        }

        GameObject spawnedEnemy = Instantiate(prefab, SpawnPosition, Quaternion.identity);

        if (spawnedEnemy == null)
        {
            Debug.LogError("WaveManager: Failed to instantiate enemy.");
            return;
        }

        enemiesNeedToSpwan.Remove(prefab);
        currentEnemiesInScene.Add(spawnedEnemy);
    }

    private IEnumerator RunWave(float spawnDuration)
    {
        if (spawnDuration < 0)
        {
            Debug.LogError("WaveManager: Spawn duration cannot be negative.");
            yield break;
        }

        if (spawnPositions == null)
        {
            Debug.LogError("WaveManager: Spawn position is missing.");
            yield break;
        }

        StartWave();

        if (waveEnded)
        {
            Debug.LogError("test 1");
            yield break;
        }

        int enemiesToSpawnCount = enemiesNeedToSpwan.Count;

        for (int i = 0; i < enemiesToSpawnCount; i++)
        {
            if (waveEnded)
            {
                Debug.LogError("test 2");
                yield break;
            }

            Vector3 randomPosition = spawnPositions.GetRandomPosition();

            SpawnEnemy(randomPosition);

            yield return new WaitForSeconds(spawnDuration);
        }

        finishedSpawning = true;

        CheckIfAllEnemiesKilled();
    }

    private void StartWave()
    {
        if (waves == null || waves.Count == 0)
        {
            Debug.LogWarning("WaveManager: Cannot start wave because waves list is empty.");
            return;
        }

        waveEnded = false;
        finishedSpawning = false;

        WaveData wave = waves.First<WaveData>();

        if (wave == null)
        {
            Debug.LogError("WaveManager: Current wave is null.");
            waveEnded = true;
            return;
        }

        if (wave.GetEnemiesList() == null)
        {
            Debug.LogError("WaveManager: Enemy list in current wave is null.");
            waveEnded = true;
            return;
        }

        if (wave.GetWaveDuration() < 0)
        {
            Debug.LogError("WaveManager: Wave duration cannot be negative.");
            waveEnded = true;
            return;
        }

        enemiesNeedToSpwan.Clear();
        enemiesNeedToSpwan.AddRange(wave.GetEnemiesList());
        waveTextAnimator.DisplayWaveText(wave.GetWaveName());
        timer.StartTimer(wave.GetWaveDuration(), EndWaveByTime);


    }
   
    private void EndWaveByTime()
    {
        if (waveEnded)
            return;

        if (timer == null)
        {
            Debug.LogError("WaveManager: Timer reference is missing.");
            return;
        }

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

        if (waves != null && waves.Count > 0)
        {
            waves.RemoveAt(0);
        }

        if (waves != null && waves.Count > 0)
        {
            if (waves.First() == null)
            {
                Debug.LogError("WaveManager: Next wave is null.");
                return;
            }

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

        if (timer == null)
        {
            Debug.LogError("WaveManager: Timer reference is missing.");
            return;
        }

        waveEnded = true;

        timer.StopTimer();
        currentEnemiesInScene.Clear();

        if (waves != null && waves.Count > 0)
        {
            waves.RemoveAt(0);
        }

        if (waves != null && waves.Count > 0)
        {
            if (waves.First() == null)
            {
                Debug.LogError("WaveManager: Next wave is null.");
                return;
            }

            currentWaveCoroutine = StartCoroutine(RunWave(waves.First().GetSpawnDuration()));
        }
        else
        {
            Debug.Log("All waves are finished");
        }
    }

    public void CharacterDeathListiner(GameObject other)
    {
        if (other == null)
        {
            Debug.LogWarning("WaveManager: CharacterDeathListiner received null.");
            return;
        }

        if (currentEnemiesInScene == null)
        {
            Debug.LogError("WaveManager: currentEnemiesInScene is null.");
            return;
        }

        if (currentEnemiesInScene.Contains(other))
        {
            currentEnemiesInScene.Remove(other);
        }

        CheckIfAllEnemiesKilled();
    }

    private void CheckIfAllEnemiesKilled()
    {
        if (currentEnemiesInScene == null)
        {
            Debug.LogError("WaveManager: currentEnemiesInScene is null.");
            return;
        }

        if (finishedSpawning && currentEnemiesInScene.Count == 0)
        {
            EndWaveByEnemiesKilled();
        }
    }

}