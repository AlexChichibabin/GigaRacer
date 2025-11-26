using UnityEngine;
using UnityEngine.Events;

namespace SpaceShip
{
    public interface ILevelCondition
    {
        bool IsCompleted { get; }
    }
    public class LevelController : MonoBehaviour, IDependency<LevelSequenceController>
    {
        [SerializeField] protected float m_ReferenceTime;
        public float ReferenceTime => m_ReferenceTime;

        [SerializeField] protected UnityEvent m_EventLevelCompleted;

        private ILevelCondition[] m_Conditions;
        private LevelSequenceController levelSequenceController;
        public void Construct(LevelSequenceController obj) => levelSequenceController = obj;
        private bool m_IsLevelCompleted;

        private float m_LevelTime;
        public float LevelTime => m_LevelTime;

        protected void Start()
        {
            m_Conditions = GetComponentsInChildren<ILevelCondition>();
            m_LevelTime = 0;
        }

        private void Update()
        {
            if (!m_IsLevelCompleted)
            {
                m_LevelTime += Time.deltaTime;

                CheckLevelConditions();
            }
        }

        private void CheckLevelConditions()
        {
            if (m_Conditions == null || m_Conditions.Length == 0) return;

            int numCompleted = 0;

            foreach (var v in m_Conditions)
            {
                if (v.IsCompleted) numCompleted++;
            }

            if (numCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;
                m_EventLevelCompleted?.Invoke();

                levelSequenceController?.FinishCurrentLevel(true);
            }
        }
    }
}