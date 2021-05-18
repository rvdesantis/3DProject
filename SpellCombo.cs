using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCombo : MonoBehaviour
{
    public BattleController battleController;
    public Combo comboController;

    public Player mWarrior;
    public Player fBerzerker;
    public Player fArcher;
    public Player fMage;

    public Player leftover;

    public Vector3 comboOnePosition;
    public Vector3 comboTwoPosition;
    public Vector3 comboThreePosition;
    public Vector3 leftoverPosition;

    public bool fireStrike;
    public bool firestrikefinish;
    public bool fireArrows;
    public bool fireArrowsfinish;
    public bool lavaStrike;
    public bool lavaStrikeFinish;


    public void SpellComboTrigger()
    {
        if (fireStrike)
        {
            firestrikefinish = true;
            fBerzerker = comboController.fBerzerker;
            fMage = comboController.fMage;
            fArcher = comboController.fArcher;
            mWarrior = comboController.mWarrior;
            

            if (comboController.fBerzerkerCount == 1)
            {
                fBerzerker.gameObject.SetActive(false);
                leftover = fBerzerker;
            }
            if (comboController.fArcherCount == 1)
            {
                fArcher.gameObject.SetActive(false);
                leftover = fArcher;
            }

            foreach (Enemy enemy in battleController.enemies)
            {
                if (enemy != fMage.attackTarget)
                {
                    enemy.gameObject.SetActive(false);
                }
                if (enemy == fMage.attackTarget)
                {
                    enemy.transform.position = battleController.enemySpawnPoint0;
                }
            }

            fMage.gameObject.SetActive(false);
            comboController.fMageTimelineAsset.attackTarget = fMage.attackTarget;

            mWarrior.gameObject.SetActive(false);
            comboController.mWarriorTimelineAsset.attackTarget = mWarrior.attackTarget;

            battleController.comboPlayables[4].Play();



            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(3.25f);
                leftover.gameObject.SetActive(true);


                fMage.gameObject.SetActive(true);
                comboController.fMageTimelineAsset.gameObject.SetActive(false);

                mWarrior.gameObject.SetActive(true);
                comboController.mWarriorTimelineAsset.gameObject.SetActive(false);

                foreach (Player character in battleController.heroes)
                {
                    character.transform.position = character.idlePosition;
                }
                foreach (Enemy enemy in battleController.enemies)
                {
                    if (enemy.dead == false)
                    {
                        enemy.transform.position = enemy.idlePosition;
                    }
                }

                int groupSTR = mWarrior.spells[0].power + fMage.spells[0].power + fMage.Weapon.magPower + 25;
                int damage = groupSTR;
                fMage.attackTarget.playerHealth = fMage.attackTarget.playerHealth - damage;
                fMage.attackTarget.combatTextPrefab.damageAmount = damage;
                fMage.attackTarget.combatTextPrefab.ToggleCombatText();

                yield return new WaitForSeconds(2f);
                foreach (Enemy enemy in battleController.enemies)
                {
                    if (enemy != battleController.heroes[0].attackTarget)
                    {
                        if (enemy.playerHealth > 0)
                        {
                            enemy.gameObject.SetActive(true);
                        }
                    }
                }                
                battleController.combo = false;
                LeftOverAction(); // will set end turn to true and go to next action ending turn
            }
            StartCoroutine(CamTimer());
        }

        if (fireArrows)
        {
            fireArrowsfinish = true;
            fBerzerker = comboController.fBerzerker;
            fMage = comboController.fMage;
            fArcher = comboController.fArcher;
            mWarrior = comboController.mWarrior;
            

            fMage.gameObject.SetActive(false);
            comboController.fMageTimelineAsset.attackTarget = fMage.attackTarget;

            fArcher.gameObject.SetActive(false);
            comboController.fArcherTimelineAsset.attackTarget = fMage.attackTarget;


            if (comboController.fBerzerkerCount == 1)
            {
                fBerzerker.gameObject.SetActive(false);
                leftover = fBerzerker;
            }
            if (comboController.mWarriorCount == 1)
            {
                mWarrior.gameObject.SetActive(false);
                leftover = mWarrior;
            }

            battleController.comboPlayables[5].Play();

            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(2.5f);
                int groupSTR = fArcher.spells[0].power + fMage.spells[0].power;
                int damage = groupSTR;
                foreach (Enemy enemy in battleController.enemies)
                {
                    if (enemy.dead == false)
                    {
                        enemy.anim.SetTrigger("gotHit");
                        enemy.playerHealth = enemy.playerHealth - damage;
                        enemy.combatTextPrefab.damageAmount = damage;
                        enemy.combatTextPrefab.ToggleCombatText();
                    }
                }
                leftover.gameObject.SetActive(true);


                fMage.gameObject.SetActive(true);
                comboController.fMageTimelineAsset.gameObject.SetActive(false);

                fArcher.gameObject.SetActive(true);
                comboController.fArcherTimelineAsset.gameObject.SetActive(false);

                foreach (Player character in battleController.heroes)
                {
                    character.transform.position = character.idlePosition;
                }                
                yield return new WaitForSeconds(1);
                foreach (Enemy enemy in battleController.enemies)
                {
                    if (enemy.playerHealth > 0)
                    {
                        enemy.gameObject.SetActive(true);
                    }
                }
                
                fireArrows = false;
                battleController.combo = false;
                LeftOverAction(); // will set end turn to true and go to next action ending turn
            }
            StartCoroutine(CamTimer());
        }

        if (lavaStrike)
        {
            lavaStrikeFinish = true;
            fBerzerker = comboController.fBerzerker;
            fMage = comboController.fMage;
            fArcher = comboController.fArcher;
            mWarrior = comboController.mWarrior;


            fMage.gameObject.SetActive(false);
            comboController.fMageTimelineAsset.attackTarget = battleController.enemies[0];

            fBerzerker.gameObject.SetActive(false);
            comboController.fWarriorTimelineAsset.attackTarget = battleController.enemies[0];


            if (comboController.fArcherCount == 1)
            {
                fArcher.gameObject.SetActive(false);
                leftover = fArcher;
            }
            if (comboController.mWarriorCount == 1)
            {
                mWarrior.gameObject.SetActive(false);
                leftover = mWarrior;
            }

            battleController.comboPlayables[6].Play();

            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(5);
                int groupSTR = fBerzerker.spells[1].power + fMage.spells[1].power + fMage.Weapon.magPower;
                int damage = groupSTR;
                foreach (Enemy enemy in battleController.enemies)
                {
                    if (enemy.dead == false)
                    {
                        enemy.anim.SetTrigger("gotHit");
                        enemy.playerHealth = enemy.playerHealth - damage;
                        enemy.combatTextPrefab.damageAmount = damage;
                        enemy.combatTextPrefab.ToggleCombatText();
                    }
                }
                leftover.gameObject.SetActive(true);


                fMage.gameObject.SetActive(true);
                comboController.fMageTimelineAsset.gameObject.SetActive(false);

                fBerzerker.gameObject.SetActive(true);
                comboController.fWarriorTimelineAsset.gameObject.SetActive(false);

                foreach (Player character in battleController.heroes)
                {
                    character.transform.position = character.idlePosition;
                }                
                foreach (Enemy enemy in battleController.enemies)
                {
                    if (enemy.playerHealth > 0)
                    {
                        enemy.gameObject.SetActive(true);
                    }
                }

                lavaStrike = false;
                battleController.combo = false;
                yield return new WaitForSeconds(2);
                LeftOverAction(); // will set end turn to true and go to next action ending turn
            }
            StartCoroutine(CamTimer());
        }
    }


    public void LeftOverAction()
    {
        bool allDead = false;
        int x = 0;
        foreach (Enemy enemy in battleController.enemies)
        {            
            if (enemy.dead)
            {
                x++;
            }
            if (x == battleController.enemies.Count)
            {
                Debug.Log("All enemies Dead");
                allDead = true;
                battleController.characterTurnIndex = 2;
                battleController.NextPlayerAct();                
            }
        }
        if (allDead == false)
        {
            battleController.characterTurnIndex = battleController.heroes.IndexOf(leftover);
            if (leftover.actionType == Player.Action.melee)
            {
                if (leftover.attackTarget.dead)
                {
                    leftover.attackTarget = battleController.GetHighestEnemy();
                }

                battleController.meleeCam = leftover.attackTarget.selfMeleeCam;
                battleController.meleeCam.Priority = 2;
                leftover.Melee();
                IEnumerator MeleeTimer()
                {
                    yield return new WaitForSeconds(2);
                    battleController.meleeCam.Priority = 0;
                    battleController.endTurn = true;
                    battleController.NextPlayerAct();
                }
                StartCoroutine(MeleeTimer());
            }
            if (leftover.actionType == Player.Action.ranged)
            {
                if (leftover.attackTarget.dead)
                {
                    leftover.attackTarget = battleController.GetHighestEnemy();
                }
                battleController.activeCam = battleController.virtualCams[0];
                leftover.Melee();
                IEnumerator MeleeTimer()
                {
                    yield return new WaitForSeconds(2);
                    battleController.endTurn = true;
                    battleController.NextPlayerAct();
                }
                StartCoroutine(MeleeTimer());
            }
            if (leftover.actionType == Player.Action.casting)
            {
                if (leftover.attackTarget.dead)
                {
                    leftover.attackTarget = battleController.GetHighestEnemy();
                }
                battleController.activeCam = battleController.virtualCams[0];
                leftover.CastSpell();
                IEnumerator CamTimer()
                {
                    yield return new WaitForSeconds(2);
                    battleController.endTurn = true;
                    battleController.NextPlayerAct();
                }
                StartCoroutine(CamTimer());
            }
        }
    }
}
