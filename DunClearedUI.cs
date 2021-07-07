using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DunClearedUI : MonoBehaviour
{
    public AreaController areaController;
    public GameObject dunUIObject;

    public Text goldFoundTXT;
    public Text XPgainedTXT;

    public List<Items> foundTreasure;    
    public List<Items> missedTreasure;
    

    public List<Image> foundIMGs;
    public List<Image> missedIMGs;

    public Button ExitBT;
    public Button ContinueBT;

    private void Start()
    {
        
    }

    public void SetValues()
    {
        ContinueBT.Select();
        TimerController.instance.StopTimer();
        dunUIObject.SetActive(true);
        areaController.areaUI.uiNavigation = true;
        areaController.moveController.enabled = false;
        goldFoundTXT.text = goldFoundTXT.text + StaticMenuItems.goldFound.ToString();
        XPgainedTXT.text = XPgainedTXT.text + StaticMenuItems.XPgained.ToString();
        if (foundTreasure.Count != 0)
        {
            foreach (Image foundIMG in foundIMGs)
            {
                int x = foundIMGs.IndexOf(foundIMG);
                if (x >= foundTreasure.Count)
                {
                    foundIMG.gameObject.SetActive(false);
                }
                if (x < foundTreasure.Count)
                {
                    foundIMG.sprite = foundTreasure[x].itemSprite;
                }
            }
        }
        if (foundTreasure.Count == 0)
        {
            foreach (Image foundIMG in foundIMGs)
            {
                foundIMG.gameObject.SetActive(false);
            }
        }


        foreach (Chest chest in areaController.chests)
        {
            if (chest.opened == 0)
            {
                missedTreasure.Add(chest.treasure);
            }
        }

        foreach (Image missedIMG in missedIMGs)
        {
            int x = missedIMGs.IndexOf(missedIMG);
            if (x >= missedTreasure.Count)
            {
                missedIMG.gameObject.SetActive(false);
            }
            if (x < missedTreasure.Count)
            {
                missedIMG.sprite = missedTreasure[x].itemSprite;
            }
        }

        
    }

    public void ExitButton()
    {
        AreaController.respawnPoint = transform.position;
        AreaController.battleReturn = false;
        DunBuilder.createDungeon = true;
        HeroSelect.dunReturn = true;
        PlayerPrefs.SetInt("Gold", StaticMenuItems.goldCount); PlayerPrefs.Save();
        StaticMenuItems.goldFound = 0;
        StaticMenuItems.XPgained = 0;
        TimerController.savedTime = 0f;
        foundTreasure.Clear();
        missedTreasure.Clear();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Character Select");
    }

    public void ContinueButton()
    {
        areaController.areaUI.uiNavigation = false;        
        areaController.moveController.enabled = true;
        this.gameObject.SetActive(false);
    }




    private void Update()
    {
        
    }

}
