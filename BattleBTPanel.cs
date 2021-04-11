using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBTPanel : MonoBehaviour
{
    public BattleController battleController;
    public Image attBTIcon;
    public Image spellBTIcon;
    public Image nullIcon;
    public Image backIcon;

    


    void Start()
    {
        battleController = FindObjectOfType<BattleController>();
    }




    void Update()
    {
        if (gameObject.activeSelf)
        {
            attBTIcon.sprite = battleController.heroes[battleController.characterTurnIndex].attBTicon;
            spellBTIcon.sprite = battleController.heroes[battleController.characterTurnIndex].spellBTicon;
        }
        if (gameObject.activeSelf)
        {
            if (battleController.characterTurnIndex == 0)
            {
                nullIcon.gameObject.SetActive(true);
                backIcon.gameObject.SetActive(false);
            }
            if (battleController.characterTurnIndex > 0)
            {
                nullIcon.gameObject.SetActive(false);
                backIcon.gameObject.SetActive(true);
            }
        }
    }
}
