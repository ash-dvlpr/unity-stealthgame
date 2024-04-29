using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Extensions;


public class FieldOfView : MonoBehaviour {
    public enum EDetectionMode : byte {
        InLineOfSight = 0,
        InViewCone    = 1,
        InRange       = 2,
    }

    // ==================== Configuration ====================
    [field: SerializeField] public Transform EyeTransform { get; private set; }
    [field: SerializeField, Range(0, 360)] public float ViewAngle { get; set; }
    [field: SerializeField, Min(0)] public float Range { get; set; }
    [field: SerializeField, Min(0)] public float PresenceRange { get; set; }
    [field: SerializeField] public EDetectionMode DetectionMode { get; set; }
    [field: SerializeField] public LayerMask TargetMask { get; private set; }
    [field: SerializeField] public LayerMask ObstacleMask { get; private set; }

    // ====================== Variables ======================
    public bool SeenAny => !VisibleTargets.NullOrEmpty();
    public readonly List<Transform> VisibleTargets = new();
    public Func<GameObject, bool> filter = (_) => true;

    public Collider[] TargetsInPresenceRange { get; private set; }
    public Collider[] TargetsInRange { get; private set; }

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
        TargetsInPresenceRange = Physics.OverlapSphere(EyeTransform.position, PresenceRange, TargetMask);
        TargetsInRange = Physics.OverlapSphere(EyeTransform.position, Range, TargetMask);

        // If there are any targets in range
        foreach (Collider targetCollider in TargetsInRange) {
            Transform target = targetCollider.transform;

            // If it's on the presence range, add it to the list.
            if (filter(targetCollider.gameObject)) { 
                if (TargetsInPresenceRange.Contains(targetCollider)) {
                    VisibleTargets.Add(target);
                }
                // If it's visible, add it to the list.
                else if (TargetIsVisible(targetCollider.bounds.center)) {
                    VisibleTargets.Add(target);
                }
            }
        }
    }

    private bool TargetIsVisible(Vector3 targetPosition) {
        // Precalculate the displacement (b-a) to the target, which also encodes direction.
        Vector3 displacementToTarget = (targetPosition - EyeTransform.position);

        switch (DetectionMode) {
            case EDetectionMode.InRange: return true;
            case EDetectionMode.InViewCone: return IsInViewCone(displacementToTarget);
            case EDetectionMode.InLineOfSight: return IsInViewCone(displacementToTarget) && HasLineOfSight(displacementToTarget);
            default: return false;
        }
    }

    private bool IsInViewCone(Vector3 displacementToTarget) {
        // If the angle between the target and our direction is less than half the view angle:
        // Then the target is inside the view cone.

        float angleToTarget = Vector3.Angle(EyeTransform.forward, displacementToTarget.normalized);
        return angleToTarget < ( ViewAngle / 2 );
    }

    private bool HasLineOfSight(Vector3 displacementToTarget) {
        // Same as Vector3.Distance(), but doing less operations in this case.
        float distanceToTarget = displacementToTarget.magnitude;

        // If the LineOfSight is not broken by obstacles, we have line of sight with the target.
        return !Physics.Raycast(EyeTransform.position, displacementToTarget.normalized, distanceToTarget, ObstacleMask);
    }
}
