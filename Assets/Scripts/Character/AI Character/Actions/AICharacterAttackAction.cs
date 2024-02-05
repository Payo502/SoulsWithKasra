using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Actions/Attack")]
public class AICharacterAttackAction : ScriptableObject
{
    [Header("Attack")]
    [SerializeField] string attackAnimation;

    [Header("Combo Action")]
    public AICharacterAttackAction comboAction;

    [Header("Action Values")]
    [SerializeField] AttackType attackType;
    public int attackWeight = 50;
    public float actionRecoveryTime = 1.5f; // TIME BEFORE THE AI CAN ATTACK AGAIN
    public float minimumAttackAngle = -35;
    public float maximumAttackAngle = 35;
    public float minimumAttackDistance = 0;
    public float maximumAttackDistance = 2;

    public void AttemptToPerformAction(AICharacterManager aiCharacter)
    {
        aiCharacter.characterAnimatorManager.PlayTargetAttackActionAnimation(attackType, attackAnimation, true);
    }
}
