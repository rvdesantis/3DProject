using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunBuilder : MonoBehaviour
{
    public DunCube startingCube;
    public DunCube hallPiece;
    public DunCube leftTurn;
    public DunCube rightTurn;
    public DunCube tJunct;
    public DunCube fourWay;
    public List<DunCube> startCubes;
    public List<DunCube> turnBank;

    public int startCubeCount;
    public int cubeCounter;
    public int totalCubes;

    public DunCube createdCube;
    public DunCube createdTurn;
    public DunCube createdRoom;



    private void Start()
    {
        // sets starting cube and builds first hallway
        DunCube start = Instantiate(startingCube, transform.position, Quaternion.identity);
        start.posPosition = start.positive.gameObject.transform.position;
        start.negPosition = start.negative.gameObject.transform.position;
        createdCube = start;
    }






    public void AttachRoom()
    {
        foreach (GameObject hallwaystarter in createdTurn.junctNegs)
        {
            if (hallwaystarter != createdTurn.junctNegs[0]) // to not build hallway on hallway that entered room
            {
                DunCube start = Instantiate(startingCube, hallwaystarter.transform.position, hallwaystarter.gameObject.transform.rotation);
                startCubes.Add(start);                                
                start.dunBuilder = this;
                start.cubeCheck = totalCubes;

                

                start.posPosition = start.positive.gameObject.transform.position; 
                start.negPosition = start.negative.gameObject.transform.position;
                start.targetCube = start;
                start.nextSpawnPosition = start.transform.position;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (totalCubes == 0)
            {
                createdCube.HallwayBuild();
                return;
            }
            if (totalCubes < 1000 && totalCubes > 0)
            {
                foreach (DunCube newStartCube in startCubes)
                {
                    if (startCubes.IndexOf(newStartCube) >= startCubeCount)
                    {
                        IEnumerator DataTimer()
                        {
                            yield return new WaitForSeconds(.5f);
                            newStartCube.HallwayBuild();
                            Debug.Log("Start Cube " + startCubeCount + " now building");
                            startCubeCount++;
                        }
                        StartCoroutine(DataTimer());
                    }
                }
                return;
            }
            if (totalCubes > 1000)
            {
                Debug.Log("Too Many Cubes");
            }
        }
    }

}





