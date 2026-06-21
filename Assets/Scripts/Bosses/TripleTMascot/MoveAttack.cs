using System.Collections;
using UnityEngine;

public class MoveAttack : BossMove
{
    
    private void Start()
    {
        ActionEvent += DebugEvent;
        Duration = 1;
        Cooldown = 2;
        IsReady = true;
    }

    private void OnDisable()
    {
        ActionEvent -= DebugEvent;
    }
    public void DebugEvent()
    {
        Debug.Log("MoveAttack executed.");
    }
}