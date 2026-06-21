using System;
using System.Collections;
using UnityEngine;
public class BossMove : MonoBehaviour 
{
    protected Action ActionEvent;
    protected float Duration;
    protected float Cooldown;
    protected bool IsReady;

    public IEnumerator Execute()
    {
        if (IsReady)
        {
            ActionEvent?.Invoke();
            yield return new WaitForSeconds(Duration);
            StartCoroutine(StartCDCoroutine());
        }
    }

    private IEnumerator StartCDCoroutine()
    {
        IsReady = false;
        yield return new WaitForSeconds(Cooldown);
        IsReady = true;
    }
}