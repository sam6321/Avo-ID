using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Bounds cameraBounds = new Bounds(Vector3.zero, Vector3.positiveInfinity);

    [SerializeField]
    private Vector2Int cameraScreenMargins = new Vector2Int(50, 50);

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float zoomSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraMovement = GetCameraMovement(Input.mousePosition);
        transform.Translate(cameraMovement, Space.World);
    }

    Vector3 GetCameraMovement(Vector2 mousePosition)
    {
        Vector3 movement = new Vector3();

        if(mousePosition.x < cameraScreenMargins.x)
        {
            // Move left
            movement.x -= moveSpeed * Time.deltaTime;
        }
        else if(mousePosition.x > Screen.width - cameraScreenMargins.x)
        {
            // Move right
            movement.x += moveSpeed * Time.deltaTime;
        }

        if(mousePosition.y < cameraScreenMargins.y)
        {
            // Move down
            movement.z -= moveSpeed * Time.deltaTime;
        }
        else if(mousePosition.y > Screen.height - cameraScreenMargins.y)
        {
            // Move up
            movement.z += moveSpeed * Time.deltaTime;
        }

        return movement;
    }
}
