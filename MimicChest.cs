using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicChest : MonoBehaviour
{
    public BattleLauncher battleLauncher;
    public AreaController areaController;
    public bool opened;

    public AudioSource audioSource;
    public AudioClip openSound;

    public Items treasure;
    public bool inArea;

    public MimicGhost ghost;


   

    private void Start()
    {
        GetComponent<Animator>().SetTrigger("shut");
    }

    public void ActivateGhost()
    {
        ghost.gameObject.SetActive(true);
    }  

    void Update()
    {
        if (PlayerPrefs.GetInt("mimic" + areaController.mimics.IndexOf(this)) == 1)
        {
            opened = true;
        }
        if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) < 5.5f)
        {
            if (opened == false)
            {
                inArea = true;
            }
            if (inArea && opened == false)
            {
                areaController.areaUI.messageText.text = "Open Chest?";
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
            }
        }
        if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) > 8)
        {
            if (inArea == true)
            {
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                inArea = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) < 5)
            {
                PlayerPrefs.SetInt("mimic" + areaController.mimics.IndexOf(this), 1);
                PlayerPrefs.Save();
                audioSource.clip = openSound;
                audioSource.Play();

                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                GetComponent<Animator>().SetTrigger("open");
                opened = true;
                IEnumerator LaunchTimer()
                {
                    
                    BattleLauncher.mimic = true;
                    battleLauncher.launching = true;
                    battleLauncher.respawnPoint = areaController.moveController.transform.position;
                    battleLauncher.rotationPoint = areaController.moveController.transform.rotation;
                    AreaController.respawnPoint = battleLauncher.respawnPoint;
                    AreaController.respawnRotation = battleLauncher.rotationPoint;
                    areaController.moveController.enabled = false;
                    areaController.areaUI.fadeOutPanel.gameObject.SetActive(true);
                    yield return new WaitForSeconds(2f);      
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
                } StartCoroutine(LaunchTimer());
            }
        }
    }
}

