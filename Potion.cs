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

    public List<AudioClip> audioClips;


    public int health;
    public int mana;
    public int poison;

    private void Start()
    {
        battleController = FindObjectOfType<BattleController>();
    }


    public void HealthPotion()
    {
        
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
        Potion castingPotion = Instantiate<Potion>(this, target.transform.position, Quaternion.identity);        
        castingPotion.castingEffect.Play();
        IEnumerator PotionTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(castingPotion);
        } StartCoroutine(PotionTimer());
    }

    public void ManaPotion()
    {
        target.playerMana = target.playerMana + mana;
        if (target.playerMana > target.playerMaxMana)
        {
            target.playerMana = target.playerMaxMana;
        }
    }

}
