using UnityEngine;

namespace Racing
{
    public class LevelDisplayController : MonoBehaviour
    {
        [SerializeField] private RaceLevel[] levels;
        //[SerializeField] private BranchLevel[] branchLevels;

        private void Start()
        {
            UpdateDrawLevels();
            gameObject.SetActive(false);
            /*for (int i = 0; i < branchLevels.Length; i++)
            {
                branchLevels[i].TryActivate();
            }*/ 
        }
        public void UpdateDrawLevels()
        {
            var drawLevel = 0;
            int score = 1;

            while (score != 0 && drawLevel < levels.Length)
            {
                levels[drawLevel].Initialize();
                score = levels[drawLevel].GetStarsAmount();
                drawLevel++;
            }
            for (int i = drawLevel; i < levels.Length; i++)
            {
                levels[i].GetHidingObject().SetActive(false);
            }
        }
    }
}