using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; set; }

    public CameraMover camMover;   
    public List<Player> heroes;
    public List<Enemy> enemies;

    public int characterTurnIndex;
    public int battleTurn; // 0 = Player Turn / 1 = Enemy Turn

    public Vector3 spawnPoint1;
    public Vector3 spawnPoint2;
    public Vector3 spawnPoint3;


    private void Start()
    {        
        characterTurnIndex = 0;
        battleTurn = 0;
    }

    public void NextPlayerAct()
    {
        if (characterTurnIndex < 2)
        {
            characterTurnIndex = characterTurnIndex + 1;
            foreach (Enemy character in enemies)
            {
                character.attackTarget = heroes[characterTurnIndex];
            }            
            camMover.desiredPosition = heroes[characterTurnIndex].camFollower.transform.position;
            camMover.target = heroes[characterTurnIndex].attackTarget.transform;
            camMover.smoothSpeed = .0075f;
            return;
        }

        if (characterTurnIndex == 2)
        {
            camMover.desiredPosition = heroes[0].camFollower.transform.position;
        }
    }

    public void PlayerMeleeAttack()
    {
        camMover.desiredPosition = camMover.meleeCamTarget.transform.position;
        camMover.smoothSpeed = .0075f;
        heroes[characterTurnIndex].Melee();        
        IEnumerator MeleeTimer()
        {
            yield return new WaitForSeconds(3);
            NextPlayerAct();
        } StartCoroutine(MeleeTimer());        
    }

    public void PlayerSpellAttack()
    {
        camMover.desiredPosition = heroes[characterTurnIndex].camFollower.transform.position + new Vector3 (0, 1, -2);
        camMover.smoothSpeed = .0075f;
        heroes[characterTurnIndex].CastSpell();
        IEnumerator CamTimer()
        {
            yield return new WaitForSeconds(3);
            NextPlayerAct();
        }
        StartCoroutine(CamTimer());
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerMeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerSpellAttack();
        }

        if (characterTurnIndex == 0)
        {            
            camMover.target = heroes[0].attackTarget.transform;
        }

        if (characterTurnIndex == 1)
        {        
            heroes[1].transform.LookAt(heroes[1].attackTarget.transform);
        }

        if (characterTurnIndex == 2)
        {
            heroes[2].transform.LookAt(heroes[2].attackTarget.transform);
        }

        if (characterTurnIndex > 2)
        {
            camMover.target = heroes[0].camFollower;
            Debug.Log("character turn index exceeded 2");
        }


    }


}
