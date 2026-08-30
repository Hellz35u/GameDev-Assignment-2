using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    private HashSet<Collider2D> hitedEnemy = new();
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && hitedEnemy.Contains(other))
        {
            Debug.Log("Sword hit Enemy!");
            hitedEnemy.Add(other);
        }
    }
    public void ClearMemory()
    {
        hitedEnemy.Clear();
    }
}
