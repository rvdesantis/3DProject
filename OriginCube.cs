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
    private void Awake()
    {
        dunBuilder = FindObjectOfType<DunBuilder>();
        areaController = FindObjectOfType<AreaController>();
        CharacterController characterSpawn = Instantiate(characterController, spawnPlatform.transform.position, spawnPlatform.transform.rotation);
        areaController.moveController = characterSpawn;
        areaController.FPcontroller = characterSpawn.GetComponentInChildren<FirstPerson>();
        areaController.playerBody = characterSpawn.GetComponentInChildren<FirstPersonPlayer>().gameObject;
        areaController.areaUI.compass1.player = characterSpawn.GetComponentInChildren<FirstPersonPlayer>().transform;
        areaController.areaUI.compass2.player = characterSpawn.GetComponentInChildren<FirstPersonPlayer>().transform;
        characterSpawn.GetComponentInChildren<FirstPerson>().areaController = areaController;
        characterSpawn.GetComponentInChildren<FirstPerson>().rotator0 = dunBuilder.characterRotators[0];
        characterSpawn.GetComponentInChildren<FirstPerson>().rotator90 = dunBuilder.characterRotators[1];
        characterSpawn.GetComponentInChildren<FirstPerson>().rotator180 = dunBuilder.characterRotators[2];
        characterSpawn.GetComponentInChildren<FirstPerson>().rotator270 = dunBuilder.characterRotators[3];
        exitdoor.player = characterSpawn.GetComponentInChildren<FirstPersonPlayer>();
        battleLauncher = FindObjectOfType<BattleLauncher>();
        battleLauncher.FPcontroller = characterSpawn.GetComponentInChildren<FirstPerson>();
    }


    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
