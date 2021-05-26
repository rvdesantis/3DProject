﻿using System.Collections;
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

    public void ChestChecker()
    {        
        if (PlayerPrefs.GetInt("mimic" + areaController.mimics.IndexOf(this)) == 0)
        {
            Debug.Log("Mimic " + areaController.mimics.IndexOf(this) + " lid shut");
        }
        if (PlayerPrefs.GetInt("mimic" + areaController.mimics.IndexOf(this)) == 1)
        {
            Debug.Log("Mimic " + areaController.mimics.IndexOf(this) + " defeated");
            OpenedUI();
        }
        if (PlayerPrefs.GetInt("mimic" + areaController.mimics.IndexOf(this)) == 100)
        {
            Debug.Log("Mimic " + areaController.mimics.IndexOf(this) + " inactive");
            gameObject.SetActive(false);
        }
    }

    public void ActivateGhost()
    {
        ghost.gameObject.SetActive(true);
    }

    

    public void OpenedUI()
    {
        if (this.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            if (treasure.itemFunction == Items.itemType.weapon)
            {
                areaController.areaUI.weaponImage.sprite = treasure.itemSprite;
                areaController.areaUI.activeItem = treasure;
                areaController.areaUI.WeaponImage();
                PlayerPrefs.SetInt("mimic" + areaController.mimics.IndexOf(this), 100);
                PlayerPrefs.Save();
            }
            if (treasure.itemFunction == Items.itemType.gold)
            {
                int goldAmount = Random.Range(25, 76);     
                string trinketName = treasure.itemName;
                StaticMenuItems.goldCount = StaticMenuItems.goldCount + goldAmount;
                areaController.areaUI.messageText.text = "Gold +" + goldAmount;

                areaController.areaUI.messageUI.GetComponent<Animator>().SetTrigger("message");
                areaController.areaUI.itemImage.sprite = treasure.itemSprite;
                areaController.areaUI.ItemImage();
            }
            if (treasure.itemFunction == Items.itemType.trinket)
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
                    if (trinketName == trinket.trinketName)
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
                            PlayerPrefs.SetInt(trinketName, 1);
                            PlayerPrefs.Save();
                        }
                    }
                }
                foreach (Trinket trinket in areaController.activeTrinkets)
                {
                    if (trinketName == trinket.trinketName)
                    {
                        trinket.active = true;
                        Instantiate(trinket, areaController.playerBody.transform.position, Quaternion.identity);
                    }
                }
                areaController.areaUI.SetTrinketImages();
                areaController.areaUI.messageText.text = trinketName + " added to Trinkets";

                areaController.areaUI.messageUI.GetComponent<Animator>().SetTrigger("message");
                areaController.areaUI.itemImage.sprite = treasure.itemSprite;
                areaController.areaUI.ItemImage();
                PlayerPrefs.SetInt(trinketName, 1);     
                PlayerPrefs.SetInt("mimic" + areaController.mimics.IndexOf(this), 100);
                PlayerPrefs.Save();
            }
            else
            {
                FindObjectOfType<AreaUIController>().itemImage.sprite = treasure.itemSprite;
                FindObjectOfType<AreaUIController>().ItemImage();
                areaController.areaInventory.Add(treasure);
                PlayerPrefs.SetInt("mimic" + areaController.mimics.IndexOf(this), 100);
                PlayerPrefs.Save();
            }
        }
    }




    void Update()
    {
        if (PlayerPrefs.GetInt("mimic" + areaController.mimics.IndexOf(this)) == 1)
        {
            opened = true;
        }
        if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) < 7)
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
            if (Vector3.Distance(areaController.moveController.transform.position, this.transform.position) < 7)
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

