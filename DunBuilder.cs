using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class DunBuilder : MonoBehaviour
{
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
    public DunCube bossRoom;
    public List<DunCube> createdStartCubes;
    public List<DunCube> createdTurnCubes;
    public List<DunCube> createdBossHallway;
    public List<DunCube> createdBossRooms;
    public List<DunCube> createdDeadEnds;

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





    private void Awake()
    {
        if (createDungeon)
        {
            createDungeonMirror = true;
        }
        if (createDungeon == false)
        {
            createDungeonMirror = false;
            RebuildDungeon();
        }

    }

    private void Start()
    {
        if (createDungeonMirror == true)
        {
            // sets starting cube and builds first hallway
            DunCube start = Instantiate(originCube, transform.position, Quaternion.identity);

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
                totalCubes++;
                startCubeCount++;
                CloseDungeon();
                return;
            }
            if (bossHallway.ColliderCheck() == false)
            {
                bossHallStarter = bossHallway;
                bossHallway.HallwayBuild();
                totalCubes++;                
                startCubeCount++;                
                return;
            }
          
        }
        if (closedDun == false && bossRoomCreated == true)
        {
            foreach (DunCube leftoverCube in createdStartCubes)
            {
                if (createdStartCubes.IndexOf(leftoverCube) >= startCubeCount)
                {
                    DunCube deadEndCube = Instantiate(deadEnd, leftoverCube.transform.position, leftoverCube.transform.rotation);                    
                }
                closedDun = true;

                areaController.moveController.gameObject.SetActive(true);
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
    }



    public void CloseRespawn()
    {
        if (closedRespawn == false && bossRoomRespawned == false)
        {
            DunCube bossHallway = createdStartCubes[startCubeCount];
            if (bossHallway.ColliderCheck() == true)
            {
                DunCube deadEndCube = Instantiate(deadEnd, bossHallway.transform.position, bossHallway.transform.rotation);
                totalCubes++;
                startCubeCount++;
                CloseRespawn();
                return;
            }
            if (bossHallway.ColliderCheck() == false)
            {
                bossHallStarter = bossHallway;
                bossHallway.Rebuild();
                totalCubes++;
                startCubeCount++;
                return;
            }

        }
        if (closedRespawn == false && bossRoomRespawned == true)
        {
            foreach (DunCube leftoverCube in createdStartCubes)
            {
                if (createdStartCubes.IndexOf(leftoverCube) >= startCubeCount)
                {
                    DunCube deadEndCube = Instantiate(deadEnd, leftoverCube.transform.position, leftoverCube.transform.rotation);
                }
                closedRespawn = true;

                areaController.moveController.gameObject.SetActive(true);
                areaController.areaUI.gameObject.SetActive(true);
                areaController.areaUI.compassSmall.gameObject.SetActive(true);


                createDungeon = false;
                createDungeonMirror = false;

                this.gameObject.SetActive(false);
            }
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
            if (totalCubes < 1000 && totalCubes > 0)
            {
                int x = 0;
                for (x = startCubeCount; x >= startCubeCount; startCubeCount++)
                {
                    createdStartCubes[startCubeCount].HallwayBuild();
                }
                return;
            }
            if (totalCubes >= 1000)
            {
                if (bossRoomCreated == false)
                {
                    CloseDungeon();
                    Debug.Log("Closing Dungeon");
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
            if (totalCubes < 1000 && totalCubes > 0)
            {
                int x = 0;
                for (x = startCubeCount; x >= startCubeCount; startCubeCount++)
                {
                    createdStartCubes[startCubeCount].Rebuild();
                }
                return;
            }
            if (totalCubes >= 1000)
            {
                if (bossRoomRespawned == false && bossRoomCreated == false)
                {
                    CloseRespawn();
                    Debug.Log("respawn boss room");
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





