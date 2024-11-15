using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float pointA = 0f;  // Starting X position
    public float pointB = 0f;   // Ending X position
    public float speed = 2f;    // Speed of movement

    private Vector2 target;

    void Start()
    {
        // Set initial target to point B on the X-axis
        target = new Vector2(pointB, transform.position.y);
    }

    void Update()
    {
        // Move platform horizontally between point A and point B
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Switch target when reaching one of the points
        if (Mathf.Approximately(transform.position.x, pointB))
        {
            target = new Vector2(pointA, transform.position.y);
        }
        else if (Mathf.Approximately(transform.position.x, pointA))
        {
            target = new Vector2(pointB, transform.position.y);
        }
    }
}
