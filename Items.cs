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

    public bool weapon;    
    public int weaponNum;
    public int weaponHero;

    public Sprite itemSprite;

    public static bool pickedUp;
    public bool pickedUpMirror;



    private void Update()
    {
        pickedUpMirror = pickedUp;

        if (key)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) && pickedUp == false)
            {
                if (Vector3.Distance(player.transform.position, this.transform.position) < 5)
                {
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
