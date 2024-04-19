using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinimapCamera : MonoBehaviour {
    // ====================== References =====================
    [SerializeField] Transform playerCameraRoot;

    // ====================== Unity Code ======================
    void Update() {
        var newRotation = transform.rotation.eulerAngles;
        newRotation.y = playerCameraRoot.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Euler(newRotation);
    }
}
