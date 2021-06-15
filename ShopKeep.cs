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
    public float weapDistance;
    public bool wInRange;

    public List<Items> availWeapons;
    public Items weapon;

    public List<Trinket> availTrinkets;

    public AudioSource audioSource;
    public List<AudioClip> shopKeepAudio;
    public GameObject secretDoor;
    public bool opened;

    // used in Update
 

    private void Start()
    {
        areaController = FindObjectOfType<AreaController>();
        SpawnWeapon();
        weapDistance = 6;
    }

    public void SpawnWeapon()
    {
        int x = 0;
        if (AreaController.firstLoad)
        {
            x = Random.Range(0, availWeapons.Count);
            Debug.Log(availWeapons[x].itemName + " available in Shop");
            PlayerPrefs.SetInt("SecretStoreWeapon", x);
            PlayerPrefs.SetInt("StoreWeaponSold", 0);
            PlayerPrefs.Save();

            storeWeaponSpawnPoint = weapnSpawnPlatform.transform.position;
            weapon = Instantiate(availWeapons[x], storeWeaponSpawnPoint, weapnSpawnPlatform.transform.rotation);
        }        
        if (AreaController.firstLoad == false)
        {
            if (PlayerPrefs.GetInt("StoreWeaponSold") == 0)
            {
                x = PlayerPrefs.GetInt("SecretStoreWeapon");
                storeWeaponSpawnPoint = weapnSpawnPlatform.transform.position;
                weapon = Instantiate(availWeapons[x], storeWeaponSpawnPoint, weapnSpawnPlatform.transform.rotation);
            }
            
        }
        

    }


    private void Update()
    {        
        if (PlayerPrefs.GetInt("StoreWeaponSold") == 0)
        {
            weapDistance = Vector3.Distance(weapon.transform.position, areaController.moveController.transform.position);
        }            
        if (weapDistance <= 6)
        {            
            wInRange = true;
            if (opened == false)
            {
                opened = true;
                audioSource.PlayOneShot(shopKeepAudio[Random.Range(0, shopKeepAudio.Count)], 1);
            }
        }
        if (wInRange && weapon.gameObject.activeSelf)
        {
            if (StaticMenuItems.goldCount >= weapon.goldCost)
            {
                areaController.areaUI.messageText.text = "Buy " + weapon.itemName + "? (" + areaController.staticBank.bank[weapon.weaponHero].name + ")\n" + weapon.goldCost + " Gold";
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
                if (FindObjectOfType<AreaController>().areaUI.goldUI.gameObject.activeSelf == false)
                {
                    FindObjectOfType<AreaController>().areaUI.ToggleGold();
                }
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    if (weapon.gameObject.activeSelf)
                    {
                        areaController.areaUI.weaponImage.sprite = weapon.itemSprite;
                        areaController.areaUI.activeItem = weapon;
                        areaController.areaUI.WeaponImage();
                        weapon.gameObject.SetActive(false);
                        areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                        if (FindObjectOfType<AreaController>().areaUI.goldUI.gameObject.activeSelf)
                        {
                            FindObjectOfType<AreaController>().areaUI.ToggleGold();
                        }
                        PlayerPrefs.SetInt("StoreWeaponSold", 1); PlayerPrefs.Save();
                    }
                }
            }
            if (StaticMenuItems.goldCount < weapon.goldCost)
            {
                areaController.areaUI.messageText.text = "Buy " + weapon.itemName + "? (" + areaController.staticBank.bank[weapon.weaponHero].name + ")\n" + weapon.goldCost + " Gold (Not Enough Gold)";
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
                if (FindObjectOfType<AreaController>().areaUI.goldUI.gameObject.activeSelf == false)
                {
                    FindObjectOfType<AreaController>().areaUI.ToggleGold();
                }
            }
            
        }
        if (weapDistance > 6 && wInRange)
        {
            wInRange = false;
            if (areaController.areaUI.messageUI.GetComponent<Animator>().GetBool("solid") == true)
            {
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                if (FindObjectOfType<AreaController>().areaUI.goldUI.gameObject.activeSelf)
                {
                    FindObjectOfType<AreaController>().areaUI.ToggleGold();
                }
            }            
        }
    }
}
