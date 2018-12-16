using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : GameComponent {
    public static ObjectSpawner Instance;

    [SerializeField]
    private float _SpawnSpan;

    [SerializeField]
    private GameObject _Prefab_MainObject;

    [SerializeField]
    private ObjectSettings _Settings_MainObject;

    [SerializeField]
    private float _ToPlayerDistance;
    public float ToPlayerDistance
    {
        get { return _ToPlayerDistance; }
    }

    [SerializeField]
    private float _ObjectGetCurveDistance;
    public float ObjectGetCurveDistance
    {
        get { return _ObjectGetCurveDistance; }
    }

    [Serializable]
    public class ObjectSettings
    {
        public int _MaxSpawn;
        public float _HideSeconds;
        public EaseSettings _Spawn_Pos_EaseSettings;
        public EaseSettings _Spawn_Scale_EaseSettings;
        public EaseSettings _Get_Pos_EaseSettings;
        public EaseSettings _Hide_Scale_EaseSettings;
    }

    [Serializable]
    public class EaseSettings
    {
        public float _NeedSeconds;
        public EasingLerps.EasingLerpsType _Get_EasingType;
        public EasingLerps.EasingInOutType _Get_InOutType;
    }

    private MainObject[] _MainObjects;
    private List<int> _MainObjects_ReadyIndexes = new List<int>();


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        _MainObjects = new MainObject[_Settings_MainObject._MaxSpawn];
        for (int i = 0; i < _Settings_MainObject._MaxSpawn; ++i)
        {
            _MainObjects[i] = Instantiate(_Prefab_MainObject).GetComponent<MainObject>();
            _MainObjects[i].transform.rotation = MainField.Instance.transform.rotation;
            _MainObjects_ReadyIndexes.Add(i);
        }
    }

    public override IEnumerator Init()
    {
        for (int i = 0; i < _MainObjects.Length; ++i)
        {
            _MainObjects[i].Init(_Settings_MainObject, i);
            yield return null;
        }
    }

    public override void StartRunning(Action CompleteCallback)
    {
        StartCoroutine(RunningRoutine());
    }

    private IEnumerator RunningRoutine()
    {
        while (true)
        {
            if (_MainObjects_ReadyIndexes.Count > 0)
            {
                _MainObjects[_MainObjects_ReadyIndexes[0]].StartRunning(MainField.Instance.GetRundomPos());
                _MainObjects_ReadyIndexes.RemoveAt(0);
            }
            yield return new WaitForSeconds(_SpawnSpan);
        }
    }

    public void MainBulletHide(int index)
    {
        _MainObjects_ReadyIndexes.Add(index);
    }

    public override void EndRunning(Action CompleteCallback)
    {
        throw new NotImplementedException();
    }
}
