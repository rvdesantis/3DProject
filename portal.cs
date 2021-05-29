using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public AreaController areaController;
    public OriginCube origionCube;

    public enum PortalType { bossPortal, returnPortal, }
    public PortalType portalType;



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
            if (portalType == PortalType.returnPortal)
            {
                areaController.areaUI.messageText.text = "Return to Dungeon Entrance?";
            }
            if (portalType == PortalType.bossPortal)
            {
                areaController.areaUI.messageText.text = "Travel to Dungeon Boss?";
            }            
            areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                areaController.moveController.enabled = false;
                if (portalType == PortalType.returnPortal)
                {
                    areaController.moveController.transform.position = origionCube.spawnPlatform.transform.position;
                    areaController.moveController.transform.rotation = origionCube.spawnPlatform.transform.rotation;
                    areaController.moveController.enabled = true;
                    AreaController.respawnPoint = origionCube.spawnPoint;
                }
                if (portalType == PortalType.bossPortal)
                {
                    areaController.moveController.transform.position = areaController.bossHallwaySpawnPoint;
                    areaController.moveController.transform.rotation = areaController.bossHallwaySPRotation;
                    areaController.moveController.enabled = true;
                    AreaController.respawnPoint = areaController.bossHallwaySpawnPoint;
                }
            }

        }
    }
}
