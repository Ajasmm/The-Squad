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
        healthInfo?.InitHealthInfo(Health, Health);
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
        {
            Health = 0;
            photonView.RPC("AddScore", shootedPlayer);

            if (IsNPC)
                PhotonNetwork.Destroy(gameObject);
            else
                GameManager.Instance.levelManager.GameOver();
        }
    }
    [PunRPC]
    private void AddScore()
    {
        GameManager.Instance.levelManager.AddScore(100);
    }
}
