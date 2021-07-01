using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorStand : Chest
{
    public List<Items> weaponBank;
    public Vector3 weaponSpawnPoint;
    public GameObject weaponSpawnTransform;

    public void AvailableWeapons()
    {
        foreach (Player hero in areaController.staticBank.bank)
        {
            if (PlayerPrefs.GetInt(hero.playerName + "Weapon2") == 1 && PlayerPrefs.GetInt(hero.playerName + "Weapon3") == 0)
            {
                weaponBank.Add(hero.weaponItemBank[2]);
            }
            if (PlayerPrefs.GetInt(hero.playerName + "Weapon1") == 1 && PlayerPrefs.GetInt(hero.playerName + "Weapon2") == 0)
            {
                weaponBank.Add(hero.weaponItemBank[1]);
            }
            if (PlayerPrefs.GetInt(hero.playerName + "Weapon1") == 0)
            {
                weaponBank.Add(hero.weaponItemBank[0]);
            }
        }
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 5.5f)
        {
            if (opened == 0)
            {
                inArea = true;
            }
            if (inArea && opened == 0)
            {
                areaController.areaUI.messageText.text = "Take " + treasure.itemName + " ?";
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

        if (inArea)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                if (treasure.gameObject.activeSelf == true)
                {
                    if (Vector3.Distance(player.transform.position, this.transform.position) < 5 && opened == 0)
                    {
                        PlayerPrefs.SetInt("chest" + areaController.chests.IndexOf(this), 1);
                        areaController.areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                        opened = 1;

                        PlayerPrefs.Save();

                        if (treasure.itemFunction == Items.itemType.weapon)
                        {
                            areaController.areaUI.weaponImage.sprite = treasure.itemSprite;
                            areaController.areaUI.activeItem = treasure;
                            areaController.areaUI.WeaponImage();
                            treasure.gameObject.SetActive(false);
                            audioSource.PlayOneShot(audioClips[0]);
                        }
                    }
                }

            }
        }


    }
}
