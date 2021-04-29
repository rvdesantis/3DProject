using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;



public class Player : MonoBehaviour
{
    public bool partyLeader;
    public Animator anim;
    public Player attackTarget;
    
    public Vector3 targetPos;   

    public Vector3 idlePosition;
    public GameObject strikePoint;
    public CinemachineVirtualCamera selfMeleeCam;

    public GameObject highlighter;
    public GameObject head;
    public Hitbox hitBox;
    public GameObject spellSpawnPoint;
    public CombatText combatTextPrefab;    

    public enum Action {melee, ranged, casting, item, flee }
    public Action actionType;
    public bool danger;

    public List<PlayableDirector> playables;
    public AudioSource audioSource;
    public List<AudioClip> audioClips;




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
    public Sprite attBTicon;
    public Sprite spellBTicon;

    // Player Objects

    public Weapons Weapon;
    public List<Weapons> equipedWeapons;

    public virtual void Start()
    {        
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
                attackTarget.combatTextPrefab.floatingText.color = Color.red;
                attackTarget.combatTextPrefab.damageAmount = damage;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;                
            }

            if (damage <= 0)
            {
                attackTarget.combatTextPrefab.floatingText.color = Color.red;
                attackTarget.combatTextPrefab.damageAmount = 0;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
                Debug.Log("damage 0 or less");
            }


            transform.position = attackTarget.strikePoint.transform.position;
            transform.LookAt(attackTarget.transform);

            yield return new WaitForSeconds(.25f);
            attackTarget.transform.LookAt(this.transform);
            LookAtTarget();
            anim.SetTrigger("AttackR");
            yield return new WaitForSeconds(1.75f);
            if (damage > 0)
            {
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }
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
                attackTarget.combatTextPrefab.floatingText.color = Color.red;
                attackTarget.combatTextPrefab.damageAmount = damage;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
            }

            if (damage <= 0)
            {
                attackTarget.combatTextPrefab.floatingText.color = Color.red;
                attackTarget.combatTextPrefab.damageAmount = damage;
                attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;                
                Debug.Log("damage 0 or less");
            }


            yield return new WaitForSeconds(.25f);
            anim.SetTrigger("AttackR");
            yield return new WaitForSeconds(1.75f);
            if (damage > 0)
            {
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }
            transform.position = idlePosition;
        }
        StartCoroutine(HitTimer());
    }



    public virtual void CastSpell()
    {
        int damage = selectedSpell.power + Weapon.magPower;
        attackTarget.combatTextPrefab.floatingText.color = Color.red;
        attackTarget.combatTextPrefab.damageAmount = damage;
        attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
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
                            enemy.combatTextPrefab.damageAmount = damage;                            
                        }
                        StartCoroutine(CastTimer());

                    }
                }
            }        

            IEnumerator SpellTimer()
            {
                yield return new WaitForSeconds(selectedSpell.damageTimer);
                

                if (selectedSpell.targetALL == false)
                {
                    attackTarget.anim.SetTrigger("gotHit");
                    attackTarget.combatTextPrefab.floatingText.color = Color.red;
                    attackTarget.combatTextPrefab.ToggleCombatText();
                    attackTarget.playerHealth = attackTarget.playerHealth - damage;

                }
                if (selectedSpell.targetALL)
                {
                    foreach (Enemy enemy in FindObjectOfType<BattleController>().enemies)
                    {
                        if (enemy.dead == false)
                        {
                            enemy.anim.SetTrigger("gotHit");
                            attackTarget.combatTextPrefab.floatingText.color = Color.red;
                            enemy.combatTextPrefab.startingPosition = enemy.transform.position;
                            enemy.combatTextPrefab.ToggleCombatText();
                            enemy.playerHealth = attackTarget.playerHealth - damage;
                        }
                    }
                }

                Debug.Log(playerName + " has cast " + selectedSpell.spellName + " at " + attackTarget.playerName);

                
                    
            }
            StartCoroutine(SpellTimer());
        }
    }

    public virtual void Danger()
    {
        if (playerHealth <= 0)
        {
            if (danger == false)
            {
                if (anim.GetBool("danger") == false)
                {
                    danger = true;
                    anim.SetBool("danger", true);
                }
            }
        }
    }

    public virtual void Die()
    {
        dead = true;
        anim.SetTrigger("Dead");
    }

    public void EnemyHitTrigger()  // for use in anims to trigger hit to trigger weapon noise and popup text
    {
        if (Weapon.attackSound != null)
        {
            Weapon.AttackSound();
        }               
        if (attackTarget.playerHealth > 0)
        {
            attackTarget.anim.SetTrigger("gotHit");
            attackTarget.combatTextPrefab.ToggleCombatText();            
        }
        if (attackTarget.playerHealth <= 0)
        {
            attackTarget.anim.SetTrigger("gotHit");            
            attackTarget.combatTextPrefab.ToggleCombatText();
            if (attackTarget.dead == false)
            {
                attackTarget.dead = true;                
            }            
        }        
    }

    public void TimelineHitTrigger()  // for use in timelines to trigger hit anim but not toggle popup text;
    {
        if (Weapon.attackSound != null)
        {
            Weapon.AttackSound();
        }        
        attackTarget.anim.SetTrigger("gotHit");   
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

    public virtual void SaveStats()
    {

    }

    public virtual void Update()
    {

    }



}
