using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trinket : MonoBehaviour
{
    public string trinketName;
    public Sprite trinketSprite;   

    public bool active;
    

    public bool dungeon;
    public bool battle;

    public int amount;

    public bool partyTrinket;

    public Player targetPlayer;

    public AreaController areaController;
    public BattleController battleController;

    private void Start()
    {
        if (dungeon)
        {
            areaController = FindObjectOfType<AreaController>();
        }
        if (battle)
        {
            battleController = FindObjectOfType<BattleController>();
        }
    }

    public void BattleEffect()
    {
        if (trinketName == "War Drums")
        {
            MeleeBoost();
        }
    }

    public void MeleeBoost()
    {
        if (partyTrinket)
        {
            foreach (Player hero in battleController.heroes)
            {
                hero.playerSTR = hero.playerSTR + amount;
            }
        }
    }

   

    private void Update()
    {
        if (dungeon && active)
        {
            if (trinketName == "Enchanted Lense")
            {
                foreach (SecretWall wall in areaController.secretWalls)
                {
                    if (wall.gameObject.activeSelf)
                    {
                        wall.GetComponent<Animator>().SetBool("highlight", true);
                    }                    
                }
            }
        }
    }

}
