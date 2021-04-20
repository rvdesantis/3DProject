using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCombo : MonoBehaviour
{
    public BattleController battleController;
    public Combo comboController;

    public Player mWarrior;
    public Player fWarrior;
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


    public void SpellComboTrigger()
    {
        if (fireStrike)
        {
            firestrikefinish = true;
            fWarrior = comboController.fWarrior;
            fMage = comboController.fMage;
            fArcher = comboController.fArcher;
            mWarrior = comboController.mWarrior;
            

            if (comboController.fBerzerkerCount == 1)
            {
                fWarrior.gameObject.SetActive(false);
                leftover = fWarrior;
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
                yield return new WaitForSeconds(4.5f);
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

                int groupSTR = mWarrior.playerSTR + fMage.spells[0].power + fMage.Weapon.magPower + 25;
                int damage = groupSTR;
                fMage.attackTarget.playerHealth = fMage.attackTarget.playerHealth - damage;

                yield return new WaitForSeconds(2f);
                foreach (Enemy enemy in battleController.enemies)
                {
                    if (enemy != battleController.heroes[0].attackTarget)
                    {
                        if (enemy.playerHealth > 0)
                        {
                            enemy.gameObject.SetActive(true);
                        }
                        if (enemy.playerHealth <= 0)
                        {
                            enemy.Die();
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
            fWarrior = comboController.fWarrior;
            fMage = comboController.fMage;
            fArcher = comboController.fArcher;
            mWarrior = comboController.mWarrior;
            

            fMage.gameObject.SetActive(false);
            comboController.fMageTimelineAsset.attackTarget = fMage.attackTarget;

            fArcher.gameObject.SetActive(false);
            comboController.fArcherTimelineAsset.attackTarget = fMage.attackTarget;


            if (comboController.fBerzerkerCount == 1)
            {
                fWarrior.gameObject.SetActive(false);
                leftover = fWarrior;
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
                    if (enemy.playerHealth <= 0)
                    {
                        enemy.Die();
                    }
                }
                
                fireArrows = false;
                battleController.combo = false;
                LeftOverAction(); // will set end turn to true and go to next action ending turn
            }
            StartCoroutine(CamTimer());
        }
    }


    public void LeftOverAction()
    {
        if (leftover.actionType == Player.Action.melee)
        {
            leftover.Melee();            
            IEnumerator MeleeTimer()
            {
                yield return new WaitForSeconds(2);
                battleController.endTurn = true;
                battleController.NextPlayerAct();
            }
            StartCoroutine(MeleeTimer());
        }
        if (leftover.actionType == Player.Action.ranged)
        {
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
