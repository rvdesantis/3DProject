using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public AreaController areaController;
    public AreaUIController areaUI;
    public FirstPersonPlayer player;

    public string itemName;
    public bool usable;
    
    
    public bool key;
    public Door targetDoor;

    public bool potion;
    public Player potionTarget;
    public int potionQuantity;

      

    public bool weapon;    
    public int weaponNum;
    public int weaponHero;

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

        if (potionTarget.berzerkerClass)
        {
            PlayerPrefs.SetInt("BerHealth", potionTarget.playerHealth);
        }
        if (potionTarget.archerClass)
        {
            PlayerPrefs.SetInt("ArHealth", potionTarget.playerHealth);
        }
        if (potionTarget.warriorClass)
        {
            PlayerPrefs.SetInt("WarHealth", potionTarget.playerHealth);
        }
        if (potionTarget.mageClass)
        {
            PlayerPrefs.SetInt("MagHealth", potionTarget.playerHealth);
        }        
    }




    private void Update()
    {
        pickedUpMirror = pickedUp;

        if (key)
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
                    gameObject.SetActive(false);
                    pickedUp = true;
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
