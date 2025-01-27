using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using Code.Scripts.Internal;

namespace Code.Scripts.LoadingSystem
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        [SerializeField] private SceneField _buildScene;

        public SceneBuilder LoadingBuilder { get; set; }

        private SceneField _currentScene;
        private SceneField _loadingScene;

        // DEBUG
        private float _minimumLoadTime = 3f;

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Load new scene parallel"))
            {
                StartCoroutine(LoadSceneCoroutine(_buildScene, true));
            }
        }

        private IEnumerator LoadSceneCoroutine(SceneField scene, bool loadOnParallel)
        {
            yield return LoadSceneAsyncRoutine(scene);

            _loadingScene = scene;
            _loadingScene.Scene.GetRootGameObjects();

            if (loadOnParallel)
            {
                while (!LoadingBuilder) yield return null;

                SceneManager.SetActiveScene(_loadingScene);

                yield return LoadingBuilder.Build();
            }

            if (_currentScene != null)
                yield return UnloadSceneAsyncRoutine(_currentScene);

            _currentScene = _loadingScene;
        }

        private IEnumerator LoadSceneAsyncRoutine(SceneField scene)
        {
            float minimumTimer = 0f;
            AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

            if (asyncLoadOperation == null)
                throw new NullReferenceException($"LoadSceneAsync error: {scene} scene is null.");

            asyncLoadOperation.allowSceneActivation = false;

            while (asyncLoadOperation.progress < 0.9f && minimumTimer < _minimumLoadTime)
            {
                minimumTimer += Time.deltaTime;

                yield return null;
            }

            asyncLoadOperation.allowSceneActivation = true;
        }

        private IEnumerator UnloadSceneAsyncRoutine(SceneField scene)
        {
            AsyncOperation asyncUnloadOperation = SceneManager.UnloadSceneAsync(scene.Scene);

            if (asyncUnloadOperation == null)
                throw new NullReferenceException($"UnloadSceneAsync error: {scene} scene is null.");

            while (!asyncUnloadOperation.isDone)
            {
                yield return null;
            }
        }
    }
}
