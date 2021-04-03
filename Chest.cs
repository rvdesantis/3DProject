using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public AreaController areaController;
    public FirstPersonPlayer player;
    public GameObject contents;
    public static int opened;
    public Items treasure;
    public bool inArea;

    private void Start()
    {
        if (opened == 1)
        {
            GetComponent<Animator>().SetTrigger("openLid");
        }
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 8)
        {
            if (opened == 0)
            {
                inArea = true;
            }
            if (inArea && opened == 0)
            {
                areaController.areaUI.messageText.text = "Open Chest?";
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
            }
        }
        if (Vector3.Distance(player.transform.position, transform.position) > 8)
        {
            if (inArea == true)
            {
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                inArea = false;
            }            
        }


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < 7 && opened == 0)
            {
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                opened = 1;
                AreaController.openedChests++;
                GetComponent<Animator>().SetTrigger("openLid");
                if (treasure.weapon)
                {
                    areaController.areaUI.weaponImage.sprite = treasure.itemSprite;
                    areaController.areaUI.activeItem = treasure;
                    areaController.areaUI.WeaponImage();

                    Player equipHero = areaController.staticBank.bank[treasure.weaponHero];

                    equipHero.Weapon.gameObject.SetActive(false);
                    equipHero.Weapon = equipHero.equipedWeapons[treasure.weaponNum];
                    equipHero.equipedWeapons[treasure.weaponNum].gameObject.SetActive(true);
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
}
