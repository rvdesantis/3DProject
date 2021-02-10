using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject player;
    public bool locked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 5)
            {
                if (locked == false)
                {
                    GetComponent<Animator>().SetTrigger("open");
                }       
                if (locked == true)
                {
                    // null
                }
            }


        }
    }
}
