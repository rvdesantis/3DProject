using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapdoor : MonoBehaviour
{
    public FirstPersonPlayer player;
    public GameObject fallChecker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < 8f)
        {
            GetComponent<Animator>().SetTrigger("open");
        }

        if (Vector3.Distance(player.transform.position, this.transform.position) < 1)
        {
            GetComponent<Animator>().SetTrigger("fall");
        }
    }
}
