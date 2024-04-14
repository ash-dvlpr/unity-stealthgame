using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NavigationConfig", menuName = "Configuration/Navigation Config", order = 1)]
public class NavigationConfig : ScriptableObject {
    
    // ====================== Variables ======================
    [field: SerializeField] public float WalkSpeed { get; private set; } = 1f; 
    [field: SerializeField] public float RunSpeed { get; private set; } = 2f; 
}
