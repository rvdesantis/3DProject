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

            IEnumerator TreasureRoomTimer()
            {
                yield return new WaitForSeconds(1);
                foreach (GameObject spawnPlatform in chestSpawnPlatforms)
                {
                    if (chestSpawnPlatforms.IndexOf(spawnPlatform) == 0)
                    {
                        int x = Random.Range(0, chests.Count);
                        if (x == 0) // Regular Chest
                        {
                            Debug.Log("Treasure Room Chests Spawned");
                            Chest chest = Instantiate(chests[0], spawnPlatform.transform.position, spawnPlatform.transform.rotation);
                            chest.areaController = areaController;
                            chest.player = areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();
                            areaController.chests.Add(chest);
                            chestCount++;
                            PlayerPrefs.SetInt("TreasureRoomChest" + chestSpawnPlatforms.IndexOf(spawnPlatform), 1);
                        }
                        if (x == 1) // Armor Stand
                        {
                            Chest chest = Instantiate(chests[1], spawnPlatform.transform.position, spawnPlatform.transform.rotation);
                            chest.areaController = areaController;
                            chest.player = areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();
                            areaController.chests.Add(chest);
                            chestCount++;
                            PlayerPrefs.SetInt("TreasureRoomChest" + chestSpawnPlatforms.IndexOf(spawnPlatform), 2);

                            ArmorStand armorStand = chest.GetComponent<ArmorStand>();
                            int w = Random.Range(0, armorStand.weaponBank.Count);
                            PlayerPrefs.SetInt("TreasureRoomArmorStand", w);
                            Items spawnedWeapon = Instantiate(armorStand.weaponBank[w], armorStand.weaponSpawnTransform.transform.position, armorStand.weaponSpawnTransform.transform.rotation);
                            chest.treasure = spawnedWeapon;
                            Debug.Log("Armor Stand Spawned");
                        }

                    }
                    if (chestSpawnPlatforms.IndexOf(spawnPlatform) != 0)
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
                    
                }
                PlayerPrefs.Save();
            } StartCoroutine(TreasureRoomTimer());
           
        }
        if (AreaController.firstLoad == false)
        {
            IEnumerator TreasureRoomTimer()
            {
                yield return new WaitForSeconds(1);
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
                    if (x == 1)
                    {
                        Chest chest = Instantiate(chests[0], spawnPlatform.transform.position, spawnPlatform.transform.rotation);
                        chest.areaController = areaController;
                        chest.player = areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();
                        areaController.chests.Add(chest);
                    }
                    if (x == 2)
                    {
                        Chest stand = Instantiate(chests[1], spawnPlatform.transform.position, spawnPlatform.transform.rotation);
                        stand.areaController = areaController;
                        stand.player = areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();
                        areaController.chests.Add(stand);

                        ArmorStand armorStand = stand.GetComponent<ArmorStand>();
                        int w = PlayerPrefs.GetInt("TreasureRoomArmorStand");
                        
                        Items spawnedWeapon = Instantiate(armorStand.weaponBank[w], armorStand.weaponSpawnTransform.transform.position, armorStand.weaponSpawnTransform.transform.rotation);
                        stand.treasure = spawnedWeapon;
                        Debug.Log("Armor Stand Spawned");
                    }
                }
            }
            StartCoroutine(TreasureRoomTimer());                
        }


    }

}
