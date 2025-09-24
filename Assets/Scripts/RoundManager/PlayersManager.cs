using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayersManager : MonoBehaviourPunCallbacks
{
    public static PlayersManager Instance;
    [SerializeField] private Vector3 DeadCamaraPosition;
    [SerializeField] private Vector3 DeadCameraRotation;

    public int godActorNumber = 1;

    private int aliveCount = 2;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        RecalculateAlivePlayers();
    }

    public void MarkAsDead(Player player)
    {
        var props = new ExitGames.Client.Photon.Hashtable
        {
            { "IsDead", true }
        };
        player.SetCustomProperties(props);
    }

    public void MarkAsAlive(Player player)
    {
        var props = new ExitGames.Client.Photon.Hashtable
        {
            { "IsDead", false }
        };
        player.SetCustomProperties(props);
    }

    public int CountAlivePlayers()
    {
        return aliveCount;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsDead"))
        {
            RecalculateAlivePlayers();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RecalculateAlivePlayers();
    }

    private void RecalculateAlivePlayers()
    {
        int alive = 0;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.ActorNumber == godActorNumber)
                continue;

            if (p.CustomProperties.TryGetValue("IsDead", out object isDeadObj))
            {
                if (!(bool)isDeadObj)
                    alive++;
            }
            else
            {
                alive++;
            }
        }
        aliveCount = alive;
    }

    public void SetCamaraOnDeath(Camera camara)
    {
        camara.transform.position = DeadCamaraPosition;
        camara.transform.rotation = Quaternion.Euler(DeadCameraRotation);
    }
}
