using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<WaveData> waves = new List<WaveData>();
    [SerializeField] private WaveTextAnimator waveTextAnimator;
    [SerializeField] private Timer timer;
    [SerializeField] private string afterLastWaveScene;

    [Header("Wave Timing")]
    [SerializeField] private int timeBetweenWaves = 3;

    private readonly HashSet<GameObject> currentEnemiesInScene = new HashSet<GameObject>();
    private readonly List<GameObject> enemiesToSpawn = new List<GameObject>();
    private List<Vector3> spawnPositions = new();

    private bool waveEnded;
    private bool finishedSpawning;
    private Coroutine currentWaveCoroutine;

    private void OnEnable()
    {
        GameEvents.CharacterDeath += OnCharacterDeath;
    }

    private void OnDisable()
    {
        GameEvents.CharacterDeath -= OnCharacterDeath;
    }

    private void Start()
    {
        PositionToSpawnEnemie[] positionsInScene = FindObjectsByType<PositionToSpawnEnemie>(FindObjectsSortMode.None);
        if (positionsInScene != null)
        {
            foreach (PositionToSpawnEnemie position in positionsInScene)
            {
                if (position == null) continue;
                spawnPositions.Add(position.GetPosition());
            }
        }

        if (!AreDependenciesValid()) return;

        ProcessAndStartNextWave();
    }

    private bool AreDependenciesValid()
    {
        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            Debug.LogError("Spawn position is not assigned!"); return false;
        }
        if (timer == null)
        {
            Debug.LogError("Timer is not assigned!"); return false;
        }
        if (waveTextAnimator == null)
        {
            Debug.LogError("WaveTextAnimator is not assigned!"); return false;
        }
        if (waves == null || waves.Count == 0 || waves[0] == null)
        {
            Debug.LogError("Waves list is empty or first wave is null!");
            return false;
        }
        return true;
    }

    private void ProcessAndStartNextWave()
    {
        if (waves == null || waves.Count == 0)
        {
            Debug.Log("All waves are finished!");
            SceneManager.LoadScene(afterLastWaveScene);
            return;
        }

        WaveData currentWave = waves[0];

        if (currentWave == null || currentWave.GetEnemiesList() == null)
        {
            Debug.LogError("Current wave or its enemy list is null.");
            return;
        }

        waveEnded = false;
        finishedSpawning = false;

        enemiesToSpawn.Clear();
        enemiesToSpawn.AddRange(currentWave.GetEnemiesList());

        waveTextAnimator.DisplayWaveText(currentWave.GetWaveName());
        timer.StartTimer(currentWave.GetWaveDuration(), HandleWaveTimeout);

        currentWaveCoroutine = StartCoroutine(SpawnEnemiesRoutine(currentWave));
    }

    private IEnumerator SpawnEnemiesRoutine(WaveData currentWave)
    {
        float spawnDuration = currentWave.GetSpawnDuration();
        int maxEnemies = currentWave.GetMaxEnemiesInScene();

        if (spawnDuration < 0)
        {
            Debug.LogError("Spawn duration cannot be negative.");
            yield break;
        }

        if (maxEnemies <= 0)
        {
            Debug.LogError("Max enemies in scene must be at least 1. Aborting wave spawn.");
            yield break;
        }

        if (spawnPositions == null)
        {
            Debug.LogError("spawnPositions can't be null.");
            yield break;
        }

        while (enemiesToSpawn.Count > 0)
        {
            if (waveEnded) yield break;

            while (currentEnemiesInScene.Count >= maxEnemies)
            {
                if (waveEnded) yield break;
                yield return null;
            }

            int randomIndex = Random.Range(0, spawnPositions.Count);
            Vector3 spawnPos = spawnPositions[randomIndex];
            SpawnNextEnemyFromQueue(spawnPos);

            yield return new WaitForSeconds(spawnDuration);
        }

        finishedSpawning = true;
        CheckWaveCompletion();
    }

    private IEnumerator StartNextWaveAfterDelay()
    {
        if (waves.Count == 0)
        {
            // Last wave already finished , skip the delay and load afterLastWaveScene
            ProcessAndStartNextWave();
            yield break;
        }
        int secondsDelay = timeBetweenWaves;
        while (secondsDelay > 0)
        {
            waveTextAnimator.DisplayWaveText($"{secondsDelay}");
            secondsDelay--;
            yield return new WaitForSeconds(1f);//wait for one secod
        }

        ProcessAndStartNextWave();
    }

    private void SpawnNextEnemyFromQueue(Vector3 spawnPosition)
    {
        if (enemiesToSpawn.Count == 0) return;

        GameObject prefab = enemiesToSpawn[0];
        enemiesToSpawn.RemoveAt(0);

        if (prefab == null)
        {
            Debug.LogWarning("Enemy prefab is null.");
            return;
        }

        GameObject spawnedEnemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
        if (spawnedEnemy != null)
        {
            currentEnemiesInScene.Add(spawnedEnemy);
        }
    }

    private void HandleWaveTimeout()
    {
        ConcludeActiveWave(destroyRemainingEnemies: true);
    }

    private void HandleAllEnemiesEliminated()
    {
        ConcludeActiveWave(destroyRemainingEnemies: false);
    }

    private void ConcludeActiveWave(bool destroyRemainingEnemies)
    {
        if (waveEnded) return;
        waveEnded = true;

        timer?.StopTimer();

        if (currentWaveCoroutine != null)
        {
            StopCoroutine(currentWaveCoroutine);
            currentWaveCoroutine = null;
        }

        if (destroyRemainingEnemies)
        {
            foreach (var enemy in currentEnemiesInScene)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
        }

        currentEnemiesInScene.Clear();

        if (waves.Count > 0)
        {
            waves.RemoveAt(0);
        }

        StartCoroutine(StartNextWaveAfterDelay());
    }

    private void OnCharacterDeath(GameObject victim, GameObject killer)
    {
        if (victim == null) return;

        if (currentEnemiesInScene.Remove(victim))
        {
            CheckWaveCompletion();
        }
    }

    private void CheckWaveCompletion()
    {
        if (finishedSpawning && currentEnemiesInScene.Count == 0)
        {
            HandleAllEnemiesEliminated();
        }
    }
}