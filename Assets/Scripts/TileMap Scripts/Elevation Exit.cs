using UnityEngine;

public class ElevationExit : MonoBehaviour
{
    [SerializeField] private Collider2D[] mountainColliders;
    [SerializeField] private Collider2D[] boundaryColliders;

    [SerializeField] private string Tagplayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tagplayer))
        {
            foreach (Collider2D mountain in mountainColliders)
            {
                mountain.enabled = true;
            }

            foreach (Collider2D boundary in boundaryColliders)
            {
                boundary.enabled = false;
            }

            other.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

    }
}
