using Photon.Pun;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject MatchMakingWindow;
    [SerializeField] GameObject SettingsWindow;

    private void OnEnable()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InLobby)
            NetworkManager.Instance.Connect();
    }

    public void Play()
    {
        MatchMakingWindow.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void Settings()
    {
        SettingsWindow.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
