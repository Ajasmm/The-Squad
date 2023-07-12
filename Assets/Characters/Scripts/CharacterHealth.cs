using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] bool IsNPC = false;
    [SerializeField] PhotonView photonView;

    [Header("Only for Player")]
    [SerializeField] HealthInfo healthInfo;

    private void Start()
    {
        if (!IsNPC && photonView.IsMine)
            healthInfo?.InitHealthInfo(Health, Health);

        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void AddDamage(int damage, Player shooterPlayer)
    {
        photonView.RPC("AddDamageRPC", photonView.Owner, damage, shooterPlayer);
    }

    [PunRPC]
    private void AddDamageRPC(int damage, Player shootedPlayer)
    {
        Health -= damage;
        if(Health < 0)
            Health = 0;

        if (photonView.IsMine)
            healthInfo?.SetCurrentHealth(Health);

        if (Health == 0)
        {

            if(IsNPC)
            {
                photonView.RPC("AddScore", shootedPlayer);
            }
            else
            {
                GameManager.Instance.GamePlayMode.GameOver();
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }
    [PunRPC]
    private void AddScore()
    {
        GameManager.Instance.GamePlayMode.Score += 100;
    }
}
