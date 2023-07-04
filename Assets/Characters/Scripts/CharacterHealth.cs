using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] PhotonView photonView;

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
            GameManager.Instance.levelManager.GameOver();
        }
    }
    [PunRPC]
    private void AddScore()
    {
        GameManager.Instance.levelManager.AddScore(100);
    }
    
}
