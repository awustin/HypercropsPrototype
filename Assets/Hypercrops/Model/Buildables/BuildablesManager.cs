using UnityEngine;

using Assets.Hypercrops.System;
using Assets.Hypercrops.Common.Serializables;
using Assets.Hypercrops.Model.Utils;
using Assets.Hypercrops.Common;

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
        public ObjectFactory Factory;
        public GameObject Player;
        public float BuildRadius = 5f;
        public GameObject BuildableMap;

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

                if (Factory == null)
                {
                    Factory = ObjectFactory.Instance;     
                }
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

        public void PlaceBuildable(Vector3 point, BuildableDescriptor descriptor)
        {
            // TODO: Redefine key for buildables. Does it make sense to keep a dictionary?
            _buildablesInSceneLookup
                .Entry(HypercropsModelUtils.VectorPositionToStringKey(point))
                .LoadOnMiss
                (
                    () =>
                    {
                        GameObject instanced = Factory.HypercropsInstance<Buildable>
                        (
                            descriptor.Type.ToString(),
                            point,
                            BuildableMap.transform
                        );

                        instanced.name = descriptor.Type.ToString();
                        Buildable behaviour = instanced.GetComponent<Buildable>();
                        BuildableEffect effectBehaviour = instanced.GetComponent<BuildableEffect>();

                        behaviour.Initialise(descriptor.Type, descriptor.Description);
                        effectBehaviour.Initialise(
                            descriptor.Effect.Type,
                            descriptor.Effect.Name,
                            descriptor.Effect.Period,
                            descriptor.Effect.Radius,
                            descriptor.Effect.Description
                        );

                        return instanced;
                    }
                );
        }
    }
}