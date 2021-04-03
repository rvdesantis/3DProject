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
    public GameObject bossDoor;
    public List<Items> areaInventory;
    public List<Items> potions;
    public List<PlayableDirector> areaPlayables;

    public PlayerBank activeBank;
    public PlayerBank staticBank;
    
    public static Vector3 respawnPoint;
    public static Quaternion respawnRotation;
    public Vector3 respawnPointMirror;

    public static int openedChests;
    public static int mimicChests;
    public static int bossBattles;
    public static int foundWalls;


    public static bool firstLoad;
    public static bool battleReturn;
    public int respawnAttempt;

    public bool battleReturnmirror;
    

    // Start is called before the first frame update
    void Start()
    {        
        if (battleReturn)
        {
            Debug.Log("battle return");

            Respawn();
        } 

        SetPlayerBank();
        SetStartingItems();
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

    // Update is called once per frame
    void Update()
    {
        battleReturnmirror = battleReturn;
        respawnPointMirror = respawnPoint;

        // for UI Navigation
        if (areaUI.inventoryPanel.activeSelf || areaUI.playerPanel.activeSelf || areaUI.weaponPanel.activeSelf || areaUI.menuUI.activeSelf)
        {
            areaUI.uiNavigation = true;
        }



        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (areaUI.uiNavigation == false)
            {
                foreach (SecretWall wall in secretWalls)
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
                            foundWalls++;
                            yield return new WaitForSeconds(2);
                            wall.disolver.gameObject.SetActive(false);
                        }
                        StartCoroutine(WallTimer());
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
            if (areaUI.inventoryPanel.activeSelf == false && areaUI.playerPanel.activeSelf == false && areaUI.weaponPanel.activeSelf == false && areaUI.menuUI.activeSelf == false)
            {
                areaUI.ToggleMenu();
            }                
        }


        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            areaUI.ToggleCompass();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            areaUI.ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            areaUI.TogglePlayerStats();
        }
    }
}
