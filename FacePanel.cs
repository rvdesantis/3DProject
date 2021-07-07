using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacePanel : MonoBehaviour
{

    public BattleController battleController;
    public Player targetHero;
    public int faceIndex;
    public Animator anim;
    public Color faded;
    public Image health;
    public Image mana;
    public Image skull;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        targetHero = battleController.heroes[faceIndex];
    }

    public void TurnOffSkull()
    {
        if (skull.gameObject.activeSelf)
        {
            skull.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (battleController.characterTurnIndex == faceIndex && battleController.battleTurn == 0)
        {            
            anim.SetBool("active", true);
        }
        if (battleController.characterTurnIndex != faceIndex)
        {
            anim.SetBool("active", false);
        }
        if (targetHero.danger)
        {
            anim.SetBool("danger", true);
        }
        if (targetHero.danger == false)
        {
            anim.SetBool("danger", false);
        }
        if (targetHero.dead)
        {
            anim.SetBool("dead", true);
        }
        if (targetHero.dead == false)
        {
            anim.SetBool("dead", false);
        }
    }
}
