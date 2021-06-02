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
            dieRoll = Random.Range(0, 3); // does not include 3
        }




        switch (dieRoll)
        {
            case 0:
                Debug.Log("Melee");
                int randomTarget = Random.Range(0, 3);
                BattleController battleController = FindObjectOfType<BattleController>();
                attackTarget = battleController.heroes[randomTarget];
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
                LookAtTarget();
                battleController.meleeCam = attackTarget.selfMeleeCam;
                battleController.meleeCam.Priority = 2;
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
                    int damage = selectedSpell.power;
                    attackTarget.combatTextPrefab.damageAmount = damage;
                    attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;

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



                    if (damage > 0)
                    {
                        if (damage <= 0)
                        {
                            Debug.Log("damage 0 or less");
                        }
                        if (selectedSpell.targetALL == false)
                        {
                            attackTarget.playerHealth = attackTarget.playerHealth - damage;
                            if (attackTarget.playerHealth <= 0)
                            {
                                if (attackTarget.danger == true)
                                {
                                    attackTarget.Die();
                                }
                                if (attackTarget.danger == false)
                                {
                                    attackTarget.Danger();
                                }
                            }
                        }
                        if (selectedSpell.targetALL == true)
                        {
                            foreach (Enemy enemy in FindObjectOfType<BattleController>().enemies)
                            {
                                if (enemy.dead == false)
                                {
                                    enemy.playerHealth = attackTarget.playerHealth - damage;
                                    if (enemy.playerHealth <= 0)
                                    {
                                        if (enemy.danger == true)
                                        {
                                            enemy.Die();
                                        }
                                        if (enemy.danger == false)
                                        {
                                            enemy.Danger();
                                        }
                                    }
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
                    return;
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
            int damage = playerSTR - attackTarget.playerDEF;
            attackTarget.combatTextPrefab.floatingText.color = Color.red;
            attackTarget.combatTextPrefab.damageAmount = damage;
            attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;

            anim.SetTrigger("attack1");

            yield return new WaitForSeconds(1.75f);
            transform.position = idlePosition;          

            

            if (damage > 0)
            {
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }

            if (damage <= 0)
            {
                Debug.Log("damage 0 or less");
            }

            if (attackTarget.playerHealth <= 0)
            {
                if (attackTarget.danger == true)
                {
                    attackTarget.Die();
                }
                if (attackTarget.danger == false)
                {
                    attackTarget.Danger();                    
                }
            }
        }
        StartCoroutine(HitTimer());
    }

    public override void Ranged()
    {
        IEnumerator HitTimer()
        {
            LookAtTarget();
            int damage = (playerSTR + Weapon.power) - attackTarget.playerDEF;

            if (damage > 0)
            {
                attackTarget.combatTextPrefab.damageAmount = damage;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
            }

            if (damage <= 0)
            {
                attackTarget.combatTextPrefab.damageAmount = damage;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
                Debug.Log("damage 0 or less");
            }


            yield return new WaitForSeconds(.25f);
            anim.SetTrigger("AttackR");
            yield return new WaitForSeconds(1.75f);
            if (damage > 0)
            {
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }

            if (attackTarget.playerHealth <= 0)
            {
                if (attackTarget.danger == true)
                {
                    attackTarget.Die();
                }
                if (attackTarget.danger == false)
                {
                    attackTarget.Danger();
                }
            }

            transform.position = idlePosition;
        }
        StartCoroutine(HitTimer());
    }

    public void TriggerHitBox()
    {
        BattleController bController = FindObjectOfType<BattleController>();

        if (bController.heroes[bController.characterTurnIndex].playerClass == Player.PlayerClass.berzerker)
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
        if (bController.heroes[bController.characterTurnIndex].playerClass == Player.PlayerClass.warrior)
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
        if (bController.heroes[bController.characterTurnIndex].playerClass == Player.PlayerClass.archer)
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
        if (bController.heroes[bController.characterTurnIndex].playerClass == Player.PlayerClass.fireMage)
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
        if (bController.heroes[bController.characterTurnIndex].playerClass == Player.PlayerClass.darkMage)
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




