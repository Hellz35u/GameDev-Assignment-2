using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int maxWaves = 3;

    private int currentWave = 1;
    private bool isGameFinished;

    private void OnEnable()
    {
        //GameEvents.OnTimerEnded += EndWave;
    }

    private void OnDisable()
    {
        //GameEvents.OnTimerEnded -= EndWave;
    }

    private void Start()
    {
        StartWave();
    }

    private void StartWave()
    {
        Debug.Log($"Wave {currentWave} started");
        //GameEvents.WaveStarted(currentWave);
    }

    private void EndWave()
    {
        if (isGameFinished)
            return;

        Debug.Log($"Wave {currentWave} ended");
        if (currentWave >= maxWaves)
        {
            WinGame();
            return;
        }

        currentWave++;
        StartWave();
    }

    public void ResetWaves()
    {
        currentWave = 1;
        isGameFinished = false;

        //GameEvents.WavesReset();
        StartWave();
    }

    private void WinGame()
    {
        isGameFinished = true;

        Debug.Log("Player Won");
        //GameEvents.GameWon();
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}