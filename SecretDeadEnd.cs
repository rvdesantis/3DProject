using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDeadEnd : DunCube
{
    public SecretWall secretWall;
    public Animator anim;

    public enum SecretCubeType { portal, weapon, shop, empty}
    public SecretCubeType secretCubeType;

    
    public GameObject orb;
    public GameObject portalParticles;
    public Chest portalChest;
    public List<Chest> chestBank;

    public enum SecretType { returnP, bossP, treasure, empty, enemy, shop }
    public SecretType secretType;


    private void Start()
    {
        dunBuilder = FindObjectOfType<DunBuilder>();
        secretWall.areaController = FindObjectOfType<AreaController>();
        if (secretCubeType == SecretCubeType.portal)
        {
            portalChest.areaController = FindObjectOfType<AreaController>();             
            portalChest.player = portalChest.areaController.moveController.GetComponentInChildren<FirstPersonPlayer>();            
        }
        if (secretCubeType == SecretCubeType.weapon)
        {
            AreaController areaController = FindObjectOfType<AreaController>();
            Chest chest = Instantiate(chestBank[1], itemSpawnPoint.transform.position, itemSpawnPoint.transform.rotation);
            chest.areaController = FindObjectOfType<AreaController>();
            chest.player = FindObjectOfType<AreaController>().moveController.GetComponentInChildren<FirstPersonPlayer>();
            areaController.chests.Add(chest);

            ArmorStand armorStand = chest.GetComponent<ArmorStand>();
            armorStand.AvailableWeapons();
            if (armorStand.weaponBank.Count > 0)
            {
                int w = Random.Range(0, armorStand.weaponBank.Count);
                if (AreaController.firstLoad)
                {
                    PlayerPrefs.SetInt("SecretArmorStand", w);
                }
                if (AreaController.firstLoad == false)
                {
                    w = PlayerPrefs.GetInt("SecretArmorStand");
                }

                if (PlayerPrefs.GetInt("chest" + areaController.chests.IndexOf(chest)) == 0)
                {
                    Items spawnedWeapon = Instantiate(armorStand.weaponBank[w], armorStand.weaponSpawnTransform.transform.position, armorStand.weaponSpawnTransform.transform.localRotation);
                    chest.treasure = spawnedWeapon;
                    Debug.Log("Armor Stand Spawned");
                }
            }            
        }

    }

    public void PortalChest()
    {
        portalChest.areaController.moveController.enabled = false;
        orb.gameObject.SetActive(true);        
        
        IEnumerator ChestTimer()
        {
            
            yield return new WaitForSeconds(1);
            portalChest.areaController.moveController.enabled = true;
            portalChest.gameObject.SetActive(false);
            yield return new WaitForSeconds(.95f);
            orb.gameObject.SetActive(false);
            portalParticles.gameObject.SetActive(true);
            anim.SetBool("portalActive", true);
        } StartCoroutine(ChestTimer());
    }


}
