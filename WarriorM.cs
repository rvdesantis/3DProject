using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class WarriorM : Player
{

    public override void LevelUp()
    {
        if (playerLevel == 1 && XP >= 500)
        {
            XP = 0;
            playerLevel = 2;
            PlayerPrefs.SetInt("WarLevel", 2);
            PlayerPrefs.SetInt("WarXP", 0);
            PlayerPrefs.SetInt("WarMaxHealth", playerMaxHealth + Random.Range(20, 26));
            PlayerPrefs.SetInt("WarHealth", PlayerPrefs.GetInt("WarMaxHealth"));
            PlayerPrefs.SetInt("WarMana", 20);
            PlayerPrefs.SetInt("WarMaxMana", 20);
            PlayerPrefs.SetInt("WarStr", playerSTR + Random.Range(15, 21));
            PlayerPrefs.SetInt("WarDef", playerDEF + Random.Range(15, 21));
            PlayerPrefs.Save();
            SetBattleStats();
        }
    }

    public override void LevelReset()
    {
        XP = 0;
        playerLevel = 1;
        PlayerPrefs.SetInt("WarLevel", 1);
        PlayerPrefs.SetInt("WarXP", 0);
        PlayerPrefs.SetInt("WarMaxHealth", 85);
        PlayerPrefs.SetInt("WarHealth", 85);
        PlayerPrefs.SetInt("WarMana", 20);
        PlayerPrefs.SetInt("WarMaxMana", 20);
        PlayerPrefs.SetInt("WarStr", 40);
        PlayerPrefs.SetInt("WarDef", 40);
        PlayerPrefs.Save();

        Weapon.gameObject.SetActive(false);
        Weapon = equipedWeapons[0];
        Weapon.gameObject.SetActive(true);


    }

    public override void SetBattleStats()
    {
        playerMaxHealth = PlayerPrefs.GetInt("WarMaxHealth");
        playerHealth = PlayerPrefs.GetInt("WarHealth");
        playerMaxMana = PlayerPrefs.GetInt("WarMaxMana");
        playerMana = playerMaxMana;
        playerSTR = PlayerPrefs.GetInt("WarStr");
        playerDEF = PlayerPrefs.GetInt("WarDef");
        XP = PlayerPrefs.GetInt("WarXP");
        playerLevel = PlayerPrefs.GetInt("WarLevel");
    }

    public override void SaveStats()
    {
        PlayerPrefs.SetInt("WarXP", playerHealth);        
        PlayerPrefs.SetInt("WarHealth", playerHealth);
        PlayerPrefs.Save();
    }

    public override void CastSpell()
    {
        if (selectedSpell.manaCost <= playerMana)
        {
            playerMana = playerMana - selectedSpell.manaCost;
            int damage = selectedSpell.power + Weapon.magPower;
            attackTarget.combatTextPrefab.damageAmount = damage;

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
                    attackTarget.anim.SetTrigger("gotHit");
                    attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
                    attackTarget.combatTextPrefab.ToggleCombatText();
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
                            player.combatTextPrefab.startingPosition = player.transform.position;
                            player.combatTextPrefab.floatingText.color = Color.white;
                            player.combatTextPrefab.ToggleCombatText();
                        }
                        StartCoroutine(CastTimer());

                    }
                }
                LookAtTarget();
            }            
        }
    }

}
