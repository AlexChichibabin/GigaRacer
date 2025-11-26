using UnityEngine;
using UnityEngine.SceneManagement;

namespace Racing
{
    public class SceneLoader : MonoBehaviour
    {
        private const string MainMenuSceneTitle = "Main Menu";
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenuSceneTitle);
        }
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}