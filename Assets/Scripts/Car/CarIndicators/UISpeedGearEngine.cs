using UnityEngine;
using UnityEngine.UI;

namespace Racing
{
    public class UISpeedGearEngine : MonoBehaviour
    {
        [SerializeField] private Text m_SpeedText;
        [SerializeField] private Text m_SelectedGearText;
        [SerializeField] private Image m_EngineTorqueImage;

        //private float engineRpmUpdateTime;
        //[SerializeField] private float engineRpmUpdateTimer;

        public void SetSpeed(int speed)
        {
            m_SpeedText.text = $"{(int)speed}";
        }
        public void SetSelectedGear(int selectedGear)
        {
            m_SelectedGearText.text = $"{(int)selectedGear}";
        }
        public void SetEngineTorque(float engineTorque)
        {
                m_EngineTorqueImage.fillAmount = engineTorque; // Showing engine RPM on car UI
        }
    }
}
