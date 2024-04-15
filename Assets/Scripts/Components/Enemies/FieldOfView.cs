using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Extensions;


public class FieldOfView : MonoBehaviour {
    // ==================== Configuration ====================
    [field: SerializeField, Range(0, 360)] public float ViewAngle { get; private set; }
    [field: SerializeField, Min(0)] public float Range { get; private set; }
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;

    // ====================== Variables ======================
    public bool SeenAny => !VisibleTargets.NullOrEmpty();
    public readonly List<Transform> VisibleTargets = new();

    // ===================== Unity Stuff =====================
    void OnEnable() {
        StartCoroutine(FOVRoutine());
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    // ===================== Custom Code =====================
    private IEnumerator FOVRoutine() {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true) {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck() {
        // Clear the previous results
        VisibleTargets.Clear();

        // Get all target objects in a sphere arround the character
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, Range, targetMask);

        // If there are any targets in range
        foreach (Collider targetCollider in targetsInRange) {
            Transform target = targetCollider.transform;

            // Calculate the direction to the target
            Vector3 displacementToTarget = (target.position - transform.position);
            Vector3 directionToTarget = displacementToTarget.normalized;

            // If the angle between the target and our direction is less than half the view angle:
            // Then the target is inside the view cone.
            if (Vector3.Angle(transform.forward, directionToTarget) < ViewAngle / 2) {
                // Same as Vector3.Distance(), but doing less operations in this case.
                float distanceToTarget = displacementToTarget.magnitude;

                // If the LineOfSight is not broken by obstacles, the target is seen.
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) {
                    VisibleTargets.Add(target);
                }
            }
        }
    }
}
