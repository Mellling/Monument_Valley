using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///   �巡�׸� ���� �ٸ��� �ڵ��� ȸ����Ű��, ���� ����� 90���� �����ϴ� ����� ������ Ŭ����
/// </summary>
public class Handle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Rotate bridge")]
    [SerializeField] Transform bridge;
    [SerializeField] Light handleLight;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] RotationAxis brigdeRotationAxis;
    [SerializeField] RotationAxis handleRotationAxis;
    Sequence rotateSeq;

    #region Rotate

    public void OnBeginDrag(PointerEventData eventData)
    {
        handleLight.enabled = true;

        if (rotateSeq != null && rotateSeq.IsActive())
            rotateSeq.Kill();   // Ȱ�� ������ rotateSeq�� ���� (Kill)
    }

    public void OnDrag(PointerEventData eventData)
    {
        float rotationX = eventData.delta.x * rotationSpeed;    // X�� �巡�׷� ���� ȸ���� ���
        float rotationY = eventData.delta.y * rotationSpeed;    // y�� �巡�׷� ���� ȸ���� ���

        // ���� �������� �����ϴ� �ּ� ȸ���� ���� (���� ����)
        if (Mathf.Abs(rotationX) < 0.5f && Mathf.Abs(rotationY) < 0.5f)
            return; // X��� Y�� ��ο��� ���� ���� �������� ��� ȸ���� �������� ����

        // �ٸ� ������Ʈ ȸ��
        RotateObject(rotationX, rotationY, bridge, brigdeRotationAxis);
        // �ڵ� ������Ʈ ȸ��
        RotateObject(rotationX, rotationY, transform, handleRotationAxis);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        handleLight.enabled = false;

        SnapToNearest90Degrees(1f);
    }

    /// <summary>
    /// ������Ʈ ȸ����Ű�� �޼ҵ�
    /// </summary>
    /// <param name="rotationX">X�� �巡�� ȸ����</param>
    /// <param name="rotationY">Y�� �巡�� ȸ����</param>
    /// <param name="rotateObject">ȸ���� ������Ʈ Transform</param>
    /// <param name="rotationAxis">ȸ�� ��</param>
    private void RotateObject(float rotationX, float rotationY, Transform rotateObject, RotationAxis rotationAxis)
    {
        // ���� bridge�� ȸ������ ������
        Vector3 currentRotation = rotateObject.rotation.eulerAngles;  // ���� ������Ʈ�� ȸ������ ���Ϸ� ������ ������

        Quaternion rotationAround;
        // X���� �������� ȸ�� ��� (X��� Y�� �巡�� �ջ�)
        if (rotationAxis.Equals(RotationAxis.right))
            rotationAround = Quaternion.AngleAxis(rotationX + rotationY, Vector3.right);
        else
            rotationAround = Quaternion.AngleAxis(rotationX + rotationY, Vector3.up);
        // X�� ȸ�� ������ 360�� ���� ���� ����
        currentRotation.x = currentRotation.x % 360f;

        // ���� bridge�� ȸ���� ����� ȸ������ ����
        rotateObject.rotation = rotateObject.rotation * rotationAround;
    }

    /// <summary>
    /// ���� ����� 90���� �ٸ� ������Ʈ �ε巴�� ȸ����Ű�� �޼���
    /// </summary>
    /// <param name="duration">�ִϸ��̼��� �Ϸ�Ǵ� �� �ɸ��� �ð�/param>
    private void SnapToNearest90Degrees(float duration)
    {
        // ���� bridge�� ȸ������ ������
        Vector3 currentRotation = bridge.rotation.eulerAngles;

        float snap;

        if (brigdeRotationAxis.Equals(RotationAxis.right))
            // X���� 90���� ���� �� �ݿø��Ͽ� ���� ����� 90�� ������ ����
            snap = Mathf.Round(currentRotation.x / 90f) * 90f;
        else
            snap = Mathf.Round(currentRotation.y / 90f) * 90f;


        // ���ο� ȸ������ ����
        Vector3 targetRotation = new (snap, currentRotation.y, currentRotation.z); // Z���� �״�� ����

        // DOTween �������� �����Ͽ� �ִϸ��̼��� ����
        rotateSeq = DOTween.Sequence();

        // bridge�� ȸ���� targetRotation���� ������ ������ duration �ð� ���� �ִϸ��̼�
        // RotateMode.FastBeyond360�� ȸ���� 360���� �Ѿ �� �ֵ��� ���
        rotateSeq.Append(bridge.DORotate(targetRotation, duration, RotateMode.FastBeyond360));
    }
    #endregion
}

/// <summary>
/// ȸ�� ��
/// </summary>
enum RotationAxis
{
    right,
    up
}