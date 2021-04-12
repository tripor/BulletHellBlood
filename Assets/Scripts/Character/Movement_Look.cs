using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Look : MonoBehaviour
{

    [SerializeField]
    private float mouseSensitivity = 100f;
    private float xRotation = 0f;

    public Transform playerBody;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        this.xRotation -= mouseY;
        this.xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerBody.Rotate(Vector3.up * mouseX);
        this.transform.localRotation = Quaternion.Euler(this.xRotation, 0f, 0f);
    }
}
