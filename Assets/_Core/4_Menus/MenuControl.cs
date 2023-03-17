using UnityEngine;
using UnityEngine.EventSystems;

namespace _Core._4_Menus
{
    public class MenuControl : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuFirstButton, optionsMenuFirstButton, optionsMenuClosedButton;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
        }

        public void EnteringOptions()
        {
            EventSystem.current.SetSelectedGameObject(optionsMenuFirstButton);
        }

        public void LeavingOptions()
        {
            EventSystem.current.SetSelectedGameObject(optionsMenuClosedButton);
        }
    }
}