using UnityEngine;
using TMPro;

namespace Assets.Hypercrops.Model.MapEntities
{
    public class MapEntitiesManager : MonoBehaviour
    {
        private static MapEntitiesManager _instance;
        public static MapEntitiesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<MapEntitiesManager>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new("MapEntitiesManager");
                        _instance = singletonObject.AddComponent<MapEntitiesManager>();
                    }
                }

                return _instance;
            }
        }

        public GameObject MapEntityMessageObject;

        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void ShowMessage(string mapEntityName)
        {
            if (mapEntityName != null)
            {
                MapEntityMessageObject.SetActive(true);
                MapEntityMessageObject.transform.Find("Canvas/Traits/Message/Value").gameObject.GetComponent<TMP_Text>().text = mapEntityName;
            }

        }
    }
}