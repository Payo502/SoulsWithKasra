using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightingDamage = 0;
    public float holyDamage = 0;

    [Header("Final Damage")]
    private int finalDamageDealt = 0; // THE DAMAGE THE CHARACTER TAKES AFTER ALL CALCULATIONS HAVE BEEN MADE

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false; // IF A CHARACTER POISE IS BROKEN PLAY STUN ANIMATION

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX; // USED ON TOP OF REGULAR SOUND FX

    [Header("Direction Damage Taken From")]
    public float angleHitFrom; // USED TO DETERMINE WHAT DAMAGE TO PLAY
    public Vector3 contactPoint; // USED TO DETERMINE WHERE THE BLOOD FX INSTANTIATE

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        // IF THE CHARACTER IS DEAD DONT DO ANYTHING
        if (character.isDead.Value)
            return;

        // CHECK IF CHARACTER IS INVULNERABLE

        CalculateDamage(character);
        PLayDirectionalBasedDamageAnimation(character);

        PlayDamageSFX(character);
        PlayDamageVFX(character);
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;
        if (characterCausingDamage != null)
        {
            // CHECK FOR DAMAGE MODIFIERS AND MODIFY BASE DAMAGE
        }

        // CHECK CHARACTER FOR FLAT DAMAGE REDUCTION

        // CHECK CHARACTER FOR ARMOR ABSORPTIONS

        // ADD ALL THE DAMAGE TYPES AND APPLY FINAL DANAME
        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + holyDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        Debug.Log("FINAL DAMAGE GIVEN " + finalDamageDealt);
        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

        // CALCULATE POISE DAMAGE TO DETERMINE IF THE CHARACTER IS STUNNED
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        // IF HAVE FIRE ATTACK, PLAY FIRE PARTICLES
        // ETC WITH ALL THE OTHER TYPES OF DAMAGE

        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        // SAME THING AS IN VFX FOR FIRE ETC

        AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

        character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
        character.characterSoundFXManager.PlayDamageGrunt();
    }

    private void PLayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (character.isDead.Value)
            return;

        // TODO CALCULATE IF POISE IS BROKEN
        poiseIsBroken = true;

        if (angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            // PLAY FRONT ANIMATION
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if(angleHitFrom <= -145 && angleHitFrom >= 180)
        {
            // PLAY FRONT ANIMATION
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            // PLAY BACK ANIMATION
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            // PLAY LEFT ANIMATION
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            // PLAY RIGHT ANIMATION
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
        }

        // IF POISE IS BROKEN, PLAY STAGGERING DAMAGE ANIMATION
        if (poiseIsBroken)
        {
            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
        }
    }
}
