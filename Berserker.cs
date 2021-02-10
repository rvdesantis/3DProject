using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : Player
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
                foreach (Player player in FindObjectOfType<BattleController>().heroes)
                {
                    if (player.dead == false)
                    {
                        IEnumerator CastTimer()
                        {
                            yield return new WaitForSeconds(1.25f);
                            Spell spellToCast = Instantiate<Spell>(selectedSpell, player.transform.position, Quaternion.identity);
                            spellToCast.targetPosition = player.transform.position;
                            player.playerSTR = player.playerSTR + selectedSpell.power;
                        }
                        StartCoroutine(CastTimer());

                    }
                }
                LookAtTarget();
                return;
            }
            if (selectedSpell == spells[1])
            {
                IEnumerator CastTimer()
                {
                    yield return new WaitForSeconds(1.5f);
                    Spell spellToCast = Instantiate<Spell>(selectedSpell, transform.position, Quaternion.identity);
                    spellToCast.transform.LookAt(attackTarget.transform);
                    yield return new WaitForSeconds(selectedSpell.damageTimer);
                    attackTarget.anim.SetTrigger("gotHit");
                    int damage = selectedSpell.power;
                    if (damage > 0)
                    {
                        attackTarget.playerHealth = attackTarget.playerHealth - damage;   
                    }
                }
                StartCoroutine(CastTimer());
                LookAtTarget();
            }
        }
    }


}
