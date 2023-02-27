using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheFrozenBanana
{
    public class GameController : MonoBehaviour
    {
        public static GameController gc;

        [SerializeField] private string MenuSceneName;
        [SerializeField] private string LevelSceneName;
        [SerializeField] private string GameOverSceneName;
        [SerializeField] private GameObject Canvas;
        [SerializeField] private Image FadeScreen;
        [SerializeField] private Image PauseScreen;
        [SerializeField] private float SceneTransitionTime;
        private bool isPaused;

		private void Awake() {
            if (gc == null) {
                gc = this.gameObject.GetComponent<GameController>();
			} else {
                Destroy(this.gameObject);
			}
    		DontDestroyOnLoad(this.gameObject);
    		DontDestroyOnLoad(Canvas);
            StartCoroutine(TransitionToScene(MenuSceneName));
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.P)) {
                PauseGame();
			}
        }

		public void StartLevel() {
            Debug.Log("Start level");
            StartCoroutine(TransitionToScene(LevelSceneName));
		}

        private IEnumerator TransitionToScene(string sceneName) {

            StartCoroutine(AlphaFadeScreen(1f));
            EnableFadeScreen(true);
            yield return new WaitForSeconds(SceneTransitionTime);

            LoadScene(sceneName);

            yield return new WaitForSeconds(SceneTransitionTime);
            StartCoroutine(AlphaFadeScreen(0f));
            yield return new WaitForSeconds(SceneTransitionTime);
            EnableFadeScreen(false);
        }

        private IEnumerator AlphaFadeScreen(float alpha) {
            float t = 0;
            while (t < SceneTransitionTime) {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
                float lerp = t / SceneTransitionTime;
                float a = Mathf.Lerp(1 - alpha, alpha, lerp);
                ColorFadeScreen(a);
			}
            ColorFadeScreen(alpha);
		}

        private void EnableFadeScreen(bool opt) {
            FadeScreen.enabled = opt;
		}

        private void LoadScene(string sceneName) {
            try {
                SceneManager.LoadSceneAsync(sceneName);
            } catch {
                Debug.LogError("Game Manager: Unable to load scene " + sceneName);
            }
        }

        private void ColorFadeScreen(float alpha) {
            FadeScreen.color = new Color(255,255,255,alpha);
		}

        private void PauseGame() {
            isPaused = !isPaused;
            if (isPaused) {
                Time.timeScale = 0f;
                PauseScreen.enabled = true;
			} else {
                Time.timeScale = 1f;
                PauseScreen.enabled = false;
            }
        }
    }
}
