using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicChest : MonoBehaviour
{
    public BattleLauncher battleLauncher;
    public AreaController areaController;
    public static bool opened;
    public static bool looted;
    public Items treasure;


    private void Start()
    {
        GetComponent<Animator>().SetTrigger("shut");
        if (opened)
        {
            if (looted)
            {
                gameObject.SetActive(false);
            }
            if (this.gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                if (treasure.weapon)
                {
                    areaController.areaUI.weaponImage.sprite = treasure.itemSprite;
                    areaController.areaUI.activeItem = treasure;
                    areaController.areaUI.WeaponImage();
                    looted = true;
                }
                else
                {
                    FindObjectOfType<AreaUIController>().itemImage.sprite = treasure.itemSprite;
                    FindObjectOfType<AreaUIController>().ItemImage();
                    areaController.areaInventory.Add(treasure);
                }             
            }
            
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) < 7)
            {
                GetComponent<Animator>().SetTrigger("open");
                opened = true;
                IEnumerator LaunchTimer()
                {
                    yield return new WaitForSeconds(2f);
                    BattleLauncher.mimic = true;
                    battleLauncher.launching = true;
                    areaController.moveController.enabled = false;
                    areaController.areaUI.fadeOutPanel.gameObject.SetActive(true);
                    battleLauncher.respawnPoint = areaController.moveController.transform.position;
                    battleLauncher.rotationPoint = areaController.moveController.transform.rotation;
                    AreaController.respawnPoint = battleLauncher.respawnPoint;
                    AreaController.respawnRotation = battleLauncher.rotationPoint;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
                } StartCoroutine(LaunchTimer());
            }
        }
    }
}

