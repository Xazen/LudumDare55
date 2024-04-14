using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    [SerializeField] private TMP_Text settingText;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    public void SetData<T>(Action<T> setter, Func<T> getter, List<T> choices)
    {
        leftButton.onClick.AddListener(() =>
        {
            var currentIndex = choices.IndexOf(getter());
            currentIndex = (currentIndex - 1 + choices.Count) % choices.Count;
            setter(choices[currentIndex]);
            settingText.text = choices[currentIndex].ToString();
        });

        rightButton.onClick.AddListener(() =>
        {
            var currentIndex = choices.IndexOf(getter());
            currentIndex = (currentIndex + 1) % choices.Count;
            setter(choices[currentIndex]);
            settingText.text = choices[currentIndex].ToString();
        });

        settingText.text = getter().ToString();
    }

    public GameObject GetLeftButton()
    {
        return leftButton.gameObject;
    }
}