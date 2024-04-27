using System;
using UnityEngine;


public class MonoBehaviourService<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;
    
    // ====================== Variables ======================
    public static bool Initialized { get; protected set; } = false;
    public static bool Quitting { get; private set; } = false;
    public static T Instance {
        get {
            // If no instance create it
            if (_instance == null && !Quitting) {
                _instance = FindObjectOfType<T>();

                if (_instance == null) {
                    GameObject go = new GameObject(typeof(T).ToString());
                    _instance = go.AddComponent<T>();

                    DontDestroyOnLoad(Instance.gameObject);
                }
            }

            return _instance;
        }
    }


    // ===================== Unity Stuff =====================
    // Mantain only one instance of the singleton
    protected virtual void Awake() {
        if (_instance == null) {
            _instance = gameObject.GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance.GetInstanceID() != GetInstanceID()) {
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit() {
        Quitting = true;
    }
}
