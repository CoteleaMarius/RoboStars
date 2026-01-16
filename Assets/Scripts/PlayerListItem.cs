using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerName;
    private Player _player;

    public void SetUp(Player player)
    {
        _player = player;
        playerName.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
    
}
