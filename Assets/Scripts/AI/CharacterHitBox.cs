using System.Collections.Generic;
using UnityEngine;

public class CharacterHitBox : MonoBehaviour
{
    [SerializeField]private int damageGiven = 10;
    private HashSet<GameObject> charactersHitThisAttack = new();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        HitGameObject(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        HitGameObject(other.gameObject);
    }

    private void HitGameObject(GameObject go)
    {
        if (charactersHitThisAttack.Contains(go)) return;
        if (enemiesCharactersTags.Contains(go.tag))
        {
            charactersHitThisAttack.Add(go);
            GameEvents.OnCharacterTakeHit(go , damageGiven);
        }
    }
    public void ClearMemory()
    {
        charactersHitThisAttack.Clear();
    }

}
