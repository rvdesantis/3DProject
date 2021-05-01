using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DunCube : MonoBehaviour
{
    public DunBuilder dunBuilder;

    public int extenderLength;

    public GameObject positive;
    public GameObject negative;
    
    public int cubeCheck;
    public List<GameObject> junctNegs; // for intersections

    public DunCube targetCube;


    public Vector3 posPosition;
    public Vector3 negPosition;
    public Vector3 nextSpawnPosition;

    public List<GameObject> cubeLights;


    private void Start()
    {
        dunBuilder = FindObjectOfType<DunBuilder>();  // only works for first load cube created in scene.  Must assign other's directly.  
    }
    public void HallwayBuild()
    {      
        if (cubeCheck == 0)
        {
            int cubeCounter = 0;
            int hallwayLength = Random.Range(5, 26);
            extenderLength = hallwayLength;
            PlayerPrefs.SetInt("FirstCubeHallNum", hallwayLength);
            int lightCounter = 0;

            for (cubeCounter = 0; cubeCounter < hallwayLength; cubeCounter++)
            {
                DunCube createdCube = dunBuilder.createdCube;                    
                Vector3 thisSpawnPosition = new Vector3(createdCube.posPosition.x - createdCube.negPosition.x, 0, createdCube.posPosition.z - createdCube.negPosition.z) * cubeCounter; // length comes out to be about 5.8 for standard cube   
                DunCube nextCube = Instantiate(dunBuilder.hallPiece, thisSpawnPosition, createdCube.transform.rotation);
                nextCube.posPosition = nextCube.positive.gameObject.transform.position;
                nextCube.negPosition = nextCube.negative.gameObject.transform.position;
                dunBuilder.createdCube = nextCube;
                dunBuilder.totalCubes++;

                lightCounter++;
                if (lightCounter == 3)
                {
                    nextCube.cubeLights[Random.Range(0, 2)].gameObject.SetActive(true);
                    lightCounter = 0;
                }

                nextSpawnPosition = new Vector3(createdCube.posPosition.x - createdCube.negPosition.x, 0, createdCube.posPosition.z - createdCube.negPosition.z) * (cubeCounter + 1); 
            }

            dunBuilder.createdTurn = Instantiate(dunBuilder.fourWay, nextSpawnPosition, dunBuilder.createdCube.transform.rotation);  
            Debug.Log("4 Way " + cubeCounter + " spawn position is " + nextSpawnPosition);
            dunBuilder.AttachRoom();
            return;
        }
        if (cubeCheck > 0 && cubeCheck < 1000)
        {
            int cubeCounter = 0;
            int hallwayLength = Random.Range(5, 26);
            extenderLength = hallwayLength;
            PlayerPrefs.SetInt("StartCube" + dunBuilder.createdStartCubes.IndexOf(targetCube) + "HallNum", hallwayLength);
            int lightCounter = 0;

            if (ColliderCheck() == false)
            {
                for (cubeCounter = 0; cubeCounter < hallwayLength; cubeCounter++)
                {
                    
                    Vector3 thisSpawnPosition = new Vector3(targetCube.posPosition.x - targetCube.negPosition.x, 0, targetCube.posPosition.z - targetCube.negPosition.z) + (Vector3.zero * cubeCounter);
                    nextSpawnPosition = targetCube.transform.position + thisSpawnPosition;
                    DunCube nextCube = Instantiate(dunBuilder.hallPiece, nextSpawnPosition, targetCube.transform.rotation);
                    nextCube.posPosition = nextCube.positive.gameObject.transform.position;
                    nextCube.negPosition = nextCube.negative.gameObject.transform.position;
                    targetCube = nextCube;
                    dunBuilder.totalCubes++;

                    lightCounter++;
                    if (lightCounter == 3)
                    {
                        nextCube.cubeLights[Random.Range(0, 2)].gameObject.SetActive(true);
                        lightCounter = 0;
                    }
                }

                int turnNumber = Random.Range(0, dunBuilder.turnBank.Count); // range does not include end, no need for - 1
                DunCube randomTurn = dunBuilder.turnBank[turnNumber];
                dunBuilder.createdTurn = Instantiate(randomTurn, nextSpawnPosition, targetCube.transform.rotation);

                Debug.Log(dunBuilder.turnBank[turnNumber].name + cubeCounter + " spawn position is " + nextSpawnPosition);
                dunBuilder.AttachRoom();
                return;
            }
            if (ColliderCheck() == true)
            {   
                DunCube deadEndCube = Instantiate(dunBuilder.deadEnd, transform.position, transform.rotation);                
                dunBuilder.totalCubes++;  
            }
        }
        if (cubeCheck >= 1000 && dunBuilder.bossRoomCreated == true)
        {
            Debug.Log("Dungeon Already Finished.  You shouldn't be seeing this message");
        }
        if (cubeCheck >= 1000 && dunBuilder.bossRoomCreated == false)
        {
            int cubeCounter = 0;
            int hallwayLength = Random.Range(5, 26);
            extenderLength = hallwayLength;
            targetCube = dunBuilder.bossHallStarter;
            PlayerPrefs.SetInt("StartCube" + dunBuilder.createdStartCubes.IndexOf(targetCube) + "HallNum", hallwayLength);
            
            Vector3 thisSpawnPosition = new Vector3(targetCube.posPosition.x - targetCube.negPosition.x, 0, targetCube.posPosition.z - targetCube.negPosition.z) + (Vector3.zero * cubeCounter);
            nextSpawnPosition = targetCube.transform.position + thisSpawnPosition;
            DunCube nextCube = Instantiate(dunBuilder.hallPiece, nextSpawnPosition, targetCube.transform.rotation);
            nextCube.posPosition = nextCube.positive.gameObject.transform.position;
            nextCube.negPosition = nextCube.negative.gameObject.transform.position;
            targetCube = nextCube;
            dunBuilder.totalCubes++;            
            dunBuilder.bossRoomCreated = Instantiate(dunBuilder.bossRoom, nextSpawnPosition, targetCube.transform.rotation);
            Debug.Log(dunBuilder.bossRoomCreated.ToString() + cubeCounter + " spawn position is " + nextSpawnPosition);
            dunBuilder.bossRoomCreated = true;
            
            return;
        }

    }
    public void Rebuild()
    {
        if (cubeCheck == 0)
        {
            int cubeCounter = 0;
            int hallwayLength = PlayerPrefs.GetInt("FirstCubeHallNum");
            extenderLength = hallwayLength;

            int lightCounter = 0;

            for (cubeCounter = 0; cubeCounter < hallwayLength; cubeCounter++)
            {
                DunCube createdCube = dunBuilder.createdCube;
                Vector3 thisSpawnPosition = new Vector3(createdCube.posPosition.x - createdCube.negPosition.x, 0, createdCube.posPosition.z - createdCube.negPosition.z) * cubeCounter; // length comes out to be about 5.8 for standard cube   
                DunCube nextCube = Instantiate(dunBuilder.hallPiece, thisSpawnPosition, createdCube.transform.rotation);
                nextCube.posPosition = nextCube.positive.gameObject.transform.position;
                nextCube.negPosition = nextCube.negative.gameObject.transform.position;
                dunBuilder.createdCube = nextCube;
                dunBuilder.totalCubes++;

                lightCounter++;
                if (lightCounter == 3)
                {
                    nextCube.cubeLights[Random.Range(0, 2)].gameObject.SetActive(true);
                    lightCounter = 0;
                }

                nextSpawnPosition = new Vector3(createdCube.posPosition.x - createdCube.negPosition.x, 0, createdCube.posPosition.z - createdCube.negPosition.z) * (cubeCounter + 1);
            }
        }
        if (cubeCheck != 0)
        {
            int cubeCounter = 0;
            int hallwayLength = PlayerPrefs.GetInt("StartCube" + dunBuilder.createdStartCubes.IndexOf(targetCube) + "HallNum");
            extenderLength = hallwayLength;

            int lightCounter = 0;

            for (cubeCounter = 0; cubeCounter < hallwayLength; cubeCounter++)
            {
                DunCube createdCube = dunBuilder.createdCube;
                Vector3 thisSpawnPosition = new Vector3(createdCube.posPosition.x - createdCube.negPosition.x, 0, createdCube.posPosition.z - createdCube.negPosition.z) * cubeCounter; // length comes out to be about 5.8 for standard cube   
                DunCube nextCube = Instantiate(dunBuilder.hallPiece, thisSpawnPosition, createdCube.transform.rotation);
                nextCube.posPosition = nextCube.positive.gameObject.transform.position;
                nextCube.negPosition = nextCube.negative.gameObject.transform.position;
                dunBuilder.createdCube = nextCube;
                dunBuilder.totalCubes++;

                lightCounter++;
                if (lightCounter == 3)
                {
                    nextCube.cubeLights[Random.Range(0, 2)].gameObject.SetActive(true);
                    lightCounter = 0;
                }

                nextSpawnPosition = new Vector3(createdCube.posPosition.x - createdCube.negPosition.x, 0, createdCube.posPosition.z - createdCube.negPosition.z) * (cubeCounter + 1);
            }
        }
            

            
    }

    public bool ColliderCheck()
    {
        RaycastHit hit;
        if (Physics.SphereCast(positive.transform.position, 5, positive.transform.forward, out hit, (extenderLength * 6) + 150)) 
        {
            bool didHit = true;
            Debug.DrawRay(posPosition, posPosition * hit.distance, Color.red);
            Debug.Log(name + dunBuilder.createdStartCubes.IndexOf(this) +" did hit at " + hit.distance);
            return didHit;
        }
        else
        {
            bool didHit = false;
            Debug.DrawRay(positive.transform.position, positive.transform.forward * 1000, Color.white);            
            return didHit;
        }
    }
}
