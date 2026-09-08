using UnityEngine;
using System.Collections.Generic;

public class CollectableLauncher : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectablesList;
    [SerializeField] private Vector2 throwOffset = new Vector2(0.0f,2f);
    [SerializeField] private float yThrowForce = 4f;
    [SerializeField] private float xThrowMinForce = -2f;
    [SerializeField] private float xThrowMaxForce = 2f;

    public bool IsEmpty()
    {
        return (collectablesList == null || collectablesList.Count == 0);
    }

    public bool ThrowNext()
    {
        if (IsEmpty()) return false;
        Vector3 throwLocation = CalculateThrowPosition();
        GameObject firstCollectable = collectablesList[0];

        ThrowCollectable(firstCollectable, throwLocation);
        collectablesList.RemoveAt(0);

        return true;
    }
    public bool PassNext()
    {
        if (IsEmpty()) return false;
        collectablesList.RemoveAt(0);
        return true;
    }
    public void ThrowAll()
    {
        if (IsEmpty()) return;
        Vector3 throwLocation = CalculateThrowPosition();
        foreach (GameObject collectable in collectablesList)
        {
            ThrowCollectable(collectable, throwLocation);
        }

        collectablesList.Clear();
    }

    private Vector3 CalculateThrowPosition()
    {
        //current location + offset
        return transform.position + (Vector3)throwOffset;
    }

    private void ThrowCollectable(GameObject collectable, Vector3 spawnPosition)
    {
        // will create the collectable without spin it
        ThrowCollectable(collectable, spawnPosition, Quaternion.identity);
    }

    private void ThrowCollectable(GameObject collectable, Vector3 spawnPosition, Quaternion initialRotation)
    {
        if (collectable == null) return;

        GameObject currentCollectable = Instantiate(collectable, spawnPosition, initialRotation);
        ApplyThrowForce(currentCollectable);
    }

    private void ApplyThrowForce(GameObject currentCollectable)
    {
        Rigidbody2D currentCollectableRB = currentCollectable.GetComponent<Rigidbody2D>();
        if (currentCollectableRB == null) return;

        float currentCollectableXForce = Random.Range(xThrowMinForce, xThrowMaxForce);
        Vector2 throwForce = new Vector2(currentCollectableXForce, yThrowForce);
        currentCollectableRB.AddForce(throwForce, ForceMode2D.Impulse);
    }
}