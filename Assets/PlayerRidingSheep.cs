using UnityEngine;

public class PlayerRidingSheep : MonoBehaviour
{
    private bool isRiding = false;   // Flag to check if the player is riding the sheep
    private GameObject sheep; // Reference to the sheep's GameObject
    [SerializeField] private Rigidbody2D sheepRigidbody; // Reference to the sheep's Rigidbody2D
    private Rigidbody2D playerRigidbody; // Reference to the player's Rigidbody2D

    private bool isCollidingWithSheep = false; // To check if the player is currently colliding with the sheep

    public KeyCode mountKey = KeyCode.E;  // Key to mount the sheep
    public KeyCode dismountKey = KeyCode.Q; // Key to dismount the sheep
    private Vector3 playerMountOffset = new Vector3(0, -0.5f, 0); // Offset from the sheep's position for mounting

    private PlayerMovment playerMovementScript; // Reference to the player movement script

    private void Start()
    {
        // Get the reference to the player's Rigidbody2D and the existing player movement script
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerMovementScript = GetComponent<PlayerMovment>();
    }

    private void Update()
    {
        // Mount the sheep when the player presses the 'Mount' key (e.g., "E") and is colliding with the sheep
        if (Input.GetKeyDown(mountKey) && isCollidingWithSheep && !isRiding)
        {
            MountSheep();
        }

        // Dismount the sheep when the player presses the 'Dismount' key (e.g., "Q") while riding
        if (Input.GetKeyDown(dismountKey) && isRiding)
        {
            DismountSheep();
        }

        // If the player is riding the sheep, adjust their position to be centered and down relative to the sheep
        if (isRiding)
        {
            Vector3 ridingPosition = sheep.transform.position + playerMountOffset;
            transform.position = ridingPosition;

            // Allow the player to move with the sheep using their existing movement script
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = true;  // Enable player movement
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with the sheep
        if (collision.gameObject.CompareTag("Sheep"))
        {
            // Initialize the sheep GameObject and Rigidbody2D only once when collision happens
            if (sheep == null)
            {
                sheep = collision.gameObject;  // Set sheep GameObject
                sheepRigidbody = sheep.GetComponent<Rigidbody2D>();  // Get sheep's Rigidbody2D component
            }

            isCollidingWithSheep = true; // Set flag to true indicating the player is colliding with the sheep
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player stops colliding with the sheep
        if (collision.gameObject.CompareTag("Sheep"))
        {
            isCollidingWithSheep = false; // Set flag to false when no longer colliding
        }
    }

    private void MountSheep()
    {
        if (sheepRigidbody == null || sheep == null || sheep.transform == null)
        {
            Debug.LogError("Sheep Rigidbody or Transform is not assigned.");
            return;
        }

        // Set the sheep's Rigidbody2D to kinematic to prevent it from being affected by physics
        sheepRigidbody.bodyType = RigidbodyType2D.Kinematic;

        // Freeze the player's Rigidbody2D to prevent acceleration
        if (playerRigidbody != null)
        {
            playerRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        // Make the sheep a child of the player so it moves with the player
        sheep.transform.parent = transform;

        // Lock the player's position to the sheep (no movement)
        transform.position = sheep.transform.position + playerMountOffset;

        // Disable the player's movement script while riding
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;  // Disable movement during mounting
        }

        // Set the flag to true indicating the player is riding
        isRiding = true;
    }

    private void DismountSheep()
    {
        // Unparent the sheep from the player
        sheep.transform.parent = null;

        // Set the flag to false indicating the player is no longer riding
        isRiding = false;

        // Set the sheep's Rigidbody2D back to dynamic if needed
        if (sheepRigidbody != null)
        {
            sheepRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        // Re-enable the player's Rigidbody2D to allow physics interactions again
        if (playerRigidbody != null)
        {
            playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        // Re-enable the player's movement script after dismounting
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;  // Re-enable movement after dismounting
        }
    }
}
