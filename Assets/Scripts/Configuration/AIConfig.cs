using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CFG_AI", menuName = "Configuration/EnemyAI Config", order = 3)]
public class AIConfig : ScriptableObject {
    // ==================== Configurations ===================
    [Serializable]
    public struct StateConfig {
        [SerializeField] public EnemyAI.EState State;
        [Header("Navigation")]
        [SerializeField, Min(0)] public float WalkSpeed;
        [SerializeField, Min(1)] public float StoppingDistance;
        [Header("Senses")]
        [SerializeField] public FieldOfView.EDetectionMode DetectionMode;
        //[SerializeField, Range(0, 360)] public float ViewAngle;
        [SerializeField, Min(0)] public float ViewRange;

        public override string ToString() {
            return JsonUtility.ToJson(this);
        }
    }

    [SerializeField] List<StateConfig> configs;
    [field: SerializeField] public float AttackDelay { get; private set; }
    [field: SerializeField] public float AttackDamage { get; private set; }
    public float AttackDPS => AttackDamage * Time.deltaTime;

    // ====================== Variables ======================
    [System.NonSerialized] internal Dictionary<EnemyAI.EState, StateConfig> _configsDict;
    internal Dictionary<EnemyAI.EState, StateConfig> ConfigsDict {
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
    public StateConfig this[EnemyAI.EState key] {
        get => ConfigsDict.TryGetValue(key, out var value) ? value : default;
    }
}
