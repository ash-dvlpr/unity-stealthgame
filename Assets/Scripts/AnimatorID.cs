using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorID {    
    // Movement
    public static readonly int Speed = Animator.StringToHash("Speed");
    public static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");
    public static readonly int Jump = Animator.StringToHash("Jump");
    public static readonly int FreeFall = Animator.StringToHash("FreeFall");
    public static readonly int Grounded = Animator.StringToHash("Grounded");
    public static readonly int Wet = Animator.StringToHash("Wet");
}