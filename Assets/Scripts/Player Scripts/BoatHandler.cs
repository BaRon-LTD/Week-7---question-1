using UnityEngine;

public class BoatHandler : MonoBehaviour
{
    public GameObject boat; // Reference to the boat
    public float detectionRadius = 2f; // Proximity radius to the boat

    private bool isOnBoat = false; // Flag to check if the player is on the boat
    private PlayerMovment playerMovement; // Reference to the player's movement script
    private BoatMovement boatMovement; // Reference to the boat's movement script
    private SpriteRenderer playerRenderer; // Reference to the player's SpriteRenderer
    private SpriteRenderer boatRenderer; // Reference to the boat's SpriteRenderer

    private int originalPlayerSortingOrder; // To store the player's original sorting order

    void Start()
    {
        // Get references to the movement scripts and renderers
        playerMovement = GetComponent<PlayerMovment>();
        boatMovement = boat.GetComponent<BoatMovement>();
        playerRenderer = GetComponent<SpriteRenderer>();
        boatRenderer = boat.GetComponent<SpriteRenderer>();

        // Store the player's original sorting order
        if (playerRenderer != null)
        {
            originalPlayerSortingOrder = playerRenderer.sortingOrder;
        }

        // Ensure the boat movement is disabled initially
        if (boatMovement != null)
        {
            boatMovement.enabled = false;
        }
    }

    void Update()
    {
        // Check for proximity to the boat
        if (Vector2.Distance(transform.position, boat.transform.position) <= detectionRadius && !isOnBoat)
        {
            if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to mount the boat
            {
                EnterBoat();
            }
        }

        // If the player is on the boat, press 'Q' to leave the boat
        if (isOnBoat && Input.GetKeyDown(KeyCode.Q))
        {
            ExitBoat();
        }
    }

    void EnterBoat()
    {
        // Player enters the boat
        isOnBoat = true;

        // Disable the player’s movement script
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Enable the boat's movement script
        if (boatMovement != null)
        {
            boatMovement.enabled = true;
        }

        // Set player's sorting order to match the boat's sorting order
        if (playerRenderer != null && boatRenderer != null)
        {
            playerRenderer.sortingOrder = boatRenderer.sortingOrder;
        }

        // Make the player the child of the boat so it follows
        transform.SetParent(boat.transform);

        // Position the player above the boat (Adjust Y position)
        transform.localPosition = new Vector3(0f, 0.1f, 0f);
    }

    void ExitBoat()
    {
        // Player exits the boat
        isOnBoat = false;

        // Enable the player’s movement script
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.bodyType = RigidbodyType2D.Dynamic;
        }

        // Disable the boat's movement script
        if (boatMovement != null)
        {
            boatMovement.enabled = false;
        }

        // Detach the player from the boat
        transform.SetParent(null);

        // Restore the player's original sorting order
        if (playerRenderer != null)
        {
            playerRenderer.sortingOrder = originalPlayerSortingOrder;
        }
    }
}
