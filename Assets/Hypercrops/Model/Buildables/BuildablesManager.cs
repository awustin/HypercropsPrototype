using UnityEngine;

using Assets.Hypercrops.System;
using Assets.Hypercrops.Model.Utils;

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
        public GameObject Player;
        public float BuildRadius = 5f;

        private readonly ObjectCache<GameObject> _buildablesInSceneLookup = new ();

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

        public bool IsBuildablePoint(Vector3 point)
        {
            if (_buildablesInSceneLookup.Entry(HypercropsModelUtils.VectorPositionToStringKey(point)).IsHit)
            {
                return false;
            }

            // Is contained in the plant radius from player
            if (!VectorUtils.IsInSphere(point, Player.transform.position, BuildRadius))
            {
                return false;
            }

            return true;

        }

    }
}