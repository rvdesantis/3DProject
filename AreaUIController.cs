using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaUIController : MonoBehaviour
{
    public AreaController areaController;
    public bool uiNavigation;

    public List<Image> trinketImages;
    public GameObject topBarUI;
    public int trinketIndex;

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
    

    public List<Text> playerStats;

    public GameObject compassSmall;
    public Compass compass1;
    public GameObject compassLarge;
    public Compass compass2;
    public RawImage mapImage;
    public GameObject mapFrame;
    public Camera mapCam;

    public GameObject menuUI;
    public List<DunMenuBT> menuButtons;

    public GameObject fadeOutPanel;

    public GameObject messageUI;
    public Text messageText;
    public Image SpaceBT;
    public Image GreenBT;

    public GameObject homeUI;
    public List<DunMenuBT> homeButtons;
    public int homeBTIndex;

    public AudioSource audioSource;


    private void Start()
    {
        TriggerMessage();
        SetTrinketImages();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ExitDungeon()
    {
        AreaController.battleReturn = false;        
        DunBuilder.createDungeon = true;        
        UnityEngine.SceneManagement.SceneManager.LoadScene("Character Select");
    }


    public void TriggerMessage()
    {
        messageUI.GetComponent<Animator>().SetTrigger("message");
    }


    public void ItemImage()
    {   
        IEnumerator ToggleItemPanel()
        {
            itemPanel.gameObject.SetActive(true);            
            yield return new WaitForSeconds(2);
            itemPanel.gameObject.SetActive(false);
        } StartCoroutine(ToggleItemPanel());       
    }

    public void SetTrinketImages()
    {
        foreach (Trinket trinket in areaController.activeTrinkets)
        {
            if (trinket.active)
            {
                trinketImages[areaController.activeTrinkets.IndexOf(trinket)].sprite = trinket.trinketSprite;
                trinketImages[areaController.activeTrinkets.IndexOf(trinket)].gameObject.SetActive(true);
            }
        }
    }

    public void WeaponImage()
    {
        areaController.moveController.enabled = false;
        weaponPanel.gameObject.SetActive(true);
        equipText.text = "Aquired " + activeItem.itemName + "!\n" + areaController.staticBank.bank[activeItem.weaponHero].playerName + " Weapon\nEquip Now?";
        noBT.Select();
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

    public void TogglePartyMenu()
    {
        if (menuUI.activeSelf == false && inventoryPanel.activeSelf == false && playerPanel.activeSelf == false && compassLarge.activeSelf == false)
        {
            if (homeUI.activeSelf)
            {
                homeUI.gameObject.SetActive(false);
                homeBTIndex = 0;
                areaController.moveController.enabled = true;
                uiNavigation = false;
                return;
            }
            if (homeUI.activeSelf == false)
            {
                uiNavigation = true;
                homeUI.gameObject.SetActive(true);
                homeButtons[0].selfBT.Select();
                homeBTIndex = 0;
                areaController.moveController.enabled = false;
                return;
            }
        }

    }

    public void HealthBTDown()
    {        
        foreach (Player character in areaController.activeBank.bank)
        {            
            if (character.playerHealth < character.playerMaxHealth)
            {
                if (areaController.potions[0].potionQuantity > 0)
                {
                    areaController.potions[0].potionTarget = character;
                    audioSource.clip = areaController.potions[0].activatedSound;
                    audioSource.PlayOneShot(areaController.potions[0].activatedSound, 1);
                    areaController.potions[0].HealthPotion();
                    inventoryPanel.gameObject.SetActive(false);
                    areaController.potions[0].potionQuantity--; Debug.Log(areaController.potions[0].potionQuantity + " Health Potions Left");
                    messageText.text = "Party is healed + 25 health";
                    TriggerMessage();
                    Debug.Log(character.playerName + " healed 25 Health");
                    character.SaveStats();
                    uiNavigation = false;
                }
                else
                {
                    inventoryPanel.gameObject.SetActive(false);
                    Debug.Log("Health Potion Quantity 0");
                    uiNavigation = false;
                }
            }
            if (areaController.activeBank.bank[0].playerHealth == areaController.activeBank.bank[0].playerMaxHealth)
            {
                if (areaController.activeBank.bank[1].playerHealth == areaController.activeBank.bank[1].playerMaxHealth)
                {
                    if (areaController.activeBank.bank[2].playerHealth == areaController.activeBank.bank[2].playerMaxHealth)
                    {
                        inventoryPanel.gameObject.SetActive(false);
                        Debug.Log("All Heroes at Max Health");
                        messageText.text = "No Party Members Injured";
                        uiNavigation = false;
                        TriggerMessage();
                    }
                }
            }
        }

        areaController.moveController.enabled = true;
    }

    public void QuickSaveBT()
    {
        foreach (Player hero in areaController.activeBank.bank)
        {
            hero.SaveStats();            
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

    public void ToggleMoveController() // for UI button use only
    {
        if (areaController.moveController.enabled == false)
        {
            areaController.moveController.enabled = true;
            return;
        }
        if (areaController.moveController.enabled == true)
        {
            areaController.moveController.enabled = false;
        }
    }

    public void TogglePlayerStats()
    {
        
        foreach(Player hero in areaController.activeBank.bank)
        {
            hero.SetBattleStats();
        }

        playerFaces[0].sprite = faceSprites[HeroSelect.hero0];
        playerFaces[1].sprite = faceSprites[HeroSelect.hero1];
        playerFaces[2].sprite = faceSprites[HeroSelect.hero2];

        playerStats[0].text = (areaController.activeBank.bank[0].playerName + "\n" + areaController.activeBank.bank[0].playerLevel 
            + "\n" + areaController.activeBank.bank[0].playerMaxHealth + "\n" + areaController.activeBank.bank[0].playerMaxMana
            + "\n" + (areaController.activeBank.bank[0].playerSTR + areaController.activeBank.bank[0].Weapon.power) + "\n" 
            + (areaController.activeBank.bank[0].playerDEF + areaController.activeBank.bank[0].Weapon.def) + "\n" + areaController.activeBank.bank[0].XP);

        playerStats[1].text = (areaController.activeBank.bank[1].playerName + "\n" + areaController.activeBank.bank[1].playerLevel
            + "\n" + areaController.activeBank.bank[1].playerMaxHealth + "\n" + areaController.activeBank.bank[1].playerMaxMana
            + "\n" + (areaController.activeBank.bank[1].playerSTR + areaController.activeBank.bank[1].Weapon.power) + "\n" 
            + (areaController.activeBank.bank[1].playerDEF + areaController.activeBank.bank[1].Weapon.def) + "\n" + areaController.activeBank.bank[1].XP);

        playerStats[2].text = (areaController.activeBank.bank[2].playerName + "\n" + areaController.activeBank.bank[2].playerLevel
            + "\n" + areaController.activeBank.bank[2].playerMaxHealth + "\n" + areaController.activeBank.bank[2].playerMaxMana
            + "\n" + (areaController.activeBank.bank[2].playerSTR + areaController.activeBank.bank[2].Weapon.power) + "\n" 
            + (areaController.activeBank.bank[2].playerDEF + areaController.activeBank.bank[2].Weapon.def) + "\n" + areaController.activeBank.bank[2].XP);

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
        if (menuUI.activeSelf == false && inventoryPanel.activeSelf == false && playerPanel.activeSelf == false && weaponPanel.activeSelf == false && mapFrame.activeSelf == false)
        {
            if (compassSmall.activeSelf)
            {
                compassSmall.gameObject.SetActive(false);
                compassLarge.gameObject.SetActive(true);
                return;
            }
        }
        if (compassLarge.activeSelf)
        {
            compassSmall.gameObject.SetActive(true);
            compassLarge.gameObject.SetActive(false);
            return;
        }
    }

    public void ToggleMap()
    {
        if (menuUI.activeSelf == false && inventoryPanel.activeSelf == false && playerPanel.activeSelf == false && weaponPanel.activeSelf == false)
        {
            if (mapFrame.activeSelf)
            {                
                mapFrame.gameObject.SetActive(false);
                areaController.moveController.enabled = true;
                uiNavigation = false;
                return;
            }
        }
        if (mapFrame.activeSelf == false)
        {
            areaController.moveController.enabled = false;
            mapCam.gameObject.SetActive(true);
            mapFrame.gameObject.SetActive(true);
            return;
        }
    }



    public void ToggleMenu()
    {
        if (menuUI.activeSelf)
        {
            menuUI.SetActive(false);
            uiNavigation = false;
            areaController.moveController.enabled = true;
            return;
        }
        if (menuUI.activeSelf == false)
        {
            areaController.moveController.enabled = false;
            menuUI.SetActive(true);
            menuButtons[0].selfBT.Select();
            return;
        }
    }

}
