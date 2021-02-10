using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaUIController : MonoBehaviour
{
    public AreaController areaController;

    public List<Items> keys;
    public Items activeItem; // for assigning and equiping

    public Image itemImage;
    public GameObject itemPanel;


    public Image weaponImage;
    public GameObject weaponPanel;
    public Text equipText;

    public GameObject inventoryPanel;
    public List<Button> inventoryButtons;
    public List<Image> inventoryImages;


    public GameObject playerPanel;
    public List<Image> playerFaces;
    public List<Sprite> faceSprites;

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
        IEnumerator ToggleItemPanel()
        {
            weaponPanel.gameObject.SetActive(true);
            equipText.text = "Discovered " + activeItem.itemName + "!\n" + areaController.staticBank.bank[activeItem.weaponHero].playerName + " Weapon";
            yield return new WaitForSeconds(2);
            weaponPanel.gameObject.SetActive(false);
        }
        StartCoroutine(ToggleItemPanel());
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
        }

        foreach (Image image in inventoryImages)
        {            
            if (inventoryImages.IndexOf(image) <= itemCount - 1)
            {
                image.sprite = areaController.areaInventory[inventoryImages.IndexOf(image)].itemSprite;
            }
        }

        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.gameObject.SetActive(false);
            areaController.moveController.enabled = true;
            return;
        }
        if (inventoryPanel.activeSelf == false)
        {
            areaController.moveController.enabled = false;
            inventoryPanel.gameObject.SetActive(true);
            return;
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
            + "\n" + areaController.activeBank.bank[0].playerSTR + "\n" + areaController.activeBank.bank[0].playerDEF);

        playerStats[1].text = (areaController.activeBank.bank[1].playerName + "\n" + areaController.activeBank.bank[1].playerLevel
            + "\n" + areaController.activeBank.bank[1].playerMaxHealth + "\n" + areaController.activeBank.bank[1].playerMaxMana
            + "\n" + areaController.activeBank.bank[1].playerSTR + "\n" + areaController.activeBank.bank[1].playerDEF);

        playerStats[2].text = (areaController.activeBank.bank[2].playerName + "\n" + areaController.activeBank.bank[2].playerLevel
    + "\n" + areaController.activeBank.bank[2].playerMaxHealth + "\n" + areaController.activeBank.bank[2].playerMaxMana
    + "\n" + areaController.activeBank.bank[2].playerSTR + "\n" + areaController.activeBank.bank[2].playerDEF);

        if (playerPanel.activeSelf)
        {
            areaController.moveController.enabled = true;
            playerPanel.gameObject.SetActive(false);           
            return;
        }
        if (playerPanel.activeSelf == false)
        {
            areaController.moveController.enabled = false;
            playerPanel.gameObject.SetActive(true);
            return;
        }

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

}
