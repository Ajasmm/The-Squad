using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Sigleton
    public static NetworkManager Instance { get { return GetNetworkManagerInstance(); } }
    private static NetworkManager instance;
    private static bool isGameQuit = false;

    private static NetworkManager GetNetworkManagerInstance()
    {
        if (isGameQuit)
            return null;

        if (instance == null)
        {
            GameObject gameManager = new GameObject("NetworkManager");
            return gameManager.AddComponent<NetworkManager>();
        }
        return instance;
    }
    #endregion

    public SyncValue<Dictionary<string, RoomInfo>> RoomList_Value = new SyncValue<Dictionary<string, RoomInfo>>();



    #region UnityMagic Functions
    private void Start()    
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        RoomList_Value.Value = new Dictionary<string, RoomInfo>();
    }
    private void OnDestroy()
    {
        if (instance == this)
            isGameQuit = true;
    }
    #endregion

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        RoomList_Value.Value.Clear();
        RoomList_Value.Invoke();

        PhotonNetwork.JoinLobby();
        
    }
    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log($"regions : {regionHandler.BestRegion.Code}");
    }
    public override void OnJoinedLobby()
    {
        Debug.Log($"Region connected : {PhotonNetwork.CloudRegion}");
        Debug.Log("Connected to Lobby");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                if (RoomList_Value.Value.ContainsKey(roomInfo.Name))
                    RoomList_Value.Value.Remove(roomInfo.Name);
            }
            else
            {
                if (RoomList_Value.Value.ContainsKey(roomInfo.Name))
                    RoomList_Value.Value[roomInfo.Name] = roomInfo;
                else
                    RoomList_Value.Value.TryAdd(roomInfo.Name, roomInfo);
            }
        }

        RoomList_Value.Invoke();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconncted. Cause {cause}");

        if (cause != DisconnectCause.DisconnectByClientLogic && cause != DisconnectCause.ApplicationQuit)
        {
            if (PhotonNetwork.InRoom)
                 PhotonNetwork.ReconnectAndRejoin();
            else
                PhotonNetwork.Reconnect();
        }
    }

}
