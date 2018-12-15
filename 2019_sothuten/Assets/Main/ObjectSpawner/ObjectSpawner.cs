using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : GameComponent {
    [SerializeField]
    private static ObjectSpawner Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public override void Init(Action CompleteCallback)
    {
        throw new NotImplementedException();
    }

    public override void StartRunning(Action CompleteCallback)
    {
        throw new NotImplementedException();
    }

    public override void EndRunning(Action CompleteCallback)
    {
        throw new NotImplementedException();
    }
}
