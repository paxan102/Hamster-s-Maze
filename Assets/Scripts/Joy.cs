using UnityEngine;
using UnityEngine.UI;

public class Joy : MonoBehaviour
{
    [SerializeField] private Image pointer;

    public void Enable(Vector2 startPosition)
    {
        this.startPosition = startPosition;
        gameObject.transform.position = startPosition;
        pointer.gameObject.transform.position = startPosition;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public bool[] Move(Vector2 position)
    {
        var directionVector = position - startPosition;
        bool[] direction = { false, false, false, false }; // up, right, down, left

        if (directionVector.magnitude >= RADIUS_OF_DEAD_ZONE)
        {
            if (directionVector.y > 0)
                direction[0] = true;
            else
                direction[2] = true;

            if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y) / 2)
            {
                if (directionVector.x > 0)
                    direction[1] = true;
                else
                    direction[3] = true;

                if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y) * 2)
                {
                    direction[0] = false;
                    direction[2] = false;
                }
            }
        }

        if (directionVector.magnitude > RADIUS_OF_INNER_CIRCLE)
        {
            directionVector.Normalize();

            var newX = startPosition.x + directionVector.x * RADIUS_OF_INNER_CIRCLE;
            var newY = startPosition.y + directionVector.y * RADIUS_OF_INNER_CIRCLE;

            position.Set(newX, newY);
        }
        
        pointer.gameObject.transform.position = position;

        return direction;
    }

    private const float RADIUS_OF_INNER_CIRCLE = 65;
    private const float RADIUS_OF_DEAD_ZONE = 20;

    private Vector2 startPosition;
}
