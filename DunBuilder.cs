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
    public int cubeCounter;
    public int totalCubes;

    public OriginCube firstCube;
    public DunCube createdCube;
    public DunCube createdTurn;
    public DunCube createdRoom;
    public DunCube bossHallStarter;

    public bool bossRoomCreated;
    public bool closedDun;
    public CinemachineVirtualCamera buildCam;
    public CinemachineVirtualCamera firstPersonCam;


    private void Awake()
    {
        if (createDungeon)
        {
            createDungeonMirror = true;
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
            RebuildDungeon();
        }
    }

    public void RebuildDungeon()
    {
        int sc = 0;

        for (sc = 0; sc < PlayerPrefs.GetInt("TotalStartCubes"); sc++)
        {
            createdStartCubes.Add(startingCube);
        }
        foreach(DunCube startingCube in createdStartCubes)
        {
            Vector3 respawnPoint = new Vector3(PlayerPrefs.GetInt("StartCube" + createdStartCubes.IndexOf(startingCube) + "X"), 0, PlayerPrefs.GetInt("StartCube" + createdStartCubes.IndexOf(startingCube) + "Z"));
            Instantiate(startingCube, respawnPoint, Quaternion.identity);
            startingCube.transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("StartCube" + createdStartCubes.IndexOf(startingCube) + "Rotate"), 0);
            startingCube.Rebuild();
        }

        int turn = 0;
        for (turn = 0; sc < PlayerPrefs.GetInt("TotalTurnCubes"); turn++)
        {
            createdTurnCubes.Add(startingCube);
        }
    }




    public void AttachRoom()
    {
        foreach (GameObject hallwaystarter in createdTurn.junctNegs)
        {
            if (hallwaystarter != createdTurn.junctNegs[0]) // to not build hallway on hallway that entered room
            {
                DunCube start = Instantiate(startingCube, hallwaystarter.transform.position, hallwaystarter.gameObject.transform.rotation);
                createdStartCubes.Add(start);
                PlayerPrefs.SetFloat("StartCube" + createdStartCubes.IndexOf(start) + "X", start.transform.position.x);
                PlayerPrefs.SetFloat("StartCube" + createdStartCubes.IndexOf(start) + "Y", start.transform.position.y);
                PlayerPrefs.SetFloat("StartCube" + createdStartCubes.IndexOf(start) + "Z", start.transform.position.z);
                PlayerPrefs.SetFloat("StartCube" + createdStartCubes.IndexOf(start) + "Rotate", start.transform.rotation.y);

                start.dunBuilder = this;
                start.cubeCheck = totalCubes;

                

                start.posPosition = start.positive.gameObject.transform.position; 
                start.negPosition = start.negative.gameObject.transform.position;
                start.targetCube = start;
                start.nextSpawnPosition = start.transform.position;
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
                buildCam.gameObject.SetActive(false);

                PlayerPrefs.SetInt("TotalStartCubes", createdStartCubes.Count);
                PlayerPrefs.SetInt("TotalTurnCubes", createdTurnCubes.Count);
                PlayerPrefs.SetInt("TotalBossCubes", createdBossRooms.Count);

                this.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {        
        //if (Input.GetKey(KeyCode.Space))
        

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

}





