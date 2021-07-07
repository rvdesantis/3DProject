using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using UnityEngine.UI;

public class AreaController : MonoBehaviour
{    
    public CharacterController moveController;
    public AreaUIController areaUI;    
    public FirstPerson FPcontroller;
    public GameObject playerBody;
    public List<SecretWall> secretWalls;
    public List<Chest> chests;
    public List<MimicChest> mimics;
    public List<DunEnemyAgent> agents;

    public GameObject bossDoor;
    public List<Items> areaInventory;
    public List<Items> potions;

    public List<Trinket> activeTrinkets;
    public List<Trinket> dunTrinketMasterList;
    public List<Items> availableItems;
    
    public List<PlayableDirector> areaPlayables;

    public PlayerBank activeBank;
    public PlayerBank staticBank;
   
    public static Vector3 respawnPoint;
    public static Quaternion respawnRotation;
    public Vector3 respawnPointMirror;

    public Vector3 bossHallwaySpawnPoint;
    public Quaternion bossHallwaySPRotation;

    public static bool firstLoad;
    
    public static bool battleReturn;
    public static int battleGold;
    public int respawnAttempt;

    public bool battleReturnmirror;
    public bool firstLoadMirror;

    public AudioSource audioSource;
    public List<AudioClip> audioClips;


    void Start()
    {
        if (firstLoad == false && battleReturn == false)
        {            
            respawnPoint = FindObjectOfType<OriginCube>().spawnPlatform.transform.position;
            StaticMenuItems.goldFound = 0;
        }
        if (firstLoad)
        {
            Debug.Log("firstload");
            areaUI.messageText.text = " Building Dungeon";
            areaUI.messageUI.GetComponent<Animator>().SetTrigger("message");
        }
        if (firstLoad == false)
        {
            Debug.Log("firstload false");
        }

        IEnumerator LoadTimer()
        {
            yield return new WaitForSeconds(2);
            SetStartingTrinkets();
            SetChestsMimics();
            
            WallChecker(); // sets firstload to false when finished

            SetPlayerBank();
            SetStartingItems();
            Unlockables();
            
            yield return new WaitForSeconds(1);
            if (battleReturn)
            {
                TimerController.instance.BeginTimer();
                areaUI.messageText.text = "+ " + battleGold + " Gold";
                areaUI.messageUI.GetComponent<Animator>().SetTrigger("message");
                areaUI.itemImage.sprite = availableItems[0].itemSprite;
                areaUI.ItemImage();
                audioSource.PlayOneShot(audioClips[0]);                

                if (BattleLauncher.bossEnemy == true)
                {   
                    BattleLauncher.bossEnemy = false;
                    areaUI.dunClearedUI.SetValues();
                    TimerController.instance.StopTimer();
                }
                if (BattleLauncher.dunEnemy == true)
                {
                    BattleLauncher.dunEnemy = false;
                }
            }

            areaUI.loadScreenAnim.SetTrigger("fadeOut");
        } StartCoroutine(LoadTimer());


    }

    public void Unlockables()
    {
        UnlockedUI unlockedUI = areaUI.unlockedUI;
        bool newUnlock = false;

    

        // Counters
        if (BattleLauncher.bossEnemy == true)
        {
            PlayerPrefs.SetInt("BossWins", PlayerPrefs.GetInt("BossWins") + 1);
            Debug.Log(PlayerPrefs.GetInt("BossWins") + " Boss Wins");
        }
        PlayerPrefs.Save();


        // Agent Unlocks
        Debug.Log(PlayerPrefs.GetInt("GuideUnlock") + " Guide Unlock");
        if (PlayerPrefs.GetInt("BossBattles") > 0 && PlayerPrefs.GetInt("GuideUnlock") == 0)
        {
            PlayerPrefs.SetInt("GuideUnlock", 1);
            unlockedUI.guideUnlocked = true;
            unlockedUI.cycle = true;
                      
        }
        Debug.Log(PlayerPrefs.GetInt("MedusaUnlock") + " Medusa Unlock");
        if (PlayerPrefs.GetInt("BossWins") > 2 && PlayerPrefs.GetInt("MedusaUnlock") == 0)
        {
            PlayerPrefs.SetInt("MedusaUnlock", 1);
            unlockedUI.medusaUnlocked = true;
            unlockedUI.cycle = true;
        }

        // Player Unlocks
        Debug.Log(PlayerPrefs.GetInt("DarkElfUnlock") + " Dark Elf Unlock");
        if (PlayerPrefs.GetInt("BossBattles") > 2 && PlayerPrefs.GetInt("DarkElfUnlock") == 0)
        {
            PlayerPrefs.SetInt("DarkElfUnlock", 1);
            unlockedUI.darkElfUnlock = true;
            unlockedUI.cycle = true;
        }
        if (newUnlock)
        {
            areaUI.ToggleUINav();
            unlockedUI.gameObject.SetActive(true);
            unlockedUI.confirmBT.Select();
            unlockedUI.UnlockBT();
        }

        PlayerPrefs.Save();
        
    }

    public void Respawn()
    {        
        moveController.enabled = false;
        moveController.transform.rotation = respawnRotation;
        moveController.transform.position = respawnPoint;
        Debug.Log("Character respawned at " + respawnPoint);
        moveController.enabled = true;

        if (moveController.transform.position != respawnPoint)
        {
            respawnAttempt++;
            Respawn();
            Debug.Log("Respawn Point Error.  Quick Fix Successful attempt " + respawnAttempt);
        }
        
    }


    public void SetPlayerBank()
    {
        activeBank.bank[0] = staticBank.bank[HeroSelect.hero0];
        activeBank.bank[1] = staticBank.bank[HeroSelect.hero1];
        activeBank.bank[2] = staticBank.bank[HeroSelect.hero2];

        foreach (Player character in activeBank.bank)
        {
            character.SetBattleStats();
        }
    }

    public void SetStartingItems()
    {
        areaInventory.Add(potions[0]);
    }

    public void SetChestsMimics()
    {
        if (firstLoad)
        {
            foreach (Chest chest in chests)
            {
                PlayerPrefs.SetInt("chest" + chests.IndexOf(chest), 0);
                PlayerPrefs.Save();
            }
            foreach (MimicChest mimic in mimics)
            {
                PlayerPrefs.SetInt("mimic" + mimics.IndexOf(mimic), 0);
                PlayerPrefs.Save();
            } 
            return;
        }
        if (!firstLoad)
        {
            
            foreach (Chest chest in chests)
            {
                Debug.Log("Checking Chest " + chests.IndexOf(chest));
                if (PlayerPrefs.GetInt("chest" + chests.IndexOf(chest)) == 0)
                {
                    chest.opened = 0;
                }
                if (PlayerPrefs.GetInt("chest" + chests.IndexOf(chest)) == 1)
                {
                    chest.opened = 1;
                }
                if (chest.opened == 1)
                {
                    chest.anim.SetTrigger("openLid");
                    areaUI.dunClearedUI.foundTreasure.Add(chest.treasure);
                }
            }

            foreach (MimicChest mimic in mimics)
             {
                 Debug.Log("Mimic Checker");
                if (PlayerPrefs.GetInt("mimic" + mimics.IndexOf(mimic)) == 0)
                {
                    Debug.Log("Mimic " + mimics.IndexOf(mimic) + " lid shut");
                }
                if (PlayerPrefs.GetInt("mimic" + mimics.IndexOf(mimic)) == 1)
                {
                    Debug.Log("Mimic " + mimics.IndexOf(mimic) + " inactive");
                    mimic.gameObject.SetActive(false);
                }
            } 
        }

    }

    public void SetStartingTrinkets()
    {
        if (firstLoad)
        {
            areaUI.topBarUI.gameObject.SetActive(false);
            if (activeTrinkets.Count != 0)
            {
                foreach (Trinket trinket in activeTrinkets)
                {
                    activeTrinkets.Remove(trinket);
                    Debug.Log("Trinkets Cleared");
                }
                foreach (Trinket masterTrinket in dunTrinketMasterList)
                {
                    PlayerPrefs.SetInt(masterTrinket.trinketName, 0);
                    PlayerPrefs.Save();
                }
            }
        }
        if (!firstLoad)
        {
            foreach (Trinket masterTrinket in dunTrinketMasterList)
            {
                if (PlayerPrefs.GetInt(masterTrinket.trinketName) == 1)
                {
                    activeTrinkets.Add(masterTrinket);
                }
            }
            if (activeTrinkets.Count > 0)
            {
                foreach (Trinket trinket in activeTrinkets)
                {
                    trinket.active = true;
                    Instantiate(trinket, playerBody.transform.position, Quaternion.identity);
                    Debug.Log(trinket.trinketName + " set active");
                }                
                areaUI.topBarUI.gameObject.SetActive(true);
                areaUI.SetTrinketImages();
            }

        }
    }

    public void WallChecker()
    {
        Debug.Log("Checking " + secretWalls.Count + " walls");
        foreach (SecretWall wall in secretWalls)
        {
            wall.wallNumber = secretWalls.IndexOf(wall);
            if (wall.wallNumber == 0)
            {
                wall.wallNumber = 100;
            }
            if (firstLoad == true)
            {
                PlayerPrefs.SetInt("Door" + wall.wallNumber, 0);                
            }
            if (firstLoad == false || battleReturn == true)
            {                
                if (PlayerPrefs.GetInt("Door" + wall.wallNumber) == 1)
                {                    
                    wall.WallDisolver();
                    wall.disolver.gameObject.SetActive(false);
                }
            }
        }
        if (firstLoad == true)
        {
            firstLoad = false;
        }
        PlayerPrefs.Save();
    }



    void Update()
    {
        battleReturnmirror = battleReturn;
        respawnPointMirror = respawnPoint;
        firstLoadMirror = firstLoad;

        // for UI Navigation
        if (areaUI.inventoryPanel.activeSelf || areaUI.playerPanel.activeSelf || areaUI.weaponPanel.activeSelf || areaUI.menuUI.activeSelf || areaUI.homeUI.activeSelf || areaUI.mapFrame.activeSelf)
        {
            areaUI.uiNavigation = true;
        }



        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (areaUI.uiNavigation == false)
            {
                foreach (SecretWall wall in secretWalls)
                {
                    if (wall.gameObject.activeSelf)
                    {
                        if (Vector3.Distance(moveController.transform.position, wall.transform.position) < 5f && wall.open == false)
                        {
                            IEnumerator WallTimer()
                            {
                                if (wall.enemyTrigger)
                                {
                                    wall.dunEnemy.anim.SetTrigger("turn");
                                    wall.dunEnemy.launchable = true;
                                    wall.enemyTrigger = false;
                                }
                                wall.WallDisolver();
                                yield return new WaitForSeconds(2);
                                wall.disolver.gameObject.SetActive(false);
                            }
                            StartCoroutine(WallTimer());
                        }
                    }
                    
                }
                if (Vector3.Distance(moveController.transform.position, bossDoor.transform.position) < 5f)
                {
                    areaPlayables[0].Play();
                    IEnumerator LaunchTimer()
                    {
                        yield return new WaitForSeconds(6f);
                        BattleLauncher.bossEnemy = true;
                        FindObjectOfType<BattleLauncher>().launching = true;
                        moveController.gameObject.SetActive(false);
                        areaUI.fadeOutPanel.gameObject.SetActive(true);
                        FindObjectOfType<BattleLauncher>().respawnPoint = moveController.transform.position;
                        FindObjectOfType<BattleLauncher>().rotationPoint = moveController.transform.rotation;
                        AreaController.respawnPoint = FindObjectOfType<BattleLauncher>().respawnPoint;
                        AreaController.respawnRotation = FindObjectOfType<BattleLauncher>().rotationPoint;
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
                    }
                    StartCoroutine(LaunchTimer());
                }
            }




        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (areaUI.inventoryPanel.activeSelf == false && areaUI.playerPanel.activeSelf == false && areaUI.weaponPanel.activeSelf == false && areaUI.homeUI.activeSelf == false)
            {
                areaUI.ToggleMenu();
                areaUI.menuButtons[0].GetComponent<Button>().Select();
            }                
        }

        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.JoystickButton3)) // x button
        {
            if (areaUI.inventoryPanel.activeSelf == false && areaUI.playerPanel.activeSelf == false && areaUI.weaponPanel.activeSelf == false && areaUI.homeUI.activeSelf == false)
            {
                areaUI.ToggleMap();
            }
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.JoystickButton1)) // B button on Xbox Controller
        {
            if (areaUI.homeUI.activeSelf)
            {
                areaUI.TogglePartyMenu();
                return;
            }
            if (areaUI.inventoryPanel.activeSelf)
            {
                areaUI.ToggleInventory();
                return;
            }
            if (areaUI.playerPanel.activeSelf)
            {
                areaUI.TogglePlayerStats();
                return;
            }
            else
            {
                areaUI.ToggleCompass();
            }
        }



        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            areaUI.TogglePartyMenu();
        }
    }
}
