using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private static Dictionary<string, HashSet<GameObject>> registeredTargetsByTag = new();
    private static HashSet<GameObject> emptyHashSet = new();

    private void Awake()
    {
        GameEvents.CharacterDeath += OnCharacterDeath;
    }

    public void OnCharacterDeath(GameObject victim, GameObject killer)
    {
        UnregisterTarget(victim);
    }

    private void OnDestroy()
    {
        registeredTargetsByTag.Clear();
    }

    public static void RegisterTarget(GameObject target)
    {
        if (!registeredTargetsByTag.ContainsKey(target.tag))
        {
            registeredTargetsByTag[target.tag] = new HashSet<GameObject>();
        }
        registeredTargetsByTag[target.tag].Add(target);
    }

    public static void UnregisterTarget(GameObject target)
    {
        if (!registeredTargetsByTag.ContainsKey(target.tag))
        {
            return;
        }
        registeredTargetsByTag[target.tag].Remove(target);
    }
    public static GameObject GetClosestTarget(List<string> tagsToLookFor, Vector3 originPosition)
    {
        if (tagsToLookFor == null || tagsToLookFor.Count == 0)
        {
            return null;
        }

        float closestDistance = float.MaxValue;//infinity
        GameObject closestGameObject = null;

        foreach (string tag in tagsToLookFor)
        {
            foreach (GameObject target in registeredTargetsByTag.GetValueOrDefault(tag, emptyHashSet))
            {
                if (target == null) continue;
                float currentDistance = Vector3.Distance(originPosition, target.transform.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestGameObject = target;
                }
            }     
        }
        return closestGameObject;
    }
}
