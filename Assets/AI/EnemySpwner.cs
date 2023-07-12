using Photon.Pun;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpwner : MonoBehaviour
{
    [SerializeField] public string enemyName;
    [SerializeField] public List<Transform> patrollingPoints;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != PhotonNetwork.CurrentRoom.masterClientId)
            return;

        GameObject spawnedEnemy = PhotonNetwork.InstantiateRoomObject(enemyName, transform.position, transform.rotation);
        NPCStates nPCStates = spawnedEnemy.GetComponent<NPCStates>();
        nPCStates.patrollingPoints = patrollingPoints;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemySpwner))]
public class EnemySpwnerInspector : Editor
{
    EnemySpwner EnemySpwner;

    private void OnEnable()
    {
        EnemySpwner = target as EnemySpwner;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("SpawnEnemy"))
        {
            Object prefab = Resources.Load(EnemySpwner.enemyName);
            GameObject enemyPrefab = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            enemyPrefab.transform.SetParent(EnemySpwner.transform.parent, false);
            enemyPrefab.transform.SetPositionAndRotation(EnemySpwner.transform.position, Quaternion.identity);
            enemyPrefab.GetComponent<NPCStates>().patrollingPoints = EnemySpwner.patrollingPoints;
        }
    }
}
#endif