using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

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

    Dictionary<string, RoomInfo> RoomList = new Dictionary<string, RoomInfo>();
    public Action<Dictionary<string, RoomInfo>> RoomListUpdated;


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
        RoomList.Clear();
        RoomListUpdated?.Invoke(RoomList);
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
        foreach(RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                if(RoomList.ContainsKey(roomInfo.Name))
                    RoomList.Remove(roomInfo.Name);
            }
            RoomList.Add(roomInfo.Name, roomInfo);
        }

        RoomListUpdated?.Invoke(RoomList);
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
