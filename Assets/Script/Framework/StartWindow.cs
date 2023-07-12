using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Button playButton;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.CurrentRoom.masterClientId)
            buttonText.text = "Play";
        else
            buttonText.text = "Wait";

        playButton.onClick.AddListener(StartGame);
    }
    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(StartGame);
    }

    public void StartGame()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != PhotonNetwork.CurrentRoom.masterClientId)
            return;

        GameManager.Instance.GamePlayMode.Play();
        playButton.interactable = false;
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            buttonText.text = "Play";
        else
            buttonText.text = "Wait";


    }
}
