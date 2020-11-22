using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayer : MonoBehaviour
{
    public GameObject torch;
    public Light playerLight;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<Animator>().SetTrigger("turnLeft");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<Animator>().SetTrigger("turnRight");
        }

    }
}
