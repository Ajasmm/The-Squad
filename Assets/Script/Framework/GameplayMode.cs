using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameplayMode : MonoBehaviour,IOnEventCallback
{
    [SerializeField] Door[] doors;
    [SerializeField] GameObject Startwindow;
    [SerializeField] GameObject gameOverWindow;
    [SerializeField] GameObject gameWinWindow;

    const string PLAYER_COUNT_KEY = "PlayerCount";

    public int Score = 0;

    public Player Player
    {
        set { player = value; }
        get { return player; }
    }
    private Player player;

    private void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);
        Startwindow.SetActive(true);
    }
    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void Initialize()
    {
        foreach(Door door in doors)
            door.enabled = false;

        gameOverWindow.SetActive(false);
        gameWinWindow.SetActive(false);
    }
    public void Play()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        SendOptions sendOptions = new SendOptions()
        {
            Reliability = true
        };
        PhotonNetwork.RaiseEvent(1, null, raiseEventOptions, sendOptions);
    }
    public void OnStart()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        foreach (Door door in doors)
            door.enabled = true;
    }
    public void GameOver()
    {
        gameOverWindow.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
    private void GameWon()
    {
        gameWinWindow.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void TriggerGameWin()
    {
        Debug.LogError("Game Win triggered");
        RaiseEventOptions eventOptions = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        SendOptions sendOptions = new SendOptions()
        {
            DeliveryMode = DeliveryMode.Reliable
        };
        PhotonNetwork.RaiseEvent(2, null, eventOptions, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        {
            OnStart();
            Startwindow.SetActive(false);
        }
        switch (photonEvent.Code)
        {
            case 1:
                OnStart();
                Startwindow.SetActive(false);
                break;
            case 2:
                GameWon();
                break;
        }
    }
}
