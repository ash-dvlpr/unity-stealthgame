using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "CFG_AI", menuName = "Configuration/EnemyAI Config", order = 2)]
public class AIConfig : ScriptableObject {
    // ==================== Configurations ===================
    [Serializable]
    public struct StateConfig {
        [SerializeField] internal EnemyAI.EState key;
        [SerializeField] public float WalkSpeed;
        [SerializeField] public FieldOfView.EDetectionMode SightMode;

        public override string ToString() {
            return JsonUtility.ToJson(this);
        }
    }

    [SerializeField] List<StateConfig> configs;

    // ====================== Variables ======================
    [DoNotSerialize] internal Dictionary<EnemyAI.EState, StateConfig> _configs;
    [DoNotSerialize] internal Dictionary<EnemyAI.EState, StateConfig> Configs {
        get {
            if (_configs == null) {
                _configs = new();
                foreach (StateConfig config in configs) {
                    _configs[config.key] = config;
                }
            }
            return _configs;
        }
    }
    public StateConfig this[EnemyAI.EState key] {
        get { return Configs[key]; }
    }
}
