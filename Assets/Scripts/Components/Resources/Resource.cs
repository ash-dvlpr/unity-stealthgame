using System;
using UnityEngine;

using CinderUtils.Attributes;


/// <summary>
/// Generic abstract resource component.
/// </summary>
public abstract class Resource : MonoBehaviour {
    public enum ResourceKind : byte {
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
    [field: SerializeField, Min(1)] public float Max { get; protected set; }

    /// <value>
    /// Determines the behaviour of the <see cref="Reset">Reset()</see> method.
    /// </value>
    public abstract ResourceKind Kind { get; }

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

    public virtual string ValuesString => $"{Amount:0.#} / {Max}";


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

        switch (Kind) {
            case ResourceKind.Scarse:
                Amount = 0; break;
            case ResourceKind.Plentiful:
                Amount = Max; break;
            default:
                throw new NotImplementedException($"'Resource.Reset()': Missing implementation for enum variant: '{Kind}'");
        }
    }

    // ================== Outside Facing API =================
    public event Action<float, float> OnChange;
    protected virtual void TriggerOnChange(float prev, float next) { 
        OnChange?.Invoke(prev, next);
    }


    public void ResetValues() {
        Reset();
    }

    public void Add(float amount) {
        Amount += amount;
    }
}