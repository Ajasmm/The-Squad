using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject defaultPlayer;

    string playerName;

    private void Start()
    {
        playerName = defaultPlayer.name;
        Destroy(defaultPlayer);
        if (PhotonNetwork.InRoom)
            PhotonNetwork.Instantiate(playerName, transform.position, transform.rotation);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(playerName, transform.position, transform.rotation);
    }
}
