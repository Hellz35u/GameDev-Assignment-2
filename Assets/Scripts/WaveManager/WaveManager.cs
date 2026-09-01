using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> waves;

    private int currentWaveIndex = 0;
    private bool isGameFinished;

    private void OnEnable()
    {
        // GameEvents.OnTimerEnded += EndWave;
    }

    private void OnDisable()
    {
        // GameEvents.OnTimerEnded -= EndWave;
    }

    private void Start()
    {
        StartWave();
    }

    private void StartWave()
    {
        if (isGameFinished)
            return;

        WaveData currentWave = waves[currentWaveIndex];
        Debug.Log($"Wave {currentWaveIndex + 1} started");
        // GameEvents.WaveStarted(currentWaveIndex + 1, currentWave.waveDuration);

        StartCoroutine(SpawnWave(currentWave));
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        foreach (GameObject enemy in wave.enemies)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    private void EndWave()
    {
        if (isGameFinished)
            return;

        Debug.Log($"Wave {currentWaveIndex + 1} ended");
        if (currentWaveIndex >= waves.Count - 1)
        {
            WinGame();
            return;
        }

        currentWaveIndex++;
        StartWave();
    }

    public void ResetWaves()
    {
        StopAllCoroutines();
        currentWaveIndex = 0;
        isGameFinished = false;

        // GameEvents.WavesReset();
        StartWave();
    }

    private void WinGame()
    {
        isGameFinished = true;
        StopAllCoroutines();

        Debug.Log("Player Won");
        // GameEvents.GameWon();
    }
    public int GetCurrentWave()
    {
        return currentWaveIndex + 1;
    }
}