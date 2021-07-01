using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public AreaController areaController;
    public AreaUIController areaUI;
    public FirstPersonPlayer player;
    public AudioClip activatedSound;

    public string itemName;
    public int goldCost;
    public bool usable;

    
    public enum itemType { gold, trinket, potion, weapon, key, portal, placeholder }
    public itemType itemFunction;     

    public Door targetDoor;    
    public Player potionTarget;
    public int potionQuantity;

    public int weaponNum;
    public int weaponHero;
    public int weaponPower;
    public int weaponDef;
    public int weaponSpell;

    public Sprite itemSprite;

    public static bool pickedUp;
    public bool pickedUpMirror;



    public void HealthPotion()
    {
        potionTarget.playerHealth = potionTarget.playerHealth + 25;
        if (potionTarget.playerHealth > potionTarget.playerMaxHealth)
        {
            potionTarget.playerHealth = potionTarget.playerMaxHealth;
            Debug.Log(potionTarget.playerName + " is at Max health");
        }

        if (potionTarget.playerClass == Player.PlayerClass.berserker)
        {
            PlayerPrefs.SetInt("BerHealth", potionTarget.playerHealth);
        }
        if (potionTarget.playerClass == Player.PlayerClass.archer)
        {
            PlayerPrefs.SetInt("ArHealth", potionTarget.playerHealth);
        }
        if (potionTarget.playerClass == Player.PlayerClass.warrior)
        {
            PlayerPrefs.SetInt("WarHealth", potionTarget.playerHealth);
        }
        if (potionTarget.playerClass == Player.PlayerClass.fireMage)
        {
            PlayerPrefs.SetInt("MagHealth", potionTarget.playerHealth);
        }
        if (potionTarget.playerClass == Player.PlayerClass.darkMage)
        {
            // PlayerPrefs.SetInt("MagHealth", potionTarget.playerHealth);
        }
        PlayerPrefs.Save();
    }




    private void Update()
    {
        pickedUpMirror = pickedUp;

        if (itemFunction == itemType.key)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < 5)
            {
                areaUI.messageText.text = "Pick Up " + itemName + " ?";
                areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);

            }
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) && pickedUp == false)
            {
                if (Vector3.Distance(player.transform.position, this.transform.position) < 5)
                {
                    areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                    areaUI.messageText.text = itemName + " added to Inventory";
                    areaUI.messageUI.GetComponent<Animator>().SetTrigger("message");
                    areaController.areaInventory.Add(this);
                    areaUI.itemImage.sprite = itemSprite;
                    targetDoor.locked = false;
                    pickedUp = true;
                    areaUI.audioSource.clip = activatedSound;
                    areaUI.audioSource.PlayOneShot(activatedSound, 1);

                    gameObject.SetActive(false);                    
                    areaUI.ItemImage();
                    return;
                }

            }
            if (pickedUp)
            {
                areaController.areaInventory.Add(this);
                targetDoor.locked = false;
                gameObject.SetActive(false);                
            }
        }

    }

}
