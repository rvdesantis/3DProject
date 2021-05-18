using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public AreaController areaController;
    public Animator anim;
    public FirstPersonPlayer player;    
    public int opened;
    public Items treasure;
    public bool inArea;

    public AudioSource audioSource;



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
                PlayerPrefs.SetInt("chest" + areaController.chests.IndexOf(this), 1);                
                areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                opened = 1;
                            
                PlayerPrefs.Save();
                anim.SetTrigger("openLid");
                audioSource.Play();
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
                if (treasure.trinket)
                {
                    if (areaController.areaUI.topBarUI.activeSelf == false)
                    {
                        areaController.areaUI.topBarUI.gameObject.SetActive(true);
                    }
                    areaController.areaUI.activeItem = treasure;
                    string trinketName = treasure.itemName;
                    bool owned = false;

                    foreach (Trinket trinket in areaController.activeTrinkets)
                    {
                        if (PlayerPrefs.GetInt(trinketName) == 1)
                        {
                            Debug.Log("Already holding tricket " + trinketName);
                            owned = true;
                        }

                    }

                    if (!owned)
                    {
                        foreach (Trinket masterTrinket in areaController.dunTrinketMasterList)
                        {
                            if (trinketName == masterTrinket.trinketName)
                            {
                                areaController.activeTrinkets.Add(masterTrinket);   
                            }
                        }
                    }
                    foreach (Trinket trinket in areaController.activeTrinkets)
                    {
                        if (trinketName == trinket.trinketName)
                        {
                            trinket.active = true;
                            if (trinket.dungeon)
                            {
                                Instantiate(trinket, areaController.playerBody.transform.position, Quaternion.identity);
                                Debug.Log("activated " + trinket.trinketName);
                            }                            
                        }
                    }
                    areaController.areaUI.SetTrinketImages();
                    areaController.areaUI.messageText.text = trinketName + " added to Trinkets";
                    PlayerPrefs.SetInt(trinketName, 1);
                    PlayerPrefs.Save();

                    areaController.areaUI.messageUI.GetComponent<Animator>().SetTrigger("message");
                    areaController.areaUI.itemImage.sprite = treasure.itemSprite;    
                    areaController.areaUI.ItemImage();
                }
            }
        }

    }
}
