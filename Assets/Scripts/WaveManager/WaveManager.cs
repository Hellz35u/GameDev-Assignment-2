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
    private WaveTextAnimator waveTextAnimator;
    private Timer timer;

    private void Start()
    {
        PositionToSpawnEnemie[] spawnPositionArray = FindObjectsByType<PositionToSpawnEnemie>(FindObjectsSortMode.None);
        foreach (PositionToSpawnEnemie sp in spawnPositionArray)
        {
            spawnPositions.Add(sp.GetPositon());
        }
        StartWave();

    }
    private void StartWave()
    {
        StartCoroutine(RunWave(waves.First<WaveData>().GetSpawnDuration()));
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
        return Random.Range(0, spawnPositions.Count);
    }
    private IEnumerator RunWave(float spawnDuration)
    {
        WaveData wave = waves.First<WaveData>();
        waveTextAnimator.DisplayWaveText(wave.GetWaveName());
        timer.StartTimer(wave.GetTimerDuration());
        while (currentEnemiesInScene.Count < waves.First<WaveData>().GetMaxEnemiesInScene())
        {
            int randomIndex = GetPositionsRandomIndex();
            SpawnEnemy(spawnPositions[randomIndex]);
            yield return new WaitForSeconds(spawnDuration);
        }
        if (waves.Count == 0)
            yield break;

        //function that check if all enemies died on the wave and clean currentEnemiesInScene
        
    }
    private void EnemyListCleaner(List<GameObject> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if(list[i] == null)
            {
                list.RemoveAt(i);
            }
        }
    }
   
}