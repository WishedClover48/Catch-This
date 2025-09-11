using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

/// <summary>
/// Simple chat system for the lobby.
/// Players can send messages through RaiseEvent,
/// and all clients update the chat text.
/// </summary>
public class LobbyChat : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private TMP_InputField inputField; // Where player types message
    [SerializeField] private TMP_Text chatDisplay;      // Where chat messages show up

    /// <summary>
    /// Called by the Send button (or Enter key).
    /// Sends the local player's message to all clients.
    /// </summary>
    public void OnSendMessage()
    {
        string message = inputField.text.Trim();
        if (string.IsNullOrEmpty(message)) return;

        // Prefix with player name
        string fullMessage = $"{PhotonNetwork.NickName}: {message}";

        // Raise Photon event to broadcast message
        object[] content = new object[] { fullMessage };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(PhotonEventCodes.ChatMessage, content, options, sendOptions);

        // Clear input
        inputField.text = "";
    }

    /// <summary>
    /// Callback when Photon receives an event.
    /// Handles chat messages.
    /// </summary>
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == PhotonEventCodes.ChatMessage)
        {
            object[] data = (object[])photonEvent.CustomData;
            string message = (string)data[0];

            // Append to chat display
            chatDisplay.text += $"\n{message}";
        }
    }

    private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);
}
