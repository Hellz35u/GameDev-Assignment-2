using UnityEngine;
using System.Collections.Generic;

public class CollectableChest : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectablesList;
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

        Vector3 currentLocation = transform.position;
        GameObject firstCollectable = collectablesList[0];

        ThrowCollectable(firstCollectable, currentLocation);
        collectablesList.RemoveAt(0);

        return true;
    }

    public void ThrowAll()
    {
        if (IsEmpty()) return;

        Vector3 currentLocation = transform.position;
        foreach (GameObject collectable in collectablesList)
        {
            ThrowCollectable(collectable, currentLocation);
        }

        collectablesList.Clear();
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