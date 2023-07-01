using UnityEngine;

public class RayCaster : MonoBehaviour
{
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 50F, Color.red, 0.5F);
    }
}
