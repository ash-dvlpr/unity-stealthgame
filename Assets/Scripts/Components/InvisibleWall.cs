using System;
using UnityEngine;


[RequireComponent(typeof(Renderer))]
public class InvisibleWall : MonoBehaviour {
    // ===================== References ======================
    Renderer _renderer;

    // ===================== Unity Stuff =====================
    void Awake() {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
    }
}
