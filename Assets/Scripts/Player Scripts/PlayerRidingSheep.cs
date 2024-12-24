using UnityEngine;

public class PlayerRideSheep : MonoBehaviour
{
    public GameObject sheep; // Reference to the sheep GameObject
    public float detectionRadius = 2f; // Proximity radius

    private bool isRiding = false; // Whether the player is riding the sheep
    private Collider2D sheepCollider; // Reference to the sheep's Collider2D

    void Start()
    {
        // Get the sheep's collider component
        sheepCollider = sheep.GetComponent<Collider2D>();
        if (sheepCollider == null)
        {
            Debug.LogError("Sheep does not have a Collider2D component.");
        }
    }

    void Update()
    {
        // Detect proximity to sheep and mount if 'E' is pressed
        if (Vector2.Distance(transform.position, sheep.transform.position) <= detectionRadius && !isRiding)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartRiding();
            }
        }

        // Demount if 'Q' is pressed while riding
        if (isRiding && Input.GetKeyDown(KeyCode.Q))
        {
            StopRiding();
        }
    }

    void StartRiding()
    {
        isRiding = true;

        // Make sheep the child of the player
        sheep.transform.SetParent(transform);
        sheep.transform.localPosition = Vector3.zero; // Align sheep with player position

        // Set sheep's collider to trigger
        if (sheepCollider != null)
        {
            sheepCollider.isTrigger = true;
        }
    }

    void StopRiding()
    {
        isRiding = false;

        // Detach sheep from the player
        sheep.transform.SetParent(null);

        // Ensure sheep stays at the same position
        sheep.transform.position = transform.position;

        // Disable trigger on the sheep's collider
        if (sheepCollider != null)
        {
            sheepCollider.isTrigger = false;
        }
    }
}
