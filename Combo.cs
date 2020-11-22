using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Combo : MonoBehaviour
{
    public BattleController battleController;
    public bool comboTrigger;

    public List<Player> comboParty;

    public Player mWarrior;
    public Player fWarrior;
    public Player fArcher;
    public Player fMage;

    // assets set active in timeline
    public Player mWarriorTimelineAsset;
    public Player fWarriorTimelineAsset;
    public Player fArcherTimelineAsset;
    public Player fMageTimelineAsset;

    public int mWarriorCount;
    public int fBerzerkerCount;
    public int fMageCount;
    public int fArcherCount;




    public void ComboChecker()
    {
        AssignPlayers();
        if (battleController.heroes[0].dead == false && battleController.heroes[1].dead == false && battleController.heroes[2].dead == false)
        {
            if (battleController.heroes[0].actionType == Player.Action.melee || battleController.heroes[0].actionType == Player.Action.ranged)
            {
                if (battleController.heroes[1].actionType == Player.Action.melee || battleController.heroes[1].actionType == Player.Action.ranged)
                {
                    if (battleController.heroes[2].actionType == Player.Action.melee || battleController.heroes[2].actionType == Player.Action.ranged)
                    {
                        battleController.combo = true;

                        if (mWarriorCount == 0)
                        {
                            fWarrior.gameObject.SetActive(false);
                            fWarriorTimelineAsset.attackTarget = fWarrior.attackTarget;

                            fArcher.gameObject.SetActive(false);
                            fArcherTimelineAsset.attackTarget = fArcher.attackTarget;

                            fMage.gameObject.SetActive(false);
                            fMageTimelineAsset.attackTarget = fMage.attackTarget;

                            battleController.comboPlayables[0].Play();


                            IEnumerator CamTimer()
                            {
                                yield return new WaitForSeconds(7.5f);
                                fWarrior.gameObject.SetActive(true);
                                fWarriorTimelineAsset.gameObject.SetActive(false);

                                fArcher.gameObject.SetActive(true);
                                fArcherTimelineAsset.gameObject.SetActive(false);

                                fMage.gameObject.SetActive(true);
                                fMageTimelineAsset.gameObject.SetActive(false);

                                foreach (Player character in battleController.heroes)
                                {
                                    character.transform.position = character.idlePosition;
                                }


                                int groupSTR = battleController.heroes[0].playerSTR + battleController.heroes[1].playerSTR + battleController.heroes[2].playerSTR + 25;
                                int damage = groupSTR - battleController.heroes[0].playerDEF;
                                battleController.heroes[0].attackTarget.playerHealth = battleController.heroes[0].attackTarget.playerHealth - damage;
                                foreach (Player character in battleController.heroes)
                                {
                                    character.transform.position = character.idlePosition;
                                }
                                battleController.endTurn = true;
                                battleController.combo = false;

                                yield return new WaitForSeconds(2);
                                battleController.NextPlayerAct();
                            }
                            StartCoroutine(CamTimer());
                        }


                        if (fMageCount == 0)
                        {
                            fWarrior.gameObject.SetActive(false);
                            fWarriorTimelineAsset.attackTarget = fWarrior.attackTarget;

                            fArcher.gameObject.SetActive(false);
                            fArcherTimelineAsset.attackTarget = fArcher.attackTarget;

                            mWarrior.gameObject.SetActive(false);
                            mWarriorTimelineAsset.attackTarget = mWarrior.attackTarget;

                            battleController.comboPlayables[1].Play();


                            IEnumerator CamTimer()
                            {
                                yield return new WaitForSeconds(4);
                                fWarrior.gameObject.SetActive(true);
                                fWarriorTimelineAsset.gameObject.SetActive(false);

                                fArcher.gameObject.SetActive(true);
                                fArcherTimelineAsset.gameObject.SetActive(false);

                                mWarrior.gameObject.SetActive(true);
                                mWarriorTimelineAsset.gameObject.SetActive(false);

                                foreach (Player character in battleController.heroes)
                                {
                                    character.transform.position = character.idlePosition;
                                }


                                int groupSTR = battleController.heroes[0].playerSTR + battleController.heroes[1].playerSTR + battleController.heroes[2].playerSTR + 25;
                                int damage = groupSTR - battleController.heroes[0].playerDEF;
                                battleController.heroes[0].attackTarget.playerHealth = battleController.heroes[0].attackTarget.playerHealth - damage;
                                foreach (Player character in battleController.heroes)
                                {
                                    character.transform.position = character.idlePosition;
                                }
                                battleController.endTurn = true;
                                battleController.combo = false;

                                yield return new WaitForSeconds(2);
                                battleController.NextPlayerAct();
                            }
                            StartCoroutine(CamTimer());
                        }

                        if (fArcherCount == 0)
                        {
                            fWarrior.gameObject.SetActive(false);
                            fWarriorTimelineAsset.attackTarget = fWarrior.attackTarget;

                            fMage.gameObject.SetActive(false);
                            fMageTimelineAsset.attackTarget = fMage.attackTarget;

                            mWarrior.gameObject.SetActive(false);
                            mWarriorTimelineAsset.attackTarget = mWarrior.attackTarget;

                            battleController.comboPlayables[2].Play();


                            IEnumerator CamTimer()
                            {
                                yield return new WaitForSeconds(5);
                                fWarrior.gameObject.SetActive(true);
                                fWarriorTimelineAsset.gameObject.SetActive(false);

                                fMage.gameObject.SetActive(true);
                                fMageTimelineAsset.gameObject.SetActive(false);

                                mWarrior.gameObject.SetActive(true);
                                mWarriorTimelineAsset.gameObject.SetActive(false);

                                foreach (Player character in battleController.heroes)
                                {
                                    character.transform.position = character.idlePosition;
                                }


                                int groupSTR = battleController.heroes[0].playerSTR + battleController.heroes[1].playerSTR + battleController.heroes[2].playerSTR + 25;
                                int damage = groupSTR - battleController.heroes[0].playerDEF;
                                battleController.heroes[0].attackTarget.playerHealth = battleController.heroes[0].attackTarget.playerHealth - damage;
                                foreach (Player character in battleController.heroes)
                                {
                                    character.transform.position = character.idlePosition;
                                }
                                battleController.endTurn = true;
                                battleController.combo = false;

                                yield return new WaitForSeconds(2);
                                battleController.NextPlayerAct();
                            }
                            StartCoroutine(CamTimer());
                        }

                    }
                }
            }
        }
    }



    public void AssignPlayers()
    {
        foreach (Player character in battleController.heroes)
        {
            if (character.archerClass)
            {
                fArcher = character;
                fArcherCount++;
            }
            if (character.warriorClass)
            {
                mWarrior = character;
                mWarriorCount++;
            }
            if (character.mageClass)
            {
                fMage = character;
                fMageCount++;
            }
            if (character.berzerkerClass)
            {
                fWarrior = character;
                fBerzerkerCount++;
            }
        }
    }


}
