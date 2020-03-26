using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Menu
{
    public class UIManager : MonoBehaviour
    {
        [Header("MainMenu")] [SerializeField] private GameObject _mainMenu;
        [SerializeField] private Button _playMenuButton;
        [SerializeField] private Button _exitButton;

        [Header("PlayMenu")] [SerializeField] private GameObject _playMenu;
        [SerializeField] private TMP_InputField _name;
        [SerializeField] private TMP_InputField _seed;
        [SerializeField] private Button _playButton;
        

        #region UnityEvents

        private void Awake()
        {
            _playMenuButton.onClick.AddListener(OnPlayMenuButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (_playMenu.activeSelf)
                {
                    SwitchMenus();
                }
            }
        }

        #endregion


        #region Click Events

        private static void OnExitButtonClick() => Application.Quit();

        private void OnPlayMenuButtonClick()
        {
            SwitchMenus();
        }

        private void OnPlayButtonClick()
        {
            SceneManager.LoadScene(1);
        }


        #endregion
       
        private void SwitchMenus()
        {
            _playMenu.SetActive(!_playMenu.activeSelf);
            _mainMenu.SetActive(!_mainMenu.activeSelf);
        }
    }
}