using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.Services.Analytics;

public class RoomCloseReporter : MonoBehaviourPunCallbacks
{
    private bool hasReported = false;

    // When a player leaves (including closing application)
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        TryReportIfLastPlayer();
    }

    // If THIS player quits the app
    private void OnApplicationQuit()
    {
        TryReportIfLastPlayer();
    }

    // Checks if this client is the last one & reports analytics only once
    private void TryReportIfLastPlayer()
    {
        // Prevent double send
        if (hasReported) return;

        // We are still in a valid room
        if (!PhotonNetwork.InRoom) return;

        // Only the last player should report the event
        if (PhotonNetwork.CurrentRoom.PlayerCount != 1) return;

        // Last player must be the Master Client
        if (!PhotonNetwork.IsMasterClient) return;


        bool naughty = false;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("badWord", out object value))
        {
            naughty = value != null && (bool)value;
        }


        NaughtyRoomEvent evt = new NaughtyRoomEvent
        {
            RoomID = ID.GetRoomID(),
            Naughty = naughty
        };

        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();

        hasReported = true;
    }
}
