using System.Collections;
using UnityEngine;

namespace ViveHandTracking.Sample
{
    internal class Laser : MonoBehaviour
    {
        protected const float angularVelocity = 50.0f;

        public Color color = Color.red;
        public Color emissionColor = new Color(0.3f, 0, 0, 1);
        public GameObject laser = null;
        public GameObject light = null;

        protected bool visible = false;
        protected Renderer hit = null;
        protected bool skeletonRotation = true;

        private bool magicalGrabActive = false;

        private IEnumerator Start()
        {
            laser.GetComponent<LineRenderer>().material.SetColor("_TintColor", color);
            if (light != null)
            {
                light.SetActive(false);
                light.GetComponent<Light>().color = color;
            }
            if (!skeletonRotation)
                yield break;
            while (GestureProvider.Status == GestureStatus.NotStarted)
                yield return null;
            if (GestureProvider.HaveSkeleton)
                laser.transform.localRotation = Quaternion.Euler(-30, 0, 0);
        }

        protected virtual void Update()
        {
            GestureResult hand = GestureProvider.RightHand;
            if (hand == null)
            {
                laser.SetActive(false);
                return;
            }

            transform.position = hand.position;

            // smooth rotation for skeleton mode
            if (laser.activeSelf && GestureProvider.HaveSkeleton)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, hand.rotation, angularVelocity * Time.deltaTime);
            else
                transform.rotation = hand.rotation;

            laser.SetActive(visible);
        }

        public void OnStateChanged(int state)
        {
            if (light != null)
                light.SetActive(state == 1);
            if (state == 2)
                visible = true;
            else
            {
                visible = false;
                if (hit != null)
                    StopHit();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "DynamicObject") return;

            if (hit != null)
                StopHit();

            hit = other.GetComponent<Renderer>();
            if (hit != null)
            {
                hit.material.EnableKeyword("_EMISSION");
                hit.material.SetColor("_EmissionColor", emissionColor);

                StartCoroutine(StartMagicalGrab());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (hit != null && hit == other.GetComponent<Renderer>())
                StopHit();
        }

        private void StopHit()
        {
            hit.material.DisableKeyword("_EMISSION");
            hit = null;
        }

        private IEnumerator StartMagicalGrab()
        {
            if (hit == null || magicalGrabActive)
            {
                Debug.Log("magical grab already active");
                yield return null;
            }

            magicalGrabActive = true;
            GestureResult hand = GestureProvider.RightHand;
            Vector3 dir = hand.position - hit.transform.position;

            while (hit != null && dir.sqrMagnitude > 1) //While object in laser and distance greater 1 meter
            {
                dir = hand.position - hit.transform.position;
                hit.transform.Translate(dir * Time.deltaTime);

                yield return null;
            }

            magicalGrabActive = false;
        }
    }
}
