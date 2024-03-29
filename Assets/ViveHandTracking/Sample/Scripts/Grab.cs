﻿using UnityEngine;

namespace ViveHandTracking.Sample
{
    internal class Grab : MonoBehaviour
    {
        private static Color enterColor = new Color(0, 0, 0.3f, 1);
        private static Color moveColor = new Color(0, 0.3f, 0, 1);

        private Rigidbody target = null;
        private Transform anchor = null;
        private int state = 0;
        private bool exit = true;

        private void Awake()
        {
            GameObject go = new GameObject("Anchor");
            anchor = go.transform;
            anchor.parent = transform;
        }

        private void Update()
        {
            if (state == 1 && target != null)
            {
                anchor.position = transform.position;
                if (GestureProvider.HaveSkeleton)
                    anchor.rotation = transform.rotation;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "DynamicObject") return;
            Rigidbody newTarget = other.GetComponent<Rigidbody>();
            if (newTarget == target)
            {
                exit = false;
                return;
            }
            if (target != null && state == 1)
                StopMove();
            target = other.GetComponent<Rigidbody>();
            if (target != null)
            {
                exit = false;
                if (state == 1)
                    StartMove();
                else
                    SetColor(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Rigidbody>() != target)
                return;
            if (state == 1)
                exit = true;
            else
            {
                SetColor(null);
                target = null;
            }
        }

        public void OnStateChanged(int state)
        {
            this.state = state;
            if (target == null)
                return;
            if (state == 1)
                StartMove();
            else if (state == 0)
            {
                StopMove();
                if (exit)
                    target = null;
                else
                    SetColor(false);
            }
        }

        private void StartMove()
        {
            Debug.Log("START MOVE");
            target.useGravity = false;
            target.isKinematic = true;
            anchor.SetParent(target.transform.parent, true);
            target.transform.SetParent(anchor, true);
            SetColor(true);
        }

        private void StopMove()
        {
            target.transform.SetParent(anchor.parent, true);
            anchor.parent = transform;
            target.useGravity = true;
            target.isKinematic = false;
            SetColor(null);
        }

        // true: moving, false: touching, null: not touched
        private void SetColor(bool? moving)
        {
            if (target == null)
                return;
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer == null)
                return;
            Material material = renderer.material;
            if (moving == null)
            {
                material.DisableKeyword("_EMISSION");
                return;
            }
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", moving.Value ? moveColor : enterColor);
        }
    }

}
