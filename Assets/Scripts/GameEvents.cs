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

    public event Action OnSpecial;

    public event Action<int,bool> OnEndGame;


    public void EndGame(int lvl, bool haveWon)
    {
        OnEndGame?.Invoke(lvl, haveWon);
    }
    
    public void Special()
    {
        OnSpecial?.Invoke();
    }
    

    public void Fire()
    {
        OnFire?.Invoke();
    }
    
    public void Take()
    {
        OnTake?.Invoke();
    }
    
    
}
