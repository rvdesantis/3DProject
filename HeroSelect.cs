using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class HeroSelect : MonoBehaviour
{

    public PlayerBank staticHeroList;
    public PlayerBank activeParty;
    public PlayerBank heroBank;

    public static int hero0;
    public static int hero1;
    public static int hero2;
    public static int enemy0;

    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public List<CinemachineVirtualCamera> focusCams;
    public int heroIndex;
    public int partyIndex;
    public bool focused;

    public List<GameObject> camAimers;
    public GameObject backgroundAimer;
    public GameObject backgroundFollower;
    public List<GameObject> lights;
    public List<GameObject> heroUI;
    public HeroStatsText heroStatsText;
    public List<WeaponBT> weaponBTs;
    
    
    public UIArrow leftArrow;
    public UIArrow rightArrow;

    public GameObject iconPanel;
    public Text partyCounter;


    private void Start()
    {        
        cam1.m_LookAt = camAimers[0].transform;
        lights[0].gameObject.SetActive(true);        
    }

    public void SaveToPrefs()
    {
        if (hero0 == 0 || hero1 == 0 || hero2 == 0)
        {
            PlayerPrefs.SetString("BerName", "Berserker");
            PlayerPrefs.SetInt("BerLevel", staticHeroList.bank[0].playerLevel);
            PlayerPrefs.SetInt("BerMaxHealth", staticHeroList.bank[0].playerMaxHealth);
            PlayerPrefs.SetInt("BerMaxMana", staticHeroList.bank[0].playerMaxMana);
            PlayerPrefs.SetInt("BerStr", staticHeroList.bank[0].playerSTR);
            PlayerPrefs.SetInt("BerDef", staticHeroList.bank[0].playerDEF);            
        }
        if (hero0 == 1 || hero1 == 1 || hero2 == 1)
        {
            PlayerPrefs.SetString("ArName", "Archer");
            PlayerPrefs.SetInt("ArLevel", staticHeroList.bank[1].playerLevel);
            PlayerPrefs.SetInt("ArMaxHealth", staticHeroList.bank[1].playerMaxHealth);
            PlayerPrefs.SetInt("ArMaxMana", staticHeroList.bank[1].playerMaxMana);
            PlayerPrefs.SetInt("ArStr", staticHeroList.bank[1].playerSTR);
            PlayerPrefs.SetInt("ArDef", staticHeroList.bank[1].playerDEF);
        }
        if (hero0 == 2 || hero1 == 2 || hero2 == 2)
        {
            PlayerPrefs.SetString("WarName", "Warrior");
            PlayerPrefs.SetInt("WarLevel", staticHeroList.bank[2].playerLevel);
            PlayerPrefs.SetInt("WarMaxHealth", staticHeroList.bank[2].playerMaxHealth);
            PlayerPrefs.SetInt("WarMaxMana", staticHeroList.bank[2].playerMaxMana);
            PlayerPrefs.SetInt("WarStr", staticHeroList.bank[2].playerSTR);
            PlayerPrefs.SetInt("WarDef", staticHeroList.bank[2].playerDEF);
        }
        if (hero0 == 3 || hero1 == 3 || hero2 == 3)
        {
            PlayerPrefs.SetString("MagName", "Mage");
            PlayerPrefs.SetInt("MagLevel", staticHeroList.bank[3].playerLevel);
            PlayerPrefs.SetInt("MagMaxHealth", staticHeroList.bank[3].playerMaxHealth);
            PlayerPrefs.SetInt("MagMaxMana", staticHeroList.bank[3].playerMaxMana);
            PlayerPrefs.SetInt("MagStr", staticHeroList.bank[3].playerSTR);
            PlayerPrefs.SetInt("MagDef", staticHeroList.bank[3].playerDEF);
        }
        PlayerPrefs.Save();
    }

    private void FixedUpdate() // for joystick.  Set to .1 in project settings.
    {
        if (Input.GetAxis("Horizontal") > .5f)
        {
            if (!focused)
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
            if (!focused)
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
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (!focused)
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
            if (!focused)
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
            if (focused)
            {
                
                cam1.m_Priority = 1;
                focusCams[heroIndex].m_Priority = 0;
                heroUI[heroIndex].gameObject.SetActive(false);
                staticHeroList.bank[heroIndex].GetComponent<Animator>().SetTrigger("sit");
                focused = false;
                iconPanel.gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (!focused)
            {                
                cam1.m_Priority = 0;
                focusCams[heroIndex].m_Priority = 1;
                focused = true;
                iconPanel.gameObject.SetActive(false);
                heroStatsText.LoadHeroSelectStats();
                heroUI[heroIndex].gameObject.SetActive(true);
                staticHeroList.bank[heroIndex].GetComponent<Animator>().SetTrigger("stand");
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


                        SaveToPrefs();



                        IEnumerator Timer()
                        {
                            yield return new WaitForSeconds(1);
                            UnityEngine.SceneManagement.SceneManager.LoadScene("Castle 1");
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
            if (focused)
            {
                Debug.Log("weapon switched");
                weaponBTs[heroIndex].NextWeapon();

            }
        }

    }
}
