using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDeadEnd : DunCube
{
    public SecretWall secretWall;
    public Animator anim;

    public bool portalCube;
    public GameObject returnOrb;
    public GameObject returnParticles;
    public Chest portalChest;


    private void Start()
    {
        dunBuilder = FindObjectOfType<DunBuilder>();
        secretWall.areaController = FindObjectOfType<AreaController>();
        if (portalCube)
        {
            portalChest.areaController = FindObjectOfType<AreaController>();             
            portalChest.player = portalChest.areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();            
        }
    }

    public void PortalChest()
    {
        portalChest.areaController.moveController.enabled = false;
        returnOrb.gameObject.SetActive(true);        
        
        IEnumerator ChestTimer()
        {
            
            yield return new WaitForSeconds(1);
            portalChest.areaController.moveController.enabled = true;
            portalChest.gameObject.SetActive(false);
            yield return new WaitForSeconds(.95f);
            returnOrb.gameObject.SetActive(false);
            returnParticles.gameObject.SetActive(true);
            anim.SetBool("portalActive", true);
        } StartCoroutine(ChestTimer());
    }
}
