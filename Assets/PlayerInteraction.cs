using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Tilemap collusionHighTilemap;
    [SerializeField] private Tilemap collusionLowTilemap;
    [SerializeField] private Tilemap nonCollusionHighTilemap;
    [SerializeField] private Tilemap nonCollusionLowTilemap;

    [SerializeField] private List<TileBase> mountainTiles; // List of mountain tiles
    [SerializeField] private GameObject pickaxe;

    private bool hasPickaxe = false;
    private PlayerMovment playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovment>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickaxe"))
        {
            hasPickaxe = true;
            pickaxe.SetActive(false);  // Hide the pickaxe when collected
        }
    }

    // Function to erase the mountain tiles the player is facing
    public void EraseMountainInDirection()
    {
        if (hasPickaxe)
        {
            // Determine the direction the player is facing (left or right)
            Vector2 direction = (playerMovement.GetFacingDirection() == 1) ? Vector2.right : Vector2.left;
            Vector3 playerPosition = transform.position;

            // Get the current position of the player in tilemap space
            Vector3Int currentTilePos = collusionHighTilemap.WorldToCell(playerPosition);

            // Use flood fill to find and erase all connected mountain tiles
            EraseConnectedMountainTiles(currentTilePos, direction);
        }
    }

    // Flood fill function to erase connected mountain tiles
    private void EraseConnectedMountainTiles(Vector3Int startPos, Vector2 direction)
    {
        // A queue to store positions that need to be checked
        Queue<Vector3Int> tilesToCheck = new Queue<Vector3Int>();
        HashSet<Vector3Int> visitedTiles = new HashSet<Vector3Int>(); // To avoid revisiting the same tile

        tilesToCheck.Enqueue(startPos);
        visitedTiles.Add(startPos);

        // Directions: left, right, up, down (4-connected)
        Vector2[] directions = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };

        // Perform flood fill
        while (tilesToCheck.Count > 0)
        {
            Vector3Int currentPos = tilesToCheck.Dequeue();

            // Check the tile at the current position in all directions
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3Int neighborPos = currentPos + new Vector3Int((int)directions[i].x, (int)directions[i].y, 0);

                // Check if the neighboring tile is a mountain tile and hasn't been visited
                if (!visitedTiles.Contains(neighborPos) && IsMountainTile(neighborPos))
                {
                    tilesToCheck.Enqueue(neighborPos);  // Add the neighbor to the queue
                    visitedTiles.Add(neighborPos);      // Mark it as visited

                    // Erase the mountain tile at that position
                    EraseTileAtPosition(collusionHighTilemap, neighborPos);
                    EraseTileAtPosition(collusionLowTilemap, neighborPos);
                    EraseTileAtPosition(nonCollusionHighTilemap, neighborPos);
                    EraseTileAtPosition(nonCollusionLowTilemap, neighborPos);
                }
            }
        }
    }

    // Check if the tile at a given position is part of the mountain (matches any of the mountain tiles)
    private bool IsMountainTile(Vector3Int position)
    {
        // Check all mountain tiles in the list
        TileBase tileAtPos = collusionHighTilemap.GetTile(position);  // Check collusionHighTilemap (you can check other tilemaps if needed)

        foreach (TileBase mountainTile in mountainTiles)
        {
            if (tileAtPos == mountainTile)
                return true;
        }

        return false;  // Return false if it's not a mountain tile
    }

    // Erase the tile at a specific position in the Tilemap
    private void EraseTileAtPosition(Tilemap tilemap, Vector3Int position)
    {
        tilemap.SetTile(position, null);  // Erase the tile by setting it to null
    }
}
