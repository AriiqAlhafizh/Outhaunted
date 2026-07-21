using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterDataAssigner : MonoBehaviour
{
    [SerializeField] private CharacterData baseCharacterData;
    [SerializeField] private CharacterDataAnchorSO runtimeAnchor;

    private void Awake()
    {
        if (baseCharacterData != null && runtimeAnchor != null)
        {
            runtimeAnchor.ResetData(baseCharacterData);
        }
    }
}
