using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));
        }
        
        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                UpdateLight();
                onLoaded?.Invoke();
                yield break;
            }
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);

            while (!waitNextScene.isDone)
                yield return null;

            UpdateLight();
            onLoaded?.Invoke();
        }

        private void UpdateLight()
        {
            LightProbes.Tetrahedralize();
            DynamicGI.UpdateEnvironment();
        }
    }
}