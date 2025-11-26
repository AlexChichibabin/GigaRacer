using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Racing
{
    public class UISelectableButton : UIButton
    {
        [SerializeField] protected Image[] selectedImage;

        public UnityEvent SelectEvent;
        public UnityEvent UnselectEvent;

        private void Start()
        {
            HideStartSelection();
        }
        protected void HideStartSelection()
        {
            for (int i = 0; i < selectedImage.Length; i++)
            {
                selectedImage[i].enabled = true;
            }
        }
        public override void SetFocus()
        {
            base.SetFocus();

            for (int i = 0; i < selectedImage.Length; i++)
            {
                selectedImage[i].enabled = false;
            }
            SelectEvent?.Invoke();
        }
        public override void SetUnfocus()
        {
            base.SetUnfocus();

            for (int i = 0; i < selectedImage.Length; i++)
            {
                selectedImage[i].enabled = true;
            }
            UnselectEvent?.Invoke();
        }
    }
}