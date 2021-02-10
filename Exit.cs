using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public FirstPersonPlayer player;


    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 5)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Character Select");
            }
        }
    }


}
