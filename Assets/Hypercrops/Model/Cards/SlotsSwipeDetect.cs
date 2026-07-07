using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using Assets.Hypercrops.Events;
using Assets.Hypercrops.State;

namespace Assets.Hypercrops.Model.Cards
{
    public delegate void SwipeHandler();

    public class SlotsSwipeDetect : GameObjectUIBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public InputAction Point;
        public GameEventSender Sender;

        private bool _isSwiping;
        private int _frameCount = 0;
        [SerializeField] private Vector2 _startPoint, _deltaUp = Vector2.zero;
        [SerializeField] private float _swipeLength = 100;
        [SerializeField] private int _skipFrames = 30;

        public void Start()
        {
            _isSwiping = false;
            _frameCount = 0;
            Point.Enable();

            if (Sender == null)
                Sender = GameEventSender.Instance;
        }

        public void Update()
        {
            if (_deltaUp.magnitude >= _swipeLength)
            {
                _deltaUp = Vector2.zero;
                _isSwiping = false;

                Sender.BroadcastEvent("InvokeCard");   
            }

            if (_frameCount < _skipFrames)
            {
                _frameCount++;
                return;
            }

            _frameCount = 0;

            if (_isSwiping)
            {
                float y = (Point.ReadValue<Vector2>() - _startPoint).y;

                if (y < 0)
                    return;

                _deltaUp.y = y;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isSwiping = true;
            _startPoint = Point.ReadValue<Vector2>();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isSwiping)
                return;

            _isSwiping = false;
            _deltaUp = Vector2.zero;
        }
    }
}