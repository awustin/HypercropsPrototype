using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using Assets.Hypercrops.Events;

namespace Assets.Hypercrops.Scene
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance
        {
            get
            {
                Instantiate();
                return _instance;
            }
        }
        public GameObject FadePanel;
        public float FadeDuration = 1f;
        public Color FadeColor = Color.black;
        public string SceneData = "TestData";
        public GameEventSender Sender;
        public ScoreManager Score;

        private static GameSceneManager _instance;

        void OnEnable()
        {
            if (Sender == null)
                Sender = GameEventSender.Instance;

            if (Score == null)
                Score = ScoreManager.Instance;

            Sender.RestartScene += RestartScene;
            Sender.EndScene += EndScene;

            FadePanel.SetActive(false);
            FadePanel.GetComponent<UnityEngine.UI.Image>().color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, 0f);
        }

        void OnDisable()
        {
            Sender.RestartScene -= RestartScene;
            Sender.EndScene -= EndScene;
        }

        public void SetSceneData(string value)
        {
            SceneData = value;
        }

        private static void Instantiate()
        {
            _instance = _instance != null ? _instance : FindFirstObjectByType<GameSceneManager>();

            if (_instance != null)
            {
                return;
            }

            GameObject singletonObject = new("GameSceneManager");
            _instance = singletonObject.AddComponent<GameSceneManager>();
        }

        private void RestartScene()
        {
            StartCoroutine(ReloadMain());
        }

        private void EndScene()
        {
            // TODO: Implement - tasks that run at the end, scene data storage, score calculation, clean dead crops, damage buildings, etc
        }

        private IEnumerator ReloadMain()
        {
            yield return FadeOut();
            yield return SceneManager.UnloadSceneAsync("Main");
            yield return SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
            yield return FadeIn();

            // Scene restarts are handled now.
            // Data will be restored during the `Start()` method in objects that persist.
        }

        private IEnumerator FadeOut()
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
        }

        private IEnumerator FadeIn()
        {
            FadePanel.SetActive(true);
            UnityEngine.UI.Image image = FadePanel.GetComponent<UnityEngine.UI.Image>();

            float waitTime = 0f;
            float elapsedTime = 0f;
            Color startColor = image.color;
            Color targetColor = new(FadeColor.r, FadeColor.g, FadeColor.b, 0f);

            while (waitTime < 1f)
            {
                waitTime += Time.deltaTime;
                yield return null;
            }

            while (elapsedTime < FadeDuration)
            {
                image.color = Color.Lerp(startColor, targetColor, elapsedTime / FadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            image.color = targetColor;
            FadePanel.SetActive(false);
        }
    }
}
