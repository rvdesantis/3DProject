
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    public Transform target;
    public GameObject meleeCamTarget;
    public Vector3 desiredPosition;
    public Vector3 meleePosition;
    public Vector3 offset; // space between target and camera position

    public float smoothSpeed = 0.125f; // always needs to be between 0 - 1;

    private void Start()
    {
        desiredPosition = target.position + offset;
    }





    private void Update()
    {
        
    }

    private void LateUpdate() // for use after target animation movement
    {        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }

}
