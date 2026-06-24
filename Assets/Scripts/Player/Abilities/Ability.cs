using System;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected PlayerContext context;
    protected virtual void Awake()
    {
        context = GetComponent<PlayerContext>();
    }
}
