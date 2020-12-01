using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{     
    public Transform playerBody;
    public float mouseSensitivity = 100f;

    float xRotation = 0;

    public float distanceTraveled;
    public float cornerDistance;
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
        xRotation = Mathf.Clamp(xRotation, -60f, 60f); // so player can't look down or up more than 90;

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);



        // keyboard & controller settings
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        

        // battle launching; 
        distanceTraveled = Vector3.Distance(transform.position, lastPosition);
        if (distanceTraveled > 3f)
        {
            steps++;
            distanceTraveled = 0;
            lastPosition = playerBody.transform.position;
        }

        foreach(GameObject corner in corners)
        {
            if (Vector3.Distance(corner.transform.position, playerBody.transform.position) < 11)
            {
                closestCorner = corner.transform.position;
            }
        }

        cornerDistance = Vector3.Distance(transform.position, closestCorner);
    }
}
