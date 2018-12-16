using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainField : GameComponent {
    [SerializeField]
    public static MainField Instance;

    [SerializeField]
    private BoxCollider _FiledCollider;

    // Use this for initialization
    void Awake () {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
	}

    public Vector3 Center
    {
        get { return transform.position; }
    }

    public Vector3 GetRundomPos()
    {
        Vector3 rand = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0.0f);
        return _FiledCollider.transform.position + Vector3.Scale(Vector3.Scale(_FiledCollider.transform.localScale, _FiledCollider.size), rand);
    }

    public override IEnumerator Init()
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
