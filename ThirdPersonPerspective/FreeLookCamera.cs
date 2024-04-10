using UnityEngine;
//Attach This Script to Main Camera Its ok If Its in Child of Player
public class FreeLookCamera : MonoBehaviour
{
    public Transform target; // Target object to look at (e.g., player)
    public float rotationSpeed = 3.0f; // Speed of camera rotation
    public float zoomSpeed = 1.0f; // Speed of camera zoom
    public float minZoomDistance = 3.0f; // Minimum distance for camera zoom
    public float maxZoomDistance = 10.0f; // Maximum distance for camera zoom
    public float dollySpeed = 2.0f; // Speed of camera dolly effect
    public float dollyDistance = 2.0f; // Distance threshold for triggering dolly effect
    public float minDollyPosition = 3.0f; // Minimum distance from player during dolly effect
    public LayerMask collisionLayer; // Layer mask for collision detection

    private float horizontalRotation = 0.0f; // Horizontal rotation around the target
    private float verticalRotation = 0.0f; // Vertical rotation around the target
    private float distance = 5.0f; // Current distance from the target

    public GameObject TouchScreen;
    public bool isMobilePlatform;
    void Start()
    {
        // Initialize camera rotation angles
        Vector3 angles = transform.eulerAngles;
        horizontalRotation = angles.y;
        verticalRotation = angles.x;

        // Initialize camera distance
        distance = Vector3.Distance(transform.position, target.position);
    }

    void LateUpdate()
    {
        if (isMobilePlatform)
        {
            //Mobile Mouse Input
             horizontalRotation += TouchScreen.GetComponent<FixedTouchField>().TouchDist.x * Time.deltaTime * rotationSpeed;
            verticalRotation -= TouchScreen.GetComponent<FixedTouchField>().TouchDist.y * Time.deltaTime * rotationSpeed;

        }
        else
        {
            // Rotate the camera based on user input
            horizontalRotation += Input.GetAxis("Mouse X") * rotationSpeed;
            verticalRotation -= Input.GetAxis("Mouse Y") * rotationSpeed;
        }
       

      
        verticalRotation = Mathf.Clamp(verticalRotation, -80.0f, 80.0f);

        // Zoom in/out the camera based on mouse scroll wheel input
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, minZoomDistance, maxZoomDistance);

        // Calculate camera position and rotation
        Quaternion rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0.0f);
        Vector3 position = target.position - (rotation * Vector3.forward * distance);

        // Apply position and rotation to the camera
        transform.rotation = rotation;

        // Check for collisions
        RaycastHit hit;
        if (Physics.Raycast(target.position, -transform.forward, out hit, maxZoomDistance, collisionLayer))
        {
            // If collision detected, move the camera closer to the player within the limit
            float newDistance = Mathf.Clamp(hit.distance, minZoomDistance, maxZoomDistance);
            position = target.position - (rotation * Vector3.forward * newDistance);
        }

        // Perform dolly effect if an object is close to the camera
        if (Vector3.Distance(transform.position, target.position) < dollyDistance)
        {
            // Move the camera closer to the player
            position = Vector3.Lerp(transform.position, target.position + (transform.position - target.position).normalized * minDollyPosition, Time.deltaTime * dollySpeed);
        }

        // Apply the final position to the camera
        transform.position = position;
    }
}
