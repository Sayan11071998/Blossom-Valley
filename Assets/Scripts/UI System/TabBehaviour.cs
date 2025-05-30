using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlossomValley.UISystem
{
    public class TabBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite selected;
        [SerializeField] private Sprite hover;
        [SerializeField] UIManager.Tab windowToOpen;

        private Image tabImage;

        public static UnityEvent onTabStateChange = new UnityEvent();

        private void Awake()
        {
            tabImage = GetComponent<Image>();
            onTabStateChange.AddListener(RenderTabState);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onTabStateChange?.Invoke();
            tabImage.sprite = selected;
            UIManager.Instance.OpenWindow(windowToOpen);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onTabStateChange?.Invoke();
            tabImage.sprite = hover;
        }

        public void OnPointerExit(PointerEventData eventData) => onTabStateChange?.Invoke();

        void RenderTabState()
        {
            if (UIManager.Instance.selectedTab == windowToOpen)
            {
                tabImage.sprite = selected;
                return;
            }
            tabImage.sprite = defaultSprite;
        }
    }
}