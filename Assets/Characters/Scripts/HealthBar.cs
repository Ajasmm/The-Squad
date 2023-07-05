using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] HealthInfo healthInfo;

    private void Start()
    {
        healthInfo.AddListenerOnHealthChange(UpdateHealthInfo);
    }
    private void OnDestroy()
    {
        healthInfo.RemoveListenerOnHealthChange(UpdateHealthInfo);
    }

    public void UpdateHealthInfo(int currentHealth, int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
}