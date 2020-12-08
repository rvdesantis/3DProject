using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaController : MonoBehaviour
{
    public FirstPersonPlayer player;
    public FirstPerson FPcontroller;
    public GameObject playerBody;
    public List<SecretWall> secretWalls;
    public Compass compass;    


    
    public static Vector3 respawnPoint;
    public Vector3 respawnPointMirror;
    public static bool battleReturn;

    public bool battleReturnmirror;
    

    // Start is called before the first frame update
    void Start()
    {        
        if (battleReturn)
        {            
            Debug.Log("battle return");
            playerBody.transform.position = respawnPoint;
            battleReturn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        battleReturnmirror = battleReturn;
        respawnPointMirror = respawnPoint;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (SecretWall wall in secretWalls)
            {
                if (Vector3.Distance(player.transform.position, wall.transform.position) < 5f && wall.open == false)
                {
                    player.GetComponent<Animator>().SetTrigger("secret");
                    IEnumerator WallTimer()
                    {
                        wall.open = true;
                        wall.gameObject.SetActive(false);
                        wall.disolver.gameObject.SetActive(true);                        
                        yield return new WaitForSeconds(2);                        
                        wall.disolver.gameObject.SetActive(false);
                    }
                    StartCoroutine(WallTimer());
                }
            }
        }
    }
}
