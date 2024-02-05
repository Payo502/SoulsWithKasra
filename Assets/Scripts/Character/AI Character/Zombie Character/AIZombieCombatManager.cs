using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombieCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] ZombieDamageCollider rightHandDamageCollider;
    [SerializeField] ZombieDamageCollider rightFootDamageCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifier = 1.4f;
    [SerializeField] float attack02DamageModifier = 1.0f;

    public void SetAttack01Damage()
    {
        rightFootDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
    }

    public void OpenRightHandDamageCollider()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void DisableRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void OpenRightFootDamageCollider()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        rightFootDamageCollider.EnableDamageCollider();
    }

    public void DisableRightFootDamageCollider()
    {
        rightFootDamageCollider.DisableDamageCollider();
    }
}
