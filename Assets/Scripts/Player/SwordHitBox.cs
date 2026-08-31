using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    private HashSet<GameObject> enemiesHitThisAttack = new();
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !enemiesHitThisAttack.Contains(other.gameObject))
        {
            //need to handle attack on the enemy
            enemiesHitThisAttack.Add(other.gameObject);
        }
    }
    public void ClearMemory()
    {
        enemiesHitThisAttack.Clear();
    }
}
