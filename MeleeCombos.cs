using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeCombos : MonoBehaviour
{
    public BattleController battleController;
    public Combo comboController;

    public Player mWarrior;
    public Player fWarrior;
    public Player fArcher;
    public Player fMage;

    public Player target;
    public bool comboFinished;



    public void MeleeCombo()
    {
        fWarrior = comboController.fBerzerker;
        fMage = comboController.fMage;
        fArcher = comboController.fArcher;
        mWarrior = comboController.mWarrior;
        comboFinished = true;


        foreach (Enemy enemy in battleController.enemies)
        {
            if (enemy != battleController.heroes[0].attackTarget)
            {
                enemy.gameObject.SetActive(false);
            }
            if (enemy == battleController.heroes[0].attackTarget)
            {
                target = enemy;
                enemy.transform.position = battleController.enemySpawnPoint0;
            }
        }


        if (comboController.mWarriorCount == 0)
        {
            fWarrior.gameObject.SetActive(false);
            comboController.fWarriorTimelineAsset.attackTarget = fWarrior.attackTarget;

            fArcher.gameObject.SetActive(false);
            comboController.fArcherTimelineAsset.attackTarget = fArcher.attackTarget;

            fMage.gameObject.SetActive(false);
            comboController.fMageTimelineAsset.attackTarget = fMage.attackTarget;

            battleController.comboPlayables[0].Play();

            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(4);
                target.transform.position = target.idlePosition;

                fWarrior.gameObject.SetActive(true);
                comboController.fWarriorTimelineAsset.gameObject.SetActive(false);

                fArcher.gameObject.SetActive(true);
                comboController.fArcherTimelineAsset.gameObject.SetActive(false);

                fMage.gameObject.SetActive(true);
                comboController.fMageTimelineAsset.gameObject.SetActive(false);

                foreach (Player character in battleController.heroes)
                {
                    character.transform.position = character.idlePosition;
                }                

                int groupSTR = battleController.heroes[0].playerSTR + battleController.heroes[0].Weapon.power +
                battleController.heroes[1].playerSTR + battleController.heroes[1].Weapon.power +
                battleController.heroes[2].playerSTR + battleController.heroes[2].Weapon.power + 25;

                int damage = groupSTR - battleController.heroes[0].attackTarget.playerDEF;
                battleController.heroes[0].attackTarget.combatTextPrefab.damageAmount = damage;
                battleController.heroes[0].attackTarget.combatTextPrefab.startingPosition = battleController.heroes[0].attackTarget.transform.position;

                battleController.heroes[0].attackTarget.TakeDamage(damage);

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
                battleController.endTurn = true;
                battleController.combo = false;
                battleController.NextPlayerAct(); // will set players back to idle positions, not needed above;
            }
            StartCoroutine(CamTimer());
        }


        if (comboController.fMageCount == 0)
        {            
            fWarrior.gameObject.SetActive(false);
            comboController.fWarriorTimelineAsset.attackTarget = fWarrior.attackTarget;

            fArcher.gameObject.SetActive(false);
            comboController.fArcherTimelineAsset.attackTarget = fArcher.attackTarget;

            mWarrior.gameObject.SetActive(false);
            comboController.mWarriorTimelineAsset.attackTarget = mWarrior.attackTarget;

            battleController.comboPlayables[1].Play();


            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(3.25f);
                target.transform.position = target.idlePosition;

                fWarrior.gameObject.SetActive(true);
                comboController.fWarriorTimelineAsset.gameObject.SetActive(false);

                fArcher.gameObject.SetActive(true);
                comboController.fArcherTimelineAsset.gameObject.SetActive(false);

                mWarrior.gameObject.SetActive(true);
                comboController.mWarriorTimelineAsset.gameObject.SetActive(false);

                foreach (Player character in battleController.heroes)
                {
                    character.transform.position = character.idlePosition;
                }                

                int groupSTR = battleController.heroes[0].playerSTR + battleController.heroes[0].Weapon.power +
                battleController.heroes[1].playerSTR + battleController.heroes[1].Weapon.power +
                battleController.heroes[2].playerSTR + battleController.heroes[2].Weapon.power + 25;

                int damage = groupSTR - battleController.heroes[0].attackTarget.playerDEF; 

                battleController.heroes[0].attackTarget.combatTextPrefab.damageAmount = damage;
                battleController.heroes[0].attackTarget.combatTextPrefab.startingPosition = battleController.heroes[0].attackTarget.transform.position;

                battleController.heroes[0].attackTarget.TakeDamage(damage);

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
                battleController.endTurn = true;
                battleController.combo = false;
                battleController.NextPlayerAct(); // will set players back to idle positions, not needed above;            
            }
            StartCoroutine(CamTimer());
        }

        if (comboController.fArcherCount == 0)
        {
            

            fWarrior.gameObject.SetActive(false);
            comboController.fWarriorTimelineAsset.attackTarget = fWarrior.attackTarget;

            fMage.gameObject.SetActive(false);
            comboController.fMageTimelineAsset.attackTarget = fMage.attackTarget;

            mWarrior.gameObject.SetActive(false);
            comboController.mWarriorTimelineAsset.attackTarget = mWarrior.attackTarget;

            battleController.comboPlayables[2].Play();


            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(4);
                target.transform.position = target.idlePosition;

                fWarrior.gameObject.SetActive(true);
                comboController.fWarriorTimelineAsset.gameObject.SetActive(false);

                fMage.gameObject.SetActive(true);
                comboController.fMageTimelineAsset.gameObject.SetActive(false);

                mWarrior.gameObject.SetActive(true);
                comboController.mWarriorTimelineAsset.gameObject.SetActive(false);

                foreach (Player character in battleController.heroes)
                {
                    character.transform.position = character.idlePosition;
                }
                battleController.heroes[0].attackTarget.transform.position = battleController.heroes[0].attackTarget.idlePosition;

                int groupSTR = battleController.heroes[0].playerSTR + battleController.heroes[0].Weapon.power +
                battleController.heroes[1].playerSTR + battleController.heroes[1].Weapon.power +
                battleController.heroes[2].playerSTR + battleController.heroes[2].Weapon.power + 25;

                int damage = groupSTR - battleController.heroes[0].attackTarget.playerDEF; 

                battleController.heroes[0].attackTarget.combatTextPrefab.damageAmount = damage;
                battleController.heroes[0].attackTarget.combatTextPrefab.startingPosition = battleController.heroes[0].attackTarget.transform.position;

                battleController.heroes[0].attackTarget.TakeDamage(damage);

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
                battleController.endTurn = true;
                battleController.combo = false;
                battleController.NextPlayerAct(); // will set players back to idle positions, not needed above;
            }
            StartCoroutine(CamTimer());
        }


        if (comboController.fBerzerkerCount == 0)
        {
            fArcher.gameObject.SetActive(false);
            comboController.fArcherTimelineAsset.attackTarget = fArcher.attackTarget;

            fMage.gameObject.SetActive(false);
            comboController.fMageTimelineAsset.attackTarget = fMage.attackTarget;

            mWarrior.gameObject.SetActive(false);
            comboController.mWarriorTimelineAsset.attackTarget = mWarrior.attackTarget;

            battleController.comboPlayables[3].Play();


            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(2.5f);
                target.transform.position = target.idlePosition;

                fArcher.gameObject.SetActive(true);
                comboController.fArcherTimelineAsset.gameObject.SetActive(false);

                fMage.gameObject.SetActive(true);
                comboController.fMageTimelineAsset.gameObject.SetActive(false);

                mWarrior.gameObject.SetActive(true);
                comboController.mWarriorTimelineAsset.gameObject.SetActive(false);

                foreach (Player character in battleController.heroes)
                {
                    character.transform.position = character.idlePosition;
                }
                battleController.heroes[0].attackTarget.transform.position = battleController.heroes[0].attackTarget.idlePosition;

                int groupSTR = battleController.heroes[0].playerSTR + battleController.heroes[0].Weapon.power + 
                    battleController.heroes[1].playerSTR + battleController.heroes[1].Weapon.power + 
                    battleController.heroes[2].playerSTR + battleController.heroes[2].Weapon.power + 25;
                int damage = groupSTR - battleController.heroes[0].attackTarget.playerDEF;

                battleController.heroes[0].attackTarget.combatTextPrefab.damageAmount = damage;
                battleController.heroes[0].attackTarget.combatTextPrefab.startingPosition = battleController.heroes[0].attackTarget.transform.position;

                battleController.heroes[0].attackTarget.TakeDamage(damage);
                battleController.heroes[0].attackTarget.transform.position = battleController.heroes[0].attackTarget.idlePosition;
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
                battleController.endTurn = true;
                battleController.combo = false;
                battleController.NextPlayerAct(); // will set players back to idle positions, not needed above;
            }
            StartCoroutine(CamTimer());
        }       
    }



}
