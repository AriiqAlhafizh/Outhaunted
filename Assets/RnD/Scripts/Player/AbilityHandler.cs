using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private List<AbilitySO> equippedAbilities;

    public bool canUseAbilities = true;

    private List<AbilitySO> _runtimeAbilities = new List<AbilitySO>();

    public event Action<int> OnPlayAbilityAnimation;

    private void Start()
    {
        foreach (var ability in equippedAbilities)
        {
            if (ability != null)
            {
                AbilitySO instance = Instantiate(ability);
                instance.Initialize(gameObject, inputReader);
                instance.OnAbilityExecuted += PlayAbilityAnimation;
                _runtimeAbilities.Add(instance);
            }
        }
    }

    private void Update()
    {
        foreach (var ability in _runtimeAbilities)
        {
            ability.Tick(Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        float fdt = Time.fixedDeltaTime;
        foreach (var ability in _runtimeAbilities)
        {
            ability.FixedTick(fdt);
        }
    }

    private void OnDestroy()
    {
        foreach (var ability in _runtimeAbilities)
        {
            if (ability != null)
            {
                ability.OnAbilityExecuted -= PlayAbilityAnimation;
                ability.OnDestroyAbility();
            }
        }
    }

    private void PlayAbilityAnimation(int animationHash)
    {
        OnPlayAbilityAnimation?.Invoke(animationHash);
    }
}
