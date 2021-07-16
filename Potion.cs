using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public string itemName;   
    public BattleController battleController;
    public int quantity;
    public Player target;
    public Sprite itemImage;
    public ParticleSystem castingEffect;

    public AudioSource audioSource;
    public List<AudioClip> audioClips;


    public int health;
    public int mana;
    public int poison;

    private void Start()
    {
        
    }


    public void HealthPotion()
    {
        Potion castingPotion = Instantiate<Potion>(this, target.transform.position, Quaternion.identity);
        castingPotion.castingEffect.Play();
        castingPotion.battleController = FindObjectOfType<BattleController>();

        target.playerHealth = target.playerHealth + health;
        if (target.playerHealth > target.playerMaxHealth)
        {
            target.playerHealth = target.playerMaxHealth;
        }
        if (target.danger)
        {
            target.danger = false;
            target.anim.SetBool("danger", false);
        }
        audioSource.clip = audioClips[0];
        audioSource.Play();


        IEnumerator PotionTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(castingPotion);
        }
        StartCoroutine(PotionTimer());
    }



    public void ManaPotion()
    {
        target.playerMana = target.playerMana + mana;
        if (target.playerMana > target.playerMaxMana)
        {
            target.playerMana = target.playerMaxMana;
        }
    }
    
    public void CamFocus()
    {
        if (battleController.characterTurnIndex == 0 && battleController.battleTurn == 0)
        {
            battleController.castingCams[0].Priority = 2;            
            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(.25f);
                battleController.castingCams[1].Priority = 2;
                battleController.castingCams[0].Priority = 0;
                yield return new WaitForSeconds(1.5f);
                battleController.castingCams[1].Priority = 0;
            }
            StartCoroutine(CamTimer());
            return;
        }
        if (battleController.characterTurnIndex == 1 && battleController.battleTurn == 0)
        {
            battleController.castingCams[2].Priority = 2;
            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(.25f);
                battleController.castingCams[3].Priority = 2;
                battleController.castingCams[2].Priority = 0;
                yield return new WaitForSeconds(1.5f);
                battleController.castingCams[3].Priority = 0;
            }
            StartCoroutine(CamTimer());
            return;
        }
        if (battleController.characterTurnIndex == 2 && battleController.battleTurn == 0)
        {
            battleController.castingCams[4].Priority = 2;
            IEnumerator CamTimer()
            {
                yield return new WaitForSeconds(.25f);
                battleController.castingCams[5].Priority = 2;
                battleController.castingCams[4].Priority = 0;
                yield return new WaitForSeconds(1.5f);
                battleController.castingCams[5].Priority = 0;
            }
            StartCoroutine(CamTimer());
            return;
        }
    }

}
