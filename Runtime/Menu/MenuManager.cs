using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace UI.Scripts.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;
        public Menu pauseMenu;
        private static Menu currentMenu;

        private VolumeProfile currentProfile;
        [HideInInspector]
        public bool canTriggerPause = true;
        public static bool isPaused;

        public event Action OnPauseMenuOpened;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            InputManager.playerInput.onControlsChanged += (input) =>
            {
                if (currentMenu) SetSelectedElement(currentMenu.firstSelected);
            };

            InputManager.controls.UIControl.Close.performed += ctx => CloseMenu();
        }

        private void Update()
        {
            if (InputManager.controls.UIControl.Pause.triggered)
            {
                if(currentMenu == null && canTriggerPause)
                {
                    OpenMenu(pauseMenu);
                    OnPauseMenuOpened?.Invoke();
                }
                else
                {
                    CloseMenu();
                }
            }

            isPaused = currentMenu == pauseMenu;
        }

        public void OpenMenu(Menu menu)
        {
            if (currentMenu == menu) return;
            if (currentMenu != null) CloseMenu();
            currentProfile = VolumeManager.GetCurrentProfile();
            VolumeManager.SetVolumeProfile(VolumeManager.menuProfile);
            Cursor.visible = true;
            currentMenu = menu;
            currentMenu.gameObject.SetActive(true);
            currentMenu.OpenMenu();
        }

        public void CloseMenu()
        {
            if (currentMenu == null) return;
            VolumeManager.SetVolumeProfile(currentProfile);
            Cursor.visible = false;
            currentMenu.CloseMenu();
            currentMenu.gameObject.SetActive(false);
            currentMenu = null;
        }

        public void SetCanPause(bool _canPause)
        {
            canTriggerPause = _canPause;
        }

        public void SetSelectedElement(GameObject element)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if(element && InputManager.IsControllerActive()) EventSystem.current.SetSelectedGameObject(element);
        }
    }
}
