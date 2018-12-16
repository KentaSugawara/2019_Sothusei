using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour {
    public static MainCharacter Instance;

    [SerializeField]
    private Vector3 _SpawnObjectOffset;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public Vector3 SpawnObjectPos
    {
        get { return transform.position + _SpawnObjectOffset; }
    }
}
