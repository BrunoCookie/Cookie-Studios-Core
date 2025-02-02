using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        private static Menu currentMenu;
        public static MenuManager instance;
        
        public Menu pauseMenu;
        public InputActionReference pauseAction;

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
            PlayerInput.GetPlayerByIndex(0).onControlsChanged += (input) =>
            {
                if (currentMenu) SetSelectedElement(currentMenu.firstSelected);
            };
        }

        private void Update()
        {
            if (pauseAction.action.triggered)
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
            Cursor.visible = true;
            currentMenu = menu;
            currentMenu.gameObject.SetActive(true);
            currentMenu.OpenMenu();
        }

        public void CloseMenu()
        {
            if (currentMenu == null) return;
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
            if(element && PlayerInput.GetPlayerByIndex(0).currentControlScheme == "Gamepad") //TODO Check if works
                EventSystem.current.SetSelectedGameObject(element);
        }
    }
}
