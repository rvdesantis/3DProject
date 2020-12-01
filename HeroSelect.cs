using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HeroSelect : MonoBehaviour
{

    public PlayerBank staticHeroList;
    public PlayerBank activeParty;

    public static int hero0;
    public static int hero1;
    public static int hero2;
    public static int enemy0;

    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public int heroIndex;
    public int partyIndex;

    public List<GameObject> camAimers;
    public GameObject backgroundAimer;
    public GameObject backgroundFollower;
    public List<GameObject> lights;


    private void Start()
    {        
        cam1.m_LookAt = camAimers[0].transform;
        lights[0].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {            
            if (partyIndex < 2)
            {
                
                activeParty.bank[partyIndex] = staticHeroList.bank[heroIndex];
                if (partyIndex == 0)
                {
                    hero0 = heroIndex;
                }
                if (partyIndex == 1)
                {
                    hero1 = heroIndex;
                }

                partyIndex++;
                return;
            }
            if (partyIndex == 2)
            {
                
                activeParty.bank[partyIndex] = staticHeroList.bank[heroIndex];
                hero2 = heroIndex;

                //enemy assign
                enemy0 = 0;
                cam1.m_LookAt = backgroundAimer.transform;
                cam1.m_Priority = 0;
                cam2.m_Priority = 1;
                backgroundFollower.transform.position = Vector3.MoveTowards(transform.position, backgroundAimer.transform.position, .00025f);
                IEnumerator Timer()
                {
                    yield return new WaitForSeconds(1);
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Castle 1");
                }
                StartCoroutine(Timer());
                return;
            }
            if (partyIndex > 2)
            {
                Debug.Log("Only 3 Heroes per party!");
            }

        }
        
    }
}
