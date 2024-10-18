using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [Header("For Switch Press")]
    [SerializeField] MeshRenderer renderer;
    [SerializeField] Material pressureSwitch_Off;
    [SerializeField] Collider collider;
    [SerializeField] float moveDis;
    [SerializeField] LayerMask canPressMask;
    public bool isPressed;
    [SerializeField] Vector3 targetPos;

    [Header("For Switch Press")]
    [SerializeField] UnityEvent onPress;

    #region Unity Event
    private void Update()
    {
        if (isPressed && collider.enabled)
            SwitchPressed();
    }
    #endregion

    #region Is Pressed
    private void SwitchPressed()
    {
        transform.position = Vector3.MoveTowards
            (transform.position, targetPos, 2f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            collider.enabled = false;
            onPress?.Invoke();
        }
    }

    /// <summary>
    /// 스위치와 플레이어 충돌 시 작업 구현한 메서드
    /// </summary>
    public void WhenSwitchPressed()
    {
        targetPos = transform.position + Vector3.down * moveDis;
        isPressed = true;
        renderer.material = pressureSwitch_Off;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isPressed && canPressMask.Contain(collision.gameObject.layer))
        {
            WhenSwitchPressed();
        }
    }
    #endregion
}
