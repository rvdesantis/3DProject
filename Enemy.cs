using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    public bool placeholder;

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
            attackTarget.transform.LookAt(this.transform);
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

    public void TriggerHitBox()
    {
        BattleController bController = FindObjectOfType<BattleController>();

        if (bController.heroes[bController.characterTurnIndex].actionType == Action.melee)
        {
            hitBox.Impacts[0].gameObject.SetActive(true);
            hitBox.Impacts[0].Play();
            IEnumerator ImpactTimer()
            {
                yield return new WaitForSeconds(1);
                hitBox.Impacts[0].Stop();
                hitBox.Impacts[0].gameObject.SetActive(false);
            } StartCoroutine(ImpactTimer());
            return;
        }
        if (bController.heroes[bController.characterTurnIndex].actionType == Action.ranged)
        {
            hitBox.Impacts[1].gameObject.SetActive(true);
            hitBox.Impacts[1].Play();
            IEnumerator ImpactTimer()
            {
                yield return new WaitForSeconds(1);
                hitBox.Impacts[1].Stop();
                hitBox.Impacts[1].gameObject.SetActive(false);
            }
            StartCoroutine(ImpactTimer());
            return;
        }
        if (bController.heroes[bController.characterTurnIndex].actionType == Action.casting)
        {
            if (bController.heroes[bController.characterTurnIndex].selectedSpell.damage == Spell.DamageType.Normal)
            {
                hitBox.Impacts[0].gameObject.SetActive(true);
                hitBox.Impacts[0].Play();
                IEnumerator ImpactTimer()
                {
                    yield return new WaitForSeconds(1);
                    hitBox.Impacts[0].Stop();
                    hitBox.Impacts[0].gameObject.SetActive(false);
                }
                StartCoroutine(ImpactTimer());
                return;
            }
            if (bController.heroes[bController.characterTurnIndex].selectedSpell.damage == Spell.DamageType.Fire)
            {
                hitBox.Impacts[2].gameObject.SetActive(true);
                hitBox.Impacts[2].Play();
                IEnumerator ImpactTimer()
                {
                    yield return new WaitForSeconds(1);
                    hitBox.Impacts[2].Stop();
                    hitBox.Impacts[2].gameObject.SetActive(false);
                }
                StartCoroutine(ImpactTimer());
                return;
            }
        }
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




