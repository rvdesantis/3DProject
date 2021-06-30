using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class MedusaAgent : DunEnemyAgent
{
    public DunBuilder dunBuilder;
    public BattleLauncher launcher;
    public CinemachineVirtualCamera medusaCam;
    private void Start()
    {        
        SelectTargetLocation();
        launcher = FindObjectOfType<BattleLauncher>();
    }

    public override void SelectTargetLocation()
    {
        targetLocation = areaController.moveController.transform.position;
    }

    private void FixedUpdate()
    {
        SelectTargetLocation();
    }

    public override void Spawn()
    {
        dunBuilder = FindObjectOfType<DunBuilder>();
        areaController = FindObjectOfType<AreaController>();  
        int roomNum = Random.Range(0, dunBuilder.createdRooms.Count);
        Debug.Log("Medusa spawned in room " + roomNum);     
        MedusaAgent medusa = Instantiate(this, dunBuilder.createdRooms[roomNum].itemSpawnPoint.transform.position, dunBuilder.createdRooms[roomNum].itemSpawnPoint.transform.rotation);
        medusa.dunBuilder = dunBuilder;
        medusa.areaController = areaController;
        areaController.agents.Add(medusa);
        medusa.move = true;
        PlayerPrefs.SetInt(agentName + "Active", 1); PlayerPrefs.Save();

    }

    public override void Respawn()
    {
        if (PlayerPrefs.GetInt(agentName + "Active") == 1)
        {
            float x = PlayerPrefs.GetFloat(agentName + "X");
            float y = PlayerPrefs.GetFloat(agentName + "Y");
            float z = PlayerPrefs.GetFloat(agentName + "Z");
            savedPosition = new Vector3(x, y, z);

            MedusaAgent respawn = Instantiate(this, savedPosition, Quaternion.identity);
            respawn.savedPosition = savedPosition;
            respawn.dunBuilder = FindObjectOfType<DunBuilder>();
            respawn.areaController = FindObjectOfType<AreaController>();
            respawn.LoadPosition();
            respawn.move = true;
            respawn.active = true;
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
        if (Vector3.Distance(transform.position, targetLocation) < 5)
        {
            launcher.FPcontroller.steps = 0;
            launcher.launching = true;
            launcher.respawnPoint = areaController.moveController.transform.position;
            launcher.rotationPoint = areaController.moveController.transform.rotation;            
            areaController.moveController.enabled = false;
            foreach (DunEnemyAgent agent in areaController.agents)
            {
                agent.SavePosition();
            }
            AreaController.respawnPoint = launcher.respawnPoint;
            AreaController.respawnRotation = launcher.rotationPoint;
            IEnumerator LoadTimer()
            {
                medusaCam.m_Priority = 10;
                bodyAnim.SetTrigger("taunt");
                yield return new WaitForSeconds(3);
                areaController.areaUI.fadeOutPanel.gameObject.SetActive(true);
                PlayerPrefs.SetInt(agentName + "Active", 0); PlayerPrefs.Save();
                yield return new WaitForSeconds(1);
                BattleController.specialEnemyNum = 2;
                BattleLauncher.dunEnemy = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
            }
            StartCoroutine(LoadTimer());
        }
    }

}
