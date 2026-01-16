using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;
    private GameObject _controller;
    
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (_photonView.IsMine)
        {
            CreateController();
        }
    }

    public void Die()
    {
        PhotonNetwork.Destroy(_controller);
        CreateController();
    }
    
    private void CreateController()
    {
        Transform point = SpawnManager.Instance.GetSpawnPoint();
       _controller = PhotonNetwork.Instantiate(Path.Combine("PlayerController"), 
           point.position, point.rotation, 0, new object[] {_photonView.ViewID});
    }
}
