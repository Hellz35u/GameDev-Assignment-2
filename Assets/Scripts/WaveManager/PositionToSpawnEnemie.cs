using UnityEngine;

public class PositionToSpawnEnemie : MonoBehaviour
{
    [SerializeField] private float rangeX = 10f;
    //fix the range position to adapt the map scale;
    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(
            transform.position.x - rangeX,
            transform.position.x + rangeX
        );

        return new Vector3(
            randomX,
            transform.position.y,
            transform.position.z
        );
    }
}