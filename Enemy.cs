using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    public bool placeholder;
    public int dieRoll;

    public override void Act()
    {
        if (spells.Count == 0)
        {
            dieRoll = Random.Range(0, 1);
        }
        if (spells.Count > 0)
        {
            dieRoll = Random.Range(0, 2);
        }




        switch (dieRoll)
        {
            case 0:
                Debug.Log("Melee");
                int randomTarget = Random.Range(0, 3);
                attackTarget = FindObjectOfType<BattleController>().heroes[randomTarget];
                if (attackTarget.dead)
                {
                    foreach (Player hero in FindObjectOfType<BattleController>().heroes)
                    {
                        if (hero.dead == false)
                        {
                            attackTarget = hero;
                        }
                    }
                }
                targetPos = attackTarget.transform.position;
                transform.LookAt(attackTarget.transform);
                Melee();
                break;

            case 1:
                Debug.Log("Casting");

                int castingTarget = Random.Range(0, 3);
                attackTarget = FindObjectOfType<BattleController>().heroes[castingTarget];
                if (attackTarget.dead)
                {
                    foreach (Player hero in FindObjectOfType<BattleController>().heroes)
                    {
                        if (hero.dead == false)
                        {
                            attackTarget = hero;
                        }
                    }
                }
                targetPos = attackTarget.transform.position;
                transform.LookAt(attackTarget.transform);

                selectedSpell = spells[Random.Range(0, spells.Count)];
                if (selectedSpell.manaCost > playerMana)
                {
                    Melee();
                }
                if (selectedSpell.manaCost <= playerMana)
                {
                    playerMana = playerMana - selectedSpell.manaCost;
                    GetComponent<Animator>().SetTrigger("castStart");

                    if (selectedSpell.targetALL == false)
                    {
                        IEnumerator CastTimer()
                        {
                            yield return new WaitForSeconds(1.5f);
                            Spell spellToCast = Instantiate<Spell>(selectedSpell, spellSpawnPoint.transform.position, Quaternion.identity);
                            spellToCast.targetPosition = attackTarget.transform.position;
                        }
                        StartCoroutine(CastTimer());

                    }
                    if (selectedSpell.targetALL)
                    {
                        foreach (Player enemy in FindObjectOfType<BattleController>().heroes)
                        {
                            if (enemy.dead == false)
                            {
                                IEnumerator CastTimer()
                                {
                                    yield return new WaitForSeconds(1.5f);
                                    Spell spellToCast = Instantiate<Spell>(selectedSpell, spellSpawnPoint.transform.position, Quaternion.identity);
                                    spellToCast.targetPosition = enemy.transform.position;
                                }
                                StartCoroutine(CastTimer());
                            }
                        }
                    }

                }

                IEnumerator SpellTimer()
                {
                    yield return new WaitForSeconds(selectedSpell.damageTimer);
                    if (selectedSpell.targetALL == false)
                    {
                        attackTarget.anim.SetTrigger("gotHit");
                    }
                    if (selectedSpell.targetALL)
                    {
                        foreach (Player enemy in FindObjectOfType<BattleController>().heroes)
                        {
                            if (enemy.dead == false)
                            {
                                enemy.anim.SetTrigger("gotHit");
                            }
                        }
                        attackTarget.anim.SetTrigger("gotHit");
                    }

                    int damage = selectedSpell.power;
                    if (damage > 0)
                    {
                        if (damage <= 0)
                        {
                            Debug.Log("damage 0 or less");
                        }
                        if (selectedSpell.targetALL == false)
                        {
                            attackTarget.playerHealth = attackTarget.playerHealth - damage;
                        }
                        if (selectedSpell.targetALL == true)
                        {
                            foreach (Enemy enemy in FindObjectOfType<BattleController>().enemies)
                            {
                                if (enemy.dead == false)
                                {
                                    enemy.playerHealth = attackTarget.playerHealth - damage;
                                }
                            }
                        }
                    }

                    transform.LookAt(FindObjectOfType<BattleController>().heroes[0].transform);

                }
                StartCoroutine(SpellTimer());

                break;

            case 2:
                if (playerHealth < playerMaxHealth/2)
                {
                    Debug.Log("Health Under 50%   Healing");
                    Potion enemyHealer = FindObjectOfType<BattleController>().battleItems.potions[0];
                    enemyHealer.target = this;
                    enemyHealer.HealthPotion();
                }
                if (playerHealth >= playerMaxHealth / 2)
                {
                    Act();
                    Debug.Log("Health over 50%  Reroll");
                }
                

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




