using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField NickName;
    [SerializeField] Button LogIn_Btn;

    const string nickNameKey = "NickName";



    private void Start()
    {
        if (PlayerPrefs.HasKey(nickNameKey))
            NickName.text = PlayerPrefs.GetString(nickNameKey);

        LogIn_Btn.onClick.AddListener(OnLogin);
    }
    private void OnDestroy()
    {
        LogIn_Btn.onClick.RemoveListener(OnLogin);
    }

    public void OnLogin()
    {
        PhotonNetwork.NickName = NickName.text;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = Application.version;

        PlayerPrefs.SetString(nickNameKey, NickName.text);
        PlayerPrefs.Save();

        NetworkManager.Instance.Connect();
        LogIn_Btn.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        LogIn_Btn.interactable = true;
    }

}
