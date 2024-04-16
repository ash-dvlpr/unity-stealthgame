using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {
    // ===================== Unity Stuff =====================
    private void OnSceneGUI() {
        FieldOfView fov = (FieldOfView) target;

        if (fov.EyeTransform) { 
            // Draw range circle
            Handles.color = Color.magenta;
            Handles.DrawWireArc(fov.EyeTransform.position, Vector3.up, Vector3.forward, 360, fov.Range);

            // Draw range circle
            Handles.color = Color.yellow;
            Handles.DrawWireArc(fov.EyeTransform.position, Vector3.up, Vector3.forward, 360, fov.PresenceRange);

            // Draw the viewcone lines
            Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.ViewAngle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.ViewAngle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(fov.EyeTransform.position, fov.EyeTransform.position + viewAngle01 * fov.Range);
            Handles.DrawLine(fov.EyeTransform.position, fov.EyeTransform.position + viewAngle02 * fov.Range);

            // Draw lines for all visible targets
            if (fov.SeenAny) {
                foreach (Collider visible in fov.TargetsInRange) { 
                    Handles.color = Color.green;
                    Handles.DrawLine(fov.EyeTransform.position, visible.bounds.center);
                }
            }
        }
    }

    // ===================== Custom Code =====================
    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees) {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
