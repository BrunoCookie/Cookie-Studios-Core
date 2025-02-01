using UnityEngine;
using UnityEngine.Events;

namespace UI.Scripts.Menu
{
    public class Menu : MonoBehaviour
    {
        public GameObject firstSelected;
        public UnityEvent onOpenMenu;
        public UnityEvent onCloseMenu;
        
        public virtual void OpenMenu()
        {
            onOpenMenu.Invoke();
            MenuManager.instance.SetSelectedElement(firstSelected);
        }

        public virtual void CloseMenu()
        {
            onCloseMenu.Invoke();
        }

    }
}
