using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 50f;
    [SerializeField] private float refreshRate = 10f;
    private TargetJoint2D joint;

    private void Awake()
    {
        joint = GetComponent<TargetJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartChase(other);
        }
    }

    public void StartChase(Collider2D playerCollider)
    {
        joint.maxForce = chaseSpeed;
        joint.frequency = refreshRate;

        joint.target = playerCollider.transform.position;
        joint.enabled = true;
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}