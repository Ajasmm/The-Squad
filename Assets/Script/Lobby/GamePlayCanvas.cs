using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayCanvas : MonoBehaviour
{
    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(1);
    }
}
