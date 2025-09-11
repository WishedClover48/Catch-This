using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    public Player Player { get; private set; }

    public void SetPlayer(Player player)
    {
        Player = player;
        nameText.text = player.NickName;
    }
}