using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    CharacterManager character;

    [Header("STAMINA REGENERATION")]
    [SerializeField] float staminaRegenerationAmount = 2;
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    public int CalculateTotalHealthBasedOnLevel(int endurance)
    {
        float health;

        // CREATE AN EQUATION FOR HOW YOU WANT YOUR STAMINA TO BE CALCULATED

        health = endurance * 15;

        return Mathf.RoundToInt(health);
    }

    public int CalculateTotalStaminaBasedOnLevel(int endurance)
    {
        float stamina;

        // CREATE AN EQUATION FOR HOW YOU WANT YOUR STAMINA TO BE CALCULATED

        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        // ONLY OWNER CAN EDIT THEIR NRTWORK VARIABLE
        if (!character.IsOwner)
            return;

        // DONT REGENERATE STAMINA IF SPRINTING
        if (character.characterNetworkManager.isSprinting.Value)
            return;

        if (character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
    {
        // ONLY WANT TO RESET REGENERATION IF THE ACTION USED STAMINA
        // YOU DONT WANT TO REGEN STAMINA IF ITS ALREADY REGENERATING STAMINA
        if(currentStaminaAmount < previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
