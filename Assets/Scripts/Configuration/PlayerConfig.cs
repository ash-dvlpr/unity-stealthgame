using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CFG_Player", menuName = "Configuration/Player Config", order = 2)]
public class PlayerConfig : ScriptableObject {
    // ==================== Configurations ===================
    //[Serializable]
    //public struct StateConfig {
    //    [SerializeField] public PlayerController.State State;
        
    //    //[Header("Movement")]
    //    //[Tooltip("Movement speed of the character in m/s")]
    //    //[SerializeField, Min(0)] public float MoveSpeed;

    //    public override string ToString() {
    //        return JsonUtility.ToJson(this);
    //    }
    //}

    //[SerializeField] List<StateConfig> configs;

    [field: Header("Camera")]
    [Tooltip("How far in degrees can you move the camera up")]
    [field: SerializeField, Range( 10,  90)] public float UpperVerticalLimit = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    [field: SerializeField, Range(-90, -10)] public float LowerVerticalLimit = -30.0f;

    [field: Header("Movement")]
    [field: Tooltip("Move speed of the character in m/s")]
    [field: SerializeField, Min(0)] public float WalkSpeed { get; private set; } = 2;
    [field: Tooltip("Run speed of the character in m/s")]
    [field: SerializeField, Min(0)] public float RunSpeed { get; private set; } = 5.335f;
    [field: Tooltip("Acceleration and deceleration")]
    [field: SerializeField, Min(0)] public float Acceleration { get; private set; } = 10.0f;


    [field: Header("Jumping")]
    [Tooltip("The height the player can jump")]
    [field: SerializeField, Min(0)] public float JumpHeight { get; private set; } = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    [field: SerializeField, Min(0)] public float Gravity { get; private set; } = -15.0f;
    [field: SerializeField] public LayerMask GroundLayers { get; private set; }
    [field: SerializeField] public LayerMask WaterLayers { get; private set; }

    //[field: Header("Smoothing")]
    //[Tooltip("How fast the character turns to face movement direction")]
    //[field: SerializeField, Range(0.0f, 0.3f)] public float RotationSmoothTime { get; private set; } = 0.12f;




    // ====================== Variables ======================
    //[System.NonSerialized] internal Dictionary<PlayerController.State, StateConfig> _configsDict;
    //internal Dictionary<PlayerController.State, StateConfig> ConfigsDict {
    //    get {
    //        if (_configsDict == null) {
    //            _configsDict = new();
    //            foreach (StateConfig config in configs) {
    //                _configsDict[config.State] = config;
    //            }
    //        }
    //        return _configsDict;
    //    }
    //}
    //public StateConfig this[PlayerController.State key] {
    //    get => ConfigsDict.TryGetValue(key, out var value) ? value : default;
    //}
}
