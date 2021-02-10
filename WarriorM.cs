using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class WarriorM : Player
{



    public override void CastSpell()
    {
        if (selectedSpell.manaCost <= playerMana)
        {
            playerMana = playerMana - selectedSpell.manaCost;
            
            if (selectedSpell == spells[0])
            {
                GetComponent<Animator>().SetTrigger("castStart");
            }
            if (selectedSpell != spells[0])
            {
                GetComponent<Animator>().SetTrigger("castStart1");
            }
            LookAtTarget();
            if (selectedSpell == spells[0])
            {
                IEnumerator CastTimer()
                {
                    yield return new WaitForSeconds(1.5f);
                    Spell spellToCast = Instantiate<Spell>(selectedSpell, spellSpawnPoint.transform.position, Quaternion.identity);
                    spellToCast.targetPosition = attackTarget.head.transform.position;
                }
                StartCoroutine(CastTimer());

                IEnumerator SpellTimer()
                {
                    yield return new WaitForSeconds(selectedSpell.damageTimer);
                    if (selectedSpell.targetALL == false)
                    {
                        attackTarget.anim.SetTrigger("gotHit");
                    }
                    if (selectedSpell.targetALL)
                    {
                        foreach (Enemy enemy in FindObjectOfType<BattleController>().enemies)
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
                    this.transform.position = idlePosition;
                    LookAtTarget();

                }
                StartCoroutine(SpellTimer());

            }
            if (selectedSpell == spells[1])
            {
                foreach (Player player in FindObjectOfType<BattleController>().heroes)
                {
                    if (player.dead == false)
                    {
                        IEnumerator CastTimer()
                        {
                            yield return new WaitForSeconds(.25f);
                            Spell spellToCast = Instantiate<Spell>(selectedSpell, player.transform.position, Quaternion.identity);
                            spellToCast.targetPosition = player.transform.position;
                            player.playerDEF = player.playerDEF + selectedSpell.power;
                        }
                        StartCoroutine(CastTimer());

                    }
                }
                LookAtTarget();
            }            
        }
    }

}
