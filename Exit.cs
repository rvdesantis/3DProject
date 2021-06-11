using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public FirstPersonPlayer player;
    public AreaController areaController;
    bool inRange;
    private void Start()
    {
        areaController = FindObjectOfType<AreaController>();        
    }

    public virtual void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 2.5f)
        {
            inRange = true;
            areaController.areaUI.messageText.text = "Leave Dungeon?\nYou will keep all found items, gold, and XP";
            areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                AreaController.respawnPoint = transform.position;                
                HeroSelect.dunReturn = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Character Select");
            }
        }
        if (Vector3.Distance(player.transform.position, transform.position) >= 2.5f)
        {
            if (inRange == true)
            {
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                inRange = false;
            }
            
        }
    }


}
