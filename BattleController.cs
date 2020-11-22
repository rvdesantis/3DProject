using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;


public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; set; }
    public Combo comboController;

    public List<CinemachineVirtualCamera> virtualCams;
    public CinemachineVirtualCamera meleeCam;
    public List<PlayableDirector> comboPlayables;


    public PlayerBank staticBank;
    public PlayerBank staticEnemyBank;
    public List<Player> heroes;
    public List<Player> enemies;

    public int characterTurnIndex;
    public int battleTurn; // 0 = Player Turn / 1 = Enemy Turn

    public Vector3 spawnPoint0;
    public Vector3 spawnPoint1;
    public Vector3 spawnPoint2;

    public Vector3 enemySpawnPoint0;

    public bool endTurn;
    public bool combo;


    private void Start()
    {        
        characterTurnIndex = 0;
        battleTurn = 0;


        heroes[0] = Instantiate<Player>(staticBank.bank[HeroSelect.hero0], spawnPoint0, Quaternion.identity);

        heroes[1] = Instantiate<Player>(staticBank.bank[HeroSelect.hero1], spawnPoint1, Quaternion.identity);
        
        heroes[2] = Instantiate<Player>(staticBank.bank[HeroSelect.hero2], spawnPoint2, Quaternion.identity);

        enemies[0] = Instantiate<Player>(staticEnemyBank.bank[0], enemySpawnPoint0, Quaternion.identity);


        foreach (Player character in heroes)
        {
            character.attackTarget = enemies[0];
            character.transform.LookAt(character.attackTarget.transform);
        }
        foreach (Player character in enemies)
        {
            character.attackTarget = heroes[0];
            character.transform.LookAt(character.attackTarget.transform);
        }

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
            virtualCams[0].Priority = 1;
            virtualCams[2].Priority = 0;
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
            IEnumerator TurnTimer()
            {
                yield return new WaitForSeconds(3);
                characterTurnIndex = 0;
                endTurn = false;
                battleTurn = 1;
                EnemyAct();
            } StartCoroutine(TurnTimer());
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
                endTurn = true;
                Debug.Log("end of player list");
                virtualCams[0].Priority = 1;
                meleeCam.m_Priority = 0;
                virtualCams[0].m_LookAt = enemies[0].transform;
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
        if (!endTurn)
        {
            comboController.ComboChecker();

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


    public void EnemyAct()
    {
        if (endTurn)
        {
            Debug.Log("Turn over, no Enemy Act");
            return;
        }
        if (enemies[characterTurnIndex].dead == true)
        {
            NextEnemyAct();
            return;
        }
        if (characterTurnIndex <= enemies.Count - 1)
        {
            enemies[characterTurnIndex].Act();
            IEnumerator TurnTimer()
            {
                yield return new WaitForSeconds(2);
                NextEnemyAct();
            } StartCoroutine(TurnTimer());
               
        }
    }

    public void NextEnemyAct() // switch to next enemy action
    {
        if (characterTurnIndex <= enemies.Count - 1)
        {
            characterTurnIndex = characterTurnIndex + 1;
            if (characterTurnIndex < enemies.Count)
            {
                enemies[characterTurnIndex].Act();
                return;
            }
            if (characterTurnIndex == enemies.Count)
            {
                foreach (Player character in enemies)
                {
                    character.transform.position = character.idlePosition;
                }
                characterTurnIndex = 0;
                battleTurn = 0;

                Debug.Log("End of Turn.  Start Player Turn");

                foreach(Player character in enemies)
                {
                    int deadEnemies = 0;
                    if (character.dead)
                    {
                        deadEnemies++;                        
                    }
                    if (deadEnemies == enemies.Count)
                    {
                        AreaController.battleReturn = true;
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Castle 1");
                    }
                }                
                return;
            }           
            
        }        
    }



    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (heroes[characterTurnIndex].warriorClass || heroes[characterTurnIndex].berzerkerClass)
            {
                heroes[characterTurnIndex].actionType = Player.Action.melee;
                if (characterTurnIndex <= 2)
                {
                    NextPlayerTurn();
                }
                return;
            }
            if (heroes[characterTurnIndex].archerClass || heroes[characterTurnIndex].mageClass)
            {
                heroes[characterTurnIndex].actionType = Player.Action.ranged;
                if (characterTurnIndex <= 2)
                {
                    NextPlayerTurn();
                }
                return;
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
    }


}
