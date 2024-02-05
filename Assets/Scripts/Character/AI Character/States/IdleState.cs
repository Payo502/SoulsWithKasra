using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.characterCombatManager.currentTarget != null)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }
        else
        {
            // RETURN THIS STATE, TO KEEP SEARCHIG FOR A TARGET
            aiCharacter.aiCharacterCombatManager.FindATargetOnLineOfSight(aiCharacter);
            Debug.Log("SEARCHING FOR A TARGET");
            return this;
        }
    }
}
