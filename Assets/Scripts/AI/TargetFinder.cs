using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private Dictionary<string, HashSet<GameObject>> registeredTargetsByTag = new();
    private HashSet<GameObject> emptyHashSet = new();
    private static TargetManager singletoneInstance = null;

    public void Awake()
    {
        if (singletoneInstance != null && singletoneInstance != this)
        {
            Destroy(gameObject);//destroy duplicate object
            return;
        }
        singletoneInstance = this;
    }

    private void OnDestroy()
    {
        if (singletoneInstance == this)
        {
            singletoneInstance = null;
        }
    }

    public static TargetManager GetInstance()
    {
        return singletoneInstance;
    }

    public void RegisterTarget(GameObject target)
    {
        if (!registeredTargetsByTag.ContainsKey(target.tag))
        {
            registeredTargetsByTag[target.tag] = new HashSet<GameObject>();
        }
        registeredTargetsByTag[target.tag].Add(target);
    }

    public void UnregisterTarget(GameObject target)
    {
        if (!registeredTargetsByTag.ContainsKey(target.tag))
        {
            return;
        }
        registeredTargetsByTag[target.tag].Remove(target);
    }
    public GameObject GetClosestTarget(List<string> tagsToLookFor, Vector3 originPosition)
    {
        if (registeredTargetsByTag.Count == 0 || tagsToLookFor == null || tagsToLookFor.Count == 0)
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
