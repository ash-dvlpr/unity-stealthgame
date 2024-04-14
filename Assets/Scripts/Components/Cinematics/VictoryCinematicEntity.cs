using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;
using CinderUtils.Attributes;


[RequireComponent(typeof(ParticleSystem))]
public class VictoryCinematicEntity : CinematicEntity {

    // ===================== References ======================
    ParticleSystem particles;

    // ===================== Unity Stuff =====================
    protected override void Awake() { 
        base.Awake();

        particles = GetComponent<ParticleSystem>();
    }
    
    // ===================== Custom Code =====================
    protected override void OnCinematicEvent(CinematicEvent e) {
        if (e.id == CinematicID.VICTORY) { 
            particles.Play();
        }
    }
}
