using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;


public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; set; }

    public List<CinemachineVirtualCamera> virtualCams;
    public CinemachineVirtualCamera meleeCam;
    public List<PlayableDirector> comboPlayables;

    public List<Player> heroes;
    public List<Enemy> enemies;

    public int characterTurnIndex;
    public int battleTurn; // 0 = Player Turn / 1 = Enemy Turn

    public Vector3 spawnPoint1;
    public Vector3 spawnPoint2;
    public Vector3 spawnPoint3;

    public bool endTurn;
    public bool combo;


    private void Start()
    {        
        characterTurnIndex = 0;
        battleTurn = 0;
        virtualCams[0].m_Follow = heroes[0].transform;
    }

    public void NextPlayerTurn() // for action selection prior to Action Cycle
    {
        if (characterTurnIndex < 2)
        {
            characterTurnIndex = characterTurnIndex + 1;
            foreach (Enemy character in enemies)
            {
                character.attackTarget = heroes[characterTurnIndex];
            }
            meleeCam.Priority = 0;
            virtualCams[characterTurnIndex].Priority = 1;
            virtualCams[characterTurnIndex - 1].Priority = 0;
            return;
        }
        if (characterTurnIndex == 2)
        {
            Debug.Log("Start Hero Action Cycle");
            characterTurnIndex = 0;
            PlayerAct();
        }

    }

    public void NextPlayerAct() // switch to next hero action
    {
        if (endTurn == true)
        {
            foreach (Player character in heroes)
            {
                character.transform.position = character.idlePosition;
            }
            virtualCams[0].Priority = 1;
            virtualCams[2].Priority = 0;
            Debug.Log("End of Turn.  Start Enemy Turn");
            return;
        }
        if (endTurn == false)
        {
            if (characterTurnIndex < 2)
            {                
                meleeCam.Priority = 0;
                characterTurnIndex = characterTurnIndex + 1;
                virtualCams[characterTurnIndex].Priority = 1;
                virtualCams[characterTurnIndex - 1].Priority = 0;
                PlayerAct();
                return;
            }
            if (characterTurnIndex == 2)
            {
                meleeCam.Priority = 0;
                characterTurnIndex = characterTurnIndex + 1;
                PlayerAct();
                return;
            }
            if (characterTurnIndex == 3)
            {
                endTurn = true;
                Debug.Log("end of player list");
                NextPlayerAct();
            }
        }     
    }


    public void PlayerAct()
    {
        if (endTurn)
        {
            Debug.Log("Turn over, no Player Act");
            return;
        }
        if (characterTurnIndex <= 2)
        {
            ComboChecker();

            if (combo == false)
            {
                if (heroes[characterTurnIndex].actionType == Player.Action.melee)
                {
                    heroes[characterTurnIndex].Melee();
                    meleeCam.Priority = 2;
                    IEnumerator MeleeTimer()
                    {
                        yield return new WaitForSeconds(2);
                        NextPlayerAct();
                    }
                    StartCoroutine(MeleeTimer());
                }
                if (heroes[characterTurnIndex].actionType == Player.Action.ranged)
                {
                    heroes[characterTurnIndex].Melee();
                    virtualCams[characterTurnIndex].Priority = 2;
                    IEnumerator MeleeTimer()
                    {
                        yield return new WaitForSeconds(2);
                        NextPlayerAct();
                    }
                    StartCoroutine(MeleeTimer());
                }
                if (heroes[characterTurnIndex].actionType == Player.Action.casting)
                {
                    heroes[characterTurnIndex].CastSpell();
                    IEnumerator CamTimer()
                    {
                        yield return new WaitForSeconds(2);
                        NextPlayerAct();
                    }
                    StartCoroutine(CamTimer());
                }
            }
            
        }
    }


    public void ComboChecker()
    {        
        if (heroes[0].dead == false && heroes[1].dead == false && heroes[2].dead == false)
        {
            if (heroes[0].actionType == Player.Action.melee || heroes[0].actionType == Player.Action.ranged)
            {
                if (heroes[1].actionType == Player.Action.melee || heroes[1].actionType == Player.Action.ranged)
                {
                    if (heroes[2].actionType == Player.Action.melee || heroes[2].actionType == Player.Action.ranged)
                    {
                        combo = true;
                        comboPlayables[0].Play();
                        IEnumerator CamTimer()
                        {
                            yield return new WaitForSeconds(4.5f);
                            endTurn = true;
                            NextPlayerAct();
                        } StartCoroutine(CamTimer());
                    }
                }
            }
        }
    }



    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            heroes[characterTurnIndex].actionType = Player.Action.melee;
            if (characterTurnIndex <= 2)
            {
                NextPlayerTurn();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            heroes[characterTurnIndex].actionType = Player.Action.ranged;
            if (characterTurnIndex <= 2)
            {
                NextPlayerTurn();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            heroes[characterTurnIndex].actionType = Player.Action.casting;
            if (characterTurnIndex <= 2)
            {
                NextPlayerTurn();
            }
        }



        if (characterTurnIndex == 1)
        {        
            heroes[1].transform.LookAt(heroes[1].attackTarget.transform);
        }

        if (characterTurnIndex == 2)
        {
            heroes[2].transform.LookAt(heroes[2].attackTarget.transform);
        }




    }


}
