using System.Collections.Generic;
using UnityEngine;

public class CharacterHitBox : MonoBehaviour
{
    private HashSet<GameObject> charectersHitThisAttack = new();
    private HashSet<string> enemiesCharactersTags = new();

    private void Awake()
    {
        //convert list to hashset
        EnemiesTags enemiesTags = GetComponentInParent<EnemiesTags>();
        if (enemiesTags == null)
        {
            Debug.LogError("can't find EnemiesTags script in parent GameObject!");
        }
        else
        {
            enemiesCharactersTags = new HashSet<string>(enemiesTags.GetList());
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (charectersHitThisAttack.Contains(other.gameObject)) return;
        if (enemiesCharactersTags.Contains(other.tag))
        {
            //need to handle attack on the enemy
            charectersHitThisAttack.Add(other.gameObject);
        }
    }
    public void ClearMemory()
    {
        charectersHitThisAttack.Clear();
    }

}
