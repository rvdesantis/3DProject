using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDeadEnd : DunCube
{
    public SecretWall secretWall;
    public Animator anim;
    public GameObject portalParticlesObject;

    private void Start()
    {
        dunBuilder = FindObjectOfType<DunBuilder>();
        secretWall.areaController = FindObjectOfType<AreaController>();
    }

    public void PortalChest()
    {
        portalParticlesObject.gameObject.SetActive(true);
        anim.SetBool("portalActive", true);
    }
}
