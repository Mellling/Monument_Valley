using UnityEngine;

/// <summary>
/// ���콺 Road Ŭ�� ��ƼŬ ���� Ŭ����
/// ������Ʈ Ǯ������ �����Ǵ� ��ƼŬ Ȱ��ȭ �� �÷���
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
