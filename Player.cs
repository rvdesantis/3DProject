using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;



public class Player : MonoBehaviour
{
    public bool partyLeader;
    public Animator anim;
    public Player attackTarget;
    

    public Vector3 targetPos;   

    public Vector3 idlePosition;
    public GameObject strikePoint;
    public GameObject highlighter;
    public GameObject head;
    public Hitbox hitBox;
    public GameObject spellSpawnPoint;
    public CombatText combatTextPrefab;    

    public enum Action {melee, ranged, casting, item, flee }
    public Action actionType;


    public List<PlayableDirector> playables;





    // Player Info
    public bool warriorClass;
    public bool mageClass;
    public bool archerClass;
    public bool berzerkerClass;

    public string playerName;
    public int playerHealth;
    public int playerMana;
    public int playerMaxHealth;
    public int playerMaxMana;
    public int playerLevel;
    public int XP;
    public int playerSTR;
    public int playerDEF;
    public bool dead;
    public List<Spell> spells;
    public Spell selectedSpell;
    public GameObject activeItem;

    public Sprite playerFace;

    // Player Objects

    public Weapons Weapon;
    public List<Weapons> equipedWeapons;

    public virtual void Start()
    {        
        anim = GetComponent<Animator>();
        Debug.Log(playerName + " idle position set");
        LookAtTarget();
        idlePosition = transform.position;
    }

    public virtual void Act()
    {
        // left blank for Enemy use;
    }

    public virtual void Melee()
    {
        IEnumerator HitTimer()
        {
            int damage = (playerSTR + Weapon.power) - attackTarget.playerDEF;

            if (damage > 0)
            {
                attackTarget.combatTextPrefab.damageAmount = damage;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }

            if (damage <= 0)
            {
                attackTarget.combatTextPrefab.damageAmount = 0;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
                Debug.Log("damage 0 or less");
            }


            transform.position = attackTarget.strikePoint.transform.position;
            
            
            yield return new WaitForSeconds(.25f);
            attackTarget.transform.LookAt(this.transform);
            LookAtTarget();

            anim.SetTrigger("AttackR");


            yield return new WaitForSeconds(1.75f);
            transform.position = idlePosition;

        }
        StartCoroutine(HitTimer());
    }

    public virtual void Ranged()
    {
        IEnumerator HitTimer()
        {
            LookAtTarget();
            int damage = (playerSTR + Weapon.power) - attackTarget.playerDEF;

            if (damage > 0)
            {
                attackTarget.combatTextPrefab.damageAmount = damage;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;                
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }

            if (damage <= 0)
            {
                attackTarget.combatTextPrefab.damageAmount = damage;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;                
                Debug.Log("damage 0 or less");
            }


            yield return new WaitForSeconds(.25f);
            anim.SetTrigger("AttackR");

            

            yield return new WaitForSeconds(1.75f);
            transform.position = idlePosition;       

        }
        StartCoroutine(HitTimer());
    }



    public virtual void CastSpell()
    {
        if (selectedSpell.manaCost <= playerMana)
        {
            playerMana = playerMana - selectedSpell.manaCost;
            GetComponent<Animator>().SetTrigger("castStart");
            LookAtTarget();
            if (selectedSpell.targetALL == false)
            {
                IEnumerator CastTimer()
                {
                    yield return new WaitForSeconds(1.5f);
                    Spell spellToCast = Instantiate<Spell>(selectedSpell, spellSpawnPoint.transform.position, Quaternion.identity);
                    spellToCast.targetPosition = attackTarget.head.transform.position;
                } StartCoroutine(CastTimer());

            }
            if (selectedSpell.targetALL)
            {
                foreach(Enemy enemy in FindObjectOfType<BattleController>().enemies)
                {
                    if (enemy.dead == false)
                    {
                        IEnumerator CastTimer()
                        {
                            yield return new WaitForSeconds(1.5f);
                            Spell spellToCast = Instantiate<Spell>(selectedSpell, spellSpawnPoint.transform.position, Quaternion.identity);
                            spellToCast.targetPosition = enemy.head.transform.position;
                        }
                        StartCoroutine(CastTimer());

                    }
                }
            }        

            IEnumerator SpellTimer()
            {
                yield return new WaitForSeconds(selectedSpell.damageTimer);
                int damage = selectedSpell.power + Weapon.magPower;
                if (damage > 0)
                {
                    if (damage <= 0)
                    {
                        Debug.Log("damage 0 or less");
                    }
                    if (selectedSpell.targetALL == false)
                    {
                        attackTarget.playerHealth = attackTarget.playerHealth - damage;
                        attackTarget.combatTextPrefab.damageAmount = damage;
                        attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;

                    }
                    if (selectedSpell.targetALL == true)
                    {
                        foreach (Enemy enemy in FindObjectOfType<BattleController>().enemies)
                        {
                            if (enemy.dead == false)
                            {
                                enemy.playerHealth = attackTarget.playerHealth - damage;
                                enemy.combatTextPrefab.damageAmount = damage;
                                enemy.combatTextPrefab.startingPosition = enemy.transform.position;
                            }
                        }
                    }
                }

                if (selectedSpell.targetALL == false)
                {
                    attackTarget.anim.SetTrigger("gotHit");
                    attackTarget.combatTextPrefab.ToggleCombatText();
                }
                if (selectedSpell.targetALL)
                {
                    foreach (Enemy enemy in FindObjectOfType<BattleController>().enemies)
                    {
                        if (enemy.dead == false)
                        {
                            enemy.anim.SetTrigger("gotHit");
                            enemy.combatTextPrefab.ToggleCombatText();
                        }                        
                    }
                    attackTarget.anim.SetTrigger("gotHit");
                }

                Debug.Log(playerName + " has cast " + selectedSpell.spellName + " at " + attackTarget.playerName);

                
                    
            }
            StartCoroutine(SpellTimer());
        }
    }


    public virtual void Die()
    {
        
    }

    public void EnemyHitTrigger()  // for use in anims & timelines to trigger hit anim but not calculate damage;
    {
        if (attackTarget.playerHealth > 0)
        {
            attackTarget.anim.SetTrigger("gotHit");
            attackTarget.combatTextPrefab.ToggleCombatText();            
        }
        if (attackTarget.playerHealth <= 0)
        {
            attackTarget.anim.SetTrigger("gotHit");
            attackTarget.anim.SetTrigger("Dead");
            attackTarget.combatTextPrefab.ToggleCombatText();
            if (attackTarget.dead == false)
            {
                attackTarget.dead = true;                
            }            
        }        
    }

    public void LookAtTarget()
    {
        if (attackTarget != null)
        {
            targetPos = attackTarget.transform.position;
            transform.LookAt(targetPos);
        }
    }

    public void ToggleHighlighter()
    {
        if (dead == false)
        {
            if (highlighter.gameObject.activeSelf)
            {
                highlighter.gameObject.SetActive(false);
            }
            else
            {
                highlighter.gameObject.SetActive(true);
            }
        }
    }


    public virtual void LevelUp()
    {

    }

    public virtual void LevelReset()
    {

    }

    public virtual void SetBattleStats()
    {

    }

    public virtual void Update()
    {

    }



}
