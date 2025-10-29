using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyChat : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text chatDisplay;
    [SerializeField] private ProfanityFilter filter;


    public void OnSendMessage()
    {
        string message = inputField.text.Trim();
        if (string.IsNullOrEmpty(message)) return;

        var sensoredText=filter.CensorText(message);
        // Prefix with player name
        string fullMessage = $"{PhotonNetwork.NickName}: {sensoredText}";

        // Raise Photon event to broadcast message
        object[] content = new object[] { fullMessage };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(PhotonEventCodes.ChatMessage, content, options, sendOptions);

        inputField.text = "";
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == PhotonEventCodes.ChatMessage)
        {
            object[] data = (object[])photonEvent.CustomData;
            string message = (string)data[0];

            chatDisplay.text += $"\n{message}";
        }
    }

    private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);
}
