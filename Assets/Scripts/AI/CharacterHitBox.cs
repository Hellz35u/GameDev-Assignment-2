using System.Collections.Generic;
using UnityEngine;

public class CharacterHitBox : MonoBehaviour
{
    [SerializeField]private int damageGiven = 10;
    private HashSet<GameObject> haveBeenHit = new();
    private HashSet<string> enemiesCharactersTags = new();
    private GameObject ownerGameObject;

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
            ownerGameObject = enemiesTags.gameObject;
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

    private void HitGameObject(GameObject victimGameObject)
    {
        if (ownerGameObject == null || haveBeenHit.Contains(victimGameObject))return;
        
        if (enemiesCharactersTags.Contains(victimGameObject.tag))
        {
            haveBeenHit.Add(victimGameObject);
            GameEvents.OnCharacterTakeHit(victimGameObject, ownerGameObject, damageGiven);
        }
    }
    public void ClearMemory()
    {
        haveBeenHit.Clear();
    }

}
