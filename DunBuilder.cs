using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class DunBuilder : MonoBehaviour
{
    public int targetCubeCount;
    public static bool createDungeon;
    public bool createDungeonMirror;
    public AreaController areaController;

    public OriginCube originCube;
    public DunCube startingCube;
    public DunCube hallPiece;
    public DunCube bossHallPiece;
    public DunCube leftTurn;
    public DunCube rightTurn;
    public DunCube tJunct;
    public DunCube fourWay;
    public DunCube deadEnd;
    public DunCube secretCube;
    public DunCube bossRoom;
    public List<DunCube> createdStartCubes;
    public List<DunCube> createdTurnCubes;    
    public List<DunCube> createdBossRooms;
    public List<DunCube> createdDeadEnds;
    public List<Chest> createdChests;
    public List<MimicChest> createdMimics;
    public List<Items> createdItems;

    public List<DunCube> turnBank;
    public List<GameObject> characterRotators;

    public int startCubeCount;
    public int turnCounter;
    public int cubeCounter;
    public int totalCubes;    

    public OriginCube firstCube;
    public DunCube createdCube;
    public DunCube createdTurn;
    public DunCube createdRoom;
    public DunCube bossHallStarter;

    public bool bossRoomCreated;
    public bool closedDun;
    public bool bossRoomRespawned;
    public bool closedRespawn;

    public Chest chestPrefab;
    public MimicChest mimicChest;



    private void Awake()
    {
        if (createDungeon)
        {
            createDungeonMirror = true;
            targetCubeCount = StaticMenuItems.dungeonCubeTarget;
        }
        if (createDungeon == false)
        {
            createDungeonMirror = false;
            targetCubeCount = StaticMenuItems.dungeonCubeTarget;
            areaController.areaUI.messageText.text = "Rebuilding...";
            RebuildDungeon();
        }

    }

    private void Start()
    {
        if (createDungeonMirror == true)
        {
            // sets starting cube and builds first hallway
            DunCube start = Instantiate(originCube, Vector3.zero, Quaternion.identity);

            start.posPosition = start.positive.gameObject.transform.position;
            start.negPosition = start.negative.gameObject.transform.position;
            
            createdCube = start;
        }
        if (createDungeonMirror == false)
        {
            
        }
    }

    public void RebuildDungeon()
    {
        DunCube start = Instantiate(originCube, transform.position, Quaternion.identity);
        start.posPosition = start.positive.gameObject.transform.position;
        start.negPosition = start.negative.gameObject.transform.position;
        start.dunBuilder = this;
        createdCube = start;
        start.targetCube = start;
        start.Rebuild();        
    }

    public void SpawnChests() // spawns Chests & Mimics, and saves location to PlayerPrefs
    {
        int chestCount = 0;
        // int targetChestCount = (int)Random.Range(4, Mathf.Round(createdDeadEnds.Count / 5)); will use once items are made
        int targetChestCount = Random.Range(4, 7);
        Debug.Log("Target Chest Count = " + targetChestCount);
        PlayerPrefs.SetInt("ChestCount", targetChestCount);
        for (chestCount = 0; chestCount < targetChestCount; chestCount++)
        {
            
            if (chestCount == 0)
            {
                int x = Random.Range(0, createdDeadEnds.Count - 1);
                PlayerPrefs.SetInt("Chest" + chestCount + "position", x); PlayerPrefs.Save();
                Chest newChest = Instantiate(chestPrefab, createdDeadEnds[x].itemSpawnPoint.transform.position, createdDeadEnds[x].itemSpawnPoint.transform.rotation);
                createdDeadEnds[x].cubeFilled = true;
                newChest.areaController = areaController;
                newChest.player = areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();
                createdChests.Add(newChest);
            }
            if (chestCount > 0)
            {
                int x = Random.Range(0, createdDeadEnds.Count - 1);
                bool chestChecker = false;
                foreach (Chest chest in createdChests)
                {
                    if (x != PlayerPrefs.GetInt("Chest" + createdChests.IndexOf(chest) + "position"))
                    {
                        chestChecker = false;
                    }
                    if (x == PlayerPrefs.GetInt("Chest" + createdChests.IndexOf(chest) + "position"))
                    {
                        chestChecker = true;
                    }
                }
                if (chestChecker == false)
                {
                    PlayerPrefs.SetInt("Chest" + chestCount + "position", x); PlayerPrefs.Save();
                    Chest newChest = Instantiate(chestPrefab, createdDeadEnds[x].itemSpawnPoint.transform.position, createdDeadEnds[x].itemSpawnPoint.transform.rotation);
                    createdDeadEnds[x].cubeFilled = true;
                    newChest.areaController = areaController;
                    newChest.player = areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();
                    createdChests.Add(newChest);
                }
            }

        }

        int mimicCount = (int)Mathf.Round(targetChestCount / 2);
        for (int i = 0; i < mimicCount; i++)
        {

            int x = Random.Range(0, createdDeadEnds.Count - 1);
            bool chestChecker = false;
            foreach (Chest chest in createdChests)
            {
                if (x != PlayerPrefs.GetInt("Chest" + createdChests.IndexOf(chest) + "position"))
                {
                    chestChecker = false;
                }
                if (x == PlayerPrefs.GetInt("Chest" + createdChests.IndexOf(chest) + "position"))
                {
                    chestChecker = true;
                }
            }
            foreach (MimicChest mimic in createdMimics)
            {
                if (i > 0)
                {
                    if (x != PlayerPrefs.GetInt("Mimic" + createdMimics.IndexOf(mimic) + "position"))
                    {
                        chestChecker = false;
                    }
                    if (x == PlayerPrefs.GetInt("Mimic" + createdMimics.IndexOf(mimic) + "position"))
                    {
                        chestChecker = true;
                    }
                }

            }
            if (chestChecker == false)
            {
                PlayerPrefs.SetInt("Mimic" + i + "position", x); PlayerPrefs.Save();
                MimicChest newMimic = Instantiate(mimicChest, createdDeadEnds[x].itemSpawnPoint.transform.position, createdDeadEnds[x].itemSpawnPoint.transform.rotation);
                createdDeadEnds[x].cubeFilled = true;
                newMimic.areaController = areaController;
                newMimic.battleLauncher = FindObjectOfType<BattleLauncher>();
                areaController.mimics.Add(newMimic);
                createdMimics.Add(newMimic);
            }
        }

        foreach (Chest createdChest in createdChests)
        {            
            createdChest.treasure = areaController.availableItems[Random.Range(0, areaController.availableItems.Count - 1)];
            bool treasureInUse = false;
            foreach (Items activeItem in createdItems)
            {
                if (createdChest.treasure != activeItem)
                {
                    
                }
                if (createdChest.treasure == activeItem)
                {
                    treasureInUse = true;
                }
            }
            if (treasureInUse == true)
            {
                List<int> numbersInUse = new List<int>();
                foreach (Items usedItem in createdItems)
                {
                    int x = areaController.availableItems.IndexOf(usedItem);
                    numbersInUse.Add(x);
                }
                foreach (Items item in areaController.availableItems)
                {                    
                    if (createdItems.Contains(item))
                    {

                    }
                    if (createdItems.Contains(item) == false)
                    {
                        createdChest.treasure = item;
                        break;
                    }
                }
            }
            createdItems.Add(createdChest.treasure);
            areaController.chests = createdChests;
            PlayerPrefs.SetInt("Chest" + createdChests.IndexOf(createdChest) + "Item", areaController.availableItems.IndexOf(createdChest.treasure));
        }
    }

    public void RespawnChests() // Respawns Chest & Mimics & assigned Treasures

    {
        int chestCount = PlayerPrefs.GetInt("ChestCount");
        for (int i = 0; i < chestCount; i++)
        {
            int targetCube = PlayerPrefs.GetInt("Chest" + i + "position");
            Chest newChest = Instantiate(chestPrefab, createdDeadEnds[targetCube].itemSpawnPoint.transform.position, createdDeadEnds[targetCube].itemSpawnPoint.transform.rotation);
            createdDeadEnds[targetCube].cubeFilled = true;
            newChest.areaController = areaController;
            newChest.player = areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();
            createdChests.Add(newChest);

            foreach (Chest respawnedChest in createdChests)
            {
                respawnedChest.treasure = areaController.availableItems[PlayerPrefs.GetInt("Chest" + createdChests.IndexOf(respawnedChest) + "Item")];
            }
        }
        int mimicCount = chestCount / 2;
        for (int i = 0; i < mimicCount; i++)
        {
            int targetCube = PlayerPrefs.GetInt("Mimic" + i + "position");
            MimicChest newMimic = Instantiate(mimicChest, createdDeadEnds[targetCube].itemSpawnPoint.transform.position, createdDeadEnds[targetCube].itemSpawnPoint.transform.rotation);
            createdDeadEnds[targetCube].cubeFilled = true;
            newMimic.areaController = areaController;
            newMimic.battleLauncher = FindObjectOfType<BattleLauncher>();
            areaController.mimics.Add(newMimic);
            createdMimics.Add(newMimic);

            foreach (Chest respawnedChest in createdChests)
            {
                respawnedChest.treasure = areaController.availableItems[PlayerPrefs.GetInt("Chest" + createdChests.IndexOf(respawnedChest) + "Item")];
            }
        }

    }
    public void AttachRoom()
    {
        foreach (GameObject hallwaystarter in createdTurn.junctSpawners)
        {
            if (hallwaystarter != createdTurn.junctSpawners[0]) // to not build hallway on hallway that entered room
            {
                DunCube start = Instantiate(startingCube, hallwaystarter.transform.position, hallwaystarter.gameObject.transform.rotation);
                createdStartCubes.Add(start);               

                start.dunBuilder = this;
                start.cubeCheck = totalCubes;

                

                start.posPosition = start.positive.gameObject.transform.position; 
                start.negPosition = start.negative.gameObject.transform.position;
                start.targetCube = start;
                start.nextSpawnPosition = start.transform.position;
                start.respawnPosition = start.transform.position;
                start.respawnQuat.y = start.transform.rotation.y;
                start.cubeIndex = createdStartCubes.IndexOf(start);   
            }
        }
    }

    public void CloseDungeon()
    {
        if (closedDun == false && bossRoomCreated == false)
        {
            DunCube bossHallway = createdStartCubes[startCubeCount];
            if (bossHallway.ColliderCheck() == true)
            {
                DunCube deadEndCube = Instantiate(deadEnd, bossHallway.transform.position, bossHallway.transform.rotation);
                createdDeadEnds.Add(deadEndCube);
                               
                startCubeCount++;
                CloseDungeon();
                return;
            }
            if (bossHallway.ColliderCheck() == false)
            {
                bossHallStarter = bossHallway;
                bossHallway.HallwayBuild();
                                
                startCubeCount++;                
                return;
            }
          
        }
        if (closedDun == false && bossRoomCreated == true)
        {
            Debug.Log(createdStartCubes.Count - startCubeCount + " Leftover Starting Cubes.  Creating Dead Ends & Secret Walls");
            int secretCubeCounter = 0;
            int secretIndex = 0;
            foreach (DunCube leftoverCube in createdStartCubes)
            {
                if (createdStartCubes.IndexOf(leftoverCube) >= startCubeCount)
                {                    
                    if (leftoverCube.SecretColliderCheck() == true)
                    {
                        DunCube deadEndCube = Instantiate(deadEnd, leftoverCube.transform.position, leftoverCube.transform.rotation);
                        createdDeadEnds.Add(deadEndCube);
                        secretIndex++;
                    }
                    if (leftoverCube.SecretColliderCheck() == false && secretCubeCounter <= 4 && secretIndex > 1)
                    {
                        DunCube deadEndCube = Instantiate(secretCube, leftoverCube.transform.position, leftoverCube.transform.rotation);
                        areaController.secretWalls.Add(deadEndCube.GetComponentInChildren<SecretWall>());
                        deadEndCube.GetComponentInChildren<SecretWall>().wallNumber = areaController.secretWalls.IndexOf(deadEndCube.GetComponentInChildren<SecretWall>());
                        createdDeadEnds.Add(deadEndCube);
                        secretCubeCounter++;
                        secretIndex = 0;
                    }
                    if (leftoverCube.SecretColliderCheck() == false && secretCubeCounter <= 4 && secretIndex <= 1)
                    {
                        DunCube deadEndCube = Instantiate(deadEnd, leftoverCube.transform.position, leftoverCube.transform.rotation);
                        createdDeadEnds.Add(deadEndCube);
                        secretIndex++;
                    }
                    if (leftoverCube.SecretColliderCheck() == false && secretCubeCounter > 4)
                    {
                        DunCube deadEndCube = Instantiate(deadEnd, leftoverCube.transform.position, leftoverCube.transform.rotation);
                        createdDeadEnds.Add(deadEndCube);
                    }
                }
            }
            SpawnChests();
            areaController.chests = createdChests;
            closedDun = true;
            areaController.moveController.gameObject.SetActive(true);
            areaController.moveController.enabled = true;
            areaController.areaUI.gameObject.SetActive(true);
            areaController.areaUI.compassSmall.gameObject.SetActive(true);

            PlayerPrefs.SetInt("TotalStartCubes", createdStartCubes.Count);
            PlayerPrefs.SetInt("TotalTurnCubes", turnCounter);
            PlayerPrefs.SetInt("TotalBossCubes", createdBossRooms.Count);
            PlayerPrefs.Save();
            createDungeon = false;
            createDungeonMirror = false;

            this.gameObject.SetActive(false);
        }
        
    }



    public void CloseRespawn()
    {
        if (closedRespawn == false && bossRoomRespawned == false)
        {
            DunCube bossHallway = createdStartCubes[startCubeCount];
            if (bossHallway.ColliderCheck() == true)
            {
                DunCube deadEndCube = Instantiate(deadEnd, bossHallway.transform.position, bossHallway.transform.rotation);
                createdDeadEnds.Add(deadEndCube);
                
                startCubeCount++;
                CloseRespawn();
                return;
            }
            if (bossHallway.ColliderCheck() == false)
            {
                bossHallStarter = bossHallway;
                bossHallway.Rebuild();
                
                startCubeCount++;
                return;
            }

        }
        if (closedRespawn == false && bossRoomRespawned == true)
        {
            Debug.Log(createdStartCubes.Count - startCubeCount + " Leftover Starting Cubes.  Creating Dead Ends & Secret Walls");
            int secretCubeCounter = 0;
            int secretIndex = 0;
            foreach (DunCube leftoverCube in createdStartCubes)
            {
                if (createdStartCubes.IndexOf(leftoverCube) >= startCubeCount)
                {
                    if (leftoverCube.SecretColliderCheck() == true)
                    {
                        DunCube respawnedDeadEndCube = Instantiate(deadEnd, leftoverCube.transform.position, leftoverCube.transform.rotation);
                        createdDeadEnds.Add(respawnedDeadEndCube);
                        secretIndex++;
                    }
                    if (leftoverCube.SecretColliderCheck() == false && secretCubeCounter <= 4 && secretIndex > 1)
                    {
                        DunCube respawnedSecretCube = Instantiate(secretCube, leftoverCube.transform.position, leftoverCube.transform.rotation);
                        areaController.secretWalls.Add(respawnedSecretCube.GetComponentInChildren<SecretWall>());
                        respawnedSecretCube.GetComponentInChildren<SecretWall>().wallNumber = areaController.secretWalls.IndexOf(respawnedSecretCube.GetComponentInChildren<SecretWall>());
                        createdDeadEnds.Add(respawnedSecretCube);
                        secretCubeCounter++;
                        secretIndex = 0;
                    }
                    if (leftoverCube.SecretColliderCheck() == false && secretCubeCounter <= 4 && secretIndex <= 1)
                    {
                        DunCube respawnedDeadEndCube = Instantiate(deadEnd, leftoverCube.transform.position, leftoverCube.transform.rotation);
                        createdDeadEnds.Add(respawnedDeadEndCube);
                        secretIndex++;
                    }
                    if (leftoverCube.SecretColliderCheck() == false && secretCubeCounter > 4)
                    {
                        DunCube respawnedDeadEndCube = Instantiate(deadEnd, leftoverCube.transform.position, leftoverCube.transform.rotation);
                        createdDeadEnds.Add(respawnedDeadEndCube);                        
                    }
                }
            }
            RespawnChests();
            areaController.chests = createdChests;
            closedRespawn = true;

            areaController.moveController.gameObject.SetActive(true);
            areaController.areaUI.gameObject.SetActive(true);
            areaController.areaUI.compassSmall.gameObject.SetActive(true);


            createDungeon = false;
            createDungeonMirror = false;
            areaController.Respawn();               

            this.gameObject.SetActive(false);            
        }
    }

    private void Update()
    {        
        if (createDungeonMirror == true)
        {
            if (totalCubes == 0)
            {
                createdCube.HallwayBuild();
                return;
            }
            if (totalCubes < targetCubeCount && totalCubes > 0)
            {
                int x = 0;
                for (x = startCubeCount; x >= startCubeCount; startCubeCount++)
                {
                    createdStartCubes[startCubeCount].HallwayBuild();
                }
                return;
            }
            if (totalCubes >= targetCubeCount)
            {
                if (bossRoomCreated == false)
                {
                    CloseDungeon();                    
                }
                if (bossRoomCreated == true && startCubeCount != createdStartCubes.Count - 1)
                {
                    if (closedDun == false)
                    {
                        CloseDungeon();
                        Debug.Log("Closing Dead Ends");
                    }
                }
            }
        }
        if (createDungeonMirror == false)
        {
            if (totalCubes == 0)
            {
                createdCube.Rebuild();
                return;
            }
            if (totalCubes < targetCubeCount && totalCubes > 0)
            {
                int x = 0;
                for (x = startCubeCount; x >= startCubeCount; startCubeCount++)
                {
                    createdStartCubes[startCubeCount].Rebuild();
                }
                return;
            }
            if (totalCubes >= targetCubeCount)
            {
                if (bossRoomRespawned == false && bossRoomCreated == false)
                {
                    CloseRespawn();                    
                }
                if (bossRoomRespawned == true && startCubeCount != createdStartCubes.Count - 1)
                {
                    if (closedRespawn == false)
                    {
                        CloseRespawn();
                        Debug.Log("Respawning Ends");
                    }
                }
            }
        }

    }

}





