using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lvlupUIController : MonoBehaviour
{

    public BattleController battleController;

    public List<GameObject> lvlUPPanels;
    public List<Image> heroFaces;
    public List<Text> heroNames;
    public List<Text> heroStats;

    public List<int> beforeValues;
    
    void Start()
    {
        if (battleController == null)
        {
            battleController = FindObjectOfType<BattleController>();
        }
    }


    public void LoadLevelUPStats()
    {
        int heroIndex = 0;
        foreach(Player hero in battleController.heroes)
        {
            if (hero.playerLevel == 1 && hero.XP >= 500)
            {
                heroIndex = battleController.heroes.IndexOf(hero);

                lvlUPPanels[heroIndex].gameObject.SetActive(true);
                heroFaces[heroIndex].sprite = hero.playerFace;
                heroNames[heroIndex].text = hero.playerName;

                int a = hero.playerLevel;
                int b = hero.playerMaxHealth;
                int c = hero.playerMaxMana;
                int d = hero.playerSTR;
                int e = hero.playerDEF;

                hero.LevelUp();  // level up removed from BattleController, add here after old values saved.

                heroStats[heroIndex].text = a + " -> " + hero.playerLevel +
                    "\n " + b + " -> " + hero.playerMaxHealth +
                    "\n " + c + " -> " + hero.playerMaxMana +
                    "\n " + d + " -> " + hero.playerSTR +
                    "\n " + e + " -> " + hero.playerDEF; 

            }
        }

         
    }
   
    void Update()
    {
        
    }
}
