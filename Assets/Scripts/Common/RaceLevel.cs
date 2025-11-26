using UnityEngine;
using UnityEngine.UI;

namespace Racing
{
    public class RaceLevel : MonoBehaviour, IDependency<MapCompletion>, IScriptableObjectProperty
    {
        private MapCompletion mapCompletion;
        [SerializeField] private RaceInfo m_Race;
        [SerializeField] private Text m_NameText;
        private int m_StarsAmount;
        [SerializeField] private GameObject hidingObject;
        public RaceInfo Race => m_Race;
        public void Construct(MapCompletion obj) => mapCompletion = obj;
        //[SerializeField] private LevelVisualScores m_VisualScores;
        //[SerializeField] private MapSceneAnimation m_MapAnimation;
        public bool IsComplete { get { return gameObject.activeSelf && m_StarsAmount > 0; } }
        private void Awake()
        {
            //m_VisualScores = GetComponentInChildren<LevelVisualScores>();
            //hidingObject = transform.GetChild(0).gameObject;
        }
        private void Start()
        {
            ApplyProperty(m_Race);
            Initialize();
            //Debug.Log(m_StarsAmount);
        }
        public void AnimationBeforeLoadLevel()
        {
            //m_MapAnimation.AnimationOnLoad(m_Episode);
        }
        public void Initialize()
        {
            m_StarsAmount = mapCompletion.GetEpisodeScore(m_Race);
            //Debug.Log(m_StarsAmount);
            //m_VisualScores.SetStars(m_StarsAmount); TODO
        }
        public int GetStarsAmount() => m_StarsAmount; // Не нужен ли return?
        public void ApplyProperty(ScriptableObject property)
        {
            if (property == null) return;
            if (property is RaceInfo == false) return;

            m_Race = property as RaceInfo;
            m_NameText.text = m_Race.Title;
        }
        public GameObject GetHidingObject() => hidingObject;
    }
}