using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private List<AbilitySO> equippedAbilities;

    private List<AbilitySO> _runtimeAbilities = new List<AbilitySO>();

    private void Start()
    {
        foreach (var ability in equippedAbilities)
        {
            if (ability != null)
            {
                AbilitySO instance = Instantiate(ability);
                instance.Initialize(gameObject, inputReader);
                _runtimeAbilities.Add(instance);
            }
        }
    }

    private void Update()
    {
        // Alirkan Delta Time ke semua ability runtime
        foreach (var ability in _runtimeAbilities)
        {
            ability.Tick(Time.deltaTime);
        }
    }
}
