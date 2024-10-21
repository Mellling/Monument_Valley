using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    [SerializeField] bool isBGMController;

    #region Unity Event
    private void Start()
    {
        // 슬라이더 값이 변경될 때마다 OnVolumeChange 함수 호출
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);

        // 슬라이더의 초기값을 AudioSource의 볼륨으로 설정
        if (isBGMController )
            volumeSlider.value = SoundManager.Instance.BGMVolme;
        else
            volumeSlider.value = SoundManager.Instance.SFXVolme;
    }
    #endregion

    void OnVolumeChange(float value)
    {
        // 슬라이더 값에 따라 AudioSource의 볼륨을 변경
        if (isBGMController)
            SoundManager.Instance.BGMVolme = value;
        else
            SoundManager.Instance.SFXVolme = value;
    }
}
