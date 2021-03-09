﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaUIController : MonoBehaviour
{
    public AreaController areaController;
    public bool uiNavigation;

    public List<Items> keys;
    public Items activeItem; // for assigning and equiping

    public Image itemImage;
    public GameObject itemPanel;
    public Text itemText;


    public Image weaponImage;
    public GameObject weaponPanel;
    public Text equipText;
    public Button yesBT;
    public Button noBT;

    public GameObject inventoryPanel;
    public List<Button> inventoryButtons;


    public GameObject playerPanel;
    public List<Image> playerFaces;
    public List<Sprite> faceSprites;
    public List<Button> playerWeaponButtons;

    public List<Text> playerStats;

    public GameObject compassSmall;
    public GameObject compassLarge;

    public GameObject fadeOutPanel;



    public void ItemImage()
    {   
        IEnumerator ToggleItemPanel()
        {
            itemPanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            itemPanel.gameObject.SetActive(false);
        } StartCoroutine(ToggleItemPanel());       
    }

    public void WeaponImage()
    {
        areaController.moveController.enabled = false;
        weaponPanel.gameObject.SetActive(true);
        equipText.text = "Discovered " + activeItem.itemName + "!\n" + areaController.staticBank.bank[activeItem.weaponHero].playerName + " Weapon\nEquip Now?";
        yesBT.Select();
    }

    public void EquipWeapon()
    {
        Player equipHero = areaController.staticBank.bank[activeItem.weaponHero];

        equipHero.Weapon.gameObject.SetActive(false);
        equipHero.Weapon = equipHero.equipedWeapons[activeItem.weaponNum];
        equipHero.equipedWeapons[activeItem.weaponNum].gameObject.SetActive(true);

        weaponPanel.gameObject.SetActive(false);
        uiNavigation = false;
        areaController.moveController.enabled = true;
    }

    public void ToggleInventory()
    {
        int itemCount = areaController.areaInventory.Count;        
        foreach (Button button in inventoryButtons)
        {
            if (inventoryButtons.IndexOf(button) > itemCount - 1)
            {
                button.gameObject.SetActive(false);
            }
            if (inventoryButtons.IndexOf(button) <= itemCount - 1)
            {
                button.gameObject.SetActive(true);     
                button.image.sprite = areaController.areaInventory[inventoryButtons.IndexOf(button)].itemSprite;
                itemText.text = areaController.areaInventory[0].itemName;
            }            
        }

        
        if (inventoryButtons[0].gameObject.activeSelf)
        {
            inventoryButtons[0].Select();
        }

        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.gameObject.SetActive(false);
            areaController.moveController.enabled = true;
            uiNavigation = false;
            return;
        }
        if (inventoryPanel.activeSelf == false)
        {
            if (playerPanel.activeSelf == false && weaponPanel.activeSelf == false)
            {
                areaController.moveController.enabled = false;
                inventoryPanel.gameObject.SetActive(true);
                return;
            }
        }

    }

    public void ToggleUINav() // for UI button use only
    {
        if (uiNavigation == false)
        {
            uiNavigation = true;
            return;
        }
        if (uiNavigation == true)
        {
            uiNavigation = false;
        }
    }

    public void TogglePlayerStats()
    {
        areaController.SetPlayerBank();

        playerFaces[0].sprite = faceSprites[HeroSelect.hero0];
        playerFaces[1].sprite = faceSprites[HeroSelect.hero1];
        playerFaces[2].sprite = faceSprites[HeroSelect.hero2];

        playerStats[0].text = (areaController.activeBank.bank[0].playerName + "\n" + areaController.activeBank.bank[0].playerLevel 
            + "\n" + areaController.activeBank.bank[0].playerMaxHealth + "\n" + areaController.activeBank.bank[0].playerMaxMana
            + "\n" + (areaController.activeBank.bank[0].playerSTR + areaController.activeBank.bank[0].Weapon.power) + "\n" 
            + (areaController.activeBank.bank[0].playerDEF + areaController.activeBank.bank[0].Weapon.def));

        playerStats[1].text = (areaController.activeBank.bank[1].playerName + "\n" + areaController.activeBank.bank[1].playerLevel
            + "\n" + areaController.activeBank.bank[1].playerMaxHealth + "\n" + areaController.activeBank.bank[1].playerMaxMana
            + "\n" + (areaController.activeBank.bank[1].playerSTR + areaController.activeBank.bank[1].Weapon.power) + "\n" 
            + (areaController.activeBank.bank[1].playerDEF + areaController.activeBank.bank[1].Weapon.def));

        playerStats[2].text = (areaController.activeBank.bank[2].playerName + "\n" + areaController.activeBank.bank[2].playerLevel
            + "\n" + areaController.activeBank.bank[2].playerMaxHealth + "\n" + areaController.activeBank.bank[2].playerMaxMana
            + "\n" + (areaController.activeBank.bank[2].playerSTR + areaController.activeBank.bank[2].Weapon.power) + "\n" 
            + (areaController.activeBank.bank[2].playerDEF + areaController.activeBank.bank[2].Weapon.def));

        if (playerPanel.activeSelf)
        {
            areaController.moveController.enabled = true;
            playerPanel.gameObject.SetActive(false);
            uiNavigation = false;
            return;
        }
        if (playerPanel.activeSelf == false)
        {
            if (weaponPanel.activeSelf == false && inventoryPanel.activeSelf == false)
            {
                areaController.moveController.enabled = false;
                playerPanel.gameObject.SetActive(true);
                return;
            }
        }

    }

    public void UpdatePlayerStats()
    {
        areaController.SetPlayerBank();

        playerFaces[0].sprite = faceSprites[HeroSelect.hero0];
        playerFaces[1].sprite = faceSprites[HeroSelect.hero1];
        playerFaces[2].sprite = faceSprites[HeroSelect.hero2];

        playerStats[0].text = (areaController.activeBank.bank[0].playerName + "\n" + areaController.activeBank.bank[0].playerLevel
            + "\n" + areaController.activeBank.bank[0].playerMaxHealth + "\n" + areaController.activeBank.bank[0].playerMaxMana
            + "\n" + (areaController.activeBank.bank[0].playerSTR + areaController.activeBank.bank[0].Weapon.power) + "\n"
            + (areaController.activeBank.bank[0].playerDEF + areaController.activeBank.bank[0].Weapon.def));

        playerStats[1].text = (areaController.activeBank.bank[1].playerName + "\n" + areaController.activeBank.bank[1].playerLevel
            + "\n" + areaController.activeBank.bank[1].playerMaxHealth + "\n" + areaController.activeBank.bank[1].playerMaxMana
            + "\n" + (areaController.activeBank.bank[1].playerSTR + areaController.activeBank.bank[1].Weapon.power) + "\n"
            + (areaController.activeBank.bank[1].playerDEF + areaController.activeBank.bank[1].Weapon.def));

        playerStats[2].text = (areaController.activeBank.bank[2].playerName + "\n" + areaController.activeBank.bank[2].playerLevel
            + "\n" + areaController.activeBank.bank[2].playerMaxHealth + "\n" + areaController.activeBank.bank[2].playerMaxMana
            + "\n" + (areaController.activeBank.bank[2].playerSTR + areaController.activeBank.bank[2].Weapon.power) + "\n"
            + (areaController.activeBank.bank[2].playerDEF + areaController.activeBank.bank[2].Weapon.def));      
    }

    public void ToggleCompass()
    {
        if (compassSmall.activeSelf)
        {
            compassSmall.gameObject.SetActive(false);
            compassLarge.gameObject.SetActive(true);
            return;
        }
        if (compassLarge.activeSelf)
        {
            compassSmall.gameObject.SetActive(true);
            compassLarge.gameObject.SetActive(false);
            return;
        }
    }


    private void FixedUpdate()
    {
        float jx = Input.GetAxis("JHorizontal");
        if (uiNavigation)
        {
            if (inventoryPanel.activeSelf)
            {
                if (jx > .5f)
                {


                }
                if (jx < .5f)
                {

                }
            }

        }

    }

}