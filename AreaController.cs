using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    public FirstPersonPlayer player;
    public FirstPerson FPcontroller;
    public Transform playerBody;
    public List<SecretWall> secretWalls;
    public List<GameObject> chestLids;

    public Vector3 spawnPoint;
    public static Vector3 respawnPoint;
    public static bool battleReturn;

    public bool battleReturnmirror;
    

    // Start is called before the first frame update
    void Start()
    {
        if (battleReturn)
        {
            FPcontroller.playerBody.position = respawnPoint;
            battleReturn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        battleReturnmirror = battleReturn;
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
            foreach (GameObject chest in chestLids)
            {
                if (Vector3.Distance(player.transform.position, chest.transform.position) < 3.5f)
                {
                    chest.GetComponent<Animator>().SetTrigger("openLid");                   
                }
            }

        }
    }
}
