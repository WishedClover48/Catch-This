using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

/// <summary>
/// Manages the lobby UI by creating player cards,
/// updating them when players join/leave or toggle ready,
/// and starting the game when all players are ready.
/// </summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerCardPrefab;  
    [SerializeField] private Transform cardParent;         

    private void Start()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            SetPlayerVariable(p, "Ready", false);
            CreatePlayerCard(p);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "gameStarted", false } });
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetPlayerVariable(newPlayer, "Ready", false);
        CreatePlayerCard(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        foreach (var card in cardParent.GetComponentsInChildren<PlayerCard>())
        {
            if (card.Player == otherPlayer)
            {
                Destroy(card.gameObject);
                break;
            }
        }

        // Re-check readiness in case they were ready
        CheckAllReady();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Ready"))
        {
            CheckAllReady();
        }
    }

    private void CreatePlayerCard(Player player)
    {
        var card = Instantiate(playerCardPrefab, cardParent);
        var playerCard = card.GetComponent<PlayerCard>();
        playerCard.SetPlayer(player);
    }

    private void CheckAllReady()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            // If any player has no "Ready" property or is false, stop
            if (!p.CustomProperties.TryGetValue("Ready", out object readyValue) || !(bool)readyValue)
            {
                return;
            }
        }
        //Mark the start of the game
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "gameStarted", true } });
        }

        // If we get here, all players are ready. And they get sent to the Gameplay scene.
        PhotonNetwork.LoadLevel("SampleScene");
    }
    
    private void SetPlayerVariable(Player player, string variable, bool value)
    {
        Hashtable props = new Hashtable { { variable, false } };

        player.SetCustomProperties(props);
    }
}
