using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public FirstPersonPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < 5)
            {
                GetComponent<Animator>().SetTrigger("open");
            }

        }
    }
}
