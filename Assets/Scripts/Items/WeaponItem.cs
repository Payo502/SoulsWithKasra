using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int holyDamage = 0;
    public int fireDamage = 0;
    public int lightningDamage = 0;

    [Header("Weapon Poise")]
    public float poiseDamage = 10;

    [Header("Attack Modifiers")]
    public float light_Attack_01_Modifier = 1f;
    public float light_Attack_02_Modifier = 1.2f;
    public float heavy_Attack_01_Modifier = 1.4f;
    public float heavy_Attack_02_Modifier = 1.6f;
    public float charged_Attack_01_Modifier = 2.0f;
    public float charged_Attack_02_Modifier = 2.2f;

    [Header("Stamina Cost Modifiers")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 0.9f;

    [Header("Actions")]
    public WeaponItemAction oh_RB_Action; // ONE HANDED RIGHT BUMPER ACTION
    public WeaponItemAction oh_RT_Action; // ONE HAND RIGHT TRIGGER ACTION

    [Header("Whooshes")]
    public AudioClip[] whooshes;
}
