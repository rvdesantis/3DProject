using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DunWeaponBT : MonoBehaviour
{
    public AreaController areaController;    
    public Text weaponTX;
    public Button weaponBT;
    public int buttonIndex;

    
    public int weaponIndex;


    // Start is called before the first frame update
    void Start()
    {

        weaponTX.text = areaController.activeBank.bank[buttonIndex].Weapon.weaponName;
        weaponIndex = areaController.activeBank.bank[buttonIndex].Weapon.weaponIndex;
        if (buttonIndex == 0)
        {
            weaponBT.Select();
        }
    }


    public void NextWeapon()
    {
        Player FocusedHero = areaController.activeBank.bank[buttonIndex];
        if (weaponIndex < areaController.activeBank.bank[buttonIndex].equipedWeapons.Count - 1)
        {
            FocusedHero.Weapon.gameObject.SetActive(false);
            weaponIndex++;            
            FocusedHero.Weapon = FocusedHero.equipedWeapons[weaponIndex];
            FocusedHero.Weapon.gameObject.SetActive(true);
            weaponTX.text = FocusedHero.Weapon.weaponName;
            Debug.Log(FocusedHero.Weapon.weaponName + " is equiped NEXT");
            areaController.areaUI.UpdatePlayerStats();
            return;
        }
        if (weaponIndex >= FocusedHero.equipedWeapons.Count - 1)
        {
            FocusedHero.Weapon.gameObject.SetActive(false);
            weaponIndex = 0;
            FocusedHero.Weapon = FocusedHero.equipedWeapons[0];
            FocusedHero.Weapon.gameObject.SetActive(true);
            weaponTX.text = FocusedHero.Weapon.weaponName;
            Debug.Log(FocusedHero.Weapon.weaponName + " is equiped ZERO");
            areaController.areaUI.UpdatePlayerStats();
        }
    }
}
