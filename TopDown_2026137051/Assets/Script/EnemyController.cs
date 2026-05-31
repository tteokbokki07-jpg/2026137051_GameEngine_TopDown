using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    public bool isMovingRight = false;
    public bool isMovingLeft = false;
    public bool isMovingUp = false;
    public bool isMovingDown = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (isMovingRight)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            //transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
        }
        if (isMovingLeft)
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
            //transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        if (isMovingUp)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveSpeed);
            //transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        if (isMovingDown)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -moveSpeed);
            //transform.localScale = new Vector3(0.7f, -0.7f, 0.7f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            if (isMovingRight)
            {
                isMovingRight = false;
                isMovingLeft = true;
            }
            else if (isMovingLeft)
            {
                isMovingLeft = false;
                isMovingRight = true;
            }
            else if (isMovingUp)
            {
                isMovingUp = false;
                isMovingDown = true;
            }
            else if (isMovingDown)
            {
                isMovingDown = false;
                isMovingUp = true;
            }
        }
    }
}
