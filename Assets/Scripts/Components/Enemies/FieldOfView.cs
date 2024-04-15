using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Extensions;


public class FieldOfView : MonoBehaviour {
    public enum EDetectionMode : byte {
        STRICT = 0,
        LOOSE  = 1,
    }

    // ==================== Configuration ====================
    [field: SerializeField, Range(0, 360)] public float ViewAngle { get; private set; }
    [field: SerializeField, Min(0)] public float Range { get; private set; }
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] EDetectionMode detectionMode;

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

            // If it's visible, add it to the list.
            if (TargetIsVisible(target)) {
                VisibleTargets.Add(target);
            }
        }
    }

    private bool TargetIsVisible(Transform target) {
        // Precalculate the displacement (b-a) to the target, which also encodes direction.
        Vector3 displacementToTarget = (target.position - transform.position);

        switch (detectionMode) {
            case EDetectionMode.STRICT: return IsInViewCone(displacementToTarget) && HasLineOfSight(displacementToTarget);
            case EDetectionMode.LOOSE: return IsInViewCone(displacementToTarget);
            default: return false;
        }
    }

    private bool IsInViewCone(Vector3 displacementToTarget) {
        // If the angle between the target and our direction is less than half the view angle:
        // Then the target is inside the view cone.
        float angleToTarget = Vector3.Angle(transform.forward, displacementToTarget.normalized);
        return angleToTarget < ( ViewAngle / 2 );
    }

    private bool HasLineOfSight(Vector3 displacementToTarget) {
        // Same as Vector3.Distance(), but doing less operations in this case.
        float distanceToTarget = displacementToTarget.magnitude;

        // If the LineOfSight is not broken by obstacles, we have line of sight with the target.
        return !Physics.Raycast(transform.position, displacementToTarget.normalized, distanceToTarget, obstructionMask);
    }
}
