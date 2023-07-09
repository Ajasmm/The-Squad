using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BoxCollider))]
public class Door : MonoBehaviour
{
    [SerializeField] string DoorStateParameter = "State";
    [SerializeField] bool defaultState = false;

    Animator animator;

    int stateHash;
    HashSet<int> inside = new HashSet<int>();

    private void Start()
    {
        animator = GetComponent<Animator>();

        stateHash = Animator.StringToHash(DoorStateParameter);
        animator.SetBool(DoorStateParameter, defaultState);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);

        if(other.CompareTag("Enemy") || other.CompareTag("Player"))
            inside.Add(other.gameObject.GetInstanceID());

        UpdateState();
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.tag);

        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
            inside.Remove(other.gameObject.GetInstanceID());

        UpdateState();
    }
    private void UpdateState()
    {
        Debug.Log("Insider Count : " + inside.Count);

        if (inside.Count > 0)
            defaultState = true;
        else
            defaultState = false;

        Debug.Log($"State Updated to {defaultState}");

        animator.SetBool(stateHash, defaultState);
    }
}
