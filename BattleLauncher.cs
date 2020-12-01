using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLauncher : MonoBehaviour
{
    public FirstPerson FPcontroller;
    public AreaController areaController;


    private float distance = 0f;
    public float cornerDistance = 0f;
    public float stepCount = 0f;

    
    public Vector3 respawnPoint;



    private void Update()
    {
        distance = FPcontroller.distanceTraveled;
        cornerDistance = FPcontroller.cornerDistance;
        stepCount = FPcontroller.steps;

        if (stepCount > 33 && cornerDistance > 10)
        {            
            int battleChance = Random.Range(0, 100);
            Debug.Log("Battle Chance " + battleChance);
            if (battleChance > 74)
            {           
                respawnPoint = FPcontroller.playerBody.transform.position;
                AreaController.respawnPoint = respawnPoint;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
            }
            if (battleChance < 74)
            {
                FPcontroller.steps = 0;
            }
        }



    }


}
