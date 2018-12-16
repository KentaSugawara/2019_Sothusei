using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBullet_Bomb : MonoBehaviour {
    [SerializeField]
    private Renderer _Renderer;

    [SerializeField]
    private Vector3 _BombScale;

    [SerializeField]
    private float _NeedSeconds;

    private void Awake()
    {
        SetVisible(false);
    }

    public void SetVisible(bool value)
    {
        _Renderer.enabled = value;
    }

    IEnumerator BombRoutine;
    public void StartBomb(Vector3 Position, System.Action callback = null)
    {
        if (BombRoutine != null) StopCoroutine(BombRoutine);
        BombRoutine = Routine_Bomb(callback);
        transform.position = Position;
        StartCoroutine(BombRoutine);
    }

    IEnumerator Routine_Bomb(System.Action callback = null)
    {
        SetVisible(true);
        for (float t = 0.0f; t < _NeedSeconds; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, _BombScale, t);
            yield return null;
        }
        SetVisible(false);

        if (callback != null) callback();
    }
}
