using UnityEngine;

public class SpotlightFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public Vector3 offset;   // Adjust the position offset

    void Update()
    {
        if (player != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(player.position + offset);
        }
    }
}
