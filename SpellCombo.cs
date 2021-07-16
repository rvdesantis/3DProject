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
                
                fMage.attackTarget.combatTextPrefab.damageAmount = damage;
                fMage.attackTarget.combatTextPrefab.floatingText.color = Color.red;
                fMage.attackTarget.TakeDamage(damage);
                fMage.attackTarget.anim.SetTrigger("gotHit");

                yield return new WaitForSeconds(2f);
                foreach (Enemy enemy in battleController.enemies)
                { 
                    if (enemy.playerHealth > 0)
                    {
                        enemy.gameObject.SetActive(true);
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
                        enemy.combatTextPrefab.damageAmount = damage;
                        enemy.combatTextPrefab.floatingText.color = Color.red;
                        enemy.TakeDamage(damage);
                        enemy.TimelineHitTrigger();

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
                        enemy.combatTextPrefab.damageAmount = damage;
                        enemy.combatTextPrefab.floatingText.color = Color.red;
                        enemy.TakeDamage(damage);    
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
        battleController.activeCam.m_Priority = 0;
        battleController.mainCam.m_Priority = 2;
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
                leftover.Ranged();
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
            if (leftover.actionType == Player.Action.item)
            {  
                if (leftover.activeItem == battleController.battleItems.potions[0].gameObject)
                {                    
                    if (battleController.battleItems.potions[0].quantity > 0)
                    {                        
                        IEnumerator ItemTimer()
                        {
                            yield return new WaitForSeconds(1); // to allow for camera to get into place.
                            leftover.GetComponent<Animator>().SetTrigger("item");
                            leftover.attackTarget = leftover;
                            battleController.battleItems.potions[0].target = leftover.attackTarget;
                            battleController.battleItems.potions[0].target.combatTextPrefab.floatingText.color = Color.green;
                            battleController.battleItems.potions[0].target.combatTextPrefab.damageAmount = battleController.battleItems.potions[0].health;
                            battleController.battleItems.potions[0].target.combatTextPrefab.ToggleCombatText();
                            battleController.battleItems.potions[0].HealthPotion();
                            battleController.battleItems.potions[0].quantity--;
                            yield return new WaitForSeconds(2);
                            battleController.endTurn = true;
                            battleController.NextPlayerAct();
                        }
                        StartCoroutine(ItemTimer());
                        return;
                    }
                }
                if (leftover.activeItem == battleController.battleItems.potions[1].gameObject)
                {                    
                    IEnumerator ItemTimer()
                    {
                        yield return new WaitForSeconds(1); // to allow for camera to get into place.
                        leftover.GetComponent<Animator>().SetTrigger("item");
                        leftover.attackTarget = leftover;
                        battleController.battleItems.potions[1].target = leftover.attackTarget;
                        battleController.battleItems.potions[1].target.combatTextPrefab.floatingText.color = Color.blue;
                        battleController.battleItems.potions[1].target.combatTextPrefab.damageAmount = battleController.battleItems.potions[1].health;
                        battleController.battleItems.potions[1].target.combatTextPrefab.ToggleCombatText();
                        battleController.battleItems.potions[1].HealthPotion();
                        battleController.battleItems.potions[1].quantity--;
                        yield return new WaitForSeconds(2);
                        battleController.endTurn = true;
                        battleController.NextPlayerAct();
                    }
                    StartCoroutine(ItemTimer());
                    return;
                }
                if (leftover.activeItem == battleController.battleItems.potions[2].gameObject)
                {
                    if (battleController.battleItems.potions[2].quantity > 0)
                    {
                        IEnumerator ItemTimer()
                        {
                            leftover.GetComponent<Animator>().SetTrigger("item");
                            battleController.battleItems.potions[2].target = leftover.attackTarget;
                            battleController.battleItems.potions[2].ManaPotion();
                            battleController.battleItems.potions[2].quantity--;
                            yield return new WaitForSeconds(2);
                            battleController.endTurn = true;
                            battleController.NextPlayerAct();
                        }
                        StartCoroutine(ItemTimer());
                        return;
                    }
                }
                
            }
        }
        
    }
}
