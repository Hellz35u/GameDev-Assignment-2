using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2d;
    Vector2 direction = Vector2.zero;
    [SerializeField] float speed = 50f;
    [SerializeField] float jumpForce = 8f;
    bool isGrounded = false;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void HandleMovement(float directionX)
    {
        direction.x = directionX;
    }

    public bool HandleJump()
    {
        if (!isGrounded)
        {
            return false; 
        }
        rb2d.AddForceY(jumpForce, ForceMode2D.Impulse);
        return true;
    }
    private void FixedUpdate()
    {
        rb2d.linearVelocityX = direction.x * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public float GetVerticalVelocity()
    {
        return rb2d.linearVelocityY;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }
}
