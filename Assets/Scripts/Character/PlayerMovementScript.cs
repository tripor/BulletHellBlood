using UnityEngine;
using UnityEngine.XR;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 12f;
    [SerializeField]
    private float accelarationOnFreeze = 100f;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 3f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool usingXr = false;

    private bool freeCamera = false;

    private float time;
    [SerializeField]
    private float slowMotionTime = 10f;
    private Vector3 positionBeforeFreeze;
    private Vector3 positionStartFreeze;
    [SerializeField]
    private float returnToPositionTime = 2f;
    private float returnToPositionTimer = 0;
    private bool returningToPostion = false;
    private float xStopped = 0;
    private float zStopped = 0;


    public GameObject XR;
    public GameObject Non_XR;
    public Transform XRCamera;


    private CharacterController controller;

    public Transform groundCheck;
    public LayerMask groundMask;

    public bool FreeCamera { get => freeCamera; set => freeCamera = value; }
    public bool ReturningToPostion { get => returningToPostion; set => returningToPostion = value; }

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        controller = this.GetComponent<CharacterController>();
        if (XRSettings.isDeviceActive)
        {
            Non_XR.SetActive(false);
            XR.SetActive(true);
            usingXr = true;
        }
        else
        {
            Non_XR.SetActive(true);
            XR.SetActive(false);
            usingXr = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!usingXr && Input.GetKeyDown(KeyCode.Q) && !freeCamera && !returningToPostion)
        {
            freeCamera = true;
            time = 0;
            positionBeforeFreeze = transform.position;
            Time.timeScale = 0;
            xStopped = 0;
            zStopped = 0;
        }
        if (freeCamera)
        {
            time += Time.unscaledDeltaTime;
            if (time >= slowMotionTime)
            {
                freeCamera = false;
                time = 0;
                returnToPositionTimer = 0;
                returningToPostion = true;
                positionStartFreeze = transform.position;
            }
        }
        if (returningToPostion)
        {
            returnToPositionTimer += Time.unscaledDeltaTime;
            if (returnToPositionTimer >= returnToPositionTime)
            {
                returnToPositionTimer = returnToPositionTime;
                returningToPostion = false;
                Time.timeScale = 1;
            }
            transform.position = Vector3.Lerp(positionStartFreeze, positionBeforeFreeze, returnToPositionTimer / returnToPositionTime);
        }
        else
        {
            if (!freeCamera)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }

                if (!usingXr)
                {
                    float x = Input.GetAxis("Horizontal");
                    float z = Input.GetAxis("Vertical");
                    Vector3 move = transform.right * x + transform.forward * z;

                    controller.Move(move * speed * Time.deltaTime);

                    if (Input.GetButtonDown("Jump") && isGrounded)
                    {
                        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                    }
                }

                this.velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);
            }
            else if (!usingXr)
            {
                if (Input.GetKey(KeyCode.W)) zStopped += accelarationOnFreeze * Time.unscaledDeltaTime;
                else if (Input.GetKey(KeyCode.S)) zStopped -= accelarationOnFreeze * Time.unscaledDeltaTime;
                else
                {
                    if (zStopped > 0)
                    {
                        zStopped -= accelarationOnFreeze * Time.unscaledDeltaTime;
                        zStopped = Mathf.Clamp(zStopped, 0, 1);
                    }
                    else if (zStopped < 0)
                    {
                        zStopped += accelarationOnFreeze * Time.unscaledDeltaTime;
                        zStopped = Mathf.Clamp(zStopped, -1, 0);
                    }
                }
                if (Input.GetKey(KeyCode.D)) xStopped += accelarationOnFreeze * Time.unscaledDeltaTime;
                else if (Input.GetKey(KeyCode.A)) xStopped -= accelarationOnFreeze * Time.unscaledDeltaTime;
                else
                {
                    if (xStopped > 0)
                    {
                        xStopped -= accelarationOnFreeze * Time.unscaledDeltaTime;
                        xStopped = Mathf.Clamp(xStopped, 0, 1);
                    }
                    else if (xStopped < 0)
                    {
                        xStopped += accelarationOnFreeze * Time.unscaledDeltaTime;
                        xStopped = Mathf.Clamp(xStopped, -1, 0);
                    }
                }
                zStopped = Mathf.Clamp(zStopped, -1, 1);
                xStopped = Mathf.Clamp(xStopped, -1, 1);
                Vector3 move = transform.right * xStopped + this.Non_XR.transform.forward * zStopped;
                controller.Move(move * speed * Time.unscaledDeltaTime);
            }
        }
    }

    public void triggerXR(float triggerValue)
    {

    }

    public void primaryButtonDown()
    {
        freeCamera = true;
        time = 0;
        Time.timeScale = 0;
    }
    public void axis2DXR(Vector2 axis)
    {
        if (freeCamera)
        {
            Vector3 move = transform.right * axis.x + this.XRCamera.forward * axis.y;
            controller.Move(move * speed * Time.unscaledDeltaTime);
        }
        else
        {
            Vector3 move = transform.right * axis.x + transform.forward * axis.y;
            controller.Move(move * speed * Time.deltaTime);
        }
    }
}
