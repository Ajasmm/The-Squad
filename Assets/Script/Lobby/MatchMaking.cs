using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MatchMaking : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject RoomListWindow;

    [SerializeField] Button JoinRandom_Btn;
    [SerializeField] Button Room_Btn;

    private void Start()
    {
        JoinRandom_Btn.onClick.AddListener(JoinRandom);
        Room_Btn.onClick.AddListener(Room);

        JoinRandom_Btn.interactable = true;
        Room_Btn.interactable = true;
    }
    private void OnDestroy()
    {
        JoinRandom_Btn?.onClick.RemoveListener(JoinRandom);
        Room_Btn?.onClick.RemoveListener(Room);
    }

    public void JoinRandom()
    {
        // Connect to a random Room
        // Do the remining in the OnJoinRoom Callback.
        // Also handle the fail.

        PhotonNetwork.JoinRandomOrCreateRoom();

        JoinRandom_Btn.interactable = false;
        Room_Btn.interactable= false;
    }
    public void Room()
    {
        // Enable the Room listing Window
        RoomListWindow.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        JoinRandom_Btn.interactable = true;
    }
}
