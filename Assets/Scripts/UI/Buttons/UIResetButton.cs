using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Racing
{
    public class UIResetButton : UISelectableButton, IDependency<MapCompletion>
    {
        [SerializeField] private GameObject resetConfirmPanel;
        [SerializeField] private GameObject racePanel;

        LevelDisplayController[] levelDisplayControllers;

        private MapCompletion mapCompletion;
        public void Construct(MapCompletion obj) => mapCompletion = obj;

        private void Start()
        {
            GetLevelDisplayControllersOnStart();
        }
        public void No()
        {
            resetConfirmPanel.SetActive(false);
            SetUnfocus();
        }
        public void Yes()
        {
            mapCompletion.Reset();
            UpdateRaceButtons();
            resetConfirmPanel.SetActive(false);
            SetUnfocus();
        }
        private void UpdateRaceButtons()
        {
            for (int i = 0; i < levelDisplayControllers.Length; i++)
            {
                UIRaceButton[] raceButtons = levelDisplayControllers[i].GetComponent<SpawnObjectByPropertiesList>().Parent.GetComponentsInChildren<UIRaceButton>();
                foreach (var button in raceButtons) button.UpdateScore();
                levelDisplayControllers[i].UpdateDrawLevels();
            }
        }
        private void GetLevelDisplayControllersOnStart()
        {
            levelDisplayControllers = new LevelDisplayController[racePanel.transform.childCount];

            for (int i = 0; i < levelDisplayControllers.Length; i++)
            {
                levelDisplayControllers[i] = racePanel.transform.GetChild(i).GetComponent<LevelDisplayController>();
            }
        }
    }
}