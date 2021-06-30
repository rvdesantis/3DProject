using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaEnemy : Enemy
{

    public override void Melee()
    {
        IEnumerator HitTimer()
        {
            transform.position = attackTarget.strikePoint.transform.position;
            attackTarget.transform.LookAt(this.transform);
            yield return new WaitForSeconds(1);
            int damage = playerSTR - attackTarget.playerDEF;
            attackTarget.combatTextPrefab.floatingText.color = Color.red;
            attackTarget.combatTextPrefab.damageAmount = damage;
            attackTarget.combatTextPrefab.startingPosition = attackTarget.transform.position;

            anim.SetTrigger("attack2");

            yield return new WaitForSeconds(1.75f);
            transform.position = idlePosition;
            
            attackTarget.TakeDamage(damage);
            
        }
        StartCoroutine(HitTimer());
    }

    public override void TakeDamage(int damage)
    {
        BattleController battleController = FindObjectOfType<BattleController>();
        int diceRoll = Random.Range(0, 3);
        if (diceRoll != 2)
        {
            combatTextPrefab.floatingText.color = Color.red;
            base.TakeDamage(damage);
        }
        if (diceRoll == 2 && battleController.combo == true)
        {            
            combatTextPrefab.floatingText.color = Color.red;
            base.TakeDamage(damage);
        }
        if (diceRoll == 2 && battleController.combo == false)
        {
            anim.SetTrigger("dodge");
            combatTextPrefab.floatingText.color = Color.blue;            
            combatTextPrefab.startingPosition = attackTarget.transform.position;
            combatTextPrefab.ToggleCombatText();
            combatTextPrefab.floatingText.text = "DODGE";
        }
    }


}
