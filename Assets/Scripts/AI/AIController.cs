
using System;
using UnityEngine;


public class AIController : MonoBehaviour
{
    private CharacterController characterController;
    private GameObject targetGameObject = null;
    [SerializeField] private float distanceToAttack = 5f;
    private float attackCooldownRemaining = 0f;
    [SerializeField] private float secondsBetweenAtacks = 2.0f;
    [SerializeField] private float reTargetLoop = 2.0f;
    [SerializeField] private int framSkipper = 10;
    private int framCounter = 0;
    private float lastTakeTargetTime = 0;
    private EnemiesTags enemiesTags = null;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("can't find characterController script in this GameObject!");
        }
        enemiesTags = GetComponent<EnemiesTags>();
        if (enemiesTags == null)
        {
            Debug.LogError("can't find EnemiesTags script in this GameObject!");
        }
    }

    void Update()
    {
        if (characterController == null || enemiesTags == null) return;
        lastTakeTargetTime += Time.deltaTime;
        CoolDownAttack(Time.deltaTime);

        framCounter = (framCounter + 1) % framSkipper;
        if (framCounter != 0) return;
        Vector3 myPosition = transform.position;
        if (HasValidTarget())
        {
            Vector3 targetPosition = targetGameObject.transform.position;

            float xDifference = targetPosition.x - myPosition.x;
            float xDistance = Mathf.Abs(xDifference);
            float directionOfTarget = Mathf.Sign(xDifference);
            if (xDistance <= distanceToAttack)
            {
                TryAttackTarget(directionOfTarget);
            }
            else
            {
                characterController.Move(new Vector2(directionOfTarget, 0f));
            }
        }
        else
        {
            characterController.Move(Vector2.zero);//stop
            FindNewClosestTarget();
        }
    }


    private void TryAttackTarget(float facingDirection)
    {
        characterController.Move(Vector2.zero);//stop before attack
        characterController.SetFacing(facingDirection);
        if (attackCooldownRemaining <= 0f)
        {
            attackCooldownRemaining = secondsBetweenAtacks;
            characterController.Attack();
        }
    }

    private void FindNewClosestTarget()
    {
            targetGameObject = TargetManager.GetClosestTarget(enemiesTags.GetList(), transform.position);
            lastTakeTargetTime = 0;
    }


    private bool HasValidTarget()
    {
        return targetGameObject != null && targetGameObject.activeInHierarchy == true && lastTakeTargetTime < reTargetLoop;
    }

    private void CoolDownAttack(float secondsFromLastCheck)
    {
        if (IsNeedToCoolDown())
        {
            attackCooldownRemaining -= secondsFromLastCheck;
        }
        else
        {
            attackCooldownRemaining = 0f;//to fix negative values
        }
    }

    private bool IsNeedToCoolDown()
    {
        return attackCooldownRemaining > 0f;
    }
}