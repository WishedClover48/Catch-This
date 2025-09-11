using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerCardPrefab;
    [SerializeField] private Transform cardParent;

    private void Start()
    {
        // Create cards for players already in the room
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            CreatePlayerCard(p);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Create a card when a new player joins
        CreatePlayerCard(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Remove a card when the player leaves the room
        PlayerCard[] cards = cardParent.GetComponentsInChildren<PlayerCard>();
        foreach (var card in cards)
        {
            if (card.Player == otherPlayer)
            {
                Destroy(card.gameObject);
                break;
            }
        }
    }

    private void CreatePlayerCard(Player player)
    {
        var card = Instantiate(playerCardPrefab, cardParent);
        var playerCard = card.GetComponent<PlayerCard>();
        playerCard.SetPlayer(player);
    }
}
