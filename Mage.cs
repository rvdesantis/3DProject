using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{        
    public ParticleSystem meleeStrike;
    public ParticleSystem fireAuara;

    public override void LevelUp()
    {
        if (playerLevel == 1 && XP >= 500)
        {
            XP = 0;
            playerLevel = 2;
            PlayerPrefs.SetInt("MagLevel", 2);
            PlayerPrefs.SetInt("MagXP", 0);
            PlayerPrefs.SetInt("MagMaxHealth", playerMaxHealth + Random.Range(15, 21));
            PlayerPrefs.SetInt("MagHealth", PlayerPrefs.GetInt("MagMaxHealth"));
            PlayerPrefs.SetInt("MagMaxMana", playerMaxMana + Random.Range(20, 31));
            PlayerPrefs.SetInt("MagMana", PlayerPrefs.GetInt("MagMaxMana"));
            PlayerPrefs.SetInt("MagStr", playerSTR + Random.Range(10, 16));
            PlayerPrefs.SetInt("MagDef", playerDEF + Random.Range(10, 16));
            PlayerPrefs.Save();
            SetBattleStats();            
        }
    }

    public override void LevelReset()
    {
        XP = 0;
        playerLevel = 1;
        PlayerPrefs.SetInt("MagLevel", 1);
        PlayerPrefs.SetInt("MagXP", 0);
        PlayerPrefs.SetInt("MagMaxHealth", 50);
        PlayerPrefs.SetInt("MagHealth", 50);
        PlayerPrefs.SetInt("MagMaxMana", 60);
        PlayerPrefs.SetInt("MagMana", 60);
        PlayerPrefs.SetInt("MagStr", 35);
        PlayerPrefs.SetInt("MagDef", 30);
        PlayerPrefs.Save();

        Weapon.gameObject.SetActive(false);
        Weapon = equipedWeapons[0];
        Weapon.gameObject.SetActive(true);




        SetBattleStats();
    }

    public override void SetBattleStats()
    {
        playerMaxHealth = PlayerPrefs.GetInt("MagMaxHealth");
        playerHealth = PlayerPrefs.GetInt("MagHealth");
        playerMaxMana = PlayerPrefs.GetInt("MagMaxMana");
        playerMana = playerMaxMana;
        playerSTR = PlayerPrefs.GetInt("MagStr");
        playerDEF = PlayerPrefs.GetInt("MagDef");
        XP = PlayerPrefs.GetInt("MagXP");
        playerLevel = PlayerPrefs.GetInt("MagLevel");

        if (playerLevel == 2)
        {
            if (spells.Count == 1)
            {
                spells.Add(masterSpellList[1]);
            }
        }
    }

    public override void SaveStats()
    {        
        PlayerPrefs.SetInt("MagXP", XP);        
        PlayerPrefs.SetInt("MagHealth", 50);     
        PlayerPrefs.Save();
    }



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
        int damage = selectedSpell.power + Weapon.magPower;
        attackTarget.combatTextPrefab.damageAmount = damage;       

        if (selectedSpell.manaCost <= playerMana)
        {
            playerMana = playerMana - selectedSpell.manaCost;
            if (selectedSpell == spells[0])
            {
                GetComponent<Animator>().SetTrigger("castStart");
            }
            if (playerLevel > 1)
            {
                if (selectedSpell == spells[1])
                {
                    GetComponent<Animator>().SetTrigger("castStart1");
                }
            }
            LookAtTarget();
            if (selectedSpell.targetALL == false)
            {
                IEnumerator CastTimer()
                {
                    attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
                    yield return new WaitForSeconds(1.5f);
                    Spell spellToCast = Instantiate<Spell>(selectedSpell, transform.position, Quaternion.identity);
                    spellToCast.targetPosition = attackTarget.aimTargetGameObject.transform.position;
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
                            enemy.combatTextPrefab.damageAmount = damage;
                            
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
                    attackTarget.combatTextPrefab.ToggleCombatText();
                }
                if (selectedSpell.targetALL)
                {
                    foreach (Enemy enemy in FindObjectOfType<BattleController>().enemies)
                    {
                        if (enemy.dead == false)
                        {
                            enemy.anim.SetTrigger("gotHit");
                            enemy.combatTextPrefab.startingPosition = enemy.transform.position;
                            enemy.combatTextPrefab.ToggleCombatText();
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
