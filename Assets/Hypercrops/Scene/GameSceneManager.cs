using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Hypercrops.Scene
{
    public class GameSceneManager : MonoBehaviour
    {
        public GameObject FadePanel;
        public float FadeDuration = 1f;
        public Color FadeColor = Color.black;
        public string SceneData = "TestData";
        public static GameSceneManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            FadePanel.SetActive(false);
            FadePanel.GetComponent<UnityEngine.UI.Image>().color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, 0f);
        }

        // Called to load the same scene again
        public void RestartScene()
        {
            StartCoroutine(FadeOutAndRestart());
        }

        private IEnumerator FadeOutAndRestart()
        {
            if (!FadePanel)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                yield break;
            }

            FadePanel.SetActive(true);
            UnityEngine.UI.Image image = FadePanel.GetComponent<UnityEngine.UI.Image>();

            float elapsedTime = 0f;
            Color startColor = image.color;
            Color targetColor = new(FadeColor.r, FadeColor.g, FadeColor.b, 1f);

            while (elapsedTime < FadeDuration)
            {
                image.color = Color.Lerp(startColor, targetColor, elapsedTime / FadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            image.color = targetColor;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            // Scene restarts are handled now.
            // Data will be restored during the `Start()` method in objects that persist.
        }

        public void SetSceneData(string value)
        {
            SceneData = value;
        }
    }
}
