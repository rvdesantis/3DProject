using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treasureRoom : MonoBehaviour
{
    
    public AreaController areaController;
    public List<Chest> chests;
    public List<MimicChest> mimicChests;
    public List<Items> chestTreasures;

    public List<GameObject> chestSpawnPlatforms;

    void Start()
    {        
        areaController = FindObjectOfType<AreaController>();

        if (AreaController.firstLoad)
        {
            int mimicCount = 0;
            int chestCount = 0;

            foreach (GameObject spawnPlatform in chestSpawnPlatforms)
            {
                int x = Random.Range(0, 5);  // 20% chance to spawn mimic, set to 0;

                if (x == 0)
                {
                    MimicChest mimic = Instantiate(mimicChests[0], spawnPlatform.transform.position, spawnPlatform.transform.rotation);
                    mimic.battleLauncher = FindObjectOfType<BattleLauncher>();
                    mimic.areaController = areaController;
                    areaController.mimics.Add(mimic);
                    mimicCount++;
                    PlayerPrefs.SetInt("TreasureRoomChest" + chestSpawnPlatforms.IndexOf(spawnPlatform), 0);
                }
                if (x != 0)
                {
                    Chest chest = Instantiate(chests[0], spawnPlatform.transform.position, spawnPlatform.transform.rotation);
                    chest.areaController = areaController;
                    chest.player = areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();
                    areaController.chests.Add(chest);
                    chestCount++;
                    PlayerPrefs.SetInt("TreasureRoomChest" + chestSpawnPlatforms.IndexOf(spawnPlatform), 1);
                }
            }
            PlayerPrefs.Save();
        }
        if (AreaController.firstLoad == false)
        {
            foreach (GameObject spawnPlatform in chestSpawnPlatforms)
            {
                int x = PlayerPrefs.GetInt("TreasureRoomChest" + chestSpawnPlatforms.IndexOf(spawnPlatform));
                if (x == 0)
                {
                    MimicChest mimic = Instantiate(mimicChests[0], spawnPlatform.transform.position, spawnPlatform.transform.rotation);
                    mimic.battleLauncher = FindObjectOfType<BattleLauncher>();
                    mimic.areaController = areaController;
                    areaController.mimics.Add(mimic);
                }
                if (x != 0)
                {
                    Chest chest = Instantiate(chests[0], spawnPlatform.transform.position, spawnPlatform.transform.rotation);
                    chest.areaController = areaController;
                    chest.player = areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();
                    areaController.chests.Add(chest);
                }
            }            
        }


    }

}
