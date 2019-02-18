using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void SetupMazeCharacteristics(float mazeHeight, float mazeWidth, Player player)
    {
        if (!mainCamera)
            mainCamera = Camera.main;

        if (!this.player)
            this.player = player;

        this.mazeWidth = mazeWidth;

        currentSize = mazeWidth / 2 * Screen.height / Screen.width;
        mainCamera.orthographicSize = currentSize;
        maxSize = currentSize;
        
        minX = mazeWidth / 2;
        maxX = minX;
        minY = maxSize;
        maxY = mazeHeight - maxSize;

        mainCamera.gameObject.transform.SetPositionAndRotation(new Vector2(minX, minY), new Quaternion());
    }

    #region private

    private Camera mainCamera;
    private Player player;
    private float mazeWidth;
    private float currentSize;
    private float minSize = 1;
    private float maxSize;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Scale(float scale)
    {
        float newSize = currentSize + scale;
        if (newSize > maxSize)
            return;
        if (newSize < minSize)
            return;

        minY = newSize;
        maxY += currentSize - newSize;
        minX *= newSize / currentSize;
        maxX = mazeWidth - minX;

        currentSize = newSize;
        mainCamera.orthographicSize = currentSize;

        float newY = mainCamera.gameObject.transform.position.y;
        float newX = mainCamera.gameObject.transform.position.x;
        
        newY = Mathf.Min(maxY, newY);
        newY = Mathf.Max(minY, newY);
        newX = Mathf.Min(maxX, newX);
        newX = Mathf.Max(minX, newX);

        mainCamera.gameObject.transform.SetPositionAndRotation(new Vector2(newX, newY), new Quaternion());
    }

    private void Update()
    {
        float move = 0.15f;

        float dirY = 0;
        float dirX = 0;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
            if (scroll > 0)
                Scale(0.2f);
            else
                Scale(-0.2f);

        /*if (Input.GetKey(KeyCode.W))
            dirY = move;
        if (Input.GetKey(KeyCode.A))
            dirX = -move;
        if (Input.GetKey(KeyCode.S))
            dirY = -move;
        if (Input.GetKey(KeyCode.D))
            dirX = move;

        if (dirX == 0 && dirY == 0)
            return;*/

        float newY = player.gameObject.transform.position.y;
        float newX = player.gameObject.transform.position.x;

        newY = Mathf.Min(maxY, newY);
        newY = Mathf.Max(minY, newY);
        newX = Mathf.Min(maxX, newX);
        newX = Mathf.Max(minX, newX);

        mainCamera.gameObject.transform.SetPositionAndRotation(new Vector2(newX, newY), new Quaternion());
    }

    #endregion
}