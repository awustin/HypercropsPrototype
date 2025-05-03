using UnityEngine;

using Assets.Hypercrops.System.CommonSerializable;
using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.System.Managers
{
    public class FarmManager : MonoBehaviour
    {
        private static FarmManager _instance;
        public static FarmManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<FarmManager>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new("FarmManager");
                        _instance = singletonObject.AddComponent<FarmManager>();
                    }
                }

                return _instance;
            }
        }

        [HideInInspector] public GameObject CurrentCrop;
        [HideInInspector] public ObjectFactory Factory;

        private readonly ObjectCache<GameObject> _plantedCache = new();

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                name = "FarmManager";
                Factory = ObjectFactory.Instance;
            }
        }

        public void StartCrop(CropDescriptor cropDescriptor, Vector3 position)
        {
            // TODO: Redefine key for crops. Does it make sense to keep a dictionary?
            // I could just use the children list and use a custom Equals method in Crop to find the one I need
            string key = FarmUtils.PositionToKey(position);

            GameObject currentCrop = _plantedCache
                .Entry(key)
                .LoadOnMiss
                (
                    () =>
                    {
                        return Factory.MakeCrop(cropDescriptor, position, transform);
                    }
                );

            CurrentCrop = currentCrop;
        }

        public void DiscardCurrentCrop()
        {
            CurrentCrop = null;
        }

        public bool IsPlantInPosition(Vector3 position)
        {
            return _plantedCache.Entry(FarmUtils.PositionToKey(position)).IsHit;
        }

        public void KillCrop(GameObject cropTarget)
        {
            string key = FarmUtils.PositionToKey(cropTarget.transform.position);

            GameObject removed = _plantedCache.Entry(key).Delete();
            Destroy(removed);
        }
    }
}