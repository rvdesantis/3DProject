using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Combo : MonoBehaviour
{
    public BattleController battleController;
    public MeleeCombos meleeCombos;
    public SpellCombo spellCombos;

    public bool comboTrigger;

    public List<Player> comboParty;

    public Player mWarrior;
    public Player fWarrior;
    public Player fArcher;
    public Player fMage;
    public Player target; // for use with a single enemy target
    public List<Player> targets; // for use with multiple enemy targets

    // assets set active in timeline
    public Player mWarriorTimelineAsset;
    public Player fWarriorTimelineAsset;
    public Player fArcherTimelineAsset;
    public Player fMageTimelineAsset;
    public Player demonTimelineAsset;

    public int mWarriorCount;
    public int fBerzerkerCount;
    public int fMageCount;
    public int fArcherCount;

    public int enemyCount;





    public void ComboChecker()
    {        
        // Melee Combo
        if (battleController.heroes[0].dead == false && battleController.heroes[1].dead == false && battleController.heroes[2].dead == false)
        {
            if (battleController.heroes[0].actionType == Player.Action.melee || battleController.heroes[0].actionType == Player.Action.ranged)
            {
                if (battleController.heroes[1].actionType == Player.Action.melee || battleController.heroes[1].actionType == Player.Action.ranged)
                {
                    if (battleController.heroes[2].actionType == Player.Action.melee || battleController.heroes[2].actionType == Player.Action.ranged)
                    {
                        if (battleController.heroes[0].attackTarget == battleController.heroes[1].attackTarget && battleController.heroes[0].attackTarget == battleController.heroes[2].attackTarget)
                        {
                            battleController.combo = true;
                            meleeCombos.MeleeCombo();
                            Debug.Log("Melee Combo Trigger");
                            return;
                        }
                        else
                            Debug.Log("No Melee Combo Trigger");
                    }
                }
            }
        }
        if (fMageCount == 1 && fArcherCount == 1)
        {
            if (fArcher.actionType == Player.Action.casting && fMage.actionType == Player.Action.casting)
            {
                if (fArcher.selectedSpell == fArcher.spells[0] && fMage.selectedSpell == fMage.spells[0] && spellCombos.fireArrowsfinish == false)
                {
                    battleController.combo = true;
                    spellCombos.fireArrows = true;
                    spellCombos.SpellComboTrigger();
                    Debug.Log("Fire Arrows Combo Trigger");
                    return;
                }
            }
        }
        if (fMageCount == 1 && mWarriorCount == 1)
        {
            if (fMage.attackTarget == mWarrior.attackTarget)
            {
                if (fMage.selectedSpell == fMage.spells[0] && mWarrior.selectedSpell == mWarrior.spells[0] && spellCombos.firestrikefinish == false)
                {
                    battleController.combo = true;
                    spellCombos.fireStrike = true;
                    spellCombos.SpellComboTrigger();
                    Debug.Log("FireStrike Combo Trigger");
                    return;
                } 
            }
        }

        else
        battleController.combo = false;
        Debug.Log("Combo = False");        
    }

        



    public void AssignPlayers()
    {
        foreach (Player character in battleController.heroes)
        {
            if (character.archerClass)
            {
                fArcher = character;
                fArcherCount++;
            }
            if (character.warriorClass)
            {
                mWarrior = character;
                mWarriorCount++;
            }
            if (character.mageClass)
            {
                fMage = character;
                fMageCount++;
            }
            if (character.berzerkerClass)
            {
                fWarrior = character;
                fBerzerkerCount++;
            }
        }
    }

    public void AssignTargets()
    {
        foreach (Enemy character in battleController.enemies)
        {
            
        }
    }

    


}
