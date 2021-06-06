using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeep : MonoBehaviour
{
    public AreaController areaController;


    public GameObject table;

    public GameObject weapnSpawnPlatform;
    public Vector3 storeWeaponSpawnPoint;
    public Vector3 storeItem0;
    public bool wInRange;

    public List<Items> availWeapons;
    public Items weapon;

    public List<Trinket> availTrinkets;

    private void Start()
    {
        areaController = FindObjectOfType<AreaController>();
        SpawnWeapon();
    }

    public void SpawnWeapon()
    {
        int x = 0;
        if (AreaController.firstLoad)
        {
            x = Random.Range(0, availWeapons.Count);
            Debug.Log(availWeapons[x].itemName + " available in Shop");
            PlayerPrefs.SetInt("SecretStoreWeapon", x);
            PlayerPrefs.Save();
        }
        if (AreaController.firstLoad == false)
        {
            x = PlayerPrefs.GetInt("SecretStoreWeapon");
        }
        storeWeaponSpawnPoint = weapnSpawnPlatform.transform.position;
        weapon = Instantiate(availWeapons[x], storeWeaponSpawnPoint, weapnSpawnPlatform.transform.rotation);

    }


    private void Update()
    {
        float w = Vector3.Distance(weapon.transform.position, areaController.moveController.transform.position);
        if (w <= 2)
        {
            wInRange = true;
        }
        if (wInRange && weapon.gameObject.activeSelf)
        {
            if (StaticMenuItems.goldCount >= weapon.goldCost)
            {
                areaController.areaUI.messageText.text = "Buy " + weapon.itemName + "? (" + areaController.staticBank.bank[weapon.weaponHero].name + ")\n" + weapon.goldCost + " Gold";
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    if (weapon.gameObject.activeSelf)
                    {
                        areaController.areaUI.weaponImage.sprite = weapon.itemSprite;
                        areaController.areaUI.activeItem = weapon;
                        areaController.areaUI.WeaponImage();
                        weapon.gameObject.SetActive(false);
                    }
                }
            }
            if (StaticMenuItems.goldCount < weapon.goldCost)
            {
                areaController.areaUI.messageText.text = "Buy " + weapon.itemName + "? (" + areaController.staticBank.bank[weapon.weaponHero].name + ")\n" + weapon.goldCost + " Gold (Not Enough Gold)";
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
            }
            
        }
        if (wInRange && w > 3)
        {
            areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
        }
    }
}
