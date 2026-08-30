using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private float secondsBetweenThrows = 2f;

    private CollectableLauncher collectableChest;
    private Coroutine throwCoroutine = null;
    private Animator chestAnimator = null;

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
        collectableChest = GetComponent<CollectableLauncher>();
        if (collectableChest == null)
        {
            Debug.LogWarning("can't find the CollectableChest script inside the GameObject!", this);
        }
        chestAnimator = GetComponent<Animator>();
        if (chestAnimator == null)
        {
            Debug.LogWarning("can't find the Animator script inside the GameObject!", this);
        }
    }

    // Throwing Logic
    private bool CanStartThrowing()
    {
        if (collectableChest == null) return false;
        if (chestAnimator == null) return false;
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
        if (!collectableChest.IsEmpty())
        {
            chestAnimator.SetBool("IsOpened", true);
            //wait until the chest is opened
            yield return new WaitUntil(()=>chestAnimator.GetCurrentAnimatorStateInfo(0).IsName("Opened"));
        }
        while (!collectableChest.IsEmpty())
        {
            collectableChest.ThrowNext();
            yield return new WaitForSeconds(secondsBetweenThrows);
        }
        chestAnimator.SetBool("IsOpened", false);
        throwCoroutine = null;
    }
}