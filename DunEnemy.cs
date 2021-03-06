﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunEnemy : MonoBehaviour
{
    public Transform head;
    public AreaController areaController;
    public GameObject player;
    public Vector3 startPosition;
    public Animator anim;
    public bool launchable;
    public static bool launched;
    public static bool finished;

    public Items treasure;

    // Update is called once per frame

    private void Start()
    {
        startPosition = this.transform.position;
        anim = GetComponent<Animator>();


        if (launched && finished == false)
        {
            if (this.gameObject.activeSelf)
            {
                finished = true;
                gameObject.SetActive(false);

                if (treasure.itemFunction == Items.itemType.weapon)
                {
                    areaController.areaUI.weaponImage.sprite = treasure.itemSprite;
                    areaController.areaUI.activeItem = treasure;
                    areaController.areaUI.WeaponImage();
                    areaController.areaUI.dunClearedUI.foundTreasure.Add(treasure);
                }
                if (treasure.itemFunction == Items.itemType.gold)
                {
                    int goldAmount = Random.Range(25, 76);
                    string trinketName = treasure.itemName;
                    StaticMenuItems.goldCount = StaticMenuItems.goldCount + goldAmount;
                    StaticMenuItems.goldFound = StaticMenuItems.goldFound + goldAmount;
                    areaController.areaUI.dunClearedUI.foundTreasure.Add(treasure);
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
                    PlayerPrefs.SetInt(trinketName, 1);
                    PlayerPrefs.SetInt("DunEnemy", 1);
                    PlayerPrefs.Save();
                    areaController.areaUI.dunClearedUI.foundTreasure.Add(treasure);
                    areaController.areaUI.messageUI.GetComponent<Animator>().SetTrigger("message");
                    areaController.areaUI.itemImage.sprite = treasure.itemSprite;
                    areaController.areaUI.ItemImage();
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


    void FixedUpdate()
    {
        this.transform.position = startPosition;

        if (launchable)
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) < 8)
            {
                GetComponent<Animator>().SetTrigger("taunt");
                launched = true;
                IEnumerator LaunchTimer()
                {
                    yield return new WaitForSeconds(2f);
                    BattleLauncher.dunEnemy = true;
                    FindObjectOfType<BattleLauncher>().launching = true;
                    areaController.moveController.enabled = false;
                    areaController.areaUI.fadeOutPanel.gameObject.SetActive(true);
                    FindObjectOfType<BattleLauncher>().respawnPoint = areaController.moveController.transform.position;
                    FindObjectOfType<BattleLauncher>().rotationPoint = areaController.moveController.transform.rotation;
                    AreaController.respawnPoint = FindObjectOfType<BattleLauncher>().respawnPoint;
                    AreaController.respawnRotation = FindObjectOfType<BattleLauncher>().rotationPoint;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
                }
                StartCoroutine(LaunchTimer());
            }
        }
    }
}
