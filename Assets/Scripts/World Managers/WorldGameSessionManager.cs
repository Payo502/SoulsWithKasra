using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameSessionManager : MonoBehaviour
{
    public static WorldGameSessionManager instance;

    [Header("Active Players In Game")]
    public List<PlayerManager> players = new List<PlayerManager>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPlayerToActivePlayersList(PlayerManager player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }

        // CHECHING FOR EMPTY SLOTS AND REMOVING THEM
        for (int i = players.Count - 1; i > -1; i--)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
        }
    }

    public void RemovePlayerFromActivePlayersList(PlayerManager player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);

        }

        // CHECHING FOR EMPTY SLOTS AND REMOVING THEM
        for (int i = players.Count - 1; i > -1; i--)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
        }
    }
}
