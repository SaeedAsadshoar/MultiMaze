using Services.Toast.Service;
using Services.UISystem.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Toast.UI
{
    public class UIToast : EachUIElement<UIToast>
    {
        private const string COME_IN_ANIMATION = "ToastComeIn";
        private const string GET_OUT_ANIMATION = "ToastGetOut";

        [SerializeField] private TextMeshProUGUI _messageLabel;
        [SerializeField] private TextMeshProUGUI _titleLabel;
        [SerializeField] private Image _messageThumb;

        private ToastService.EachToastMessage _message;


        public Animation _animation;

        public ToastService.EachToastMessage Message => _message;

        public void Show(ToastService.EachToastMessage message, Transform toastParent)
        {
            _message = message;

            _messageLabel.text = _message.Message;
            _titleLabel.text = _message.Title;
            _messageThumb.sprite = _message.Thumb;

            ObjectRoot.SetParent(toastParent);
            ObjectRoot.localPosition = Vector3.zero;
            ObjectRoot.localScale = Vector3.one;
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.one;
            rectTransform.sizeDelta = new Vector2(0, 100);

            _animation.Play(COME_IN_ANIMATION);
        }

        public void Close()
        {
            _animation.Play(GET_OUT_ANIMATION);
            Invoke(nameof(BackToPool), 1);
        }
    }
}