using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    public AreaController areaController;
    public int wallNumber;

    
    
    
    
    public GameObject disolver;
    public bool open;
    
    public bool enemyTrigger;
    public DunEnemy dunEnemy;



    private void Start()
    {
       

        
    }

    public void WallDisolver()
    {
        open = true;
        gameObject.SetActive(false);
        disolver.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Door" + wallNumber, 1);
        PlayerPrefs.Save();
    }

    

}
