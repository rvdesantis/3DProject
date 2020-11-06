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
    public Vector3 strikePosition;
    public GameObject strikePoint;
    public enum Action {melee, ranged, casting, item, flee }
    public Action actionType;


    public List<PlayableDirector> playables;





    // Player Info
    public bool warriorClass;
    public bool mageClass;
    public bool archerClass;

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

    // Player Objects

    public GameObject Weapon;
    public List<GameObject> equipedWeapons;

    public virtual void Start()
    {
        strikePosition = strikePoint.transform.position;
        anim = GetComponent<Animator>();
        IEnumerator AnimTimer()
        {
            yield return new WaitForSeconds(3);
            idlePosition = transform.position;
            Debug.Log(playerName + " idle position set");
        } StartCoroutine(AnimTimer());
    }

    public virtual void Melee()
    {
        playables[0].Play();
        IEnumerator HitTimer()
        {            
            yield return new WaitForSeconds(2.1f); // slightly longer than timeline animation for movable position.
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
        playables[0].Play();
        IEnumerator HitTimer()
        {
            yield return new WaitForSeconds(2.1f); // slightly longer than timeline animation for movable position.
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



    public virtual void Update()
    {
        if (attackTarget != null)
        {
            targetPos = attackTarget.transform.position;
            transform.LookAt(targetPos);
        }
    }



}
