using UnityEngine;

/// <summary>
/// 마우스 Road 클릭 파티클 실행 클래스
/// 오브젝트 풀링으로 관리되는 파티클 활성화 시 플레이
/// </summary>
public class PlayUIParticle : PooledObject
{
    [SerializeField] ParticleSystem uiParticle;
    protected override void OnEnable()
    {
        base.OnEnable();
        uiParticle.Play();
    }
}
