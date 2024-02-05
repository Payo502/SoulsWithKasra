using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("Testing")]
    [SerializeField] InstantCharacterEffect effectToTest;
    [SerializeField] bool processEffect = true;

    private void Update()
    {
        if (processEffect)
        {
            processEffect = false;
            InstantCharacterEffect effect = Instantiate(effectToTest);
            ProcessInstantEffect(effect);
        }
    }
}
