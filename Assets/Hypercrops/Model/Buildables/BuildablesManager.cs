using UnityEngine;

namespace Assets.Hypercrops.Model.Buildables
{
    public class BuildablesManager : MonoBehaviour
    {
        private static BuildablesManager _instance;
        public static BuildablesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<BuildablesManager>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new("BuildablesManager");
                        _instance = singletonObject.AddComponent<BuildablesManager>();
                    }
                }

                return _instance;
            }
        }

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                name = "BuildablesManager";
            }
        }

    }
}