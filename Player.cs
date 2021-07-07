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
    public GameObject aimTargetGameObject;
    public Hitbox hitBox;
    public GameObject spellSpawnPoint;
    public CombatText combatTextPrefab;    

    public enum Action {melee, ranged, casting, item, flee }
    public Action actionType;

    public enum StatusEffect { paralyze, frozen, burn,}
    public StatusEffect statusEffect;

    public bool danger;

    public List<PlayableDirector> playables;
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    // 0 = attack, 1 = spell, 2 = damage 3 = die

    // Player Info
    public enum PlayerClass { warrior, fireMage, archer, berserker, darkMage}
    public PlayerClass playerClass;


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
    public List<Spell> masterSpellList;
    public Spell selectedSpell;
    public GameObject activeItem;

    public Sprite playerFace;
    public Sprite attBTicon;
    public Sprite spellBTicon;

    // Player Objects

    public Weapons Weapon;
    public List<Weapons> equipedWeapons;
    public List<Items> weaponItemBank;
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

            attackTarget.FaceAttacker(this);
            transform.position = attackTarget.strikePoint.transform.position;
            transform.LookAt(attackTarget.transform);
            

            yield return new WaitForSeconds(.25f);
            attackTarget.transform.LookAt(this.transform);
            LookAtTarget();
            anim.SetTrigger("AttackR");
            yield return new WaitForSeconds(1.75f);
            if (damage > 0)
            {
                attackTarget.TakeDamage(damage);                
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
            attackTarget.FaceAttacker(this);
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


            yield return new WaitForSeconds(.25f);
            anim.SetTrigger("AttackR");
            yield return new WaitForSeconds(1.75f);
            if (damage > 0)
            {
                attackTarget.TakeDamage(damage);                
            }
            transform.position = idlePosition;
        }
        StartCoroutine(HitTimer());
    }

    public virtual void TakeDamage(int damage)
    {
        combatTextPrefab.ToggleCombatText();
        if (damage > 0)
        {
            playerHealth = playerHealth - damage;
        }

        if (damage <= 0)
        {
            Debug.Log("damage 0 or less");
        }

        if (playerHealth <= 0)
        {
            if (danger == true)
            {
                Die();
            }
            if (danger == false)
            {
                Danger();
            }
        }
    }



    public virtual void CastSpell()
    {
        int damage = selectedSpell.power + Weapon.magPower;
       
        if (selectedSpell.manaCost <= playerMana)
        {
            playerMana = playerMana - selectedSpell.manaCost;
            GetComponent<Animator>().SetTrigger("castStart");
            LookAtTarget();
            if (selectedSpell.targetALL == false)
            {
                IEnumerator CastTimer()
                {
                    attackTarget.FaceAttacker(this);
                    yield return new WaitForSeconds(1.5f);
                    Spell spellToCast = Instantiate<Spell>(selectedSpell, spellSpawnPoint.transform.position, Quaternion.identity);
                    spellToCast.targetPosition = attackTarget.aimTargetGameObject.transform.position;                    
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
                            enemy.FaceAttacker(this);
                            yield return new WaitForSeconds(1.5f);
                            Spell spellToCast = Instantiate<Spell>(selectedSpell, spellSpawnPoint.transform.position, Quaternion.identity);
                            spellToCast.targetPosition = enemy.aimTargetGameObject.transform.position;
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
                    attackTarget.combatTextPrefab.floatingText.color = Color.red;
                    attackTarget.combatTextPrefab.damageAmount = damage;
                    attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;
                    attackTarget.TakeDamage(damage);
                    attackTarget.anim.SetTrigger("gotHit"); 
                }
                if (selectedSpell.targetALL)
                {
                    foreach (Enemy enemy in FindObjectOfType<BattleController>().enemies)
                    {
                        if (enemy.dead == false)
                        {                            
                            enemy.combatTextPrefab.floatingText.color = Color.red;
                            enemy.combatTextPrefab.damageAmount = damage;
                            enemy.combatTextPrefab.startingPosition = enemy.transform.position;
                            enemy.TakeDamage(damage);
                            enemy.anim.SetTrigger("gotHit");
                        }
                    }
                }     
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
        if (danger)
        {
            dead = true;
            anim.SetTrigger("Dead");
        }
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
        }
        if (attackTarget.playerHealth <= 0)
        {
            attackTarget.anim.SetTrigger("gotHit");   
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

    public void FaceAttacker(Player attacker)
    {
        transform.LookAt(attacker.transform);
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

    public void PlayAudioAttack()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }
    public void PlayAudioSpell()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }
    public void PlayAudioDamage()
    {
        audioSource.clip = audioClips[2];
        audioSource.Play();
    }
    public void PlayAudioDie()
    {
        audioSource.clip = audioClips[3];
        audioSource.Play();
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
