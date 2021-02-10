using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public AreaController areaController;
    public FirstPersonPlayer player;
    public GameObject contents;
    public static bool opened;
    public Items treasure;

    private void Start()
    {
        if (opened)
        {
            GetComponent<Animator>().SetTrigger("openLid");
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < 7)
            {
                opened = true;
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
