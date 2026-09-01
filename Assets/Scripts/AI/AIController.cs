using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private CharacterActions characterActions;
    private GameObject targetGameObject = null;
    [SerializeField] private float distanceToAttack = 5f;
    private float attackCooldownRemaining = 0f;
    [SerializeField] private float secondsBetweenAtacks = 2.0f;
    private EnemiesTags enemiesTags = null;

    private void OnEnable()
    {
        TargetManager.GetInstance()?.RegisterTarget(gameObject);
    }

    private void OnDisable()
    {
        TargetManager.GetInstance()?.UnregisterTarget(gameObject);
    }

    void Start()
    {
        characterActions = GetComponent<CharacterActions>();
        if (characterActions == null)
        {
            Debug.LogError("can't find CharacterActions script in this GameObject!");
        }
        enemiesTags = GetComponent<EnemiesTags>();
        if (enemiesTags == null)
        {
            Debug.LogError("can't find EnemiesTags script in this GameObject!");
        }
    }

    void FixedUpdate()
    {
        if (characterActions == null) return;

        if (attackCooldownRemaining > 0f)
        {
            attackCooldownRemaining -= Time.fixedDeltaTime;
        }

        if (targetGameObject != null)
        {
            float xDistance = Mathf.Abs(targetGameObject.transform.position.x - transform.position.x);
            if (xDistance <= distanceToAttack)
            {
                characterActions.Move(Vector2.zero);//stop
                if (attackCooldownRemaining <= 0f)
                {
                    attackCooldownRemaining = secondsBetweenAtacks;
                    characterActions.Attack();
                }
            }
            else
            {
                float xDirection = 0;
                if (targetGameObject.transform.position.x < transform.position.x)
                {
                    xDirection = -1;
                }
                else
                {
                    xDirection = 1;
                }
                characterActions.Move(new Vector2(xDirection, 0f));
            }
        }
        else
        {
            characterActions.Move(Vector2.zero);//stop
            if (TargetManager.GetInstance() != null && enemiesTags != null)
            {
                //find next target
                targetGameObject = TargetManager.GetInstance().GetClosestTarget(enemiesTags.GetList(), transform.position);
            }
        }
    }
}