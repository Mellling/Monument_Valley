using UnityEngine;

/// <summary>
/// PlaneTextureCutter Ŭ������ Plane ������Ʈ�� �ؽ�ó �� �Ϻθ� �߶󳻾�, 
/// �ش� �ؽ�ó�� Ư�� ������Ʈ�� �Ϻκп��� ����.
/// </summary>
public class PlaneTextureCutter : MonoBehaviour
{
    public GameObject planeObject;  // Plane ������Ʈ
    public Vector2 uvStart = new Vector2(0.4f, 0.4f);  // �߾� �κ��� �ڸ� ���� UV ��ǥ
    public Vector2 uvEnd = new Vector2(0.6f, 0.6f);    // �߾� �κ��� �ڸ� �� UV ��ǥ

    [SerializeField] Material planeMaterial;
    [SerializeField] MeshRenderer objRender;
    [SerializeField] MeshFilter objMeshFiter;
    private Mesh objMesh;
    private Material objMaterial;

    void Start()
    {

        // �� ��ũ��Ʈ�� �Ҵ�� ������Ʈ�� Mesh�� Material�� ������
        objMaterial = objRender.material;
        objMesh = objMeshFiter.mesh;

        // Plane�� ������ �ؽ�ó ����
        ApplyTextureToNewObject();

        // UV ���� ���� �����
        ForceUVMapping();
    }

    void ApplyTextureToNewObject()
    {
        if (planeMaterial != null)
        {
            // Plane �ؽ�ó�� ���ο� ������Ʈ�� Material�� ����
            objMaterial.mainTexture = planeMaterial.mainTexture;
        }
    }

    void ForceUVMapping()
    {
        if (objMesh != null)
        {
            Vector3[] vertices = objMesh.vertices;
            Vector2[] uv = new Vector2[vertices.Length];

            // ��� ������ ���� Plane �ؽ�ó�� �߾� �κ��� ������ UV ��ǥ�� ����
            for (int i = 0; i < vertices.Length; i++)
            {
                uv[i] = new Vector2(
                    Mathf.Lerp(uvStart.x, uvEnd.x, vertices[i].x),
                    Mathf.Lerp(uvStart.y, uvEnd.y, vertices[i].z)
                );
            }

            // �޽��� UV ��ǥ �����
            objMesh.uv = uv;
        }
    }
}
