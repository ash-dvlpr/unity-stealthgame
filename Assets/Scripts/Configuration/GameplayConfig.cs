using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CFG_Gameplay", menuName = "Configuration/Gameplay Config", order = 1)]
public class GameplayConfig : ScriptableObject {
    // ====================== Variables ======================
    [field: SerializeField] public float MaxTime { get; private set; } = 60f; 
    [field: SerializeField] public float TimePerObjective { get; private set; } = 20f; 
    [field: SerializeField] public int MinutesPerDay { get; private set; } = 60; 
    
    
    [NonSerialized] int? _secondsPerDay; 
    public int SecondsPerDay {
        get {
            if (_secondsPerDay == null) {
                _secondsPerDay = MinutesPerDay * 60;
            }
            return _secondsPerDay ?? 0;
        }
    }
}
