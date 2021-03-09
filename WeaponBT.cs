using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBT : MonoBehaviour
{

    public Text weaponTX;
    public Button weaponBT;
    public HeroSelect heroSelect;
    public int weaponIndex;


    // Start is called before the first frame update
    void Start()
    {
        weaponBT = GetComponent<Button>();
        weaponTX.text = heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.weaponName;
        weaponIndex = heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.weaponIndex;
    }


    public void NextWeapon()
    {
        if (weaponIndex < heroSelect.heroBank.bank[heroSelect.heroIndex].equipedWeapons.Count - 1)
        {
            heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.gameObject.SetActive(false);

            weaponIndex++;
            
            heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon = heroSelect.heroBank.bank[heroSelect.heroIndex].equipedWeapons[weaponIndex];
            heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.gameObject.SetActive(true);
            weaponTX.text = heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.weaponName;
            Debug.Log(heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.weaponName + " is equiped NEXT");
            heroSelect.heroStatsText.LoadHeroSelectStats();
            return;
        }
        if (weaponIndex >= heroSelect.heroBank.bank[heroSelect.heroIndex].equipedWeapons.Count - 1)
        {
            heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.gameObject.SetActive(false);

            weaponIndex = 0;
            heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon = heroSelect.heroBank.bank[heroSelect.heroIndex].equipedWeapons[0];
            heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.gameObject.SetActive(true);
            weaponTX.text = heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.weaponName;
            Debug.Log(heroSelect.heroBank.bank[heroSelect.heroIndex].Weapon.weaponName + " is equiped ZERO");
            heroSelect.heroStatsText.LoadHeroSelectStats();
        }        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
