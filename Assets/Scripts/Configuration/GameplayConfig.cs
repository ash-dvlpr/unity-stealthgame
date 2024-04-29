using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CFG_Gameplay", menuName = "Configuration/Gameplay Config", order = 1)]
public class GameplayConfig : ScriptableObject {
    // ====================== Variables ======================
    //[field: Header("General")]

    [field: Header("Time")]
    [field: SerializeField, Min(0)] public int MinutesPerDay { get; private set; } = 60; 
    [field: SerializeField, Min(0)] public int MinutesMaxTime { get; private set; } = 1; 
    [field: SerializeField, Min(0)] public int MinutesPerPatrol { get; private set; } = 5; 
    [field: SerializeField, Min(0)] public int SecondsPerObjective { get; private set; } = 20;
    
    
    [NonSerialized] int? _secondsPerDay; 
    public int SecondsPerDay {
        get {
            if (_secondsPerDay == null) {
                _secondsPerDay = MinutesPerDay * 60;
            }
            return _secondsPerDay ?? 0;
        }
    }

    [NonSerialized] int? _secondsMaxTime; 
    public int SecondsMaxTime {
        get {
            if (_secondsMaxTime == null) {
                _secondsMaxTime = MinutesMaxTime * 60;
            }
            return _secondsMaxTime ?? 0;
        }
    }

    [NonSerialized] int? _secondsPerPatrol; 
    public int SecondsPerPatrol {
        get {
            if (_secondsPerPatrol == null) {
                _secondsPerPatrol = MinutesPerPatrol * 60;
            }
            return _secondsPerPatrol ?? 0;
        }
    }
}
