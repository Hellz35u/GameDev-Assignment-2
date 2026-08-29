using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private CollectableChest collectableChest;
    [SerializeField] float SecondsBetweenThrows = 2f;
    private Coroutine throwCoroutine = null;
    void Start()
    {
        collectableChest = GetComponent<CollectableChest>();
        if (!collectableChest) Debug.LogWarning("can't find the CollectableChest script inside the GameObject!");
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if(!collectableChest.IsEmpty())
        { 
            if (other.CompareTag("Player") && throwCoroutine == null)
            {
                throwCoroutine = StartCoroutine(ThrowChestItems());
            }
        }
    }

    void OnTriggerExit2D(UnityEngine.Collider2D other)
    {
        if (other.CompareTag("Player") && throwCoroutine != null)
        {
            StopCoroutine(throwCoroutine);
            throwCoroutine = null;
        }
    }

    IEnumerator ThrowChestItems()
    {
        while(!collectableChest.IsEmpty())
        {
            collectableChest.ThrowNext();
            yield return new WaitForSeconds(SecondsBetweenThrows);
        }
        throwCoroutine = null;
        yield return null;
    }
}
