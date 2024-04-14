using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SettingSliderControl : MonoBehaviour
    {
        [SerializeField] private TMP_Text settingText;
        [SerializeField] private Slider slider;
        [SerializeField] private Button incrementButton;
        [SerializeField] private Button decrementButton;

        public void SetData(Action<int> setter, Func<int> getter, float min, float max)
        {
            slider.minValue = min;
            slider.maxValue = max;
            slider.wholeNumbers = true;
            slider.value = getter();
            slider.onValueChanged.AddListener(value => setter((int) value));

            settingText.text = getter().ToString();

            // Add listeners to the new buttons
            incrementButton.onClick.AddListener(() => IncrementSlider(setter, getter));
            decrementButton.onClick.AddListener(() => DecrementSlider(setter, getter));
        }

        private void IncrementSlider(Action<int> setter, Func<int> getter)
        {
            int newValue = Math.Min(getter() + 10, (int) slider.maxValue);
            SetValue(setter, newValue);
        }

        private void DecrementSlider(Action<int> setter, Func<int> getter)
        {
            int newValue = Math.Max(getter() - 10, (int) slider.minValue);
            SetValue(setter, newValue);
        }

        private void SetValue(Action<int> setter, int newValue)
        {
            setter(newValue);
            slider.value = newValue;
            settingText.text = newValue + "ms";
        }
    }
}