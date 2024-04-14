using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Extensions;


public class AIPatrol : MonoBehaviour, IEnumerable<Transform> {
    // ===================== References ======================
    [SerializeField] List<Transform> patrolPoints = new();

    // ===================== Unity Stuff =====================
    void Awake() {
        patrolPoints.Clear();
        foreach (var c in gameObject.Children()) {
            patrolPoints.Add(c.transform);
        }
    }

    // ===================== Custom Code =====================
    public IEnumerator<Transform> GetEnumerator() {
        return patrolPoints.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }
}
