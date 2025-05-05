using UnityEngine;
using UnityEngine.InputSystem;

using Assets.Hypercrops.System;
using Assets.Hypercrops.Model.Utils;

namespace Assets.Hypercrops.Model.Ghosts
{
    /* GameObject structure

        |- Crop/Buildable Ghost (srcipt here)
            |- Ghost
                |- GhostCollider
                    |- Collider
                |- Visuals
                    |- Prefab
    */
    public class Ghost : MonoBehaviour
    {
        public InputAction ScreenPointAction;
        public Camera MainCamera;
        public LayerMask ActionLayer;
        public ObjectFactory Factory;
        public bool IsAllowed;
        public Vector3 ActionPoint;

        private GameObject _currentVisual;
        private readonly ObjectCache<GameObject> _ghostVisualsLookup = new ();
        
        // Trackers
        private bool _isAllowedTracker;
        private Vector2 _screenPointTracker;

        void Start()
        {
            MainCamera = Camera.main;
            ScreenPointAction = InputSystem.actions.FindActionMap("Player").FindAction("ScreenPoint");
            Factory = ObjectFactory.Instance;

            StartDerived();
        }

        void Update()
        {
            if (gameObject.activeSelf)
            {
                TrackVariables();
            }
        }

        protected void Activate<T>(string visualsKey)
        {
            ActionLayer = LayerMask.GetMask("Default");

            if (_currentVisual != null)
            {
                _currentVisual.SetActive(false);
            }

            _currentVisual = _ghostVisualsLookup
                .Entry(visualsKey)
                .LoadOnMiss
                (
                    () =>
                    {
                        GameObject instance =
                            Factory.HypercropsInstance<T>(visualsKey, transform.position, transform);

                        instance.name = $"{typeof(T).Name}:{visualsKey}";
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

            if (Physics.Raycast(ray, out RaycastHit hit, 100, ActionLayer))
            {
                Vector3 snapped = HypercropsModelUtils.SnapPoint(hit.point);
                GameObject targetObject = hit.collider.gameObject;

                transform.position = snapped;

                if
                (
                    snapped.y <= 0.1 &&
                    targetObject.CompareTag("Ground") &&
                    IsPointActionable(snapped)
                )
                {
                    IsAllowed = true;
                    ActionPoint = snapped;

                    return;
                }

                IsAllowed = false;
            }
        }

        #nullable enable
        protected virtual bool IsPointActionable(Vector3 target)
        {
            return false;
        }

        protected virtual void StartDerived()
        {
            return;
        }
    }
}