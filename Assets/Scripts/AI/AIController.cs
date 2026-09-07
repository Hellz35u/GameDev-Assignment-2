
using System;
using UnityEngine;


public class AIController : MonoBehaviour
{
    private CharacterController characterActions;
    private GameObject targetGameObject = null;
    [SerializeField] private float distanceToAttack = 5f;
    private float attackCooldownRemaining = 0f;
    [SerializeField] private float secondsBetweenAtacks = 2.0f;
    [SerializeField] private float reTargetLoop = 2.0f;
    private float lastTakeTargetTime = 0;
    private EnemiesTags enemiesTags = null;



    void Start()
    {
        GameEvents.CharacterDeath += GiveScoreOnEnemyKill;
       
        characterActions = GetComponent<CharacterController>();
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
    private void OnDestroy()
    {
        GameEvents.CharacterDeath -= GiveScoreOnEnemyKill;
    }
    private void GiveScoreOnEnemyKill(GameObject go)
    {
        if (go == this.gameObject && this.tag == "Enemy")
        {
            //adding score on killing enemy
            GameEvents.OnScoreChanged(100);
        }
    }

    void FixedUpdate()
    {
        if (characterActions == null) return;
        lastTakeTargetTime += Time.deltaTime;
        CoolDownAttack(Time.fixedDeltaTime);

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
                characterActions.Move(new Vector2(directionOfTarget, 0f));
            }
        }
        else
        {
            characterActions.Move(Vector2.zero);//stop
            FindNewClosestTarget();
        }
    }


    private void TryAttackTarget(float facingDirection)
    {
        characterActions.Move(Vector2.zero);//stop before attack
        characterActions.SetFacing(facingDirection);
        if (attackCooldownRemaining <= 0f)
        {
            attackCooldownRemaining = secondsBetweenAtacks;
            characterActions.Attack();
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