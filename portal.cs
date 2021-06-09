using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public AreaController areaController;
    public OriginCube origionCube;
    public GameObject portalGameObject;
    public enum PortalType { bossPortal, returnPortal, treasure}
    public PortalType portalType;

    public GameObject portalFX;
    public GameObject returnPortalFX;

    public GameObject travelPlatform;
    public Vector3 travelPosition;
    public AudioSource audioSource;
    public List<AudioClip> audioClips; // 0 set for FX, 1 set for voice(if any)
    public bool voice;


    private void Start()
    {
        areaController = FindObjectOfType<AreaController>();
        origionCube = FindObjectOfType<OriginCube>();
        if (voice)
        {
            audioSource.PlayOneShot(audioClips[1], 1);
        }        
    }

    private void Update()
    {
        if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) > 5)
        {
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);                
        }
        if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) < 5)
        {
            if (portalType == PortalType.returnPortal)
            {
                areaController.areaUI.messageText.text = "Return to Dungeon Entrance?";
            }
            if (portalType == PortalType.bossPortal)
            {
                areaController.areaUI.messageText.text = "Travel to Dungeon Boss?";
            }
            if (portalType == PortalType.treasure)
            {
                areaController.areaUI.messageText.text = "???";
            }
            areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                areaController.moveController.enabled = false;
                if (portalType == PortalType.returnPortal)
                {
                    areaController.moveController.transform.position = origionCube.spawnPlatform.transform.position;
                    areaController.moveController.transform.rotation = origionCube.spawnPlatform.transform.rotation;
                    areaController.moveController.transform.Rotate(0, 180, 0);
                    areaController.moveController.enabled = true;
                    AreaController.respawnPoint = origionCube.spawnPoint;
                    portalFX.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                    areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                }
                if (portalType == PortalType.bossPortal)
                {                    
                    areaController.moveController.transform.position = areaController.bossHallwaySpawnPoint;
                    areaController.moveController.transform.rotation = areaController.bossHallwaySPRotation;
                    areaController.moveController.enabled = true;
                    AreaController.respawnPoint = areaController.bossHallwaySpawnPoint;
                    portalFX.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                    areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                }
                if (portalType == PortalType.treasure)
                {
                    areaController.moveController.transform.position = travelPlatform.transform.position;
                    areaController.moveController.transform.rotation = travelPlatform.transform.rotation;
                    areaController.moveController.enabled = true;
                    AreaController.respawnPoint = travelPlatform.transform.position;
                    areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                }
            }

        }
    }
}
