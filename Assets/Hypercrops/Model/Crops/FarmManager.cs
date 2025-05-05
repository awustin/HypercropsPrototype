using UnityEngine;

using Assets.Hypercrops.System;
using Assets.Hypercrops.System.CommonSerializable;
using Assets.Hypercrops.Model.Utils;

namespace Assets.Hypercrops.Model.Crops
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
        public GameObject Player;
        public float PlantRadius = 10f;

        private readonly ObjectCache<GameObject> _plantedCropsLookup = new();

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

                if (Player == null)
                {
                    Player = GameObject.Find("Player");
                }
            }
        }

        public bool IsPlantablePoint(Vector3 target)
        {
            if (_plantedCropsLookup.Entry(HypercropsModelUtils.VectorPositionToStringKey(target)).IsHit)
            {
                return false;
            }

            // Is contained in the plant radius from player
            if (!VectorUtils.IsInSphere(target, Player.transform.position, PlantRadius))
            {
                return false;
            }

            return true;
        }

        public void StartCrop(CropDescriptor cropDescriptor, Vector3 position)
        {
            // TODO: Redefine key for crops. Does it make sense to keep a dictionary?
            // I could just use the children list and use a custom Equals method in Crop to find the one I need
            string key = HypercropsModelUtils.VectorPositionToStringKey(position);

            GameObject currentCrop = _plantedCropsLookup
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

        public void KillCrop(GameObject cropTarget)
        {
            string key = HypercropsModelUtils.VectorPositionToStringKey(cropTarget.transform.position);

            GameObject removed = _plantedCropsLookup.Entry(key).Delete();
            Destroy(removed);
        }
    }
}