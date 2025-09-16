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
    [SerializeField] private GameObject playerCardPrefab;  // Prefab for the player card
    [SerializeField] private Transform cardParent;         // Parent transform with HorizontalLayoutGroup

    private void Start()
    {
        // Create a card for each player already in the room
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            CreatePlayerCard(p);
        }
    }

    /// <summary>
    /// Called when a new player enters the room.
    /// Creates a card for that player.
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CreatePlayerCard(newPlayer);
    }

    /// <summary>
    /// Called when a player leaves the room.
    /// Destroys their card and re-checks readiness.
    /// </summary>
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

    /// <summary>
    /// Called when any player's properties are updated.
    /// If the "Ready" property changed, check if all are ready.
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Ready"))
        {
            CheckAllReady();
        }
    }

    /// <summary>
    /// Creates a player card for a given player inside the HorizontalLayoutGroup.
    /// </summary>
    private void CreatePlayerCard(Player player)
    {
        var card = Instantiate(playerCardPrefab, cardParent);
        var playerCard = card.GetComponent<PlayerCard>();
        playerCard.SetPlayer(player);
    }

    /// <summary>
    /// Checks if all players in the lobby are ready.
    /// If so, load the game scene.
    /// </summary>
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
}
