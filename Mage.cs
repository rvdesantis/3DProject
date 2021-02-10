using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{        
    public ParticleSystem meleeStrike;
    public ParticleSystem fireAuara;



    public void FireAura()
    {
        fireAuara.gameObject.SetActive(true);
        fireAuara.Play();
        IEnumerator AuraTimer()
        {
            yield return new WaitForSeconds(2);
            fireAuara.Stop();
            fireAuara.gameObject.SetActive(false);
        } StartCoroutine(AuraTimer ());
    }

    public void IdleFire()
    {
        fireAuara.gameObject.SetActive(true);
        fireAuara.Play();
    }

    public override void CastSpell()
    {
        if (selectedSpell.manaCost <= playerMana)
        {
            playerMana = playerMana - selectedSpell.manaCost;
            if (selectedSpell == spells[0])
            {
                GetComponent<Animator>().SetTrigger("castStart");
            }
            if (selectedSpell == spells[1])
            {
                GetComponent<Animator>().SetTrigger("castStart1");
            }

            LookAtTarget();
            if (selectedSpell.targetALL == false)
            {
                IEnumerator CastTimer()
                {
                    yield return new WaitForSeconds(1.5f);
                    Spell spellToCast = Instantiate<Spell>(selectedSpell, transform.position, Quaternion.identity);
                    spellToCast.targetPosition = attackTarget.head.transform.position;
                }
                StartCoroutine(CastTimer());

            }
            if (selectedSpell.targetALL)
            {
                foreach (Enemy enemy in FindObjectOfType<BattleController>().enemies)
                {
                    if (enemy.dead == false)
                    {
                        IEnumerator CastTimer()
                        {
                            yield return new WaitForSeconds(1.5f);
                            Spell spellToCast = Instantiate<Spell>(selectedSpell, enemy.transform.position, Quaternion.identity);
                            spellToCast.targetPosition = enemy.head.transform.position;
                        }
                        StartCoroutine(CastTimer());

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

            }
            StartCoroutine(SpellTimer());
        }
    }

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
