using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private float secondsBetweenThrows = 2f;

    private CollectableChest collectableChest;
    private Coroutine throwCoroutine = null;

    private void Start()
    {
        CacheComponents();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!CanStartThrowing()) return;

        throwCoroutine = StartCoroutine(ThrowChestItems());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StopThrowing();
    }

    // Setup / Initialization
    private void CacheComponents()
    {
        collectableChest = GetComponent<CollectableChest>();
        if (collectableChest == null)
        {
            Debug.LogWarning("can't find the CollectableChest script inside the GameObject!", this);
        }
    }

    // Throwing Logic
    private bool CanStartThrowing()
    {
        if (collectableChest == null) return false;
        if (collectableChest.IsEmpty()) return false;
        if (throwCoroutine != null) return false;

        return true;
    }

    private void StopThrowing()
    {
        if (throwCoroutine == null) return;

        StopCoroutine(throwCoroutine);
        throwCoroutine = null;
    }

    private IEnumerator ThrowChestItems()
    {
        while (!collectableChest.IsEmpty())
        {
            collectableChest.ThrowNext();
            yield return new WaitForSeconds(secondsBetweenThrows);
        }
        throwCoroutine = null;
    }
}