using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();

        CalculateTotalHealthBasedOnLevel(player.playerNetworkManager.vitality.Value);
        CalculateTotalStaminaBasedOnLevel(player.playerNetworkManager.endurance.Value);
    }
}
