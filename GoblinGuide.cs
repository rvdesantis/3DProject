using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinGuide : DunEnemyAgent
{

    public DunBuilder dunBuilder;    



    private void Start()
    {
        
    }
    public override void SavePosition()
    {
        savedPosition = agentBodyTransform.transform.position;
        PlayerPrefs.SetFloat(agentName + "X", savedPosition.x);
        PlayerPrefs.SetFloat(agentName + "Y", savedPosition.y);
        PlayerPrefs.SetFloat(agentName + "Z", savedPosition.z);

        PlayerPrefs.SetFloat(agentName + "XR", transform.rotation.x);
        PlayerPrefs.SetFloat(agentName + "YR", transform.rotation.y);
        PlayerPrefs.SetFloat(agentName + "ZR", transform.rotation.z);

        Debug.Log(agentName + " position " + savedPosition.ToString());

        PlayerPrefs.Save();
    }   

    public override void Spawn()
    {
        dunBuilder = FindObjectOfType<DunBuilder>();
        areaController = FindObjectOfType<AreaController>();
        GoblinGuide goblin = Instantiate(this, dunBuilder.createdTurnCubes[0].itemSpawnPoint.transform.position, dunBuilder.createdTurnCubes[0].itemSpawnPoint.transform.rotation);
        goblin.dunBuilder = dunBuilder;
        goblin.areaController = areaController;
        areaController.agents.Add(goblin);
    }

    public override void Respawn()
    {
        float x = PlayerPrefs.GetFloat(agentName + "X");
        float y = PlayerPrefs.GetFloat(agentName + "Y");
        float z = PlayerPrefs.GetFloat(agentName + "Z");
        Debug.Log("Goblin Saved Position at " + x + " " + y + " " + z);
        savedPosition = new Vector3(x, y, z);

        GoblinGuide respawn = Instantiate(this, savedPosition, Quaternion.identity);
        respawn.savedPosition = savedPosition;
        respawn.dunBuilder = FindObjectOfType<DunBuilder>();
        respawn.areaController = FindObjectOfType<AreaController>();
        respawn.areaController.agents.Add(respawn);
        respawn.LoadPosition();
        respawn.SelectTargetLocation();
        if (PlayerPrefs.GetFloat(agentName + "Active") == 1)
        {
            move = true;
            active = true;
        }
    }

    public override void SelectTargetLocation()
    {        
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(areaController.bossHallwaySpawnPoint, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            Debug.Log(agentName + " Path Complete");
            targetLocation = areaController.bossHallwaySpawnPoint;
        }


        if (path.status == NavMeshPathStatus.PathPartial)
        {
            Debug.Log(agentName + " Path Partial, Checking Turn Agents");
            bool foundPath = false;
            foreach (DunCube turn in dunBuilder.createdTurnCubes)
            {
                NavMeshPath path1 = new NavMeshPath();
                NavMeshPath path2 = new NavMeshPath();
                bool pathToAgent = false;
                bool pathToTarget = false;

                agent.CalculatePath(turn.itemSpawnPoint.transform.position, path1);
                if (path1.status == NavMeshPathStatus.PathComplete)
                {
                    Debug.Log("Path to Turn " + dunBuilder.createdTurnCubes.IndexOf(turn) + " from " + agentName + " clear");
                    pathToAgent = true;
                }
                turn.navAgent.CalculatePath(areaController.bossHallwaySpawnPoint, path2);
                if (path2.status == NavMeshPathStatus.PathComplete)
                {
                    Debug.Log("Path from Turn " + dunBuilder.createdTurnCubes.IndexOf(turn) + " to boss hallway clear");
                    pathToTarget = true;
                }
                if (pathToAgent && pathToTarget)
                {
                    Debug.Log(dunBuilder.createdTurnCubes.IndexOf(turn) + " agent target set");
                    foundPath = true;
                    nextLocation = areaController.bossHallwaySpawnPoint;
                    targetLocation = turn.itemSpawnPoint.transform.position;
                    break;
                }
            }

            if (foundPath == false)
            {
                gameObject.SetActive(false);
                Debug.Log(agentName + " path not found.  deactivating");
            }
        }
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log(agentName + " Path Invalid");
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
                if (FindObjectOfType<AreaController>().areaUI.goldUI.gameObject.activeSelf)
                {
                    FindObjectOfType<AreaController>().areaUI.ToggleGold();
                    agentMessage = false;
                }
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
                    if (FindObjectOfType<AreaController>().areaUI.goldUI.gameObject.activeSelf == false)
                    {
                        FindObjectOfType<AreaController>().areaUI.ToggleGold();
                    }
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
                            if (FindObjectOfType<AreaController>().areaUI.goldUI.gameObject.activeSelf)
                            {
                                FindObjectOfType<AreaController>().areaUI.ToggleGold();
                            }
                            move = true;
                            active = true;
                            audioSource.clip = activatedClips[0];
                            audioSource.Play();
                            PlayerPrefs.SetInt(agentName + "Active", 1); PlayerPrefs.Save();
                            return;
                        }
                        if (StaticMenuItems.goldCount < hireCost)
                        {
                            audioSource.clip = activatedClips[3];
                            audioSource.Play();                            
                        }
                        return;
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
        if (intro && Vector3.Distance(transform.position, areaController.moveController.transform.position) > 12 && move == false)
        {
            if (!negative)
            {
                negative = true;
                audioSource.clip = activatedClips[2];
                audioSource.Play();
                if (FindObjectOfType<AreaController>().areaUI.goldUI.gameObject.activeSelf)
                {
                    FindObjectOfType<AreaController>().areaUI.ToggleGold();
                }
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
                if (FindObjectOfType<AreaController>().areaUI.goldUI.gameObject.activeSelf)
                {
                    FindObjectOfType<AreaController>().areaUI.ToggleGold();
                }
            }
        }

    }
}
