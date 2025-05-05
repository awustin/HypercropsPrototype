using UnityEngine;
using UnityEngine.InputSystem;

using Assets.Hypercrops.Model.Crops;
using Assets.Hypercrops.Model.Utils;

namespace Assets.Hypercrops.Model.Ghosts
{
    public class CropGhost : MonoBehaviour
    {
        public InputAction ScreenPointAction;
        public Camera MainCamera;
        public LayerMask ActionLayer;
        public FarmManager Farm;
        public GameObject Visuals;
        public bool IsAllowed;
        public Vector3 ActionPoint;
        
        // Trackers
        private bool _isAllowedTracker;
        private Vector2 _screenPointTracker;

        void Start()
        {
            Farm = GameObject.Find("FarmManager").GetComponent<FarmManager>();
            MainCamera = Camera.main;
            ScreenPointAction = InputSystem.actions.FindActionMap("Player").FindAction("ScreenPoint");
            ActionLayer = LayerMask.GetMask("Default");

            SetGhostColor();
        }

        void Update()
        {
            TrackVariables();
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
            MeshRenderer renderer = Visuals.GetComponent<MeshRenderer>();
            string colorHex = IsAllowed ? "#00FF65" : "#FF5000";

            if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
            {
                color.a = 0.3f;
                renderer.material.color = color;
            }
        }

        protected void FollowPointer()
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
                    IsPointActionable(snapped, targetObject)
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
        protected virtual bool IsPointActionable(Vector3 target, GameObject? raycastHitObject)
        {
            return Farm.IsPlantablePoint(target, raycastHitObject);
        }
    }
}
