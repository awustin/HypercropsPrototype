using UnityEngine;

using Assets.Hypercrops.Gameplay;

namespace Assets.Hypercrops.Scene
{
    public class MainSceneManager : MonoBehaviour
    {
        private static MainSceneManager _instance;
        public static MainSceneManager Instance
        {
            get
            {
                Instantiate();
                return _instance;
            }
        }

        public SingleRoundManager RoundManager;

        public void Start()
        {
            if (RoundManager == null)
                RoundManager = GameObject.Find("SingleRoundManager").GetComponent<SingleRoundManager>();

            RoundManager.StartSingleRound();
        }

        private static void Instantiate()
        {
            _instance = _instance != null ? _instance : FindFirstObjectByType<MainSceneManager>();

            if (_instance != null)
            {
                return;
            }

            GameObject singletonObject = new("MainSceneManager");
            _instance = singletonObject.AddComponent<MainSceneManager>();
        }
    }
}