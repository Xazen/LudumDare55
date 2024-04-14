﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseContainer;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button restartButton;

        public Action<bool> OnPaused;
        public bool IsPaused { get; set; }

        private void Awake()
        {
            Singletons.RegisterPauseMenu(this);
        }

        private void Start()
        {
            continueButton.onClick.AddListener(ContinueGame);
            menuButton.onClick.AddListener(OpenMenu);
            restartButton.onClick.AddListener(RestartGame);
            pauseContainer.SetActive(false);

            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        }

        private void RestartGame()
        {
            GlobalSettings.PendingRestart = true;
            SceneManager.LoadScene("Game");
        }

        private void OpenMenu()
        {
            Singletons.AudioManager.ResumeMusic();
            Singletons.AudioManager.ToMenu();
            SceneManager.LoadScene("MainMenu");
        }

        private void Update()
        {
            if (Singletons.GameOver.IsOpen || GlobalSettings.BlockInput)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseContainer.SetActive(!pauseContainer.activeSelf);
                if (pauseContainer.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
                }
                IsPaused = pauseContainer.activeSelf;
                OnPaused?.Invoke(pauseContainer.activeSelf);
            }

            if (IsPaused)
            {
                if (!EventSystem.current.currentSelectedGameObject)
                {
                    EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
                }
            }
        }

        private void ContinueGame()
        {
            pauseContainer.SetActive(false);
            OnPaused?.Invoke(false);
        }
    }
}