using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CFG_Gameplay", menuName = "Configuration/Gameplay Config", order = 1)]
public class GameplayConfig : ScriptableObject {
    // ====================== Variables ======================
    [field: SerializeField] public float MaxTime { get; private set; } = 60f; 
}
