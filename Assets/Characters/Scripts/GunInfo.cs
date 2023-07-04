using System;
using UnityEngine;

[CreateAssetMenu(menuName ="UI/GunInfo", fileName ="GunInfo")]
public class GunInfo : ScriptableObject
{
    public int BulletInMag
    {
        set 
        { 
            bulletInMag = value;
            OnValueChange?.Invoke(bulletInMag, bulletInHand);
        }
    }
    public int BulletInHand
    {
        set
        {
            bulletInHand = value;
            OnValueChange?.Invoke(bulletInMag, bulletInHand);
        }
    }

    private int bulletInMag;
    private int bulletInHand;

    Action<int, int> OnValueChange;

    
    public void AddChangeListener(Action<int, int> listener)
    {
        listener(bulletInMag, bulletInHand);
        OnValueChange += listener;
    }
    public void RemoveChangeListener(Action<int, int> listener)
    {
        OnValueChange -= listener;
    }
}
