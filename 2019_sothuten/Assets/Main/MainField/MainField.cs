using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainField : GameComponent {
    [SerializeField]
    private static MainField Instance;

    [SerializeField]
    private Collider _FiledCollider;

    // Use this for initialization
    void Awake () {
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
