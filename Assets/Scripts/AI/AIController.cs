
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
        if (TargetManager.GetInstance() == null)
        {
            Debug.LogError($"'{gameObject.name}' can't register to Target Manager");
        }
        else
        {
            TargetManager.GetInstance().RegisterTarget(gameObject);
        }
    }

    private void OnDisable()
    {
        if (TargetManager.GetInstance() == null)
        {
            Debug.LogError($"'{gameObject.name}' can't unregister to Target Manager ,\n if the game is ending ignore this msg!");
        }
        else
        {
            TargetManager.GetInstance().UnregisterTarget(gameObject);
        }
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

        Vector3 myPosition = transform.position;
        if (targetGameObject != null && targetGameObject.activeInHierarchy == true)
        {
            Vector3 targetPosition = targetGameObject.transform.position;

            float xDifference = targetPosition.x - myPosition.x;
            float xDistance = Mathf.Abs(xDifference);

            if (xDistance <= distanceToAttack)
            {
                characterActions.Move(Vector2.zero);//stop
                characterActions.SetFacing(xDifference);
                if (attackCooldownRemaining <= 0f)
                {
                    attackCooldownRemaining = secondsBetweenAtacks;
                    characterActions.Attack();
                }
            }
            else
            {
                float xDirection = xDifference < 0 ? -1 : 1;
                characterActions.Move(new Vector2(xDirection, 0f));
            }
        }
        else
        {
            characterActions.Move(Vector2.zero);//stop
            if (TargetManager.GetInstance() != null && enemiesTags != null)
            {
                //find next target
                targetGameObject = TargetManager.GetInstance().GetClosestTarget(enemiesTags.GetList(), myPosition);
            }
        }
    }
}