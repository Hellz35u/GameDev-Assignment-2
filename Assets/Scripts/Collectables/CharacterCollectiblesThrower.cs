using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterCollectableThrower : MonoBehaviour
{
    [SerializeField] private int eachThrowChances = 50;
    private int maxChances = 100;
    private CollectableLauncher launcher;
    private string killerTag = "Player";
    private bool needToThrow = false;
    void Start()
    {
        launcher = GetComponent<CollectableLauncher>();
        if(launcher == null)
        {
            Debug.LogError("cant find CollectableLauncher in this GameObject!");
        }
        GameEvents.CharacterDeath += HandleCharacterDeath;
    }

    private void HandleCharacterDeath(GameObject victim, GameObject killer)
    {
        if (victim != this.gameObject) return;
        if (killer == null)
        {
            if (killerTag.Length != 0) return;
        }
        else 
        {
            if (killerTag != killer.tag) return;
        }


        needToThrow = true;
        
    }

    private void OnDestroy()
    {
        GameEvents.CharacterDeath -= HandleCharacterDeath;
        if (launcher != null && needToThrow)
        {
            while(!launcher.IsEmpty())
            {
                int currentChance = Random.Range(0, maxChances);
                if(eachThrowChances > currentChance)
                {
                    launcher.ThrowNext();
                }
                else
                {
                    launcher.PassNext();
                }
            }
        }
    }
}
