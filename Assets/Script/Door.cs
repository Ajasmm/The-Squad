using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BoxCollider))]
public class Door : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string DoorStateParameter = "State";
    [SerializeField] bool defaultState = false;


    int stateHash;
    HashSet<int> inside = new HashSet<int>();

    private void OnEnable()
    {
        stateHash = Animator.StringToHash(DoorStateParameter);
        animator.SetBool(DoorStateParameter, defaultState);

        defaultState = true;
        UpdateState();
    }
    private void OnDisable()
    {
        defaultState = false;
        UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Player"))
            inside.Add(other.gameObject.GetInstanceID());

        UpdateState();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
            inside.Remove(other.gameObject.GetInstanceID());

        UpdateState();
    }
    private void UpdateState()
    {
        if(this.enabled == false)
            defaultState = false;
        else if (inside.Count > 0)
            defaultState = true;
        else
            defaultState = false;

        animator.SetBool(stateHash, defaultState);
    }
}
