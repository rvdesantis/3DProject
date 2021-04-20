﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : Player
{

    public override void LevelUp()
    {
        if (playerLevel == 1 && XP >= 500)
        {
            XP = 0;
            playerLevel = 2;
            PlayerPrefs.SetInt("BerLevel", 2);
            PlayerPrefs.SetInt("BerXP", 0);
            PlayerPrefs.SetInt("BerMaxHealth", playerMaxHealth + Random.Range(15, 21));
            PlayerPrefs.SetInt("BerHealth", PlayerPrefs.GetInt("BerMaxHealth"));
            PlayerPrefs.SetInt("BerMaxMana", 20);
            PlayerPrefs.SetInt("BerMana", 20);
            PlayerPrefs.SetInt("BerStr", playerSTR + Random.Range(20, 26));
            PlayerPrefs.SetInt("BerDef", playerDEF + Random.Range(10, 16));
            PlayerPrefs.Save();
            SetBattleStats();
        }
    }

    public override void LevelReset()
    {
        XP = 0;
        playerLevel = 1;
        PlayerPrefs.SetInt("BerLevel", 1);
        PlayerPrefs.SetInt("BerXP", 0);
        PlayerPrefs.SetInt("BerMaxHealth",100);
        PlayerPrefs.SetInt("BerHealth", 100);
        PlayerPrefs.SetInt("BerMaxMana", 0);
        PlayerPrefs.SetInt("BerMana", 0);
        PlayerPrefs.SetInt("BerStr", 50);
        PlayerPrefs.SetInt("BerDef", 35);
        PlayerPrefs.Save();

        Weapon.gameObject.SetActive(false);
        Weapon = equipedWeapons[0];
        Weapon.gameObject.SetActive(true);
    }

    public override void SetBattleStats()
    {
        playerMaxHealth = PlayerPrefs.GetInt("BerMaxHealth");
        playerHealth = PlayerPrefs.GetInt("BerHealth");
        playerMaxMana = PlayerPrefs.GetInt("BerMaxMana");
        playerMana = playerMaxMana;
        playerSTR = PlayerPrefs.GetInt("BerStr");
        playerDEF = PlayerPrefs.GetInt("BerDef");
        XP = PlayerPrefs.GetInt("BerXP");
        playerLevel = PlayerPrefs.GetInt("BerLevel");
    }
    public override void SaveStats()
    {
        PlayerPrefs.SetInt("BerXP", XP);        
        PlayerPrefs.SetInt("BerHealth", playerHealth);
        PlayerPrefs.Save();
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
                    int damage = selectedSpell.power + Weapon.magPower;
                    attackTarget.combatTextPrefab.damageAmount = damage;
                    attackTarget.combatTextPrefab.damageAmount = damage;
                    yield return new WaitForSeconds(1.5f);
                    Spell spellToCast = Instantiate<Spell>(selectedSpell, transform.position, Quaternion.identity);
                    spellToCast.transform.LookAt(attackTarget.transform);
                    attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
                    yield return new WaitForSeconds(selectedSpell.damageTimer);
                    attackTarget.anim.SetTrigger("gotHit");
                    attackTarget.combatTextPrefab.ToggleCombatText();
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
