using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBullet_Bomb : MonoBehaviour {
    [SerializeField]
    private Collider _Collider;

    [SerializeField]
    private ParticleSystem _Particle;

    [SerializeField]
    private int _NumOfParticle;

    [SerializeField]
    private Vector3 _BombScale;

    [SerializeField]
    private float _NeedSeconds;

    private void Awake()
    {
        _Collider.enabled = false;
        _Particle.Stop();
    }

    IEnumerator BombRoutine;
    public void StartBomb(Vector3 Position, System.Action callback = null)
    {
        if (BombRoutine != null) StopCoroutine(BombRoutine);
        BombRoutine = Routine_Bomb(callback);
        transform.position = Position;
        _Particle.Play();
        _Particle.Emit(_NumOfParticle);
        _Collider.enabled = true;
        StartCoroutine(BombRoutine);
    }

    IEnumerator Routine_Bomb(System.Action callback = null)
    {
        for (float t = 0.0f; t < _NeedSeconds; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, _BombScale, t / _NeedSeconds);
            yield return null;
        }

        _Particle.Stop();
        _Collider.enabled = false;

        if (callback != null) callback();
    }

    private void OnTriggerEnter(Collider other)
    {
        var comp = other.GetComponent<MainObject>();
        if (comp != null)
        {
            comp.Get();
        }
    }
}
