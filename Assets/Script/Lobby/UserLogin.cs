using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserLogin : MonoBehaviour
{
    [SerializeField] TMP_InputField NickName;
    [SerializeField] Button LogIn_Btn;

    const string nickNameKey = "NickName";

    private void OnEnable()
    {
        LogIn_Btn.onClick.AddListener(OnLogin);
    }
    private void OnDisable()
    {
        LogIn_Btn.onClick.RemoveListener(OnLogin);
    }


    private void Start()
    {
        if (PlayerPrefs.HasKey(nickNameKey))
            NickName.text = PlayerPrefs.GetString(nickNameKey);
    }

    public void OnLogin()
    {
        PhotonNetwork.NickName = NickName.text;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = Application.version;

        PlayerPrefs.SetString(nickNameKey, NickName.text);
        PlayerPrefs.Save();

        NetworkManager.Instance.Connect();
        // load to lobby
    }
}
