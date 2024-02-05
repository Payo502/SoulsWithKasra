using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] GameObject characterGameObject;
    [SerializeField] GameObject instanciatedGameObject;

    private void Awake()
    {

    }

    private void Start()
    {
        WorldAIManager.instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttamptToSpawnCharacter()
    {
        if (characterGameObject != null)
        {
            instanciatedGameObject = Instantiate(characterGameObject);
            instanciatedGameObject.transform.position = transform.position;
            instanciatedGameObject.transform.rotation = transform.rotation;
            instanciatedGameObject.GetComponent<NetworkObject>().Spawn();
        }
    }
}
