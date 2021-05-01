using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginCube : DunCube
{
    public AreaController areaController;
    public BattleLauncher battleLauncher;
    public CharacterController characterController;
    public GameObject spawnPlatform;
    public Vector3 spawnPoint;
    public FirstPerson firstPerson;
    public FirstPersonPlayer firstPersonPlayer;
    public Exit exitdoor;

    // Start is called before the first frame update
    void Start()
    {
        dunBuilder = FindObjectOfType<DunBuilder>();
        areaController = FindObjectOfType<AreaController>();
        CharacterController characterSpawn = Instantiate(characterController, spawnPlatform.transform.position, spawnPlatform.transform.rotation);        
        areaController.moveController = characterSpawn;        
        areaController.FPcontroller = characterSpawn.GetComponentInChildren<FirstPerson>(); 
        firstPerson = characterSpawn.GetComponentInChildren<FirstPerson>();
        firstPerson.areaController = areaController;
        firstPerson.rotator0 = dunBuilder.characterRotators[0];
        firstPerson.rotator90 = dunBuilder.characterRotators[1];
        firstPerson.rotator180 = dunBuilder.characterRotators[2];
        firstPerson.rotator270 = dunBuilder.characterRotators[3];
        exitdoor.player = characterSpawn.GetComponentInChildren<FirstPersonPlayer>();
        battleLauncher = FindObjectOfType<BattleLauncher>();
        battleLauncher.FPcontroller = firstPerson;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
