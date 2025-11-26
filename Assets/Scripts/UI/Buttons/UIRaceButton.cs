using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Racing
{
    public class UIRaceButton : UISelectableButton, IScriptableObjectProperty, IDependency<LevelSequenceController>, IDependency<MapCompletion>
    {
        [SerializeField] private RaceInfo raceInfo;
        [SerializeField] private Image icon;
        [SerializeField] private Text title;
        [SerializeField] private Image strokeImage;
        [SerializeField] private Text scoreText;

        
        private RaceLevel raceLevel;
        private MapCompletion mapCompletion;

        private LevelSequenceController levelSequenceController;
        public void Construct(MapCompletion obj) => mapCompletion = obj;
        public void Construct(LevelSequenceController obj) => levelSequenceController = obj;

        private void Start()
        {
            ApplyProperty(raceInfo);
            raceLevel = GetComponent<RaceLevel>();
            UpdateScore();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            levelSequenceController.StartRace(raceLevel);
        }
        public void ApplyProperty(ScriptableObject property)
        {
            if (property == null) return;
            if (property is RaceInfo == false) return;

            raceInfo = property as RaceInfo;

            icon.sprite = raceInfo.Icon;
            title.text = raceInfo.Title;
            strokeImage.color = raceInfo.StrokeColor;
        }
        public void UpdateScore()
        {
            scoreText.text = $"{mapCompletion.GetEpisodeScore(raceInfo)}/3";
            Debug.Log(mapCompletion.GetEpisodeScore(raceInfo));
        }
    }
}