using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EckTechGames.FloatingCombatText;

public class Enemy : Player
{
    public bool demon;

    public override void Act()
    {
        int dieRoll = Random.Range(0, 1);
        
        switch (dieRoll)
        {
            case 0:
                Debug.Log("Case 0");
                int randomTarget = Random.Range(0, 3);
                attackTarget = FindObjectOfType<BattleController>().heroes[randomTarget];
                targetPos = attackTarget.transform.position;
                transform.LookAt(attackTarget.transform);
                Melee();
                break;

            case 1:
                Debug.Log("Case 1");

                break;
        }
       
    }

    public override void Melee()
    {
        IEnumerator HitTimer()
        {
            transform.position = attackTarget.strikePoint.transform.position;
            yield return new WaitForSeconds(1);
            
            anim.SetTrigger("attack1");

            yield return new WaitForSeconds(1.75f);
            transform.position = idlePosition;
            

            int damage = playerSTR - attackTarget.playerDEF;

            if (damage > 0)
            {
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }

            if (damage <= 0)
            {
                Debug.Log("damage 0 or less");
            }

        }
        StartCoroutine(HitTimer());
    }

    public override void Die()
    {
        dead = true;
        anim.SetTrigger("Dead");
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




