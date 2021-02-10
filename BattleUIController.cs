using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class BattleUIController : MonoBehaviour
{
    public BattleController battlecontroller;
    public List<Image> heroFaces;
    public List<Slider> healthbars;
    public List<Slider> manabars;
    public List<Text> heroHealth;
    public List<Text> heroMana;

    public List<Button> spellButtons;
    public GameObject spellPanel;
    public GameObject facePanel;
    public int spellIndex;



    public bool activeUI;

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
        heroFaces[0].sprite = battlecontroller.heroes[0].playerFace;
        heroFaces[1].sprite = battlecontroller.heroes[1].playerFace;
        heroFaces[2].sprite = battlecontroller.heroes[2].playerFace;
    }



    public void ToggleSpellPanel()
    {
        activeUI = true;
        if (battlecontroller.heroes[battlecontroller.characterTurnIndex].spells.Count != 0)
        {
            spellIndex = 0;
            spellButtons[0].image.sprite = battlecontroller.heroes[battlecontroller.characterTurnIndex].spells[0].panelImage;
            spellButtons[0].Select();
            spellPanel.gameObject.SetActive(true);
            if (battlecontroller.heroes[battlecontroller.characterTurnIndex].spells.Count > 1)
            {
                spellButtons[1].gameObject.SetActive(true);
                spellButtons[1].image.sprite = battlecontroller.heroes[battlecontroller.characterTurnIndex].spells[1].panelImage;
            }
        }
        if (battlecontroller.heroes[battlecontroller.characterTurnIndex].spells.Count == 0)
        {
            activeUI = false;
        }
    }    



    public void SpellBTDown()
    {
        battlecontroller.enemies[battlecontroller.focusIndex].ToggleHighlighter();
        battlecontroller.heroes[battlecontroller.characterTurnIndex].attackTarget = battlecontroller.enemies[battlecontroller.focusIndex];
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
        if (battlecontroller.characterTurnIndex <= 2)
        {
            battlecontroller.NextPlayerTurn();
        }
    }

    private void Update()
    {
        SetHealth();
        SetMana();
    }
}
