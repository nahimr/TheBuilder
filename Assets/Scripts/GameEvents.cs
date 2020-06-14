using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Current;

    private void Awake()
    {
        Current = this;
    }

    public event Action OnTake;
    public event Action OnFire;

    public void Fire()
    {
        OnFire?.Invoke();
    }
    
    public void Take()
    {
        OnTake?.Invoke();
    }
    
    
}
