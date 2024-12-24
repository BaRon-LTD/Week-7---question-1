using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int facingDirection = -1;  // 1 = Right, -1 = Left

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

        rb.linearVelocity = new Vector2(horizontalInput, verticalInput) * speed;
    }

    void FlipDirection()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    public float GetSpeed()
    {
        return speed;
    }

}
