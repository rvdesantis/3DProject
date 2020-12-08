using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EckTechGames.FloatingCombatText;

public class Mage : Player
{

    public GameObject castingHand;
    public ParticleSystem meleeStrike;
    






    public override void Melee()
    {
        IEnumerator HitTimer()
        {
            anim.SetTrigger("AttackR");
            yield return new WaitForSeconds(.5f);            
            meleeStrike.transform.position = attackTarget.transform.position + new Vector3(0, .5f, 0); // .8 per specific prefab used
            meleeStrike.gameObject.SetActive(true); meleeStrike.Play(withChildren:true);            
            yield return new WaitForSeconds(1.25f);
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
}
