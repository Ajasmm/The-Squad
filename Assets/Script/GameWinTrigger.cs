using ExitGames.Client.Photon.StructWrapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameWinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError($"Collided with {other.name}");

        if (!other.CompareTag("Player"))
            return;

        GameManager.Instance.GamePlayMode.TriggerGameWin();
    }
}
