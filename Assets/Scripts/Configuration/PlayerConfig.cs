using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CFG_Player", menuName = "Configuration/Player Config", order = 2)]
public class PlayerConfig : ScriptableObject {
    // ==================== Configurations ===================
    [Serializable]
    public struct StateConfig {
        [SerializeField] public PlayerController.State State;
        
        //[Header("Navigation")]
        //[SerializeField, Min(0)] public float WalkSpeed;
        //[SerializeField, Min(0)] public float JumpForce;

        public override string ToString() {
            return JsonUtility.ToJson(this);
        }
    }

    [SerializeField] List<StateConfig> configs;
    
    
    [field: Header("Movement")]
    [field: SerializeField, Min(0)] public float WalkSpeed { get; private set; }
    [field: SerializeField, Min(0)] public float RunSpeed { get; private set; }
    [field: SerializeField, Min(0)] public float JumpForce { get; private set; }

    // ====================== Variables ======================
    [System.NonSerialized] internal Dictionary<PlayerController.State, StateConfig> _configsDict;
    internal Dictionary<PlayerController.State, StateConfig> ConfigsDict {
        get {
            if (_configsDict == null) {
                _configsDict = new();
                foreach (StateConfig config in configs) {
                    _configsDict[config.State] = config;
                }
            }
            return _configsDict;
        }
    }
    public StateConfig this[PlayerController.State key] {
        get => ConfigsDict.TryGetValue(key, out var value) ? value : default;
    }
}
