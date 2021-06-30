using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorStand : Chest
{
    public List<Items> weaponBank;
    public Vector3 weaponSpawnPoint;
    public GameObject weaponSpawnTransform;



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
