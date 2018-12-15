using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameComponent : MonoBehaviour {
    public abstract void Init(System.Action CompleteCallback);
    public abstract void StartRunning(System.Action CompleteCallback);
    public abstract void EndRunning(System.Action CompleteCallback);
}
