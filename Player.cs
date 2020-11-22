using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using EckTechGames.FloatingCombatText;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Player attackTarget;
    public Player spellTarget;
    

    public Vector3 targetPos;   

    public Vector3 idlePosition;
    public GameObject strikePoint;
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
    public int playerSTR;
    public int playerDEF;
    public bool dead;
    public List<Spell> spells;

    public Sprite playerFace;

    // Player Objects

    public GameObject Weapon;
    public List<GameObject> equipedWeapons;

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
            transform.position = attackTarget.strikePoint.transform.position;
            LookAtTarget();
            yield return new WaitForSeconds(.25f);

            anim.SetTrigger("AttackR");

            yield return new WaitForSeconds(1.75f);
            transform.position = idlePosition;


            int damage = playerSTR - attackTarget.playerDEF;

            if (damage > 0)
            {
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }

            if (damage <= 0)
            {
                Debug.Log("damage 0 or less");
            }

        }
        StartCoroutine(HitTimer());
    }

    public virtual void Ranged()
    {
        IEnumerator HitTimer()
        {
            LookAtTarget();
            yield return new WaitForSeconds(1);
            anim.SetTrigger("AttackR");

            yield return new WaitForSeconds(1.75f);
            transform.position = idlePosition;
            

            int damage = playerSTR - attackTarget.playerDEF;

            if (damage > 0)
            {
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }

            if (damage <= 0)
            {
                Debug.Log("damage 0 or less");
            }

        }
        StartCoroutine(HitTimer());
    }



    public virtual void CastSpell()
    {
        LookAtTarget();        
        GetComponent<Animator>().SetTrigger("castStart");
        IEnumerator SpellTimer()
        {
            yield return new WaitForSeconds(.75f);
            attackTarget.anim.SetTrigger("gotHit");

            int damage = spells[0].power;

            if (damage > 0)
            {
                attackTarget.playerHealth = attackTarget.playerHealth - damage;
            }

            if (damage <= 0)
            {
                Debug.Log("damage 0 or less");
            }
        }
        StartCoroutine(SpellTimer());        
    }


    public virtual void Die()
    {

    }

    public void EnemyHitTrigger()  // for use in anims & timelines to trigger hit anim but not calculate damage;
    {
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

    public virtual void Update()
    {

    }



}
