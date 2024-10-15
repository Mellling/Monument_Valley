using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] MeshRenderer renderer;
    [SerializeField] Material pressureSwitch_Off;
    [SerializeField] Collider collider;
    [SerializeField] float moveDis;
    [SerializeField] LayerMask canPressMask;
    bool isPressed;
    Vector3 targetPos;

    #region Unity Event
    private void Start()
    {
        targetPos = transform.position + Vector3.down * moveDis;
    }

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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isPressed && canPressMask.Contain(collision.gameObject.layer))
        {
            isPressed = true;
            renderer.material = pressureSwitch_Off;
        }
    }
    #endregion
}
