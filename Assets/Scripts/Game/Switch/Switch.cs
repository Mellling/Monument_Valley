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
    bool isPressed;
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
    public void SwitchPressed()
    {
        transform.position = Vector3.MoveTowards
            (transform.position, targetPos, 2f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            collider.enabled = false;
            onPress?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isPressed && canPressMask.Contain(collision.gameObject.layer))
        {
            targetPos = transform.position + Vector3.down * moveDis;
            isPressed = true;
            renderer.material = pressureSwitch_Off;
        }
    }
    #endregion
}
