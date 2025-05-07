using UnityEngine;

using Assets.Hypercrops.State;
using Assets.Hypercrops.Events;

namespace Assets.Hypercrops.Handlers
{
    /*
        TODO:
        - Add precision to movement
        - Rethink collision detection... yOffset?
        - Rethink occluded walkable areas (e.g. ground behind mountains)
    */
    public class GameEventOnWalk : MonoBehaviour
    {
        public float speed = 5f;
        public float stopPrecision = 0.2f;
        public float collisionPrecision = 0.1f;
        private GameEventSender _eventSender;
        private Vector3 _target;

        private void OnWalkEvent(object sender, WalkEventArguments e)
        {
            GameState state = GameState.Instance;
            state.IsWalking = true;
            _target = e.Target;
        }

        void Update()
        {
            Vector3 currentForward = transform.forward;
            Vector3 arrowEndPoint = transform.position + currentForward * 1f;

            Debug.DrawLine(transform.position, arrowEndPoint, Color.blue);

            if (!GameState.Instance.IsWalking)
            {
                return;
            }

            if (IsCloseToTarget())
            {
                StopWalking(true);
                return;
            }

            if (IsCollisionDetected())
            {
                StopWalking();
                return;
            }

            if (transform.position != _target)
            {
                Vector3 deltaPosition = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);
                transform.position = deltaPosition;
            }

            Quaternion targetRotation = Quaternion.LookRotation(transform.position - _target);

            if (transform.rotation != targetRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 2000f * Time.deltaTime);
            }
        }

        void OnEnable()
        {
            _eventSender = GameEventSender.Instance;

            if (_eventSender != null)
            {
                _eventSender.WalkEvent += OnWalkEvent;
            }
            else
            {
                Debug.LogWarning("No GameEventSender detected");
            }
        }

        void OnDisable()
        {
            _eventSender.WalkEvent -= OnWalkEvent;
        }

        private void StopWalking(bool snap = false)
        {
            GameState state = GameState.Instance;
            state.IsWalking = false;

            if (snap)
            {
                transform.position = _target;
            }
        }

        private bool IsCloseToTarget()
        {
            return VectorUtils.IsInSphere(transform.position, _target, stopPrecision);
        }

        private bool IsCollisionDetected()
        {
            Vector3 origin = transform.position;
            Vector3 direction = _target - origin;
            RaycastHit hit;

            if (Physics.Raycast(origin, direction, out hit, collisionPrecision) &&
                hit.collider.gameObject.CompareTag("Unmovable"))
            {
                return true;
            }

            return false;
        }
    }
}