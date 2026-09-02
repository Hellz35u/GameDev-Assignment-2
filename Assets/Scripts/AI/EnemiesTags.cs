using System.Collections.Generic;
using UnityEngine;

public class EnemiesTags : MonoBehaviour
{
    [SerializeField] private List<string> enemiesTagsList = new();

    public List<string> GetList()
    {
        return enemiesTagsList;
    }
}
