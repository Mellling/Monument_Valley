using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    [SerializeField] bool isBGMController;

    #region Unity Event
    private void Start()
    {
        // �����̴� ���� ����� ������ OnVolumeChange �Լ� ȣ��
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);

        // �����̴��� �ʱⰪ�� AudioSource�� �������� ����
        if (isBGMController )
            volumeSlider.value = SoundManager.Instance.BGMVolme;
        else
            volumeSlider.value = SoundManager.Instance.SFXVolme;
    }
    #endregion

    void OnVolumeChange(float value)
    {
        // �����̴� ���� ���� AudioSource�� ������ ����
        if (isBGMController)
            SoundManager.Instance.BGMVolme = value;
        else
            SoundManager.Instance.SFXVolme = value;
    }
}
