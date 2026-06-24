using System.Collections;
using UnityEngine;

public class DebugAttack : BossAttack
{
    
    public override void Start()
    {
        base.Start();
        ActionEvent += DebugEvent;
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