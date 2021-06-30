using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class BattleUIController : MonoBehaviour
{
    public BattleController battleController;
    public List<FacePanel> facePanels;
    public List<Image> heroFaces;
    public List<Slider> healthbars;
    public List<Slider> manabars;
    public List<Text> heroHealth;
    public List<Text> heroMana;

    public BattleBTPanel buttonUIPanel;

    public GameObject battleMenuUI;

    public Button spellButton;
    public Text spellBTtxt;
    public Text spellInfoTXT;
    public GameObject spellPanel;    
    public int spellIndex;


    public GameObject itemPanel;
    public List<Button> itembuttons;
    public Sprite itemImage;
    public int itemIndex;


    public GameObject levelUpUI;
    public lvlupUIController lvlController;
    public Button exitBT;
    public Image lvlHeroFace;
    public Text levelText;

    public GameObject loadScreen;

    public List<Image> trinketIcons;

    public bool activeUI;
    public GameObject currentUI;

    public AudioSource uiAudio;
    public List<AudioClip> uiSounds;
    // 0 - select enemy, 1 - open menu, 2 - close menu, 3- victory
    // 4 - spellbook open, 5 - spellbook close, 6 - spellbook page

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
        healthbars[0].maxValue = battleController.heroes[0].playerMaxHealth;
        healthbars[1].maxValue = battleController.heroes[1].playerMaxHealth;
        healthbars[2].maxValue = battleController.heroes[2].playerMaxHealth;
        heroHealth[0].text = battleController.heroes[0].playerMaxHealth + "/" + battleController.heroes[0].playerMaxHealth;
        heroHealth[1].text = battleController.heroes[1].playerMaxHealth + "/" + battleController.heroes[1].playerMaxHealth;
        heroHealth[2].text = battleController.heroes[2].playerMaxHealth + "/" + battleController.heroes[2].playerMaxHealth;
    }

    public void SetMaxMana()
    {
        manabars[0].maxValue = battleController.heroes[0].playerMaxMana;
        manabars[1].maxValue = battleController.heroes[1].playerMaxMana;
        manabars[2].maxValue = battleController.heroes[2].playerMaxMana;
        heroMana[0].text = battleController.heroes[0].playerMaxMana + "/" + battleController.heroes[0].playerMaxMana;
        heroMana[1].text = battleController.heroes[1].playerMaxMana + "/" + battleController.heroes[1].playerMaxMana;
        heroMana[2].text = battleController.heroes[2].playerMaxMana + "/" + battleController.heroes[2].playerMaxMana;
    }

    public void SetHealth()
    {
        healthbars[0].value = battleController.heroes[0].playerHealth;
        healthbars[1].value = battleController.heroes[1].playerHealth;
        healthbars[2].value = battleController.heroes[2].playerHealth;
        heroHealth[0].text = battleController.heroes[0].playerHealth + "/" + battleController.heroes[0].playerMaxHealth;
        heroHealth[1].text = battleController.heroes[1].playerHealth + "/" + battleController.heroes[1].playerMaxHealth;
        heroHealth[2].text = battleController.heroes[2].playerHealth + "/" + battleController.heroes[2].playerMaxHealth;
    }

    public void SetMana()
    {
        manabars[0].value = battleController.heroes[0].playerMana;
        heroMana[0].text = battleController.heroes[0].playerMana + "/" + battleController.heroes[0].playerMaxMana;
        if (battleController.heroes[0].playerMaxMana == 0)
        {
            manabars[0].gameObject.SetActive(false);
        }
        manabars[1].value = battleController.heroes[1].playerMana;
        heroMana[1].text = battleController.heroes[1].playerMana + "/" + battleController.heroes[1].playerMaxMana;
        if (battleController.heroes[1].playerMaxMana == 0)
        {
            manabars[1].gameObject.SetActive(false);
        }
        manabars[2].value = battleController.heroes[2].playerMana;
        heroMana[2].text = battleController.heroes[2].playerMana + "/" + battleController.heroes[2].playerMaxMana;
        if (battleController.heroes[2].playerMaxMana == 0)
        {
            manabars[2].gameObject.SetActive(false);
        }
    }

    public void SetFace()
    {
        facePanels[0].targetHero = battleController.heroes[0];        
        heroFaces[0].sprite = battleController.heroes[0].playerFace;
        facePanels[1].targetHero = battleController.heroes[1];
        heroFaces[1].sprite = battleController.heroes[1].playerFace;
        facePanels[1].targetHero = battleController.heroes[1];
        heroFaces[2].sprite = battleController.heroes[2].playerFace;
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
            spellPanel.gameObject.SetActive(false);
            spellIndex = 0;
            ToggleButtonIcons();
            uiAudio.PlayOneShot(uiSounds[5]);
            activeUI = false;

            return;
        }
        if (spellPanel.gameObject.activeSelf == false)
        {
            activeUI = true;
            currentUI = spellPanel;
            uiAudio.PlayOneShot(uiSounds[4]);
            if (battleController.heroes[battleController.characterTurnIndex].spells.Count != 0)
            {
                spellIndex = 0;
                spellPanel.gameObject.SetActive(true);
                spellButton.gameObject.SetActive(true);                   
                
                spellBTtxt.text = battleController.heroes[battleController.characterTurnIndex].spells[0].spellName;
                spellInfoTXT.text = battleController.heroes[battleController.characterTurnIndex].spells[0].spellInfo + "\nSpell Power = " + battleController.heroes[battleController.characterTurnIndex].spells[spellIndex].power
                    + "\nMana Cost = " + battleController.heroes[battleController.characterTurnIndex].spells[spellIndex].manaCost;
                spellButton.image.sprite = battleController.heroes[battleController.characterTurnIndex].spells[0].panelImage;
                spellButton.Select();
            }
        }        
    }

    public void ToggleItemPanel()
    {
        uiAudio.clip = uiSounds[1];
        uiAudio.Play();
        if (itemPanel.activeSelf)
        {
            itemPanel.gameObject.SetActive(false);
            ToggleButtonIcons();
            activeUI = false;
            return;
        }
        if (itemPanel.activeSelf == false)
        {
            itemPanel.gameObject.SetActive(true);
            activeUI = true;
            currentUI = itemPanel;
        }

    }

    public void ToggleMenuUI()
    {
        if (battleMenuUI.activeSelf)
        {
            battleMenuUI.gameObject.SetActive(false);
            activeUI = false;
            currentUI = null;
            return;
        }
        if (itemPanel.activeSelf == false && spellPanel.activeSelf == false && battleMenuUI.activeSelf == false)
        {
            battleMenuUI.gameObject.SetActive(true);
            activeUI = true;
            currentUI = battleMenuUI;
        }
    }



    public void SpellBTDown()
    {
        uiAudio.clip = uiSounds[0];
        uiAudio.Play();
        battleController.heroes[battleController.characterTurnIndex].attackTarget = battleController.enemies[battleController.focusIndex];
        battleController.heroes[battleController.characterTurnIndex].attackTarget.ToggleHighlighter();
        battleController.TargetChecker();

        battleController.heroes[battleController.characterTurnIndex].selectedSpell = battleController.heroes[battleController.characterTurnIndex].spells[spellIndex];        
        battleController.heroes[battleController.characterTurnIndex].actionType = Player.Action.casting;
        spellPanel.gameObject.SetActive(false);
        ToggleButtonIcons();
        if (battleController.characterTurnIndex <= 2)
        {
            battleController.NextPlayerTurn();
        }
    }

    public void LevelUpUI()
    {
        uiAudio.clip = uiSounds[3];
        uiAudio.Play();
        lvlController.LoadLevelUPStats();
        levelUpUI.gameObject.SetActive(true);
        activeUI = true;
        foreach(FacePanel facepanel in facePanels)
        {
            facepanel.gameObject.SetActive(false);
        }
        currentUI = levelUpUI;
        exitBT.gameObject.SetActive(true);
        exitBT.Select();
    }

    public void ExitBattleUI()
    {
        IEnumerator ExitTimer()
        {
            yield return new WaitForSeconds(2);
            loadScreen.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            DunBuilder.createDungeon = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("DunGenerator");
        }
        StartCoroutine(ExitTimer());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        SetHealth();
        SetMana();
    }
}
