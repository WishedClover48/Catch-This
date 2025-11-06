using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class RoomListUI : MonoBehaviourPunCallbacks
{
    [Header("References")]
    [SerializeField] private GameObject roomButtonPrefab;
    [SerializeField] private Transform roomListParent;
    [SerializeField] private RoomJoin connectButton;

    private Dictionary<string, GameObject> roomButtons = new Dictionary<string, GameObject>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList || room.PlayerCount == 0)
            {
                if (roomButtons.ContainsKey(room.Name))
                {
                    Destroy(roomButtons[room.Name]);
                    roomButtons.Remove(room.Name);
                }
                continue;
            }

            if (room.PlayerCount >= room.MaxPlayers)
                continue;

            if (roomButtons.ContainsKey(room.Name))
            {
                var text = roomButtons[room.Name].GetComponentInChildren<TextMeshProUGUI>();
                text.text = $"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})";
                continue;
            }

            GameObject button = Instantiate(roomButtonPrefab, roomListParent);
            button.name = room.Name;
            button.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})";

            button.GetComponent<Button>().onClick.AddListener(() => connectButton.ConnectToRoom(room.Name));
            roomButtons.Add(room.Name, button);
        }
    }
}
