using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }

    #region Fpp Variable
    public float MoveSpeed = 3f;
    public float SprintSpeedMultiplier = 2f;
    public Rigidbody rb;
    public float Vertical;
    public float Horizontal;
    public Vector2 inputDir;
    #endregion

    #region Tpp Variable
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    public bool isFpp;
    Transform cameraT;
    #endregion

    #region Jump Variable
    public float jumpForce = 8f;
    public bool isGrounded;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraT = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFpp)
        {
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");
            Vector3 Movement = new Vector3(Horizontal, 0f, Vertical) * MoveSpeed * Time.deltaTime;

            // Sprint logic
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Movement *= SprintSpeedMultiplier;
            }

            transform.Translate(Movement);
        }
        else
        {
            inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            if (inputDir != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

                // Calculate movement direction based on input without relying on the forward direction
                Vector3 moveDir = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
                float moveSpeed = MoveSpeed;

                // Sprint logic
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    moveSpeed *= SprintSpeedMultiplier;
                }

                transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
            }
        }

        // Jumping logic
        if (!isFpp && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (!isFpp)
        {
            // Ground detection logic
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f);
        }
    }
}
