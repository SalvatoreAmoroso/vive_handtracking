using UnityEngine;

namespace ViveHandTracking.Sample
{
    internal class PinchLaser : Laser
    {
        private int state = 0;

        private void Awake()
        {
            skeletonRotation = false;
        }

        protected override void Update()
        {
            int newState = GetState();
            if (newState != state)
            {
                state = newState;
                // always show laser for state 1 & 2
                OnStateChanged(state == 0 ? 0 : 2);
                if (state == 2 && hit != null)
                    PushCube();
            }

            GestureResult hand = GestureProvider.LeftHand;
            if (hand == null)
            {
                laser.SetActive(false);
                return;
            }

            transform.position = hand.pinch.pinchStart;
            transform.rotation = hand.pinch.pinchRotation;
            laser.SetActive(visible);
        }

        private int GetState()
        {
            GestureResult hand = GestureProvider.LeftHand;
            if (hand == null || GestureProvider.RightHand != null)
                return 0;
            if (hand.pinch.isPinching)
                return 2;
            return 1;
        }

        private void PushCube()
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if (rigidbody == null)
                return;
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            // don't push cubes that is too close, otherwise left hand grab interaction fails
            if (distance > 1f)
                rigidbody.AddForce(0, 8 / Time.fixedDeltaTime, 0, ForceMode.Acceleration);
        }
    }

}
