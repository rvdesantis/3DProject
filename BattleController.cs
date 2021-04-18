using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;


public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; set; }
    public BattleUIController uiController;
    public Combo comboController;
    public BattleItems battleItems;

    public List<CinemachineVirtualCamera> virtualCams;
    // 0-2 Hero Player Back cams, 3-5, Hero Player Front Cams
    public List<CinemachineVirtualCamera> castingCams;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera activeCam;
    public CinemachineVirtualCamera meleeCam;
    public List<PlayableDirector> comboPlayables;   
    
    
    public PlayerBank playerBank;
    public List<GameObject> partyInventory;

    public List<Trinket> masterTrinketlist;
    public List<Trinket> activeTrinketlist;
    
    public EnemyBank staticEnemyBank;
    public EnemyBank SpecialEnemyBank;
    public int enemyChoice;
    public int enemyNumber;


    public List<Player> heroes;
    public List<Player> enemies;
    public int deadEnemies;

    public int characterTurnIndex;
    public int battleTurn; // 0 = Player Turn / 1 = Enemy Turn
    public int focusIndex; // used for enemy focus when selecting targets
    public bool keyboard; // used to make sure key strokes to dot register after action selection

    public Vector3 spawnPoint0;
    public Vector3 spawnPoint1;
    public Vector3 spawnPoint2;

    public Vector3 enemySpawnPoint0;
    public Vector3 enemySpawnPoint1;
    public Vector3 enemySpawnPoint2;

    public bool endTurn;
    public bool combo;



    

    private void Start()
    {        
        characterTurnIndex = 0;
        battleTurn = 0;
        activeCam = mainCam;
        
        enemyNumber = Random.Range(1, 4);  // range does not include end number

        if (heroes[0] == null) // in place for testing scene, may use for battles with required party members.
        {
            heroes[0] = Instantiate<Player>(playerBank.bank[HeroSelect.hero0], spawnPoint0, Quaternion.identity);
        }
        if (heroes[0] != null) 
        {
            heroes[0].gameObject.SetActive(true);
            heroes[0].transform.position = spawnPoint0;
        }
        if (heroes[1] == null) 
        {
            heroes[1] = Instantiate<Player>(playerBank.bank[HeroSelect.hero1], spawnPoint1, Quaternion.identity);
        }
        if (heroes[1] != null)
        {
            heroes[1].gameObject.SetActive(true);
            heroes[1].transform.position = spawnPoint1;
        }
        if (heroes[2] == null) 
        {
            heroes[2] = Instantiate<Player>(playerBank.bank[HeroSelect.hero2], spawnPoint2, Quaternion.identity);
        }
        if (heroes[2] != null)
        {
            heroes[2].gameObject.SetActive(true);
            heroes[2].transform.position = spawnPoint2;
        }


        foreach (Player character in heroes)
        {
            character.SetBattleStats();            
        }

        // used to define enemies for specific launches, like Bosses && mimics.
        if (BattleLauncher.mimic == false && BattleLauncher.dunEnemy == false && BattleLauncher.bossEnemy == false)
        {            
            if (enemyNumber == 1)
            {
                enemyChoice = Random.Range(1, staticEnemyBank.bank.Count);
                enemies[0] = Instantiate<Player>(staticEnemyBank.bank[enemyChoice], enemySpawnPoint0, Quaternion.identity);
                enemies[1] = Instantiate<Player>(staticEnemyBank.bank[0], enemySpawnPoint1, Quaternion.identity);
                enemies[2] = Instantiate<Player>(staticEnemyBank.bank[0], enemySpawnPoint2, Quaternion.identity);
                enemies[0].GetComponent<Animator>().SetTrigger("taunt");
                enemies[0].ToggleHighlighter();
            }
            if (enemyNumber == 2)
            {
                enemyChoice = Random.Range(1, staticEnemyBank.bank.Count);
                enemies[0] = Instantiate<Player>(staticEnemyBank.bank[0], enemySpawnPoint0, Quaternion.identity);
                enemies[1] = Instantiate<Player>(staticEnemyBank.bank[enemyChoice], enemySpawnPoint1, Quaternion.identity);
                enemyChoice = Random.Range(1, staticEnemyBank.bank.Count);
                enemies[2] = Instantiate<Player>(staticEnemyBank.bank[enemyChoice], enemySpawnPoint2, Quaternion.identity);
                enemies[1].GetComponent<Animator>().SetTrigger("taunt");
                enemies[2].GetComponent<Animator>().SetTrigger("taunt");
                enemies[1].ToggleHighlighter();
                focusIndex = 1;
            }
            if (enemyNumber == 3)
            {
                enemyChoice = Random.Range(1, staticEnemyBank.bank.Count);
                enemies[0] = Instantiate<Player>(staticEnemyBank.bank[enemyChoice], enemySpawnPoint0, Quaternion.identity);
                enemyChoice = Random.Range(1, staticEnemyBank.bank.Count);
                enemies[1] = Instantiate<Player>(staticEnemyBank.bank[enemyChoice], enemySpawnPoint1, Quaternion.identity);
                enemyChoice = Random.Range(1, staticEnemyBank.bank.Count);
                enemies[2] = Instantiate<Player>(staticEnemyBank.bank[enemyChoice], enemySpawnPoint2, Quaternion.identity);
                enemies[0].GetComponent<Animator>().SetTrigger("taunt");
                enemies[0].ToggleHighlighter();
            }

        }
        if (BattleLauncher.mimic == true)
        {
            enemies[0] = Instantiate<Player>(SpecialEnemyBank.bank[0], enemySpawnPoint0, Quaternion.identity);
            enemies[1] = Instantiate<Player>(staticEnemyBank.bank[0], enemySpawnPoint1, Quaternion.identity);
            enemies[2] = Instantiate<Player>(staticEnemyBank.bank[0], enemySpawnPoint2, Quaternion.identity);
            BattleLauncher.mimic = false;
        }
        if (BattleLauncher.dunEnemy == true || BattleLauncher.bossEnemy == true)
        {
            enemies[0] = Instantiate<Player>(SpecialEnemyBank.bank[1], enemySpawnPoint0, Quaternion.identity);
            enemies[1] = Instantiate<Player>(staticEnemyBank.bank[0], enemySpawnPoint1, Quaternion.identity);
            enemies[2] = Instantiate<Player>(staticEnemyBank.bank[0], enemySpawnPoint2, Quaternion.identity);
            BattleLauncher.dunEnemy = false;            
        }

        foreach (Player character in heroes)
        {
            character.attackTarget = enemies[0];
            character.transform.LookAt(character.attackTarget.transform);
        }
        foreach (Player character in enemies)
        {
            character.attackTarget = heroes[0];
            character.transform.LookAt(character.attackTarget.transform);
            if (character.dead)
            {
                deadEnemies++;
            }
        }

        mainCam.LookAt = enemies[0].transform;
        virtualCams[0].LookAt = enemies[0].head.transform;
        virtualCams[1].LookAt = enemies[0].head.transform;
        virtualCams[2].LookAt = enemies[0].head.transform;    
        
        foreach (Enemy enemy in enemies)
        {
            virtualCams.Add(enemy.selfMeleeCam);
        }
        IEnumerator CamTimer()
        {
            yield return new WaitForSeconds(.5f);
            mainCam.m_Priority = 0;
            virtualCams[0].m_Priority = 1;           

        } StartCoroutine(CamTimer());
        
               
        comboController.AssignPlayers();
        TrinketChecker();
        keyboard = true;
    }




    public void TrinketChecker()
    {        
        foreach(Trinket battleTrinket in masterTrinketlist)
        {            
            if (PlayerPrefs.GetInt(battleTrinket.trinketName) == 1)
            {
                activeTrinketlist.Add(battleTrinket);
            }
        }
        foreach(Trinket trinket in activeTrinketlist)
        {
            Trinket newTrinket = Instantiate(trinket, Vector3.zero, Quaternion.identity);
            newTrinket.battleController = this;
            newTrinket.active = true;
            newTrinket.BattleEffect();
            uiController.trinketIcons[activeTrinketlist.IndexOf(trinket)].sprite = trinket.trinketSprite;
            uiController.trinketIcons[activeTrinketlist.IndexOf(trinket)].gameObject.SetActive(true);
        }
    }




    public void NextPlayerTurn() // for action selection prior to Action Cycle
    {
        if (characterTurnIndex < 2)
        {
            keyboard = true;
            characterTurnIndex = characterTurnIndex + 1;
            foreach (Enemy character in enemies)
            {
                character.attackTarget = heroes[characterTurnIndex];
            }
            meleeCam.Priority = 0;
            virtualCams[characterTurnIndex].Priority = 1;
            virtualCams[characterTurnIndex - 1].Priority = 0;
            virtualCams[characterTurnIndex].LookAt = enemies[focusIndex].head.transform;
            return;
        }
        if (characterTurnIndex == 2)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.dead == false)
                {
                    enemy.highlighter.gameObject.SetActive(false);
                }                
            }
            Debug.Log("Start Hero Action Cycle");
            keyboard = false;
            virtualCams[0].Priority = 1;
            virtualCams[2].Priority = 0;
            characterTurnIndex = 0;
            uiController.buttonUIPanel.gameObject.SetActive(false);
            PlayerAct();
        }

    }

    public void NextPlayerAct() // switch to next hero action
    {
        if (endTurn == true)
        {
            foreach (Player character in heroes)
            {
                character.transform.position = Vector3.MoveTowards(character.transform.position, character.idlePosition, .05f);
            }
            foreach (Enemy enemy in enemies)
            {
                if (enemy.playerHealth > 0)
                {
                    enemy.transform.position = enemy.idlePosition;
                }                
            }

            mainCam.Priority = 1;                        
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
                IEnumerator TurnTimer()
                {
                    yield return new WaitForSeconds(2); // to allow for previous turn to finish processing prior to updating characterTurnIndex 
                    meleeCam.Priority = 0;
                    characterTurnIndex = characterTurnIndex + 1;
                    virtualCams[characterTurnIndex].Priority = 1;
                    virtualCams[characterTurnIndex - 1].Priority = 0;
                    PlayerAct();                                
                }
                StartCoroutine(TurnTimer());

                return;
            }
            if (characterTurnIndex == 2)
            {
                endTurn = true;
                Debug.Log("end of player list");
                mainCam.Priority = 1;
                meleeCam.m_Priority = 0;
                mainCam.m_LookAt = enemies[0].transform;
                IEnumerator TurnTimer()
                {
                    yield return new WaitForSeconds(2);
                    NextPlayerAct();                               
                }
                StartCoroutine(TurnTimer());
                
            }
        }     
    }

    public Player GetHighestEnemy()
    {
        Player highestEnemy = enemies[0];
        foreach (Player enemy in enemies)
        {
            if (enemy.playerHealth > highestEnemy.playerHealth)
            {
                highestEnemy = enemy;
            }
        }

        return highestEnemy;
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
            foreach (FacePanel face in uiController.facePanels)
            {
                if (face.gameObject.activeSelf)
                {
                    face.gameObject.SetActive(false);
                }
            }
            comboController.ComboChecker();

            if (combo == false)
            {
                if (heroes[characterTurnIndex].attackTarget.playerHealth <= 0)
                {
                    heroes[characterTurnIndex].attackTarget = GetHighestEnemy();
                    heroes[characterTurnIndex].transform.LookAt(GetHighestEnemy().transform);
                    virtualCams[characterTurnIndex].LookAt = GetHighestEnemy().head.transform;
                }
                if (heroes[characterTurnIndex].attackTarget.playerHealth > 0)
                {
                    if (heroes[characterTurnIndex].actionType == Player.Action.melee)
                    {
                        heroes[characterTurnIndex].Melee();
                        meleeCam = heroes[characterTurnIndex].attackTarget.selfMeleeCam;
                        meleeCam.Priority = 2;
                        IEnumerator MeleeTimer()
                        {
                            yield return new WaitForSeconds(2.5f);
                            meleeCam.Priority = 0;
                            yield return new WaitForSeconds(.5f);
                            NextPlayerAct();
                        }
                        StartCoroutine(MeleeTimer());
                    }
                    if (heroes[characterTurnIndex].actionType == Player.Action.ranged)
                    {
                        heroes[characterTurnIndex].Ranged();
                        virtualCams[characterTurnIndex].Priority = 2;
                        IEnumerator MeleeTimer()
                        {
                            yield return new WaitForSeconds(3);
                            NextPlayerAct();
                        }
                        StartCoroutine(MeleeTimer());
                    }
                    if (heroes[characterTurnIndex].actionType == Player.Action.casting)
                    {
                        if (characterTurnIndex == 0)
                        {
                            castingCams[0].Priority = 2;
                            heroes[characterTurnIndex].CastSpell();
                            IEnumerator CamTimer()
                            {
                                yield return new WaitForSeconds(.25f);
                                castingCams[1].Priority = 2;
                                castingCams[0].Priority = 0;
                                yield return new WaitForSeconds(heroes[0].selectedSpell.castingTime);
                                castingCams[1].Priority = 0;
                                yield return new WaitForSeconds(1f);
                                NextPlayerAct();
                            }
                            StartCoroutine(CamTimer());
                        }
                        if (characterTurnIndex == 1)
                        {
                            castingCams[2].Priority = 2;
                            heroes[characterTurnIndex].CastSpell();
                            IEnumerator CamTimer()
                            {
                                yield return new WaitForSeconds(.25f);
                                castingCams[3].Priority = 2;
                                castingCams[2].Priority = 0;
                                yield return new WaitForSeconds(heroes[1].selectedSpell.castingTime);
                                castingCams[3].Priority = 0;
                                yield return new WaitForSeconds(1f);
                                NextPlayerAct();
                            }
                            StartCoroutine(CamTimer());
                        }
                        if (characterTurnIndex == 2)
                        {
                            castingCams[4].Priority = 2;
                            heroes[characterTurnIndex].CastSpell();
                            IEnumerator CamTimer()
                            {
                                yield return new WaitForSeconds(.25f);
                                castingCams[5].Priority = 2;
                                castingCams[4].Priority = 0;
                                yield return new WaitForSeconds(heroes[2].selectedSpell.castingTime);
                                castingCams[5].Priority = 0;
                                yield return new WaitForSeconds(1f);
                                NextPlayerAct();
                            }
                            StartCoroutine(CamTimer());
                        }
                    }
                    if (heroes[characterTurnIndex].actionType == Player.Action.item)
                    {                       
                        if (heroes[characterTurnIndex].activeItem == battleItems.potions[0].gameObject)
                        {
                            Debug.Log(heroes[characterTurnIndex].playerName + " is using Health Potion");
                            if (battleItems.potions[0].quantity > 0)
                            {
                                Debug.Log(heroes[characterTurnIndex].playerName + " has " + battleItems.potions[0].quantity + " health potions");
                                IEnumerator ItemTimer()
                                {
                                    heroes[characterTurnIndex].GetComponent<Animator>().SetTrigger("item");
                                    heroes[characterTurnIndex].attackTarget = heroes[characterTurnIndex];
                                    battleItems.potions[0].target = heroes[characterTurnIndex].attackTarget;
                                    battleItems.potions[0].HealthPotion();
                                    battleItems.potions[0].quantity--;
                                    yield return new WaitForSeconds(2);
                                    NextPlayerAct();
                                }
                                StartCoroutine(ItemTimer());
                            }
                        }
                        if (heroes[characterTurnIndex].activeItem == battleItems.potions[1].gameObject)
                        {
                            if (battleItems.potions[1].quantity > 0)
                            {
                                IEnumerator ItemTimer()
                                {
                                    heroes[characterTurnIndex].GetComponent<Animator>().SetTrigger("item");
                                    heroes[characterTurnIndex].attackTarget = heroes[characterTurnIndex];
                                    battleItems.potions[1].target = heroes[characterTurnIndex].attackTarget;
                                    battleItems.potions[1].ManaPotion();
                                    battleItems.potions[1].quantity--;
                                    yield return new WaitForSeconds(2);
                                    NextPlayerAct();
                                }
                                StartCoroutine(ItemTimer());
                            }
                        }
                        if (heroes[characterTurnIndex].activeItem == battleItems.potions[2].gameObject)
                        {
                            if (battleItems.potions[2].quantity > 0)
                            {
                                IEnumerator ItemTimer()
                                {
                                    heroes[characterTurnIndex].GetComponent<Animator>().SetTrigger("item");
                                    battleItems.potions[2].target = heroes[characterTurnIndex].attackTarget;
                                    battleItems.potions[2].ManaPotion();
                                    battleItems.potions[2].quantity--;
                                    yield return new WaitForSeconds(2);
                                    NextPlayerAct();
                                }
                                StartCoroutine(ItemTimer());
                            }
                        }
                    }
                }
                else
                {
                    NextPlayerAct();
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
            mainCam.m_LookAt = enemies[characterTurnIndex].transform;
            enemies[characterTurnIndex].Act();
            IEnumerator TurnTimer()
            {
                yield return new WaitForSeconds(3);
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
                EnemyAct();
                return;
            }
            if (characterTurnIndex == enemies.Count)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.playerHealth > 0)
                    {
                        enemy.transform.position = enemy.idlePosition;
                        enemy.highlighter.gameObject.SetActive(false);
                    }                   
                    
                    if (enemy.playerHealth <= 0 && enemy.gameObject.activeSelf && enemy.placeholder == false) // checks for only active gameobjects to avoid double counting
                    {
                        deadEnemies++;
                        enemy.gameObject.SetActive(false);
                    }
                }
                characterTurnIndex = 0;
                battleTurn = 0;                
                heroes[0].attackTarget = enemies[0];
                focusIndex = 0;
                TargetChecker();
                virtualCams[0].m_LookAt = enemies[focusIndex].transform;
                mainCam.m_Priority = 0;
                virtualCams[0].m_Priority = 1;
                foreach (FacePanel face in uiController.facePanels)
                {
                    if (face.gameObject.activeSelf == false)
                    {
                        face.gameObject.SetActive(true);
                    }
                }
                uiController.buttonUIPanel.gameObject.SetActive(true);
                Debug.Log("End of Turn.  Start Player Turn");
                keyboard = true;



                foreach (Player character in enemies) // check to see if all enemies are dead to send battle.
                {                    
                    if (deadEnemies == enemies.Count)
                    {
                        // calculate experience.
                        keyboard = false;
                        AreaController.battleReturn = true;
                        AfterBattle();
                        
                    }
                    return;
                }                
                
            }           
            
        }        
    }

    public void AfterBattle()
    {
        int totalXP = enemies[0].XP + enemies[1].XP + enemies[2].XP;

        foreach (Player character in heroes)
        {
            if (character.playerName == "Archer")
            {
                character.XP = character.XP + totalXP;
                PlayerPrefs.SetInt("ArXP", character.XP);
                PlayerPrefs.SetInt("ArHealth", character.playerHealth);
                if (character.playerLevel == 1 && character.XP >= 500)
                {
                    character.LevelUp();
                    uiController.levelUpUI.gameObject.SetActive(true);
                }
            }
            if (character.playerName == "Berserker")
            {
                character.XP = character.XP + totalXP;
                PlayerPrefs.SetInt("BerXP", character.XP);
                PlayerPrefs.SetInt("BerHealth", character.playerHealth);
                if (character.playerLevel == 1 && character.XP >= 500)
                {
                    character.LevelUp();
                    uiController.levelUpUI.gameObject.SetActive(true);
                }
            }
            if (character.playerName == "Warrior")
            {
                character.XP = character.XP + totalXP;
                PlayerPrefs.SetInt("WarXP", character.XP);
                PlayerPrefs.SetInt("WarHealth", character.playerHealth);
                if (character.playerLevel == 1 && character.XP >= 500)
                {
                    character.LevelUp();
                    uiController.levelUpUI.gameObject.SetActive(true);
                }
            }
            if (character.playerName == "Mage")
            {
                character.XP = character.XP + totalXP;
                PlayerPrefs.SetInt("MagXP", character.XP);
                PlayerPrefs.SetInt("MagHealth", character.playerHealth);
                if (character.playerLevel == 1 && character.XP >= 500)
                {
                    character.LevelUp();
                    uiController.levelUpUI.gameObject.SetActive(true);
                }
            }
            PlayerPrefs.Save();
        }

        if (uiController.levelUpUI.activeSelf == false)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Castle 1");
        }
        if (uiController.levelUpUI.activeSelf)
        {
            uiController.LevelUpUI();
        }

    }

    

    public void TargetChecker()
    {
        if (enemies[0].dead == false)
        {
            enemies[0].ToggleHighlighter();
            focusIndex = 0;
        }
        if (enemies[0].dead == true && enemies[1].dead == false)
        {
            enemies[1].ToggleHighlighter();
            focusIndex = 1;
        }
        if (enemies[0].dead == true && enemies[1].dead == true)
        {
            enemies[2].ToggleHighlighter();
            focusIndex = 2;
        }
    }

    public void CamTracker()
    {        
        foreach (CinemachineVirtualCamera cam in virtualCams)
        {
            if (cam.m_Priority > activeCam.m_Priority)
            {
                activeCam = cam;
            }
        }
    }

    private void Update()
    {
        CamTracker();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.JoystickButton2)) // X on XBOX Controller
        {
            if (uiController.spellPanel.activeSelf == false && uiController.itemPanel.activeSelf == false)
            {
                uiController.ToggleSpellPanel();
                uiController.ToggleButtonIcons();
            }
        }
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton3)) // Y on XBOX Controller
        {
            if (uiController.itemPanel.activeSelf == false && uiController.spellPanel.activeSelf == false)
            {
                uiController.ToggleItemPanel();
                uiController.ToggleButtonIcons();
            }
        }
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton1)) // B on XBOX Controller
        {
            if (uiController.spellPanel.activeSelf != true && uiController.itemPanel.activeSelf != true)
            {
                if (battleTurn == 0)
                {
                    if (characterTurnIndex != 0)
                    {
                        characterTurnIndex = characterTurnIndex - 1;
                        virtualCams[characterTurnIndex].Priority = 1;
                        virtualCams[characterTurnIndex + 1].Priority = 0;
                        if (heroes[characterTurnIndex].attackTarget.highlighter.activeSelf == false)
                        {
                            TargetChecker();
                        }
                    }
                }
            }
        }


        if (keyboard)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (battleTurn == 0)
                {
                    if (uiController.activeUI == false)
                    {
                        if (focusIndex < enemies.Count - 1)
                        {
                            foreach (Enemy enemy in enemies)
                            {
                                if (enemy.dead == false)
                                {
                                    enemy.highlighter.gameObject.SetActive(false);
                                }
                            }
                            focusIndex++;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                            if (enemies[focusIndex].dead && focusIndex < enemies.Count - 1)
                            {
                                focusIndex++;
                                activeCam.LookAt = enemies[focusIndex].head.transform;
                                enemies[focusIndex].ToggleHighlighter();
                            }
                            if (enemies[focusIndex].dead && focusIndex == enemies.Count - 1)
                            {
                                focusIndex = 0;
                                activeCam.LookAt = enemies[focusIndex].head.transform;
                                enemies[focusIndex].ToggleHighlighter();
                            }
                            return;
                        }
                        if (focusIndex == enemies.Count - 1)
                        {
                            foreach (Enemy enemy in enemies)
                            {
                                if (enemy.dead == false)
                                {
                                    enemy.highlighter.gameObject.SetActive(false);
                                }
                            }
                            focusIndex = 0;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                            if (enemies[focusIndex].dead)
                            {
                                focusIndex++;
                                activeCam.LookAt = enemies[focusIndex].head.transform;
                                enemies[focusIndex].ToggleHighlighter();
                            }
                            if (enemies[focusIndex].dead)
                            {
                                focusIndex++;
                                activeCam.LookAt = enemies[focusIndex].head.transform;
                                enemies[focusIndex].ToggleHighlighter();
                            }
                            return;
                        }
                    }

                    // for Spell Menu
                    if (uiController.activeUI == true)
                    {
                        if (uiController.activeUI = uiController.spellPanel)
                        {
                            if (uiController.spellIndex == 0)
                            {
                                uiController.spellIndex = heroes[characterTurnIndex].spells.Count - 1;

                            }
                            if (uiController.spellIndex > 0)
                            {
                                uiController.spellIndex--;

                            }
                        }
                    }

                    if (uiController.activeUI == true)
                    {
                        if (uiController.currentUI = uiController.itemPanel)
                        {
                            if (uiController.itemIndex == 0)
                            {
                                uiController.itemIndex = battleItems.potions.Count;

                            }
                            if (uiController.itemIndex > 0)
                            {
                                uiController.itemIndex--;

                            }
                        }
                    }
                }

            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (battleTurn == 0)
                {
                    if (uiController.activeUI == false)
                    {
                        if (focusIndex > 0)
                        {
                            foreach (Enemy enemy in enemies)
                            {
                                if (enemy.dead == false)
                                {
                                    enemy.highlighter.gameObject.SetActive(false);
                                }
                            }
                            focusIndex--;                            
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                            if (enemies[focusIndex].dead && focusIndex > 0)
                            {
                                focusIndex--;
                                activeCam.LookAt = enemies[focusIndex].head.transform;
                                enemies[focusIndex].ToggleHighlighter();
                            }
                            if (enemies[focusIndex].dead && focusIndex == 0)
                            {
                                focusIndex = 2;
                                activeCam.LookAt = enemies[focusIndex].head.transform;
                                enemies[focusIndex].ToggleHighlighter();
                            }
                            return;
                        }
                        if (focusIndex == 0)
                        {
                            foreach (Enemy enemy in enemies)
                            {
                                if (enemy.dead == false)
                                {
                                    enemy.highlighter.gameObject.SetActive(false);
                                }
                            }
                            focusIndex = enemies.Count - 1;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                            if (enemies[focusIndex].dead)
                            {
                                focusIndex--;
                                activeCam.LookAt = enemies[focusIndex].head.transform;
                                enemies[focusIndex].ToggleHighlighter();
                            }
                            if (enemies[focusIndex].dead)
                            {
                                focusIndex--;
                                activeCam.LookAt = enemies[focusIndex].head.transform;
                                enemies[focusIndex].ToggleHighlighter();
                            }
                            return;
                        }
                    }


                    if (uiController.activeUI == true)
                    {
                        if (uiController.currentUI = uiController.spellPanel)
                        {
                            if (uiController.spellIndex == heroes[characterTurnIndex].spells.Count - 1)
                            {
                                uiController.spellIndex = 0;
                            }
                            if (uiController.spellIndex < heroes[characterTurnIndex].spells.Count - 1)
                            {
                                uiController.spellIndex++;
                            }

                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                if (uiController.spellPanel.activeSelf)
                {
                    uiController.ToggleSpellPanel();
                }
                if (uiController.itemPanel.activeSelf)
                {
                    uiController.ToggleItemPanel();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                if (uiController.activeUI == false)
                {
                    heroes[characterTurnIndex].attackTarget = enemies[focusIndex];
                    enemies[focusIndex].ToggleHighlighter();
                    

                    if (heroes[characterTurnIndex].warriorClass || heroes[characterTurnIndex].berzerkerClass)
                    {
                        heroes[characterTurnIndex].actionType = Player.Action.melee;
                        if (characterTurnIndex <= 2)
                        {
                            TargetChecker();
                            NextPlayerTurn();
                        }
                        return;
                    }
                    if (heroes[characterTurnIndex].archerClass || heroes[characterTurnIndex].mageClass)
                    {                        
                        heroes[characterTurnIndex].actionType = Player.Action.ranged;
                        if (characterTurnIndex <= 2)
                        {
                            TargetChecker();
                            NextPlayerTurn();
                        }
                        return;
                    }
                }
                if (uiController.activeUI == true)
                {
                    if (uiController.activeUI == uiController.spellPanel)
                    {
                        // handled in BattleUIController since Space Key would only select active spell menu button
                        uiController.activeUI = false;
                    }

                }
            }
        }

    }


    private void FixedUpdate() // Set to .1 in project settings.
    {
        if (Input.GetAxis("Horizontal") > .5f)
        {
            if (battleTurn == 0)
            {
                if (uiController.activeUI == false)
                {
                    if (focusIndex > 0)
                    {
                        foreach (Enemy enemy in enemies)
                        {
                            if (enemy.dead == false)
                            {
                                enemy.highlighter.gameObject.SetActive(false);
                            }
                        }
                        focusIndex--;
                        activeCam.LookAt = enemies[focusIndex].head.transform;
                        enemies[focusIndex].ToggleHighlighter();
                        if (enemies[focusIndex].dead && focusIndex > 0)
                        {
                            focusIndex--;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                        }
                        if (enemies[focusIndex].dead && focusIndex == 0)
                        {
                            focusIndex = 2;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                        }
                        return;
                    }
                    if (focusIndex == 0)
                    {
                        foreach (Enemy enemy in enemies)
                        {
                            if (enemy.dead == false)
                            {
                                enemy.highlighter.gameObject.SetActive(false);
                            }
                        }
                        focusIndex = enemies.Count - 1;
                        activeCam.LookAt = enemies[focusIndex].head.transform;
                        enemies[focusIndex].ToggleHighlighter();
                        if (enemies[focusIndex].dead)
                        {
                            focusIndex--;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                        }
                        if (enemies[focusIndex].dead)
                        {
                            focusIndex--;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                        }
                        return;
                    }
                }


                if (uiController.activeUI == true)
                {
                    if (uiController.currentUI == uiController.spellPanel)
                    {
                        if (uiController.spellIndex == heroes[characterTurnIndex].spells.Count - 1)
                        {
                            uiController.spellIndex = 0;                            
                        }
                        if (uiController.spellIndex < heroes[characterTurnIndex].spells.Count - 1)
                        {
                            uiController.spellIndex++;                            
                        }

                    }
                }
            }
        }

        if (Input.GetAxis("Horizontal") < -.5f)
        {
            if (battleTurn == 0)
            {
                if (uiController.activeUI == false)
                {
                    if (focusIndex < enemies.Count - 1)
                    {
                        foreach (Enemy enemy in enemies)
                        {
                            if (enemy.dead == false)
                            {
                                enemy.highlighter.gameObject.SetActive(false);
                            }
                        }
                        focusIndex++;
                        activeCam.LookAt = enemies[focusIndex].head.transform;
                        enemies[focusIndex].ToggleHighlighter();
                        if (enemies[focusIndex].dead && focusIndex < enemies.Count - 1)
                        {
                            focusIndex++;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                        }
                        if (enemies[focusIndex].dead && focusIndex == enemies.Count - 1)
                        {
                            focusIndex = 0;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                        }
                        return;
                    }
                    if (focusIndex == enemies.Count - 1)
                    {
                        foreach (Enemy enemy in enemies)
                        {
                            if (enemy.dead == false)
                            {
                                enemy.highlighter.gameObject.SetActive(false);
                            }
                        }
                        focusIndex = 0;
                        activeCam.LookAt = enemies[focusIndex].head.transform;
                        enemies[focusIndex].ToggleHighlighter();
                        if (enemies[focusIndex].dead)
                        {
                            focusIndex++;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                        }
                        if (enemies[focusIndex].dead)
                        {
                            focusIndex++;
                            activeCam.LookAt = enemies[focusIndex].head.transform;
                            enemies[focusIndex].ToggleHighlighter();
                        }
                        return;
                    }
                }

                // for Spell Menu
                if (uiController.activeUI == true)
                {
                    if (uiController.currentUI == uiController.spellPanel)
                    {
                        if (uiController.spellIndex == 0)
                        {
                            uiController.spellIndex = heroes[characterTurnIndex].spells.Count - 1;                            
                        }
                        if (uiController.spellIndex > 0)
                        {
                            uiController.spellIndex--;                            
                        }
                    }
                    
                }
            }
        }

        if (Input.GetAxis("Vertical") < -.5f)
        {
            if (uiController.currentUI == uiController.itemPanel)
            {
                if (uiController.itemIndex < battleItems.potions.Count - 1)
                {
                    uiController.itemIndex++;                    
                }
                if (uiController.itemIndex == battleItems.potions.Count - 1)
                {
                    
                }
            }
        }
        if (Input.GetAxis("Vertical") > .5f)
        {
            if (uiController.currentUI == uiController.itemPanel)
            {
                if (uiController.itemIndex == 0)
                {
                    
                }
                if (uiController.itemIndex > 0)
                {
                    uiController.itemIndex--;
                }
            }
        }
    }

}
