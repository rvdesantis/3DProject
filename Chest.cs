using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public FirstPersonPlayer player;
    public GameObject contents;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < 5)
            {
                GetComponent<Animator>().SetTrigger("openLid");
            }

        }

    }
}
