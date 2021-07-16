using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMenuItems : MonoBehaviour
{    

    public static int musicVol;
    public static int soundFXVol;
    public static int goldCount;
    public static int dungeonCubeTarget;

    

    // tracked values per run
    public static int XPgained;
    public static int goldFound;

    // unlocks
    // players
   

    public void ResetUnlocks()
    {
        PlayerPrefs.SetInt("DarkElfUnlock", 0);
        PlayerPrefs.SetInt("GuideUnlock", 0);
        PlayerPrefs.SetInt("MedusaUnlock", 0);
        PlayerPrefs.SetInt("BossWins", 0);
        PlayerPrefs.SetInt("BossBattles", 0);

        PlayerPrefs.Save();
    }

    public static void ResetSavedValues()
    {
        goldCount = 50;
        PlayerPrefs.SetInt("ChestCount", 0);

        for (int x = 0; x < 100; x++)
        {
            PlayerPrefs.SetInt("Door" + x, 0);
            PlayerPrefs.SetInt("chest" + x, 0);
            PlayerPrefs.SetInt("mimic" + x, 0);
            PlayerPrefs.SetInt("Chest" + x + "position", 0);
            PlayerPrefs.SetInt("Mimic" + x + "position", 0);            
            PlayerPrefs.SetInt("Chest" + x + "Item", 0);
            PlayerPrefs.SetInt("Agent" + x + "Active", 0);
            PlayerPrefs.SetInt("ChestCount", 0);
            PlayerPrefs.SetInt("MimicCount", 0);
        }

        // reset agents spawn status
        foreach (DunEnemyAgent agent in FindObjectOfType<HeroSelect>().masterAgentList)
        {
            PlayerPrefs.SetInt(agent.agentName + "Spawn", 0);
        }

        // reset dun shops
        PlayerPrefs.SetInt("SecretStoreWeapon", 0);
        PlayerPrefs.SetInt("StoreWeaponSold", 0);
        

        PlayerPrefs.Save();
    }

    public static void ResetDungeonValues()
    {
        PlayerPrefs.SetInt("ChestCount", 0);

        for (int x = 0; x < 100; x++)
        {
            PlayerPrefs.SetInt("Door" + x, 0);
            PlayerPrefs.SetInt("chest" + x, 0);
            PlayerPrefs.SetInt("mimic" + x, 0);
            PlayerPrefs.SetInt("Chest" + x + "position", 0);
            PlayerPrefs.SetInt("Mimic" + x + "position", 0);
            PlayerPrefs.SetInt("Chest" + x + "Item", 0);
            PlayerPrefs.SetInt("Agent" + x + "Active", 0);
            PlayerPrefs.SetInt("ChestCount", 0);
            PlayerPrefs.SetInt("MimicCount", 0);
        }

        // reset agents spawn status
        foreach (DunEnemyAgent agent in FindObjectOfType<HeroSelect>().masterAgentList)
        {
            PlayerPrefs.SetInt(agent.agentName + "Spawn", 0);
        }
        // reset dun shops
        PlayerPrefs.SetInt("SecretStoreWeapon", 0);
        PlayerPrefs.SetInt("StoreWeaponSold", 0);


        PlayerPrefs.Save();
    }


}
