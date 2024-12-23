using UnityEngine;

public class PlayerMovment : MonoBehaviour
{

    [SerializeField] private float speed = 5;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [SerializeField] private int facingDirection = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput < 0 && transform.localScale.x > 0 ||
                    horizontalInput > 0 && transform.localScale.x < 0)
                {
                    FlipDirection();
                }

        anim.SetFloat("horizontal", Mathf.Abs(horizontalInput));
        anim.SetFloat("vertical", Mathf.Abs(verticalInput));

        

        rb.linearVelocity = new Vector2(horizontalInput, verticalInput) * speed;

    }

    void FlipDirection()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

    }
}
