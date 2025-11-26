using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Racing
{
    public class GlobalDependenciesContainer : Dependency
    {
        private static GlobalDependenciesContainer instance;
        [SerializeField] private Pauser pauser;
        [SerializeField] private MapCompletion mapCompletion;
        [SerializeField] private LevelSequenceController levelSequenceController;
        
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
             
            instance = this;

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnLoadScene;
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnLoadScene;
        }
        protected override void BindAll(MonoBehaviour monoBehaviourInScene)
        {
            Bind<Pauser>(pauser, monoBehaviourInScene);
            Bind<MapCompletion>(mapCompletion, monoBehaviourInScene);
            Bind<LevelSequenceController>(levelSequenceController, monoBehaviourInScene);
        }
        private void OnLoadScene(Scene arg0, LoadSceneMode arg1)
        {
            FindAllObjectsToBind();
        }
    }
}