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
    public List<PlayableDirector> areaPlayables;

    public PlayerBank activeBank;
    public PlayerBank staticBank;
    
    public static Vector3 respawnPoint;
    public static Quaternion respawnRotation;
    public Vector3 respawnPointMirror;



    public static bool battleReturn;

    public bool battleReturnmirror;
    

    // Start is called before the first frame update
    void Start()
    {        
        if (battleReturn)
        {
            Debug.Log("battle return");
            moveController.transform.rotation = respawnRotation;
            moveController.transform.position = respawnPoint;
            Debug.Log("Character respawned at " + respawnPoint);            
            battleReturn = false;
        }        

        if (respawnPoint != Vector3.zero)
        {
            if (moveController.transform.position != respawnPoint)
            {
                moveController.transform.position = respawnPoint;
                Debug.Log("Respawn Point Error.  Quick Fix Successful at " + respawnPoint);
            }
        }
    }


    public void SetPlayerBank()
    {
        activeBank.bank[0] = staticBank.bank[HeroSelect.hero0];
        activeBank.bank[1] = staticBank.bank[HeroSelect.hero1];
        activeBank.bank[2] = staticBank.bank[HeroSelect.hero2];

        foreach (Player character in activeBank.bank)
        {
            if (character.playerName == "Berserker")
            {
                character.playerLevel = PlayerPrefs.GetInt("BerLevel");
                character.playerMaxHealth = PlayerPrefs.GetInt("BerMaxHealth");
                character.playerMaxMana = PlayerPrefs.GetInt("BerMaxMana");
                character.playerSTR = PlayerPrefs.GetInt("BerStr");
                character.playerDEF = PlayerPrefs.GetInt("BerDef");
                character.XP = PlayerPrefs.GetInt("BerXP");
            }
            if (character.playerName == "Archer")
            {
                character.playerLevel = PlayerPrefs.GetInt("ArLevel");
                character.playerMaxHealth = PlayerPrefs.GetInt("ArMaxHealth");
                character.playerMaxMana = PlayerPrefs.GetInt("ArMaxMana");
                character.playerSTR = PlayerPrefs.GetInt("ArStr");
                character.playerDEF = PlayerPrefs.GetInt("ArDef");
                character.XP = PlayerPrefs.GetInt("ArXP");
            }
            if (character.playerName == "Warrior")
            {
                character.playerLevel = PlayerPrefs.GetInt("WarLevel");
                character.playerMaxHealth = PlayerPrefs.GetInt("WarMaxHealth");
                character.playerMaxMana = PlayerPrefs.GetInt("WarMaxMana");
                character.playerSTR = PlayerPrefs.GetInt("WarStr");
                character.playerDEF = PlayerPrefs.GetInt("WarDef");
                character.XP = PlayerPrefs.GetInt("WarXP");
            }
            if (character.playerName == "Mage")
            {
                character.playerLevel = PlayerPrefs.GetInt("MagLevel");
                character.playerMaxHealth = PlayerPrefs.GetInt("MagMaxHealth");
                character.playerMaxMana = PlayerPrefs.GetInt("MagMaxMana");
                character.playerSTR = PlayerPrefs.GetInt("MagStr");
                character.playerDEF = PlayerPrefs.GetInt("MagDef");
                character.XP = PlayerPrefs.GetInt("MagXP");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        battleReturnmirror = battleReturn;
        respawnPointMirror = respawnPoint;

        // for UI Navigation
        if (areaUI.inventoryPanel.activeSelf || areaUI.playerPanel.activeSelf || areaUI.weaponPanel.activeSelf)
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
                            wall.open = true;
                            wall.gameObject.SetActive(false);
                            wall.disolver.gameObject.SetActive(true);
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
