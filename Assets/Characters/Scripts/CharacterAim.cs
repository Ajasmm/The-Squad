using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAim : MonoBehaviour
{
    [SerializeField] Transform aimTarger;
    [SerializeField] Rig AimRig;
    [SerializeField] Animator animator;
    [SerializeField] string Firing_ParameterName = "Firing";

    public Vector3 TargetPos;
    public float AimRigWeight;
    public bool Firing = false;

    private bool preFireState = false;

    private int FiringHash;

    private void Start()
    {
        FiringHash = Animator.StringToHash(Firing_ParameterName);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        aimTarger.position = TargetPos;
        AimRig.weight = Mathf.MoveTowards(AimRig.weight, AimRigWeight, deltaTime * 4);

        if(preFireState != Firing)
        {
            animator.SetBool(FiringHash, Firing);
            preFireState = Firing;
        }
    }
}
