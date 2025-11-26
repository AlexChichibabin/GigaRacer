using UnityEngine;
using UnityEngine.UI;

namespace Racing
{
    public class UIExitButton : UISelectableButton
    {
        [SerializeField] private Text questionText;
        [SerializeField] private GameObject selectText;
        [SerializeField] private GameObject exitConfirmPanel;
        [SerializeField] private string[] questionString = new string[]
        {
            "Are you sure?",
            "Really?"
        };
        private int yesCount = 0;
        private void Start()
        {
            questionText.text = questionString[yesCount];
        }
        public void No()
        {
            ResetQuestion();
            SetUnfocus();
        }
        public void Yes()
        {
            if (yesCount == questionString.Length - 1) ExitGame();
            else NextText();
        }
        private void NextText()
        {
            yesCount++;
            questionText.text = questionString[yesCount];
        }
        private void ExitGame()
        {
            Application.Quit();
        }
        private void ResetQuestion()
        {
            yesCount = 0;
            questionText.text = questionString[yesCount];
        }
        /*public override void SetFocus()
        {
            base.SetFocus();
            OnExitSelect();
        }
        public override void SetUnfocus()
        {
            base.SetUnfocus();
            OnExitUnSelect();
        }*/

        public void OnExitSelect()
        {
            selectText.SetActive(false);
            exitConfirmPanel.SetActive(true);
        }
        public void OnExitUnSelect()
        {
            selectText.SetActive(true);
            exitConfirmPanel.SetActive(false);
            ResetQuestion();
        }
    }
}