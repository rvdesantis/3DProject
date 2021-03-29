using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class CombatText : MonoBehaviour
{
    public Text floatingText;
    public Animator anim;
    public BattleController battleController;
    
    public int damageAmount;
    public Vector3 startingPosition;






    public void ToggleCombatText()
    {
        Debug.Log("Start Combat Text");
        CombatText damageText = Instantiate<CombatText>(this, startingPosition, Quaternion.identity);
        Debug.Log("Instatiate Combat Text");
        damageText.battleController = FindObjectOfType<BattleController>();
        damageText.anim = damageText.floatingText.GetComponent<Animator>();
        Debug.Log("Battle Controller & Animator Set");

        damageText.floatingText.gameObject.SetActive(true);
        damageText.floatingText.text = damageAmount.ToString();
        damageText.floatingText.transform.position = startingPosition;
        damageText.anim.SetTrigger("float");
    }

    

    private void Update()
    {
        if (floatingText.gameObject.activeSelf)
        {            
            transform.LookAt(battleController.activeCam.transform.position + battleController.activeCam.transform.rotation * Vector3.forward, battleController.activeCam.transform.rotation * Vector3.up);
        }  
    }
}
