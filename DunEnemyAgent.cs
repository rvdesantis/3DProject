using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DunEnemyAgent : MonoBehaviour
{
    public string agentName;
    public bool forHire;
    public AreaController areaController;
    public NavMeshAgent agent;
    public GameObject agentBodyTransform;
    public AgentLinkMover mover;
    public Vector3 targetLocation;
    public Vector3 savedPosition;

    public Animator bodyAnim;
    public BoxCollider boxCollider;

    public AudioSource audioSource;
    public List<AudioClip> introClips;
    public List<AudioClip> activatedClips;
    public List<AudioClip> returnClips;

    
    public bool move;
    public bool active;
    public bool finished;

    // below used in updates, not public
    bool intro = false;
    bool goodBye = false;
    bool negative = false;


    private void Start()
    {
        areaController = FindObjectOfType<AreaController>();
    }

    public void SavePosition()
    {
        savedPosition = transform.position;
        PlayerPrefs.SetFloat("Agent" + 0 + "X", savedPosition.x);
        PlayerPrefs.SetFloat("Agent" + 0 + "Y", savedPosition.y);
        PlayerPrefs.SetFloat("Agent" + 0 + "Z", savedPosition.z);
        PlayerPrefs.Save();
    }
    public void LoadPosition()
    {
        agent.gameObject.SetActive(false);
        if (AreaController.battleReturn == true)
        {
            float x = PlayerPrefs.GetFloat("Agent" + 0 + "X");
            float y = PlayerPrefs.GetFloat("Agent" + 0 + "Y");
            float z = PlayerPrefs.GetFloat("Agent" + 0 + "Z");
            savedPosition = new Vector3(x, y, z);
            transform.position = savedPosition;
        }
        if (AreaController.battleReturn == false)
        {
            transform.position = FindObjectOfType<DunBuilder>().createdTurnCubes[0].itemSpawnPoint.transform.position;
        }        
        agent.gameObject.SetActive(true);

        if (PlayerPrefs.GetInt("Agent" + 0 + "Active") == 1)
        {
            int talk = Random.Range(0, 2);
            if (talk == 1)
            {
                audioSource.clip = returnClips[Random.Range(0, returnClips.Count)];
                audioSource.Play();
            }
            move = true;
            active = true;
        }        
    }



    public void SelectTargetLocation()
    {
        
    }

    private void Update()
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
            move = false;
            finished = true;
        }
        if (Vector3.Distance(transform.position, areaController.moveController.transform.position) < 5)
        {
            if (intro == false && active == false)
            {
                bodyAnim.SetTrigger("idleBreak");
                audioSource.clip = introClips[Random.Range(0, introClips.Count)];
                audioSource.Play();
                areaController.areaUI.messageText.text = "Hire " + agentName + " (Free)";
                areaController.areaUI.TriggerMessage();
                intro = true;
                
            }
            if (!move)
            {
                if (!active && !finished)
                {

                }
                if (Input.GetKeyDown(KeyCode.Space))
                {                    
                    areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);                    
                    move = true;
                    active = true;
                    audioSource.clip = activatedClips[0];
                    audioSource.Play();
                    PlayerPrefs.SetInt("Agent" + 0 + "Active", 1); PlayerPrefs.Save();
                }
            }            
        }
        if (intro && Vector3.Distance(transform.position, areaController.moveController.transform.position) > 15 && move == false)
        {
            if (!negative)
            {
                negative = true;
                audioSource.clip = activatedClips[2];
                audioSource.Play();
            }
        }
        if (Vector3.Distance(transform.position, areaController.moveController.transform.position) < 5 && finished == true)
        {
            if (goodBye == false)
            {
                bodyAnim.SetTrigger("idleBreak");
                audioSource.clip = activatedClips[1];
                audioSource.Play();
                goodBye = true;
            }
        }
    }

}
