using UnityEngine;
using UnityEngine.UI;

public class SkillBarUI : MonoBehaviour
{
    [SerializeField] private Slider skillSlider;

private void Awake()
    {
        skillSlider.minValue = 0;
        skillSlider.maxValue = 100;
        skillSlider.value = 0;
    }

    public void AddProgress(float amount)
    {
        skillSlider.value += amount;

        if (skillSlider.value > skillSlider.maxValue)
        {
            skillSlider.value = skillSlider.maxValue;
        }
    }

    public void ResetProgress()
    {
        skillSlider.value = 0;
    }

    public float GetProgress()
    {
        return skillSlider.value;
    }
}

