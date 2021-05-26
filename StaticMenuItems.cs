using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMenuItems : MonoBehaviour
{    

    public static int musicVol;
    public static int soundFXVol;

    public static int goldCount;

    public static int dungeonCubeTarget;
    

    public static void ResetSavedValues()
    {
        goldCount = 0;
        PlayerPrefs.SetInt("ChestCount", 0);

        for (int x = 0; x < 100; x++)
        {
            PlayerPrefs.SetInt("Door" + x, 0);
            PlayerPrefs.SetInt("chest" + x, 0);
            PlayerPrefs.SetInt("mimic" + x, 0);
            PlayerPrefs.SetInt("Chest" + x + "position", 0);
            PlayerPrefs.SetInt("Mimic" + x + "position", 0);            
            PlayerPrefs.SetInt("Chest" + x + "Item", 0);
        }        

        PlayerPrefs.Save();
    }


}
