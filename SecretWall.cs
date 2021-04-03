using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    public AreaController areaController;
    public int wallNumber;

    public List<SecretWall> walls;
    
    public GameObject disolver;
    public bool open;
    public static bool staticOpen;
    public bool enemyTrigger;
    public DunEnemy dunEnemy;


    private void Start()
    {
        areaController = FindObjectOfType<AreaController>();
        walls = areaController.secretWalls;
        
        
        wallNumber = walls.IndexOf(this);
        if (wallNumber == 0)
        {
            wallNumber = 100;
        }
        foreach(SecretWall wall in walls)
        {
            if (open)
            {

            }
        }        
    }

    public void WallDisolver()
    {
        open = true;
        gameObject.SetActive(false);
        disolver.gameObject.SetActive(true);
        staticOpen = true;        
    }

}
