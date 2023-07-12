using Photon.Pun;
using UnityEngine;

public class LevelManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject defaultPlayer;
    [SerializeField] GameplayMode gameplayMode;

    public int Score { get { return score; } }
    int score = 0;

    string playerName;
    bool isInstanced = false;

    private void Start()
    {
        playerName = defaultPlayer.name;
        Destroy(defaultPlayer);

        GameManager.Instance.GamePlayMode = gameplayMode;

        if (PhotonNetwork.InRoom && !isInstanced)
        {
            PhotonNetwork.Instantiate(playerName, transform.position, transform.rotation);
            isInstanced = true;
        }

        GameManager.Instance.levelManager = this;
    }

    public override void OnJoinedRoom()
    {
        if (isInstanced)
            return;

        PhotonNetwork.Instantiate(playerName, transform.position, transform.rotation);
        isInstanced = true;
    }
}
