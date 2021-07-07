using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class DunEnemyAgent : MonoBehaviour
{
    public string agentName;
    public int spawnChance;
    public bool forHire;
    public int hireCost;


    public AreaController areaController;
    public NavMeshAgent agent;
    public NavMeshQuery agentQuery;
    public GameObject agentBodyTransform;
    public AgentLinkMover mover;
    public Vector3 targetLocation;
    public Vector3 nextLocation;
    public Vector3 savedPosition;   
    public Animator bodyAnim;   

    public AudioSource audioSource;
    public List<AudioClip> introClips;
    public List<AudioClip> activatedClips;
    public List<AudioClip> returnClips;

    
    public bool move;
    public bool active;
    public bool finished;


    // below used in updates
    public bool intro = false;
    public bool goodBye = false;
    public bool negative = false;
    public bool agentMessage = false;

    private void Start()
    {
        areaController = FindObjectOfType<AreaController>(); 
    }

    public virtual void SpawnChance()
    {

    }
    public virtual void SavePosition()
    {
        savedPosition = agentBodyTransform.transform.position;
        PlayerPrefs.SetFloat(agentName + "X", savedPosition.x);
        PlayerPrefs.SetFloat(agentName + "Y", savedPosition.y);
        PlayerPrefs.SetFloat(agentName + "Z", savedPosition.z);

        PlayerPrefs.SetFloat(agentName + "XR", transform.rotation.x);
        PlayerPrefs.SetFloat(agentName + "YR", transform.rotation.y);
        PlayerPrefs.SetFloat(agentName + "ZR", transform.rotation.z);
        PlayerPrefs.Save();
    }
    public virtual void LoadPosition()
    {
        this.gameObject.SetActive(false);
        if (AreaController.battleReturn == true)
        {
            float x = PlayerPrefs.GetFloat(agentName + "X");
            float y = PlayerPrefs.GetFloat(agentName + "Y");
            float z = PlayerPrefs.GetFloat(agentName + "Z");
            savedPosition = new Vector3(x, y, z);
            transform.position = savedPosition;
        }
        if (AreaController.battleReturn == false)
        {
            transform.position = FindObjectOfType<DunBuilder>().createdTurnCubes[0].itemSpawnPoint.transform.position;
        }
        this.gameObject.SetActive(true);

        if (PlayerPrefs.GetInt(agentName + "Active") == 1)
        {
            move = true;
            active = true;
        }        
    }

    public virtual void Spawn()
    {

    }

    public virtual void Respawn()
    {

    }

    public virtual void SelectTargetLocation()
    {
        
    }

    public virtual void Update()
    {        

        if (move)
        {            
            bodyAnim.SetBool("walk", true);
            agent.SetDestination(targetLocation);
        }
        if (!move)
        {
            bodyAnim.SetBool("walk", false);            
        }
        if (Vector3.Distance(transform.position, targetLocation) < 1)
        {
            if (targetLocation == nextLocation)
            {
                move = false;
                finished = true;
            }
            if (targetLocation != nextLocation)
            {
                targetLocation = nextLocation;
            }

        }
        if (Vector3.Distance(transform.position, areaController.moveController.transform.position) >= 5)
        {
            if (areaController.areaUI.messageUI.GetComponent<Animator>().GetBool("solid") == true)
            {
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
            }           
        }
        if (Vector3.Distance(transform.position, areaController.moveController.transform.position) < 5)
        {
            agentMessage = true;
            if (intro == false && active == false)
            {
                bodyAnim.SetTrigger("idleBreak");             
                intro = true;                
            }
            if (active == false)
            {
                if (forHire)
                {
                    areaController.areaUI.messageText.text = "Hire " + agentName + " (" + hireCost + " Gold)";
                    areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
                }
            }
            if (!move)
            {
                if (!active && !finished)
                {

                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (forHire)
                    {
                        if (StaticMenuItems.goldCount >= hireCost)
                        {
                            StaticMenuItems.goldCount = StaticMenuItems.goldCount - hireCost;
                            PlayerPrefs.SetInt("Gold", StaticMenuItems.goldCount);
                            areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                            move = true;
                            active = true;
                            PlayerPrefs.SetInt("Agent" + 0 + "Active", 1); PlayerPrefs.Save();
                        }
                    }
                    if (!forHire)
                    {
                        areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                        move = true;
                        active = true;
                        PlayerPrefs.SetInt("Agent" + 0 + "Active", 1); PlayerPrefs.Save();
                    }
                }
            }            
        }
        if (intro && Vector3.Distance(transform.position, areaController.moveController.transform.position) > 15 && move == false)
        {
            if (!negative)
            {
                negative = true;
            }
        }
        if (Vector3.Distance(transform.position, areaController.moveController.transform.position) < 5 && finished == true)
        {
            if (goodBye == false)
            {                
                goodBye = true;
            }
        }
    }

}
