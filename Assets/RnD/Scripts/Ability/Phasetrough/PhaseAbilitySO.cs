using UnityEngine;

public class PhaseAbilitySO : AbilitySO
{
    [Header("References")]
    [SerializeField] private CharacterData characterData;
    private InputReader input;
    public override void Initialize(GameObject player, InputReader input)
    {
        base.Initialize(player, input);
        this.input = input;
    }

    public override void OnDestroyAbility()
    {
        throw new System.NotImplementedException();
    }
}
