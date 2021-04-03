using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStatsText : MonoBehaviour
{
    public List<Text> heroStats;
    public HeroSelect heroSelect;



    public void LoadHeroSelectStats()
    {
        if (heroSelect.heroIndex == 0)
        {
            int playerLevel = PlayerPrefs.GetInt("BerLevel");
            int playerHealth = PlayerPrefs.GetInt("BerMaxHealth");
            int playerMana = PlayerPrefs.GetInt("BerMaxMana");
            int playerSTR = PlayerPrefs.GetInt("BerStr");
            int playerDEF = PlayerPrefs.GetInt("BerDef");
            heroStats[0].text = playerLevel.ToString() + "\n" + playerHealth.ToString() + "\n" + playerMana.ToString() + "\n" + (playerSTR + heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.power).ToString() + "\n" + playerDEF.ToString();
        }
        if (heroSelect.heroIndex == 1)
        {
            int playerLevel = PlayerPrefs.GetInt("ArLevel", 0);
            int playerHealth = PlayerPrefs.GetInt("ArMaxHealth", 0);
            int playerMana = PlayerPrefs.GetInt("ArMaxMana", 0);
            int playerSTR = PlayerPrefs.GetInt("ArStr", 0);
            int playerDEF = PlayerPrefs.GetInt("ArDef", 0);
            heroStats[1].text = playerLevel.ToString() + "\n" + playerHealth.ToString() + "\n" + playerMana.ToString() + "\n" + (playerSTR + heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.power).ToString() + "\n" + playerDEF.ToString();
        }
        if (heroSelect.heroIndex == 2)
        {
            int playerLevel = PlayerPrefs.GetInt("WarLevel", 0);
            int playerHealth = PlayerPrefs.GetInt("WarMaxHealth", 0);
            int playerMana = PlayerPrefs.GetInt("WarMaxMana", 0);
            int playerSTR = PlayerPrefs.GetInt("WarStr", 0);
            int playerDEF = PlayerPrefs.GetInt("WarDef", 0);
            heroStats[2].text = playerLevel.ToString() + "\n" + playerHealth.ToString() + "\n" + playerMana.ToString() + "\n" + (playerSTR + heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.power).ToString() + "\n" + playerDEF.ToString();
        }
        if (heroSelect.heroIndex == 3)
        {
            int playerLevel = PlayerPrefs.GetInt("MagLevel", 0);
            int playerHealth = PlayerPrefs.GetInt("MagMaxHealth", 0);
            int playerMana = PlayerPrefs.GetInt("MagMaxMana", 0);
            int playerSTR = PlayerPrefs.GetInt("MagStr", 0);
            int playerDEF = PlayerPrefs.GetInt("MagDef", 0);
            heroStats[3].text = playerLevel.ToString() + "\n" + playerHealth.ToString() + "\n" + playerMana.ToString() + "\n" + (playerSTR + heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.power).ToString() + "\n" + playerDEF.ToString();
        }
    }



}
