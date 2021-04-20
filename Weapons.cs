using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public string weaponName;
    public int weaponIndex;
    public int power;
    public int def;
    public int magPower;
    public int magDEF;

    public AudioSource audioSource;
    public AudioClip attackSound;



    public void AttackSound()
    {        
        audioSource.Play();
    }

    public int boost;

    public void boostweapon()
    {
        power = power + boost;
    }

    public void ResetBoost()
    {
        power = power - boost;
        boost = 0;
    }




}
