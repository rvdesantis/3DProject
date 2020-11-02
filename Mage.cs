using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EckTechGames.FloatingCombatText;

public class Mage : Player
{

    public GameObject castingHand;
    public ParticleSystem castingFXShock;


    public void CastShock() // triggered in animations
    {
        castingFXShock.Play();
    }


    public override void CastSpell()
    {
        Weapon.gameObject.SetActive(false);
        Weapon = equipedWeapons[1];
        Weapon.gameObject.SetActive(true);
        Spell spellToCast = Instantiate<Spell>(spells[0], castingHand.transform.position, Quaternion.identity);
        spellToCast.targetPosition = attackTarget.transform.position;

        base.CastSpell();

        Weapon = equipedWeapons[0];
    }


}
