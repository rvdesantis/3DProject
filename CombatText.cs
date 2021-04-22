using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class CombatText : MonoBehaviour
{
    public Player attachedPlayer;
    public Text floatingText;
    public Animator anim;
    public BattleController battleController;
    
    public int damageAmount;
    public Vector3 startingPosition;


    private void Start()
    {
        attachedPlayer = GetComponentInParent<Player>();
        battleController = FindObjectOfType<BattleController>();        
        startingPosition = transform.position;
    }



    public void ToggleCombatText()
    {        
        gameObject.SetActive(true);
        floatingText.gameObject.SetActive(true);
        floatingText.text = damageAmount.ToString();
        transform.position = startingPosition;
        anim.SetTrigger("float");        
    }

    public void TextRefresh() // added to end of float animation above to reset at last frame of animation
    {        
        transform.position = startingPosition;
        floatingText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
  



    private void Update()
    {
        if (floatingText.gameObject.activeSelf)
        {            
            transform.LookAt(battleController.activeCam.transform.position + battleController.activeCam.transform.rotation * Vector3.forward, battleController.activeCam.transform.rotation * Vector3.up);
        }  
    }
}
