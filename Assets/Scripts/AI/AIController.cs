using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private GameObject targetGameObject = null;
    [SerializeField] private float distanceToAttack = 5f;
    private EnemiesTags enemiesTags = null;

    private void Awake()
    {
        TargetManager.GetInstance()?.RegisterTarget(gameObject);
    }

    private void OnDestroy()
    {
        TargetManager.GetInstance()?.UnregisterTarget(gameObject);
    }

    void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        if (characterMovement == null)
        {
            Debug.LogError("can't find CharacterMovement script in this GameObject!");
        }
        enemiesTags = GetComponent<EnemiesTags>();
        if( enemiesTags == null )
        {
            Debug.LogError("can't find EnemiesTags script in this GameObject!");
        }

        //for tests

        targetGameObject = FindAnyObjectByType<PlayerController>()?.gameObject;
    }

    void FixedUpdate()
    {
        if (characterMovement == null) return;
        if(targetGameObject != null)
        {
            float xDistance = Mathf.Abs(targetGameObject.transform.position.x - transform.position.x);
            if(xDistance <= distanceToAttack)
            {
                characterMovement.HandleMovement(0f);//stop
                //attack
            }
            else
            {
                float xDirection = 0;
                if(targetGameObject.transform.position.x < transform.position.x)
                {
                    xDirection = -1;
                }
                else
                {
                    xDirection = 1;
                }
                characterMovement.HandleMovement(xDirection);
            }
        }
        else
        {
            characterMovement.HandleMovement(0f);//stop
            if(TargetManager.GetInstance() != null && enemiesTags != null)
            {
                //find next target
                targetGameObject = TargetManager.GetInstance().GetClosestTarget(enemiesTags.GetList(), transform.position);
            }
        }
    }
}
