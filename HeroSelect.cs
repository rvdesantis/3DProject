using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class HeroSelect : MonoBehaviour
{

    public Button newGameBT;
    public Button resumeBT;

    public bool start;

    public PlayerBank staticHeroList;
    public PlayerBank activeParty;
    public PlayerBank heroBank;
    

    public static int hero0;
    public static int hero1;
    public static int hero2;
    public static int enemy0;

    public static int keys;
    public static int wallsFound;
    public static int chestsFound;
    public static int BBattles;

    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public List<CinemachineVirtualCamera> focusCams;
    public int heroIndex;
    public int partyIndex;
    public bool focused;
    public bool door;

    public List<GameObject> camAimers;
    public GameObject backgroundAimer;
    public GameObject backgroundFollower;
    public List<GameObject> lights;
    public List<GameObject> heroUI;
    public HeroStatsText heroStatsText;
    public List<WeaponBT> weaponBTs;
    
    
    public UIArrow leftArrow;
    public UIArrow rightArrow;

    public bool joystick;
    public GameObject directionUI;
    public Text directionText;
    public Image spaceBar;
    public Image greenButton;
    

    public Text backText;
    public Image redButton;
    public Image escButton;
    public Image blueButton;
    public Image xButton;

    public Animator directionUIanim;

    public GameObject iconPanel;
    public Text partyCounter;

    public GameObject dungeonInfoUI;
    public Text dungeonInfoTXT;
    public GameObject doorButtonsUI;
    public Button startRunBT;

    public GameObject trinketPanel;    
    public List<Image> trinketImages;
    public List<Trinket> masterTrinketList;



    private void Start()
    {
        newGameBT.Select();
        directionUIanim = directionUI.GetComponent<Animator>();
    }

    public void NewRun()
    {
        IEnumerator StartTimer()
        {
            foreach (Player hero in heroBank.bank)
            {
                hero.LevelReset();
            }
            foreach (Trinket trinket in masterTrinketList)
            {
                trinket.active = false;
                PlayerPrefs.SetInt(trinket.trinketName, 0);
                PlayerPrefs.Save();
            }               

            yield return new WaitForSeconds(.25f);
            start = false;
            AreaController.firstLoad = true;
            DunBuilder.createDungeon = true;
            StaticMenuItems.dungeonCubeTarget = 250; // sets default dungeon size to "small"
            StaticMenuItems.ResetSavedValues();
            cam1.m_LookAt = camAimers[0].transform;
            lights[0].gameObject.SetActive(true);
        }
        StartCoroutine(StartTimer());
        
    }

    public void SmallButton()
    {
        StaticMenuItems.dungeonCubeTarget = 250;
    }

    public void MedButton()
    {
        StaticMenuItems.dungeonCubeTarget = 500;
    }

    public void LargeButton()
    {
        StaticMenuItems.dungeonCubeTarget = 750;
    }

    public void StartRunBT()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DunGenerator");
    }

    public void ResumeRun()
    {        
        IEnumerator StartTimer()
        {
            AreaController.firstLoad = false;            
            foreach (Player hero in heroBank.bank)
            {
                hero.SetBattleStats();
            }            
            yield return new WaitForSeconds(.25f);
            start = false;
            cam1.m_LookAt = camAimers[0].transform;
            lights[0].gameObject.SetActive(true);
        }
        StartCoroutine(StartTimer());
    }

   

    public void UpdateDirections()
    {
        if (door)
        {
            if (joystick)
            {
                greenButton.gameObject.SetActive(true);
                redButton.gameObject.SetActive(true);
            }
            if (!joystick)
            {
                spaceBar.gameObject.SetActive(true);
                escButton.gameObject.SetActive(true);
            }
            directionText.text = "Enter Dungeon";
            directionUIanim.SetBool("left", false);
            directionUIanim.SetBool("right", false);
            backText.text = "Back";
            backText.gameObject.SetActive(true); 

            blueButton.gameObject.SetActive(false);
        }
        if (!focused && !door)
        {
            directionText.text = "Select " + staticHeroList.bank[heroIndex].playerName;
            directionUIanim.SetBool("left", false);
            directionUIanim.SetBool("right", false);

            redButton.gameObject.SetActive(false); backText.gameObject.SetActive(false); escButton.gameObject.SetActive(false); blueButton.gameObject.SetActive(false);
        }
        if (focused && !door)
        {
            backText.text = "Return";
            if (heroIndex != hero0 || heroIndex != hero1 || heroIndex != hero2)
            {                
                directionText.text = "Add " + staticHeroList.bank[heroIndex].playerName;                
            }
            if (heroIndex == hero0 || heroIndex == hero1 || heroIndex == hero2)
            {
                if (partyIndex > 0)
                {
                    directionText.text = "Remove " + staticHeroList.bank[heroIndex].playerName;
                    if (joystick)
                    {
                        greenButton.gameObject.SetActive(false);
                        blueButton.gameObject.SetActive(true);
                    }
                    if (!joystick)
                    {
                        greenButton.gameObject.SetActive(false);
                        spaceBar.gameObject.SetActive(false);
                        xButton.gameObject.SetActive(true);
                    }

                }                               
            }

            if (joystick)
            {
                escButton.gameObject.SetActive(false);
                redButton.gameObject.SetActive(true); backText.gameObject.SetActive(true);
            }
            if (!joystick)
            {
                redButton.gameObject.SetActive(false);
                escButton.gameObject.SetActive(true); backText.gameObject.SetActive(true);
            }


            if (heroIndex == 1 || heroIndex == 2)
            {
                directionUIanim.SetBool("right", false);
                directionUIanim.SetBool("left", true);
                
            }
            if (heroIndex == 0 || heroIndex == 3)
            {
                directionUIanim.SetBool("left", false);
                directionUIanim.SetBool("right", true);
            }

        }
        if (joystick)
        {
            spaceBar.gameObject.SetActive(false);
            if (!focused)
            {
                greenButton.gameObject.SetActive(true);
            }            
        }
        if (joystick == false)
        {
            if (greenButton.gameObject.activeSelf)
            {
                spaceBar.gameObject.SetActive(true);
                greenButton.gameObject.SetActive(false);
                blueButton.gameObject.SetActive(false);
            }

        }
    }

    private void FixedUpdate() // for joystick.  Set to .1 in project settings.
    {
        

        if (Input.GetAxis("Horizontal") > .5f)
        {
            if (joystick == false)
            {
                joystick = true;
            }
            if (!focused && newGameBT.gameObject.activeSelf == false)
            {
                rightArrow.ArrowTrigger();
                if (heroIndex < staticHeroList.bank.Count - 1)
                {
                    heroIndex++;
                    cam1.m_LookAt = camAimers[heroIndex].transform;
                    lights[heroIndex - 1].gameObject.SetActive(false);
                    lights[heroIndex].gameObject.SetActive(true);
                    return;
                }
                if (heroIndex >= staticHeroList.bank.Count - 1)
                {
                    heroIndex = 0;
                    cam1.m_LookAt = camAimers[0].transform;
                    lights[staticHeroList.bank.Count - 1].gameObject.SetActive(false);
                    lights[0].gameObject.SetActive(true);
                    return;
                }
            }
        }

        if (Input.GetAxis("Horizontal") < -.5f)
        {
            if (joystick == false)
            {
                joystick = true;
            }
            if (!focused && newGameBT.gameObject.activeSelf == false)
            {
                leftArrow.ArrowTrigger();
                if (heroIndex > 0)
                {
                    heroIndex--;
                    cam1.m_LookAt = camAimers[heroIndex].transform;
                    lights[heroIndex + 1].gameObject.SetActive(false);
                    lights[heroIndex].gameObject.SetActive(true);
                    return;
                }
                if (heroIndex == 0)
                {
                    heroIndex = staticHeroList.bank.Count - 1;
                    cam1.m_LookAt = camAimers[staticHeroList.bank.Count - 1].transform;
                    lights[staticHeroList.bank.Count - 1].gameObject.SetActive(true);
                    lights[0].gameObject.SetActive(false);
                    return;
                }
            }

        }
    }



    private void Update()
    {        
        UpdateDirections();
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (joystick == true)
            {
                joystick = false;
            }
            if (!focused && newGameBT.gameObject.activeSelf == false)
            {
                rightArrow.ArrowTrigger();
                if (heroIndex < staticHeroList.bank.Count - 1)
                {
                    heroIndex++;
                    cam1.m_LookAt = camAimers[heroIndex].transform;
                    lights[heroIndex - 1].gameObject.SetActive(false);
                    lights[heroIndex].gameObject.SetActive(true);
                    return;
                }
                if (heroIndex >= staticHeroList.bank.Count - 1)
                {
                    heroIndex = 0;
                    cam1.m_LookAt = camAimers[0].transform;
                    lights[staticHeroList.bank.Count - 1].gameObject.SetActive(false);
                    lights[0].gameObject.SetActive(true);
                    return;
                }
            }           
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (joystick == true)
            {
                joystick = false;
            }
            if (!focused && newGameBT.gameObject.activeSelf == false)
            {
                leftArrow.ArrowTrigger();
                if (heroIndex > 0)
                {
                    heroIndex--;
                    cam1.m_LookAt = camAimers[heroIndex].transform;
                    lights[heroIndex + 1].gameObject.SetActive(false);
                    lights[heroIndex].gameObject.SetActive(true);
                    return;
                }
                if (heroIndex == 0)
                {
                    heroIndex = staticHeroList.bank.Count - 1;
                    cam1.m_LookAt = camAimers[staticHeroList.bank.Count - 1].transform;
                    lights[staticHeroList.bank.Count - 1].gameObject.SetActive(true);
                    lights[0].gameObject.SetActive(false);
                    return;
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (focused && start == false)
            {
                focused = false;
                cam1.m_Priority = 1;
                focusCams[heroIndex].m_Priority = 0;
                heroUI[heroIndex].gameObject.SetActive(false);
                staticHeroList.bank[heroIndex].GetComponent<Animator>().SetTrigger("sit");                
                iconPanel.gameObject.SetActive(true);
                if (!joystick)
                {
                    spaceBar.gameObject.SetActive(true);
                    xButton.gameObject.SetActive(false);
                }
                
            }
            if (door)
            {
                door = false;
                cam2.m_Priority = 0;
                dungeonInfoUI.GetComponent<Animator>().SetBool("fadein", false);
                trinketPanel.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (focused && start == false)
            {
                if (staticHeroList.bank[heroIndex].anim.GetBool("select") == true)
                {
                    staticHeroList.bank[heroIndex].anim.SetBool("select", false);
                    if (staticHeroList.bank[heroIndex] == activeParty.bank[partyIndex])
                    {
                        activeParty.bank[partyIndex] = null;

                    }
                    if (staticHeroList.bank[heroIndex] != activeParty.bank[partyIndex])
                    {
                        if (partyIndex == 2)
                        {
                            activeParty.bank[0] = activeParty.bank[1];
                            activeParty.bank[1] = null;
                        }

                    }
                    partyIndex--;
                }               

                focused = false;
                cam1.m_Priority = 1;
                focusCams[heroIndex].m_Priority = 0;
                heroUI[heroIndex].gameObject.SetActive(false);
                
                
                partyCounter.text = "CHOOSE PARTY (" + partyIndex + " out of 3)";
                iconPanel.gameObject.SetActive(true);
                if (!joystick)
                {
                    spaceBar.gameObject.SetActive(true);
                    xButton.gameObject.SetActive(false);
                }


            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (door)
            {
                               
            }            
            if (!focused && start == false)
            {                
                cam1.m_Priority = 0;
                focusCams[heroIndex].m_Priority = 1;
                focused = true;
                iconPanel.gameObject.SetActive(false);
                heroStatsText.LoadHeroSelectStats();
                heroUI[heroIndex].gameObject.SetActive(true);
                if (staticHeroList.bank[heroIndex].GetComponent<Animator>().GetBool("select") == false)
                {
                    staticHeroList.bank[heroIndex].GetComponent<Animator>().SetTrigger("stand");
                }                
                return;
            }
            if (focused)
            {
                if (partyIndex < 2)
                {                    
                    if (partyIndex == 0)
                    {
                        activeParty.bank[partyIndex] = staticHeroList.bank[heroIndex];
                        hero0 = heroIndex; // sets 0 slot to highlighted character 
                        partyIndex++;                        
                        partyCounter.text = "CHOOSE PARTY (" + partyIndex + " out of 3)";                                                                
                    }
                    if (partyIndex == 1)
                    {
                        if (hero0 != heroIndex)
                        {
                            activeParty.bank[partyIndex] = staticHeroList.bank[heroIndex];
                            hero1 = heroIndex;   
                            partyIndex++;                            
                            partyCounter.text = "CHOOSE PARTY (" + partyIndex + " out of 3)";
                        }                        
                    }
                   
                    cam1.m_Priority = 1;
                    focusCams[heroIndex].m_Priority = 0;
                    heroUI[heroIndex].gameObject.SetActive(false);
                    iconPanel.gameObject.SetActive(true);
                    staticHeroList.bank[heroIndex].GetComponent<Animator>().SetTrigger("select");
                    focused = false;

                    return;
                }
                if (partyIndex == 2)
                {

                    if (hero0 != heroIndex && hero1 != heroIndex)
                    {                        
                        activeParty.bank[partyIndex] = staticHeroList.bank[heroIndex];
                        hero2 = heroIndex;
                        //enemy assign
                        heroUI[heroIndex].gameObject.SetActive(false);
                        enemy0 = 0;
                        cam1.m_LookAt = backgroundAimer.transform;
                        cam1.m_Priority = 0;
                        cam2.m_Priority = 1;
                        backgroundFollower.transform.position = Vector3.MoveTowards(transform.position, backgroundAimer.transform.position, .00025f);
                        startRunBT.Select();                       
                        door = true;


                        IEnumerator Timer()
                        {
                            directionText.text = "Enter Random Dungeon";
                            directionUI.gameObject.SetActive(false);
                            
                            yield return new WaitForSeconds(1);
                            doorButtonsUI.gameObject.SetActive(true);

                            keys = 1;

                            // values will need to be set                            

                            foreach (Image image in trinketImages)
                            {
                                int x = trinketImages.IndexOf(image);
                                if (x > masterTrinketList.Count - 1)
                                {
                                    image.gameObject.SetActive(false);
                                }
                                if (x <= masterTrinketList.Count - 1)
                                {
                                    if (PlayerPrefs.GetInt(masterTrinketList[x].trinketName) == 0)
                                    {
                                        image.gameObject.SetActive(false);
                                    }
                                    if (PlayerPrefs.GetInt(masterTrinketList[x].trinketName) == 1)
                                    {
                                        image.sprite = masterTrinketList[x].trinketSprite;
                                        image.gameObject.SetActive(true);
                                    }
                                }
                            }
                            trinketPanel.gameObject.SetActive(true);

                        }
                        StartCoroutine(Timer());
                        return;
                    }                    
                }
            }

            if (partyIndex > 2)
            {
                Debug.Log("Only 3 Heroes per party!");
            }

        }


        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if (focused && door == false)
            {
                Debug.Log("weapon switched");
                weaponBTs[heroIndex].NextWeapon();

            }
        }

    }
}
