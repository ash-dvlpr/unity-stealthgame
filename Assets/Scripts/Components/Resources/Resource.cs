using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

using CinderUtils.Attributes;


/// <summary>
/// Generic abstract resource component.
/// </summary>
public abstract class Resource : MonoBehaviour {
    public enum ResourceType : byte {
        /// <summary>
        /// <see cref="Amount">Amount</see> will start off at it's <see cref="Max">Max</see> value.
        /// </summary>
        Plentiful = 0,
        /// <summary>
        /// <see cref="Amount">Amount</see> will start off at 0.
        /// </summary>
        Scarse = 1,
    }

    // ==================== Configuration ====================
    [field: Header("Configuration")]
    [field: SerializeField, Min(1)] public float Max { get; private set; }

    /// <value>
    /// Determines the behaviour of the <see cref="Reset">Reset()</see> method.
    /// </value>
    public abstract ResourceType ResType { get; }

    // ====================== Variables ======================
    [SerializeField, Disabled] private float _amount;

    public float Amount {
        get => _amount;
        protected set {
            var old = _amount;
            _amount = Math.Clamp(value, 0, Max);
            TriggerOnChange(old, _amount);
        }
    }


    // ====================== Unity Code ======================
    void OnEnable() {
        Reset();
    }

    void OnValidate() {
#if UNITY_EDITOR
        if (!Application.isPlaying) Reset();
#endif
    }

    void Reset() {
        Max = Math.Max(1, Max);

        switch (ResType) {
            case ResourceType.Scarse:
                Amount = 0; break;
            case ResourceType.Plentiful:
                Amount = Max; break;
            default:
                throw new NotImplementedException($"'Resource.Reset()': Missing implementation for enum variant: '{ResType}'");
        }
    }

    // ================== Outside Facing API =================
    public event Action<Resource> OnChange;
    protected virtual void TriggerOnChange(float prev, float next) { 
        OnChange?.Invoke(this);
    }

    public void ResetValues() {
        Reset();
    }
}