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

    public int enemyNumber; // refers to MasterEnemyBank position in Battle Scene
    public static int staticEnemyNumber;

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
                // enemy selection:
                int enemyRoll = Random.Range(1, areaEnemyBank.bank.Count);  // range starts at 1 as placeholders are always in position 0;
                staticEnemyNumber = enemyRoll;
                // Launching Battle
                launching = true;
                respawnPoint = areaController.moveController.transform.position;
                rotationPoint = areaController.moveController.transform.rotation;                
                areaController.areaUI.fadeOutPanel.gameObject.SetActive(true);

                AreaController.respawnPoint = respawnPoint;
                AreaController.respawnRotation = rotationPoint;
                IEnumerator LoadTimer()
                {
                    yield return new WaitForSeconds(1);
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
                } StartCoroutine(LoadTimer());               
            }
            if (battleChance < 74)
            {
                FPcontroller.steps = 0;
            }
        }



    }


}
