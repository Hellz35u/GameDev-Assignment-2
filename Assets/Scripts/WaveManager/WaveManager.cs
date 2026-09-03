using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private List<GameObject> currentEnemiesInScene;
    
    private bool isGameFinished = false;
    private List<GameObject> enemiesNeedToSpwan = new List<GameObject>();
    private List<Vector3> spawnPositions = new List<Vector3>();
    private int currentWaveNumber = 0;

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
        StartCoroutine(RunWave(waves[currentWaveNumber].GetSpawnDuration()));
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
        while (currentEnemiesInScene.Count <= waves[currentWaveNumber].GetMaxEnemiesInScene())
        {
            int randomIndex = GetPositionsRandomIndex();
            SpawnEnemy(spawnPositions[randomIndex]);
            yield return new WaitForSeconds(spawnDuration);

        }
    }
    public void RemoveEnemyFromList(GameObject enemy)
    {
        currentEnemiesInScene.Remove(enemy);
    }
}