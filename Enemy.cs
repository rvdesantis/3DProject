using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EckTechGames.FloatingCombatText;

public class Enemy : Player
{

    public override void Die()
    {
        dead = true;
        anim.SetTrigger("death");
        base.Die();
    }

    public override void Update()
    {
        if (playerHealth <= 0)
        {
            if (dead == false)
            {
                Die();
            }            
        }

        base.Update();
    }
}




