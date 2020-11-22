using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour
{
    public BattleController battlecontroller;
    public List<Image> heroFaces;
    public List<Slider> healthbars;
    public List<Slider> manabars;



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
    }

    public void SetMaxMana()
    {
        manabars[0].maxValue = battlecontroller.heroes[0].playerMaxMana;
        manabars[1].maxValue = battlecontroller.heroes[1].playerMaxMana;
        manabars[2].maxValue = battlecontroller.heroes[2].playerMaxMana;
    }

    public void SetHealth()
    {
        healthbars[0].value = battlecontroller.heroes[0].playerHealth;
        healthbars[1].value = battlecontroller.heroes[1].playerHealth;
        healthbars[2].value = battlecontroller.heroes[2].playerHealth;
    }

    public void SetMana()
    {
        manabars[0].value = battlecontroller.heroes[0].playerMana;
        if (battlecontroller.heroes[0].playerMaxMana == 0)
        {
            manabars[0].gameObject.SetActive(false);
        }
        manabars[1].value = battlecontroller.heroes[1].playerMana;
        if (battlecontroller.heroes[1].playerMaxMana == 0)
        {
            manabars[1].gameObject.SetActive(false);
        }
        manabars[2].value = battlecontroller.heroes[2].playerMana;
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

    private void Update()
    {
        SetHealth();
        SetMana();
    }
}
