using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    public AreaController areaController;

    public Transform playerBody;
    public float mouseSensitivity = 100f;

    float xRotation = 0;

    public float distanceTraveled;    
    public float steps;


    public float speed = 12;
    public float gravity = -3f;
    Vector3 velocity; // for gravity and falling;

    public CharacterController controller;

    // for battle start

    public Vector3 lastPosition;
    public List<GameObject> corners;
    public Vector3 closestCorner;

    
    void Start()
    {
        lastPosition = transform.position;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // must use Time.deltaTime since this is in Update()
        // mouse settings
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; //using -= because rotation is flipped)
        xRotation = Mathf.Clamp(xRotation, 0, 0); // so player can't look down or up;

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);



        // keyboard & CharacterController settings
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        // Joystick

        float jx = Input.GetAxis("JHorizontal");
        if (areaController.areaUI.uiNavigation == false)
        {
            if (jx > .5f)
            {
                transform.localRotation = Quaternion.Euler(90, 0f, 0f);
                playerBody.Rotate(Vector3.up * (jx * .9f));

            }
            if (jx < .5f)
            {
                transform.localRotation = Quaternion.Euler(-90, 0f, 0f);
                playerBody.Rotate(Vector3.up * (jx * .9f));
            }
        }

        // battle launching; 
        distanceTraveled = Vector3.Distance(transform.position, lastPosition);
        if (distanceTraveled > 3f)
        {
            steps++;
            distanceTraveled = 0;
            lastPosition = playerBody.transform.position;
        }
    }
}
