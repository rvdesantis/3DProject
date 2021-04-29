using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunCube : MonoBehaviour
{
    public DunBuilder dunBuilder;

    public GameObject positive;
    public GameObject negative;
    
    public int cubeCheck;
    public List<GameObject> junctNegs; // for intersections

    public DunCube targetCube;


    public Vector3 posPosition;
    public Vector3 negPosition;
    public Vector3 nextSpawnPosition;


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
            for (cubeCounter = 0; cubeCounter < hallwayLength; cubeCounter++)
            {
                DunCube createdCube = dunBuilder.createdCube;                    
                Vector3 thisSpawnPosition = new Vector3(createdCube.posPosition.x - createdCube.negPosition.x, 0, createdCube.posPosition.z - createdCube.negPosition.z) * cubeCounter; // length comes out to be about 5.8 for standard cube                
                Debug.Log("Cube " + cubeCounter + " spawn position is " + thisSpawnPosition);
                DunCube nextCube = Instantiate(dunBuilder.hallPiece, thisSpawnPosition, createdCube.transform.rotation);
                nextCube.posPosition = nextCube.positive.gameObject.transform.position;
                nextCube.negPosition = nextCube.negative.gameObject.transform.position;
                dunBuilder.createdCube = nextCube;
                dunBuilder.totalCubes++;

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
            for (cubeCounter = 0; cubeCounter < hallwayLength; cubeCounter++)
            {                                   
                Vector3 thisSpawnPosition = new Vector3(targetCube.posPosition.x - targetCube.negPosition.x, 0, targetCube.posPosition.z - targetCube.negPosition.z) + (Vector3.zero * cubeCounter);
                nextSpawnPosition = targetCube.transform.position + thisSpawnPosition;
                Debug.Log("Cube " + cubeCounter + " spawn position is " + thisSpawnPosition);
                DunCube nextCube = Instantiate(dunBuilder.hallPiece, nextSpawnPosition, targetCube.transform.rotation);
                nextCube.posPosition = nextCube.positive.gameObject.transform.position;
                nextCube.negPosition = nextCube.negative.gameObject.transform.position;
                targetCube = nextCube;                
                dunBuilder.totalCubes++;                
            }

            int turnNumber = Random.Range(0, dunBuilder.turnBank.Count); // range does not include end, no need for - 1
            DunCube randomTurn = dunBuilder.turnBank[turnNumber];
            dunBuilder.createdTurn = Instantiate(randomTurn, nextSpawnPosition, targetCube.transform.rotation);  
            Debug.Log("4 Way " + cubeCounter + " spawn position is " + nextSpawnPosition);
            dunBuilder.AttachRoom();  

        }
        if (cubeCheck >= 1000)
        {
            Debug.Log("Too many Cubes");                
        }    
    }
}
