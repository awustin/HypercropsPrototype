using UnityEngine;
using UnityEngine.InputSystem;

using Assets.Hypercrops.System;
using Assets.Hypercrops.Model.Utils;

namespace Assets.Hypercrops.Model.Buildables
{
    // TODO: Make this Ghost class GENERAL and be able to change the visuals based on the buildable layout. Never destroy
    public class BuildableGhost : MonoBehaviour
    {
        public ObjectFactory Factory;
        public InputAction ScreenPointAction;
        public Camera MainCamera;
        public LayerMask BuildableLayer;
        public GameObject Player;
        public bool IsAllowed;
        public Vector3 ActionPoint;
        public float PlantRadius = 10f;
        
        private GameObject _currentVisual;
        private readonly ObjectCache<GameObject> _ghostVisualsLookup = new ();

        // Trackers
        private bool _isAllowedTracker;
        private Vector2 _screenPointTracker;

        void Start()
        {
            Factory = ObjectFactory.Instance;
            MainCamera = Camera.main;
            ScreenPointAction = InputSystem.actions.FindActionMap("Player").FindAction("ScreenPoint");
            BuildableLayer = LayerMask.GetMask("Default");
            Player = GameObject.Find("Player");

            gameObject.SetActive(false);
            SetGhostColor();
        }

        void Update()
        {
            if (gameObject.activeSelf)
            {
                TrackVariables();
            }
        }

        public void Activate(BuildableLayoutType layoutType)
        {
            if (_currentVisual != null)
            {
                _currentVisual.SetActive(false);
            }

            _currentVisual = _ghostVisualsLookup
                .Entry(layoutType.ToString())
                .LoadOnMiss
                (
                    () =>
                    {
                        GameObject instance =
                            Factory.HypercropsInstance<BuildableGhost>(layoutType.ToString(), transform.position, transform);

                        instance.name = $"GhostLayout{layoutType}";
                        return instance;
                    }
                );

            _currentVisual.SetActive(true);
            gameObject.SetActive(true);
            SetGhostColor();
        }

        public void Deactivate()
        {
            _currentVisual.SetActive(false);
            _currentVisual = null;
            gameObject.SetActive(false);
        }

        private void TrackVariables()
        {
            if (_isAllowedTracker != IsAllowed)
            {
                _isAllowedTracker = IsAllowed;
                SetGhostColor();
            }

            Vector2 screenPoint = ScreenPointAction.ReadValue<Vector2>();

            if (_screenPointTracker != screenPoint)
            {
                _screenPointTracker = screenPoint;
                FollowPointer();
            }
        }

        private void SetGhostColor()
        {
            if (!gameObject.activeSelf || _currentVisual == null)
            {
                return;
            }

            MeshRenderer renderer = _currentVisual.transform.Find("Visuals/Prefab").GetComponent<MeshRenderer>();
            string colorHex = IsAllowed ? "#00FF65" : "#FF5000";

            if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
            {
                color.a = 0.3f;
                renderer.material.color = color;
            }
        }

        private void FollowPointer()
        {
            Vector2 screenPoint = ScreenPointAction.ReadValue<Vector2>();
            Vector2 viewportPoint = MainCamera.ScreenToViewportPoint(screenPoint);
            Ray ray = MainCamera.ViewportPointToRay(viewportPoint);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, BuildableLayer))
            {
                Vector3 snapped = HypercropsModelUtils.SnapPoint(hit.point);
                // SetPosition(snapped);

                // if (!IsPlantableAtPosition(snapped, hit.collider.gameObject))
                // {
                //     IsAllowed = false;

                //     return;
                // }

                // IsAllowed = true;
                // ActionPoint = snapped;
            }
        }

        private void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        // private bool IsPlantableAtPosition(Vector3 pos, GameObject target)
        // {
        //     return !Farm.IsPlantInPosition(pos) &&
        //         Farm.IsPlantablePoint(pos, Player, target, PlantRadius);
        // }

    }
}