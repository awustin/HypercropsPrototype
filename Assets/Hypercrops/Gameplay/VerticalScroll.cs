using UnityEngine;
using UnityEngine.InputSystem;

using Assets.Hypercrops.State;

namespace Assets.Hypercrops.Gameplay
{
    public class VerticalScroll : MonoBehaviour
    {
        public InputAction PointerDown;
        public InputAction Point;
        public float MaxDistance = 100f;
        public float ScrollDelta = 0.5f;
        public GameState State;
        public GameObject DraggableContainer;

        private bool _isDragging;
        private float _startScreenPoint;
        private float _startWorldPoint;

        public void Start()
        {
            enabled = false;
            _isDragging = false;

            if (State == null)
                State = GameState.Instance;
        }

        public void OnEnable()
        {
            Point.Enable();
            PointerDown.Enable();

            PointerDown.performed += OnPointerDown;
            PointerDown.canceled += OnPointerUp;
        }

        public void OnDisable()
        {
            Point.Disable();
            PointerDown.Disable();
        }

        public void Update()
        {
            if (_isDragging)
            {
                Vector3 currentPos = DraggableContainer.transform.localPosition;
                float delta = Point.ReadValue<Vector2>().y - _startScreenPoint;
                float scrollAmount = delta * ScrollDelta;
                float verticalPosition = _startWorldPoint + scrollAmount;

                verticalPosition = Mathf.Round
                (
                    Mathf.Clamp(verticalPosition, _startWorldPoint - MaxDistance, _startWorldPoint + MaxDistance)
                );

                if (verticalPosition > 0 || verticalPosition <= -MaxDistance)
                    return;

                DraggableContainer.transform.localPosition = new
                (
                    currentPos.x,
                    currentPos.y,
                    verticalPosition
                );
            }
        }

        private void OnPointerDown(InputAction.CallbackContext context)
        {
            if (State.IsUIInteraction)
                return;

            _isDragging = true;
            _startScreenPoint = Point.ReadValue<Vector2>().y;
            _startWorldPoint = DraggableContainer.transform.localPosition.z;
        }

        private void OnPointerUp(InputAction.CallbackContext context)
        {
            if (!_isDragging)
                return;

            _isDragging = false;
        }
    }
}