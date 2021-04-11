using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class BattleUIController : MonoBehaviour
{
    public BattleController battlecontroller;
    public List<FacePanel> facePanels;
    public List<Image> heroFaces;
    public List<Slider> healthbars;
    public List<Slider> manabars;
    public List<Text> heroHealth;
    public List<Text> heroMana;

    public BattleBTPanel buttonUIPanel;

    public List<Button> spellButtons;
    public GameObject spellPanel;    
    public int spellIndex;


    public GameObject itemPanel;
    public List<Button> itembuttons;
    public Sprite itemImage;
    public int itemIndex;

    public GameObject levelUpUI;
    public Button exitBT;
    public Image lvlHeroFace;
    public Text levelText;



    public bool activeUI;
    public GameObject currentUI;

    private void Start()
    {
        SetMaxHealth();
        SetHealth();
        SetMaxMana();
        SetMana();
        SetFace();
    }

    public void SetMaxHealth()
    {
        healthbars[0].maxValue = battlecontroller.heroes[0].playerMaxHealth;
        healthbars[1].maxValue = battlecontroller.heroes[1].playerMaxHealth;
        healthbars[2].maxValue = battlecontroller.heroes[2].playerMaxHealth;
        heroHealth[0].text = battlecontroller.heroes[0].playerMaxHealth + "/" + battlecontroller.heroes[0].playerMaxHealth;
        heroHealth[1].text = battlecontroller.heroes[1].playerMaxHealth + "/" + battlecontroller.heroes[1].playerMaxHealth;
        heroHealth[2].text = battlecontroller.heroes[2].playerMaxHealth + "/" + battlecontroller.heroes[2].playerMaxHealth;
    }

    public void SetMaxMana()
    {
        manabars[0].maxValue = battlecontroller.heroes[0].playerMaxMana;
        manabars[1].maxValue = battlecontroller.heroes[1].playerMaxMana;
        manabars[2].maxValue = battlecontroller.heroes[2].playerMaxMana;
        heroMana[0].text = battlecontroller.heroes[0].playerMaxMana + "/" + battlecontroller.heroes[0].playerMaxMana;
        heroMana[1].text = battlecontroller.heroes[1].playerMaxMana + "/" + battlecontroller.heroes[1].playerMaxMana;
        heroMana[2].text = battlecontroller.heroes[2].playerMaxMana + "/" + battlecontroller.heroes[2].playerMaxMana;
    }

    public void SetHealth()
    {
        healthbars[0].value = battlecontroller.heroes[0].playerHealth;
        healthbars[1].value = battlecontroller.heroes[1].playerHealth;
        healthbars[2].value = battlecontroller.heroes[2].playerHealth;
        heroHealth[0].text = battlecontroller.heroes[0].playerHealth + "/" + battlecontroller.heroes[0].playerMaxHealth;
        heroHealth[1].text = battlecontroller.heroes[1].playerHealth + "/" + battlecontroller.heroes[1].playerMaxHealth;
        heroHealth[2].text = battlecontroller.heroes[2].playerHealth + "/" + battlecontroller.heroes[2].playerMaxHealth;
    }

    public void SetMana()
    {
        manabars[0].value = battlecontroller.heroes[0].playerMana;
        heroMana[0].text = battlecontroller.heroes[0].playerMana + "/" + battlecontroller.heroes[0].playerMaxMana;
        if (battlecontroller.heroes[0].playerMaxMana == 0)
        {
            manabars[0].gameObject.SetActive(false);
        }
        manabars[1].value = battlecontroller.heroes[1].playerMana;
        heroMana[1].text = battlecontroller.heroes[1].playerMana + "/" + battlecontroller.heroes[1].playerMaxMana;
        if (battlecontroller.heroes[1].playerMaxMana == 0)
        {
            manabars[1].gameObject.SetActive(false);
        }
        manabars[2].value = battlecontroller.heroes[2].playerMana;
        heroMana[2].text = battlecontroller.heroes[2].playerMana + "/" + battlecontroller.heroes[2].playerMaxMana;
        if (battlecontroller.heroes[2].playerMaxMana == 0)
        {
            manabars[2].gameObject.SetActive(false);
        }
    }

    public void SetFace()
    {
        facePanels[0].targetHero = battlecontroller.heroes[0];        
        heroFaces[0].sprite = battlecontroller.heroes[0].playerFace;
        facePanels[1].targetHero = battlecontroller.heroes[1];
        heroFaces[1].sprite = battlecontroller.heroes[1].playerFace;
        facePanels[1].targetHero = battlecontroller.heroes[1];
        heroFaces[2].sprite = battlecontroller.heroes[2].playerFace;
    }

    public void ToggleButtonIcons()
    {
        if (buttonUIPanel.gameObject.activeSelf)
        {
            buttonUIPanel.gameObject.SetActive(false);
            return;
        }
        if (buttonUIPanel.gameObject.activeSelf == false)
        {
            buttonUIPanel.gameObject.SetActive(true);
        }
    }

    public void ToggleSpellPanel()
    {
        if (spellPanel.gameObject.activeSelf)
        {
            foreach (Button button in spellButtons)
            {
                button.gameObject.SetActive(false);                
            }
            spellPanel.gameObject.SetActive(false);
            ToggleButtonIcons();
            activeUI = false;

            return;
        }
        if (spellPanel.gameObject.activeSelf == false)
        {
            activeUI = true;
            currentUI = spellPanel;
            if (battlecontroller.heroes[battlecontroller.characterTurnIndex].spells.Count != 0)
            {
                spellIndex = 0;
                spellPanel.gameObject.SetActive(true);
                foreach (Button button in spellButtons)
                {
                    button.gameObject.SetActive(true);                    
                    int spellCount = battlecontroller.heroes[battlecontroller.characterTurnIndex].spells.Count;
                    spellButtons[spellCount - 1].Select(); // to trick UI into reselecting 0 button after first use
                    if (spellButtons.IndexOf(button) > spellCount - 1)
                    {
                        button.gameObject.SetActive(false);
                    }
                    if (spellButtons.IndexOf(button) <= spellCount - 1)
                    {
                        button.gameObject.SetActive(true);
                        button.image.sprite = battlecontroller.heroes[battlecontroller.characterTurnIndex].spells[spellButtons.IndexOf(button)].panelImage;
                    }

                }                
                spellButtons[0].Select();
            }
        }        
    }

    public void ToggleItemPanel()
    {
        if (itemPanel.activeSelf)
        {
            itemPanel.gameObject.SetActive(false);
            ToggleButtonIcons();
            return;
        }
        if (itemPanel.activeSelf == false)
        {
            itemPanel.gameObject.SetActive(true);
            activeUI = true;
            currentUI = itemPanel;
            
            foreach (Button button in itembuttons)
            {
                int itemIndex = itembuttons.IndexOf(button);
                if (itemIndex < battlecontroller.battleItems.potions.Count)
                {
                    button.GetComponent<Image>().sprite = battlecontroller.battleItems.potions[itemIndex].itemImage;
                    if (battlecontroller.battleItems.potions[itemIndex].quantity > 0)
                    {
                        itembuttons[itemIndex].gameObject.SetActive(true);
                    }
                }
                if (itemIndex >= battlecontroller.battleItems.potions.Count)
                {
                    button.gameObject.SetActive(false);
                }

            }
            itembuttons[0].Select();
            itemIndex = 0;
        }
    }

    public void ItemButtonDown()
    {
        battlecontroller.heroes[battlecontroller.characterTurnIndex].activeItem = battlecontroller.battleItems.potions[itemIndex].gameObject;
        //
        battlecontroller.heroes[battlecontroller.characterTurnIndex].attackTarget = battlecontroller.heroes[battlecontroller.characterTurnIndex];
        battlecontroller.heroes[battlecontroller.characterTurnIndex].actionType = Player.Action.item;
        itemPanel.gameObject.SetActive(false);
        ToggleButtonIcons();

        if (battlecontroller.enemies[0].dead == false)
        {
            battlecontroller.focusIndex = 0;
            battlecontroller.enemies[0].ToggleHighlighter();
        }
        if (battlecontroller.enemies[0].dead == true && battlecontroller.enemies[1].dead == false)
        {
            battlecontroller.enemies[1].ToggleHighlighter();
            battlecontroller.focusIndex = 1;
        }
        if (battlecontroller.enemies[0].dead == true && battlecontroller.enemies[1].dead == true)
        {
            battlecontroller.enemies[2].ToggleHighlighter();
            battlecontroller.focusIndex = 2;
        }


        if (battlecontroller.characterTurnIndex <= 2)
        {
            battlecontroller.NextPlayerTurn();
        }
    }



    public void SpellBTDown()
    {
        battlecontroller.heroes[battlecontroller.characterTurnIndex].attackTarget = battlecontroller.enemies[battlecontroller.focusIndex];
        battlecontroller.heroes[battlecontroller.characterTurnIndex].attackTarget.ToggleHighlighter();
        if (battlecontroller.enemies[0].dead == false)
        {
            battlecontroller.focusIndex = 0;
            battlecontroller.enemies[0].ToggleHighlighter();
        }
        if (battlecontroller.enemies[0].dead == true && battlecontroller.enemies[1].dead == false)
        {
            battlecontroller.enemies[1].ToggleHighlighter();
            battlecontroller.focusIndex = 1;
        }
        if (battlecontroller.enemies[0].dead == true && battlecontroller.enemies[1].dead == true)
        {
            battlecontroller.enemies[2].ToggleHighlighter();
            battlecontroller.focusIndex = 2;
        }

        battlecontroller.heroes[battlecontroller.characterTurnIndex].selectedSpell = battlecontroller.heroes[battlecontroller.characterTurnIndex].spells[spellIndex];        
        battlecontroller.heroes[battlecontroller.characterTurnIndex].actionType = Player.Action.casting;
        spellPanel.gameObject.SetActive(false);
        ToggleButtonIcons();
        if (battlecontroller.characterTurnIndex <= 2)
        {
            battlecontroller.NextPlayerTurn();
        }
    }

    public void LevelUpUI()
    {
        activeUI = true;
        currentUI = levelUpUI;        
        exitBT.Select();
    }

    public void ExitBattleUI()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Castle 1");
    }

    private void Update()
    {
        SetHealth();
        SetMana();
    }
}
