using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoButton : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text PlayerCount;
    [SerializeField] Button JoinButton;

    RoomInfo roomInfo;

    public void Init(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;
        roomName.text = roomInfo.Name;
        PlayerCount.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;

        JoinButton.onClick.AddListener(Join);
    }

    public void Join()
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        JoinButton.interactable = false;
    }
}