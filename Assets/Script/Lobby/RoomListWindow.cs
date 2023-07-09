using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListWindow : MonoBehaviourPunCallbacks
{
    // List all the available rooms
    // listen for any roomchange
    // keep track of all the room buttons

    [SerializeField] RoomInfoButton roomInfoButton_Prefab;
    [SerializeField] Transform scrollView_ContentArea;
    [SerializeField] TMP_InputField roomNameInput;

    [SerializeField] Button search_Btn;
    [SerializeField] Button create_Btn;

    List<RoomInfoButton> buttonList = new List<RoomInfoButton>();

    private void Start()
    {
        NetworkManager.Instance.RoomList_Value.AddListener(UpdateRoomList);

        create_Btn.interactable = true;
        search_Btn.onClick.AddListener(Search);
        create_Btn.onClick.AddListener(Create);
    }
    private void OnDestroy()
    {
        NetworkManager.Instance.RoomList_Value.AddListener(UpdateRoomList);

        search_Btn.onClick.RemoveListener(Search);
        create_Btn.onClick.RemoveListener(Create);
    }

    public void Search()
    {
        if (roomNameInput.text == "")
            RoomButtonsSetActive(true);
        else
            RoomButtonsSetActive(roomNameInput.text, true);

    }
    public void Create()
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 5,
            PlayerTtl = 0,
            EmptyRoomTtl = 0,
        };

        TypedLobby typedLobby = PhotonNetwork.CurrentLobby;
        PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, roomOptions, typedLobby);

        create_Btn.interactable = false;
    }
    private void UpdateRoomList(Dictionary<string, RoomInfo> roomList)
    {
        foreach(RoomInfoButton room in buttonList)
            if(room != null)
                Destroy(room);
        
        foreach(string key in roomList.Keys)
        {
            RoomInfoButton button = Instantiate(roomInfoButton_Prefab);
            button.Init(roomList[key]);
            button.transform.SetParent(scrollView_ContentArea);
            buttonList.Add(button);
        }
    }
    private void RoomButtonsSetActive(bool state)
    {
        foreach(RoomInfoButton button in buttonList)
            if(button != null)
                button.gameObject.SetActive(state);
    }
    private void RoomButtonsSetActive(string roomName, bool state)
    {
        foreach (RoomInfoButton button in buttonList)
            if (button != null) 
            {
                if (button.name == roomName)
                    button.gameObject.SetActive(state);
                else
                    button.gameObject.SetActive(!state);
            } 
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        create_Btn.interactable = true;
    }
}