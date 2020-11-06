using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Player
{
    public GameObject ammoModel;
    public List<Ammo> quiver;

    public override void Start()
    {        
        base.Start();
        Reload();
    }


    public override void Melee()
    {       
        
        IEnumerator HitTimer()
        {
            yield return new WaitForSeconds(1);
            anim.SetTrigger("AttackR");                      
            
            yield return new WaitForSeconds(1.75f);
            transform.position = idlePosition;
            Reload();

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

    public void TriggerBowDraw()
    {
        Weapon.GetComponent<Animator>().SetTrigger("setup2L");
    }

    public void TriggerBowShoot()
    {
        Instantiate<Ammo>(quiver[0], ammoModel.transform.position, Quaternion.identity);
        ammoModel.gameObject.SetActive(false);        
        quiver[0].targetPosition = attackTarget.transform.position + new Vector3(0, 1, 0);
        Weapon.GetComponent<Animator>().SetTrigger("shoot2L");
    }

    public void Reload()
    {
        ammoModel.gameObject.SetActive(true);
    }


}
