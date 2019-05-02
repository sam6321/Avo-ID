using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Bounds cameraBounds = new Bounds(Vector3.zero, Vector3.positiveInfinity);

    [SerializeField]
    private Utils.RangeFloat zoomRange = new Utils.RangeFloat(10, 30);

    [SerializeField]
    private Vector2Int cameraScreenMargins = new Vector2Int(50, 50);

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float lerpMoveSpeed = 1.0f;

    [SerializeField]
    private float zoomSpeed = 1.0f;

    [SerializeField]
    private float lerpZoomSpeed = 1.0f;

    private Vector3 target = new Vector3();
    private Vector3 lerpTarget = new Vector3();

    private float distanceFromTarget = 20.0f;
    private float lerpDistance = 20.0f;

    // Update is called once per frame
    void Update()
    {
        target += GetCameraMovement(Input.mousePosition);
        target = cameraBounds.ClosestPoint(target); // Clamp to bounds

        distanceFromTarget -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
        distanceFromTarget = zoomRange.Clamp(distanceFromTarget);

        float dist = Vector3.Distance(lerpTarget, target);
        lerpTarget = Vector3.MoveTowards(lerpTarget, target, dist * lerpMoveSpeed * Time.deltaTime);

        dist = Mathf.Abs(distanceFromTarget - lerpDistance);
        lerpDistance = Mathf.MoveTowards(lerpDistance, distanceFromTarget, dist * lerpZoomSpeed * Time.deltaTime);

        Vector3 localForward = transform.TransformDirection(Vector3.forward);
        transform.position = lerpTarget - localForward * lerpDistance;
    }

    private Vector3 GetCameraMovement(Vector2 mousePosition)
    {
        Vector3 movement = new Vector3();

        if(mousePosition.x < cameraScreenMargins.x)
        {
            // Move left
            movement -= Vector3.right * moveSpeed * Time.deltaTime;
        }
        else if(mousePosition.x > Screen.width - cameraScreenMargins.x)
        {
            // Move right
            movement += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if(mousePosition.y < cameraScreenMargins.y)
        {
            // Move backward
            movement -= Vector3.forward * moveSpeed * Time.deltaTime;
        }
        else if(mousePosition.y > Screen.height - cameraScreenMargins.y)
        {
            // Move forward
            movement += Vector3.forward * moveSpeed * Time.deltaTime;
        }

        return movement;
    }
}
