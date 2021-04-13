using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 12f;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 3f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool usingXr = false;


    public GameObject XR;
    public GameObject Non_XR;


    private CharacterController controller;

    public Transform groundCheck;
    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
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

    public void triggerXR(float triggerValue)
    {

    }

    public void primaryButtonDown()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }
    public void axis2DXR(Vector2 axis)
    {
        Vector3 move = transform.right * axis.x + transform.forward * axis.y;
        controller.Move(move * speed * Time.deltaTime);
    }
}
