using System;
using UnityEngine;

[CreateAssetMenu(menuName ="UI/HealthInfo", fileName = "HealthInfo")]
public class HealthInfo : ScriptableObject
{
    private int currentHealth;
    private int maxHealth;

    Action<int, int> OnHealthChange;

    public void InitHealthInfo(int CurrentHealth, int MaxHealth)
    {
        this.currentHealth = CurrentHealth;
        this.maxHealth = MaxHealth;
        OnHealthChange?.Invoke(currentHealth, maxHealth);
    }
    public void SetCurrentHealth(int CurrentHealth)
    {
        this.currentHealth = CurrentHealth;
        OnHealthChange?.Invoke(currentHealth, maxHealth);
    }
    public void AddListenerOnHealthChange(Action<int , int> OnHealthChange)
    {
        OnHealthChange(currentHealth, maxHealth);
        this.OnHealthChange += OnHealthChange;
    }
    public void RemoveListenerOnHealthChange(Action<int , int> OnHealthChange)
    {
        this.OnHealthChange -= OnHealthChange;
    }
}
