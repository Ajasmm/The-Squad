using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        PhotonNetwork.AddCallbackTarget(this);
        ActivateOfflineMode();
    }
    private void OnDestroy()
    {
        if (instance == this)
            isGameQuit = true;
    }
    #endregion


    public void ActivateOfflineMode()
    {
        PhotonNetwork.OfflineMode = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room");
    }

}
