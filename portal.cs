using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public AreaController areaController;
    public OriginCube origionCube;

    private void Start()
    {
        areaController = FindObjectOfType<AreaController>();
        origionCube = FindObjectOfType<OriginCube>();
    }

    private void Update()
    {
        if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) > 8)
        {
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);                
        }
        if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) < 7)
        {
            areaController.areaUI.messageText.text = "Return to Dungeon Entrance?";
            areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                areaController.moveController.enabled = false;
                areaController.moveController.transform.position = origionCube.spawnPlatform.transform.position;
                areaController.moveController.transform.rotation = origionCube.spawnPlatform.transform.rotation;
                areaController.moveController.enabled = true;
                AreaController.respawnPoint = origionCube.spawnPoint;
            }

        }
    }
}
