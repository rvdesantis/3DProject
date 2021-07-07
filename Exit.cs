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
        if (Vector3.Distance(player.transform.position, transform.position) < 2.75f)
        {
            inRange = true;
            areaController.areaUI.messageText.text = "Leave Dungeon?\nYou will keep all found items, gold, and XP";
            areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                AreaController.respawnPoint = transform.position;
                AreaController.battleReturn = false;
                DunBuilder.createDungeon = true;
                HeroSelect.dunReturn = true;
                PlayerPrefs.SetInt("Gold", StaticMenuItems.goldCount); PlayerPrefs.Save();
                StaticMenuItems.goldFound = 0;
                StaticMenuItems.XPgained = 0;
                TimerController.savedTime = 0f;
                areaController.areaUI.dunClearedUI.foundTreasure.Clear();
                areaController.areaUI.dunClearedUI.missedTreasure.Clear();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Character Select");
            }
        }
        if (Vector3.Distance(player.transform.position, transform.position) >= 2.75f)
        {
            if (inRange == true)
            {
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                inRange = false;
            }
            
        }
    }


}
