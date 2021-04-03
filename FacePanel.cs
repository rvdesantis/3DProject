using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacePanel : MonoBehaviour
{

    public BattleController battleController;
    public Player targetHero;
    public int heroPosition;
    public Animator anim;
    public Color faded;
    public Image health;
    public Image mana;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();        
    }


    // Update is called once per frame
    void Update()
    {
        if (battleController.characterTurnIndex == heroPosition && battleController.battleTurn == 0)
        {
            faded.a = .3f;
            anim.SetBool("active", true);
        }
        if (battleController.characterTurnIndex != heroPosition)
        {
            anim.SetBool("active", false);
        }
    }
}
