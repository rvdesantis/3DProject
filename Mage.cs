using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EckTechGames.FloatingCombatText;

public class Mage : Player
{

    public GameObject castingHand;
    public ParticleSystem meleeStrike;
    public ParticleSystem castingFXShock;


    public void TriggerMelee() // triggered in animations
    {
        
        meleeStrike.gameObject.SetActive(true);
        meleeStrike.Play();
    }

    public void CastShock() // triggered in animations
    {
        castingFXShock.gameObject.SetActive(true);
        castingFXShock.Play();
    }

    public override void Melee()
    {
        IEnumerator HitTimer()
        {   
            yield return new WaitForSeconds(.25f);
            anim.SetTrigger("AttackR");
            meleeStrike.transform.position = attackTarget.transform.position;
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


    public override void CastSpell()
    {
        if (spells[0].manaCost <= playerMana)
        {
            playerMana = playerMana - spells[0].manaCost;
            
            Spell spellToCast = Instantiate<Spell>(spells[0], castingHand.transform.position, Quaternion.identity);
            spellToCast.targetPosition = attackTarget.transform.position;

            base.CastSpell();
            return;
        }

        if (spells[0].manaCost > playerMana)
        {

        }

         
    }


}
