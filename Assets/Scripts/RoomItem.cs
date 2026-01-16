using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;
    private RoomInfo _info;

    public void SetUp(RoomInfo info)
    {
        _info = info;
        roomName.text = info.Name;
    }

    public void OnClick()
    {
        ConnectionToServer.Instance.JoinRoom(_info);
    }
    
}
