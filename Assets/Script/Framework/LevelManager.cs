using Photon.Pun;
using UnityEngine;

public class LevelManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject defaultPlayer;
    [SerializeField] GameObject gameOverWindow;

    public int Score { get { return score; } }
    int score = 0;

    string playerName;

    private void Start()
    {
        playerName = defaultPlayer.name;
        Destroy(defaultPlayer);
        if (PhotonNetwork.InRoom)
            PhotonNetwork.Instantiate(playerName, transform.position, transform.rotation);

        GameManager.Instance.levelManager = this;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(playerName, transform.position, transform.rotation);
    }
    public void AddScore(int score)
    {
        this.score += score;
    }
    public void GameOver()
    {
        PhotonNetwork.LeaveRoom();
        // enable the gameover window
    }
}
