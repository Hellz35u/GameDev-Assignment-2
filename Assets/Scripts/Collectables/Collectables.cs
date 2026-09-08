using System;
using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int scoreValue = 100;
    [SerializeField] private float springFrequency = 5f;
    [SerializeField] private float dampingRatio = 0.7f;
    [SerializeField] private float finalTargetDistance = 0.5f;
    [SerializeField] private float chaseDelay = 2f;
    [SerializeField] private float initialOpacity = 0.5f;

    private SpriteRenderer spriteRenderer;
    private SpringJoint2D springJoint;
    private Collider2D triggerCollider;

    private Color originalColor;
    private bool chaseEnabled = false;

    private void Awake()
    {
        CacheComponents();
        ApplyInitialVisualState();
    }

    private void Start()
    {
        StartCoroutine(ChaseEnabler());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!CanStartChase()) return;
        if (!other.CompareTag("Player")) return;

        Rigidbody2D playerRB = other.GetComponent<Rigidbody2D>();
        StartChase(playerRB);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameEvents.OnAddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    public void StartChase(Rigidbody2D playerRigidbody)
    {
        if (playerRigidbody == null) return;

        ConfigureSpringJoint(playerRigidbody);
        DisableTriggerCollider();
    }

    // Setup / Initialization


    private void CacheComponents()
    {
        springJoint = GetComponent<SpringJoint2D>();
        if (springJoint != null)
        {
            springJoint.enabled = false;
        }
        else
        {
            Debug.LogError("can't find SpringJoint2D in this GameObject!", this);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("can't find SpriteRenderer in this GameObject!, so weird??", this);
        }

        triggerCollider = FindTriggerCollider();
        if (triggerCollider == null)
        {
            Debug.LogError("can't find Collider2D as trigger in this GameObject!", this);
        }
    }

    private Collider2D FindTriggerCollider()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D coll in colliders)
        {
            if (coll.isTrigger)
            {
                return coll;
            }
        }
        return null;
    }

    private void ApplyInitialVisualState()
    {
        if (spriteRenderer == null) return;

        originalColor = spriteRenderer.color;

        Color fadedColor = originalColor;
        fadedColor.a = initialOpacity;
        spriteRenderer.color = fadedColor;
    }


    // Chase Logic


    private bool CanStartChase()
    {
        return chaseEnabled && springJoint != null && !springJoint.enabled;
    }

    private void ConfigureSpringJoint(Rigidbody2D playerRigidbody)
    {
        if (springJoint == null) return;

        springJoint.connectedBody = playerRigidbody;
        springJoint.autoConfigureDistance = false;
        springJoint.distance = finalTargetDistance;
        springJoint.frequency = springFrequency;
        springJoint.dampingRatio = dampingRatio;
        springJoint.enabled = true;
    }

    private void DisableTriggerCollider()
    {
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }
    }

    private IEnumerator ChaseEnabler()
    {
        yield return new WaitForSeconds(chaseDelay);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        chaseEnabled = true;
    }
}