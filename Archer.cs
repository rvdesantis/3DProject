using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Player
{    
    public GameObject ammoModel;
    public List<Ammo> quiver;
    public Ammo equipedArrow;
    public int ammoIndex;

    public int healthMirror;
    public int manaMirror;
    public int levelMirror;
    public int XPMirror;

    public override void Start()
    {        
        base.Start();
        Weapon.power = quiver[ammoIndex].ammoPower;        
    }

    public override void LevelUp()
    {
        if (playerLevel == 1 && XP >= 500)
        {
            XP = 0;
            playerLevel = 2;
            PlayerPrefs.SetInt("ArLevel", 2);
            PlayerPrefs.SetInt("ArXP", 0);
            PlayerPrefs.SetInt("ArMaxHealth", playerMaxHealth + Random.Range(10, 16));
            PlayerPrefs.SetInt("ArHealth", PlayerPrefs.GetInt("ArMaxHealth"));
            PlayerPrefs.SetInt("ArMaxMana", playerMaxMana + Random.Range(10, 16));
            PlayerPrefs.SetInt("ArMana", PlayerPrefs.GetInt("ArMaxMana"));
            PlayerPrefs.SetInt("ArStr", playerSTR + Random.Range(15, 21));
            PlayerPrefs.SetInt("ArDef", playerDEF + Random.Range(10, 16));            

            PlayerPrefs.Save();
            SetBattleStats();
        }
        if (playerLevel == 2 && XP >= 1000)
        {
            XP = 0;
            playerLevel = 3;
            PlayerPrefs.SetInt("ArLevel", 3);
            PlayerPrefs.SetInt("ArXP", 0);
            PlayerPrefs.SetInt("ArMaxHealth", playerMaxHealth + Random.Range(15, 21));
            PlayerPrefs.SetInt("ArHealth", PlayerPrefs.GetInt("ArMaxHealth"));
            PlayerPrefs.SetInt("ArMaxMana", playerMaxMana + Random.Range(15, 21));
            PlayerPrefs.SetInt("ArMana", PlayerPrefs.GetInt("ArMaxMana"));
            PlayerPrefs.SetInt("ArStr", playerSTR + Random.Range(15, 21));
            PlayerPrefs.SetInt("ArDef", playerDEF + Random.Range(10, 16));

            PlayerPrefs.SetInt(playerName + "Weapon1", 1);

            PlayerPrefs.Save();
            SetBattleStats();
        }
    }

    public override void LevelReset()
    {
        XP = 0;
        playerLevel = 1;
        PlayerPrefs.SetInt("ArLevel", 1);
        PlayerPrefs.SetInt("ArXP", 0);
        PlayerPrefs.SetInt("ArMaxHealth", 75);
        PlayerPrefs.SetInt("ArHealth", 75);
        PlayerPrefs.SetInt("ArMaxMana", 20);
        PlayerPrefs.SetInt("ArMana", 20);
        PlayerPrefs.SetInt("ArStr", 45);
        PlayerPrefs.SetInt("ArDef", 35);
        PlayerPrefs.Save();        
        equipedArrow = quiver[0];

        PlayerPrefs.SetInt(playerName + "Weapon1", 0);
        PlayerPrefs.SetInt(playerName + "Weapon2", 0);
        PlayerPrefs.SetInt(playerName + "Weapon3", 0);

        for (int i = 0; i < spells.Count; i++)
        {
            if (i != 0)
            {
                spells.Remove(spells[i]);
            }
        }
    }

    public override void SetBattleStats()
    {
        playerMaxHealth = PlayerPrefs.GetInt("ArMaxHealth");
        playerHealth = PlayerPrefs.GetInt("ArHealth");
        playerMaxMana = PlayerPrefs.GetInt("ArMaxMana");
        playerMana = playerMaxMana;
        playerSTR = PlayerPrefs.GetInt("ArStr");
        playerDEF = PlayerPrefs.GetInt("ArDef");
        XP = PlayerPrefs.GetInt("ArXP");
        playerLevel = PlayerPrefs.GetInt("ArLevel");

        if (playerLevel == 2)
        {
            if (spells.Count == 1)
            {
                spells.Add(masterSpellList[1]);
            }
        }
    }

    public override void SaveStats()
    {
        PlayerPrefs.SetInt("ArXP", XP);        
        PlayerPrefs.SetInt("ArHealth", playerHealth);
        PlayerPrefs.Save();
    }

    public override void Ranged()
    {
        IEnumerator HitTimer()
        {
            attackTarget.FaceAttacker(this);
            yield return new WaitForSeconds(1);
            anim.SetTrigger("AttackR");

            yield return new WaitForSeconds(1.75f);
            transform.position = idlePosition;
            Reload();

            int damage = (playerSTR + equipedArrow.ammoPower) - attackTarget.playerDEF;

            attackTarget.TakeDamage(damage);

        }
        StartCoroutine(HitTimer());
    }

    public void TriggerBowDraw()
    {
        Weapon.GetComponent<Animator>().SetTrigger("setup2R");
    }

    public void TriggerBowRelease()
    {
        Weapon.GetComponent<Animator>().SetTrigger("finish"); 
    }

    public void TriggerBowShoot()
    {
        quiver[0].targetPosition = attackTarget.hitBox.transform.position;
        Instantiate<Ammo>(quiver[0], ammoModel.transform.position, Quaternion.identity);
        ammoModel.gameObject.SetActive(false);                
        Weapon.GetComponent<Animator>().SetTrigger("shoot2R");
    }

    public void Reload()
    {
        ammoModel.gameObject.SetActive(true);
    }


    public override void Update()
    {
        int healthMirror = PlayerPrefs.GetInt("ArMaxHealth");
        int manaMirror = PlayerPrefs.GetInt("ArMaxMana");
        int levelMirror = PlayerPrefs.GetInt("ArLevel");
        int XPMirror = PlayerPrefs.GetInt("ArXP");
        base.Update();
    }

}
