using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLauncher : MonoBehaviour
{
    public FirstPerson FPcontroller;
    public AreaController areaController;

    public EnemyBank areaEnemyBank; // enemy positions must match enemy bank in Battle;
    public EnemyBank specialEnemyBank; // for bosses, mimics, and enemys launch at a specific event, not based off steps


    private float distance = 0f;
    public float cornerDistance = 0f;
    public float stepCount = 0f;

    
    public Vector3 respawnPoint;
    public Quaternion rotationPoint;
    public bool launching;



    public static bool mimic;
    public static bool dunEnemy;
    public static bool bossEnemy;

    private void Update()
    {
        distance = FPcontroller.distanceTraveled;        
        stepCount = FPcontroller.steps;
        if (stepCount > 33)
        {
            
            int battleChance = Random.Range(0, 100);
            Debug.Log("Battle Chance " + battleChance);
            if (battleChance > 66)
            {
                FPcontroller.steps = 0;
                launching = true;
                respawnPoint = areaController.moveController.transform.position;
                rotationPoint = areaController.moveController.transform.rotation;                
                areaController.areaUI.fadeOutPanel.gameObject.SetActive(true);
                areaController.moveController.enabled = false;
                TimerController.instance.StopTimer();
                foreach(DunEnemyAgent agent in areaController.agents)
                {
                    agent.SavePosition();
                }
                AreaController.respawnPoint = respawnPoint;
                AreaController.respawnRotation = rotationPoint;
                IEnumerator LoadTimer()
                {
                    yield return new WaitForSeconds(1);
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
                } StartCoroutine(LoadTimer());               
            }
            if (battleChance < 66)
            {
                FPcontroller.steps = 0;
            }
        }



    }


}
