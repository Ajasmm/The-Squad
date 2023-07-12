using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] TMP_Text score;
    [SerializeField] Button Lobby_btn;

    private void Start()
    {
        Lobby_btn.onClick.AddListener(ToLobby);
    }
    public void OnEnable()
    {
        score.text = GameManager.Instance.GamePlayMode.Score.ToString();
    }
    private void OnDestroy()
    {
        Lobby_btn.onClick.RemoveListener(ToLobby);
    }

    public void ToLobby()
    {
        SceneManager.LoadScene(1);
    }
}
