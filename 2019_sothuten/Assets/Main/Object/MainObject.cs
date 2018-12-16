using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObject : MonoBehaviour {
    [SerializeField]
    private Renderer _Renderer;

    [SerializeField]
    private Collider _Collider;

    private ObjectSpawner.ObjectSettings _Settings;
    private int _ListIndex;

    public void SetVisible(bool value)
    {
        _Renderer.enabled = value;
    }

    public void Init(ObjectSpawner.ObjectSettings Settings, int index)
    {
        _Settings = Settings;
        _ListIndex = index;
        SetVisible(false);
        StopAllCoroutines();
    }

	private void Start () {
        SetVisible(false);
    }

    public void StartRunning(Vector3 Position)
    {
        StopAllCoroutines();
        transform.position = Position;
        _Collider.enabled = true;
        SetVisible(true);
        StartCoroutine(Spawn_Pos_Routine(Position));
        StartCoroutine(Spawn_Scale_Routine());
    }

    private IEnumerator Spawn_Pos_Routine(Vector3 Position)
    {
        Vector3 SpawnPos = MainCharacter.Instance.SpawnObjectPos;
        for (float t = 0.0f; t < _Settings._Spawn_Pos_EaseSettings._NeedSeconds; t += Time.deltaTime)
        {
            transform.position
                = Vector3.Lerp(
                    SpawnPos,
                    Position,
                    EasingLerps.EasingLerp(
                    _Settings._Spawn_Pos_EaseSettings._Get_EasingType,
                    _Settings._Spawn_Pos_EaseSettings._Get_InOutType,
                    t / _Settings._Spawn_Pos_EaseSettings._NeedSeconds,
                    0.0f,
                    1.0f)
                    );
            yield return null;
        }
    }

    private IEnumerator Spawn_Scale_Routine()
    {
        for (float t = 0.0f; t < _Settings._Spawn_Scale_EaseSettings._NeedSeconds; t += Time.deltaTime)
        {
            transform.localScale
                = Vector3.Lerp(
                    Vector3.zero,
                    Vector3.one,
                    EasingLerps.EasingLerp(
                    _Settings._Spawn_Scale_EaseSettings._Get_EasingType,
                    _Settings._Spawn_Scale_EaseSettings._Get_InOutType,
                    t / _Settings._Spawn_Scale_EaseSettings._NeedSeconds,
                    0.0f,
                    1.0f)
                    );
            yield return null;
        }
    }

    private IEnumerator RunningRoutine()
    {
        yield return new WaitForSeconds(_Settings._HideSeconds);

        //ここで消し始める
        _Collider.enabled = false;

        for (float t = 0.0f; t < _Settings._Hide_Scale_EaseSettings._NeedSeconds; t += Time.deltaTime)
        {
            transform.localScale
                = Vector3.Lerp(
                    Vector3.zero,
                    Vector3.one,
                    EasingLerps.EasingLerp(
                    _Settings._Hide_Scale_EaseSettings._Get_EasingType,
                    _Settings._Hide_Scale_EaseSettings._Get_InOutType,
                    t / _Settings._Hide_Scale_EaseSettings._NeedSeconds,
                    0.0f,
                    1.0f)
                    );
            yield return null;
        }
    }

    private IEnumerator RoutineGet = null;
    public void Get()
    {
        if (RoutineGet != null) StopCoroutine(RoutineGet);
        RoutineGet = GetRoutine((transform.position - MainField.Instance.Center).normalized);
        _Collider.enabled = false;
        StartCoroutine(RoutineGet);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="FromCenterVector">中心からの方向ベクトル（どう曲がるか）</param>
    /// <returns></returns>
    private IEnumerator GetRoutine(Vector3 FromCenterVector)
    {
        float e;
        Vector3 PlayerPos = Camera.main.transform.position + Vector3.down * 0.5f + Vector3.forward * 0.2f;
        Vector3 StartPos = transform.position;
        Vector3 StartScale = transform.localScale;
        Vector3 v = (StartPos - PlayerPos).normalized;
        Vector3 BezierPos =
            PlayerPos +
            v * ObjectSpawner.Instance.ToPlayerDistance * 0.5f +
            FromCenterVector * ObjectSpawner.Instance.ObjectGetCurveDistance;

        Vector3 bz1, bz2;

        for (float t = 0.0f; t < _Settings._Get_Pos_EaseSettings._NeedSeconds; t += Time.deltaTime)
        {
            e = EasingLerps.EasingLerp(
                    _Settings._Get_Pos_EaseSettings._Get_EasingType,
                    _Settings._Get_Pos_EaseSettings._Get_InOutType,
                    t / _Settings._Get_Pos_EaseSettings._NeedSeconds,
                    0.0f,
                    1.0f);

            bz1 = Vector3.Lerp(StartPos, BezierPos, e);
            bz2 = Vector3.Lerp(BezierPos, PlayerPos, e);
            transform.position = Vector3.Lerp(bz1, bz2, e);
            transform.localScale = Vector3.Lerp(StartScale, Vector3.zero, e);

            yield return null;
        }

        Hide();
    }

    public void Hide()
    {
        StopAllCoroutines();
        SetVisible(false);
        ObjectSpawner.Instance.MainBulletHide(_ListIndex);
    }
}
