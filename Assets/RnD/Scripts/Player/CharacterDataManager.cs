using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataManager : MonoBehaviour
{
    [SerializeField] private CharacterDataAnchorSO runtimeAnchor;

    private readonly Dictionary<BuffDataSO, Coroutine> _activeCoroutines = new Dictionary<BuffDataSO, Coroutine>();

    public void ApplyBuff(BuffDataSO buff)
    {
        if (buff == null) return;

        runtimeAnchor.AddBuff(buff);

        if (buff.IsTemporary)
        {
            if (_activeCoroutines.ContainsKey(buff) && _activeCoroutines[buff] != null)
            {
                StopCoroutine(_activeCoroutines[buff]);
            }

            Coroutine routine = StartCoroutine(RemoveBuffRoutine(buff));
            _activeCoroutines[buff] = routine;
        }
    }

    public void RemoveBuff(BuffDataSO buff)
    {
        if (buff == null) return;

        if (_activeCoroutines.TryGetValue(buff, out var routine))
        {
            if (routine != null) StopCoroutine(routine);
            _activeCoroutines.Remove(buff);
        }

        runtimeAnchor.RemoveBuff(buff);
    }

    private IEnumerator RemoveBuffRoutine(BuffDataSO buff)
    {
        yield return new WaitForSeconds(buff.duration);
        RemoveBuff(buff);
    }

    public void TakeDamage(float rawDamage)
    {
        runtimeAnchor.ModifyHealth(-rawDamage);

        Debug.Log($"Character menerima {rawDamage} damage!");
    }
}
