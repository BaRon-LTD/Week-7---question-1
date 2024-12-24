// using UnityEngine;

// public class PlayerRideSheep : MonoBehaviour
// {
//     public GameObject sheep; // Reference to the sheep GameObject
//     public float detectionRadius = 2f; // Proximity radius

//     private bool isRiding = false; // Whether the player is riding the sheep
//     private Collider2D sheepCollider; // Reference to the sheep's Collider2D

//     void Start()
//     {
//         // Get the sheep's collider component
//         sheepCollider = sheep.GetComponent<Collider2D>();
//         if (sheepCollider == null)
//         {
//             Debug.LogError("Sheep does not have a Collider2D component.");
//         }
//     }

//     void Update()
//     {
//         // Detect proximity to sheep and mount if 'E' is pressed
//         if (Vector2.Distance(transform.position, sheep.transform.position) <= detectionRadius && !isRiding)
//         {
//             if (Input.GetKeyDown(KeyCode.E))
//             {
//                 StartRiding();
//             }
//         }

//         // Demount if 'Q' is pressed while riding
//         if (isRiding && Input.GetKeyDown(KeyCode.Q))
//         {
//             StopRiding();
//         }
//     }

//     void StartRiding()
//     {
//         isRiding = true;

//         // Make sheep the child of the player
//         sheep.transform.SetParent(transform);
//         sheep.transform.localPosition = Vector3.zero; // Align sheep with player position

//         // Set sheep's collider to trigger
//         if (sheepCollider != null)
//         {
//             sheepCollider.isTrigger = true;
//         }
//     }

//     void StopRiding()
//     {
//         isRiding = false;

//         // Detach sheep from the player
//         sheep.transform.SetParent(null);

//         // Ensure sheep stays at the same position
//         sheep.transform.position = transform.position;

//         // Disable trigger on the sheep's collider
//         if (sheepCollider != null)
//         {
//             sheepCollider.isTrigger = false;
//         }
//     }
// }


using UnityEngine;
using UnityEngine.Tilemaps; // <-- Add this namespace

public class PlayerRideSheep : MonoBehaviour
{
    public GameObject sheep; // Reference to the sheep GameObject
    public float detectionRadius = 2f; // Proximity radius
    private bool isRiding = false; // Whether the player is riding the sheep
    private Collider2D sheepCollider; // Reference to the sheep's Collider2D

    // Expose this array in the Inspector so you can assign the TilemapCollider2D components
    public TilemapCollider2D[] tilemapColliders;

    // Reference to the player's SpriteRenderer
    private SpriteRenderer playerSpriteRenderer;

    // Store the original sorting layer
    private string originalSortingLayer;

    // Array to hold references to the scripts you want to disable
    public MonoBehaviour[] scriptsToDisable; // Add your scripts here from the Inspector

    void Start()
    {
        // Get the sheep's collider component
        sheepCollider = sheep.GetComponent<Collider2D>();
        if (sheepCollider == null)
        {
            Debug.LogError("Sheep does not have a Collider2D component.");
        }

        // Get the player's SpriteRenderer component
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer == null)
        {
            Debug.LogError("Player does not have a SpriteRenderer component.");
        }
        else
        {
            // Store the player's original sorting layer
            originalSortingLayer = playerSpriteRenderer.sortingLayerName;
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

        // Disable all Tilemap colliders that are assigned in the Inspector
        foreach (var tilemapCollider in tilemapColliders)
        {
            if (tilemapCollider != null)
            {
                tilemapCollider.enabled = false;
            }
        }

        // Change the player's sorting layer to 11 when riding
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sortingLayerName = "Layer11"; // You should make sure you have a sorting layer named "Layer11"
            playerSpriteRenderer.sortingOrder = 11;
        }

        // Disable the selected scripts while riding
        foreach (var script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = false;
            }
        }

        Debug.Log("Selected Tilemap Colliders disabled, player sorting layer changed to 11, and scripts disabled.");
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

        // Re-enable all Tilemap colliders that were disabled
        foreach (var tilemapCollider in tilemapColliders)
        {
            if (tilemapCollider != null)
            {
                tilemapCollider.enabled = true;
            }
        }

        // Revert the player's sorting layer back to the original when not riding
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sortingLayerName = originalSortingLayer;
            playerSpriteRenderer.sortingOrder = 0; // Reset sorting order to the default value
        }

        // Re-enable the selected scripts when dismounting
        foreach (var script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = true;
            }
        }

        Debug.Log("Selected Tilemap Colliders re-enabled, player sorting layer reverted, and scripts re-enabled.");
    }
}
