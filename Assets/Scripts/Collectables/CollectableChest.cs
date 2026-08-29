using UnityEngine;
using System.Collections.Generic;

public class CollectableChest : MonoBehaviour
{
    [SerializeField] private List<GameObject> Collectables_List;
    [SerializeField] private float YThrowForce = 4f;
    [SerializeField] private float XThrowMinForce = -2f;
    [SerializeField] private float XThrowMaxForce = 2f;

    public bool IsEmpty()
    {
        return ( Collectables_List == null || Collectables_List.Count == 0 );
    }


    public bool ThrowNext()
    {
        if (IsEmpty()) return false;
        Vector3 currentLocation = transform.position;
        GameObject firstCollectable = Collectables_List[0];
        ThrowCollectable(firstCollectable, currentLocation);
        Collectables_List.RemoveAt(0);
        return true;
    }
    
    public void ThrowAll()
    {
        if (IsEmpty()) return;
        Vector3 currentLocation = transform.position;
        foreach (GameObject collectable in Collectables_List)
        {
            ThrowCollectable(collectable, currentLocation);
        }
        Collectables_List.Clear();
    }
    void ThrowCollectable(GameObject collectable, Vector3 spawnPosition)
    {
        //will create the collectable without spin it
        ThrowCollectable(collectable, spawnPosition, Quaternion.identity);
    }

    void ThrowCollectable(GameObject collectable, Vector3 spawnPosition, Quaternion initial_rotation)
    {
        if (collectable != null)
        {
            GameObject currentCollectable = Instantiate(collectable, spawnPosition, initial_rotation);
            Rigidbody2D currentCollectableRB = currentCollectable.GetComponent<Rigidbody2D>();
            if (currentCollectableRB != null)
            {
                float currentCollectableXForce = Random.Range(XThrowMinForce, XThrowMaxForce);
                Vector2 throwForce = new Vector2(currentCollectableXForce, YThrowForce);
                currentCollectableRB.AddForce(throwForce, ForceMode2D.Impulse);
            }
        }
    }
}
