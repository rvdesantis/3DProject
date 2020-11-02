using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EckTechGames.FloatingCombatText;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Player attackTarget;
    public Player spellTarget;

    public Vector3 targetPos;
    public Transform camFollower;

    public Vector3 idlePosition;
    public Vector3 strikePosition;
    public GameObject strikePoint;

    public List<Spell> spells;



    // Player Info

    public string playerName;
    public int playerHealth;
    public int playerMana;
    public int playerMaxHealth;
    public int playerMaxMana;
    public int playerLevel;
    public int playerSTR;
    public int playerDEF;
    public bool dead;

    // Player Equipment

    public GameObject Weapon;
    public List<GameObject> equipedWeapons;

    public void Start()
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

    public void Melee()
    {
        transform.position = attackTarget.strikePosition;
        GetComponent<Animator>().SetTrigger("AttackR");
        IEnumerator HitTimer()
        {
            yield return new WaitForSeconds(.75f);
            attackTarget.anim.SetTrigger("gotHit");
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

        } StartCoroutine(HitTimer());     
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


    public virtual void Update()
    {
        if (attackTarget != null)
        {
            targetPos = attackTarget.transform.position;
            transform.LookAt(targetPos);
        }
    }



}
