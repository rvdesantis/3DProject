using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinGuide : DunEnemyAgent
{


    public override void SavePosition()
    {
        savedPosition = transform.position;
        PlayerPrefs.SetFloat("Agent" + 0 + "X", savedPosition.x);
        PlayerPrefs.SetFloat("Agent" + 0 + "Y", savedPosition.y);
        PlayerPrefs.SetFloat("Agent" + 0 + "Z", savedPosition.z);
        PlayerPrefs.Save();
    }
    public override void LoadPosition()
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

    public override void Update()
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
        if (Vector3.Distance(transform.position, areaController.moveController.transform.position) >= 5)
        {
            if (agentMessage == true)
            {
                if (areaController.areaUI.messageUI.GetComponent<Animator>().GetBool("solid") == true)
                {
                    areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                }
            }
        }
        if (Vector3.Distance(transform.position, areaController.moveController.transform.position) < 5)
        {
            agentMessage = true;
            if (intro == false && active == false)
            {
                bodyAnim.SetTrigger("idleBreak");
                audioSource.clip = introClips[Random.Range(0, introClips.Count)];
                audioSource.Play();
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
                            audioSource.clip = activatedClips[0];
                            audioSource.Play();
                            PlayerPrefs.SetInt("Agent" + 0 + "Active", 1); PlayerPrefs.Save();
                        }
                        if (StaticMenuItems.goldCount >= hireCost)
                        {
                            audioSource.clip = activatedClips[3];
                            audioSource.Play();                            
                        }
                    }
                    if (!forHire)
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
