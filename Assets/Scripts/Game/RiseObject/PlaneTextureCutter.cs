using UnityEngine;

/// <summary>
/// PlaneTextureCutter 클래스는 Plane 오브젝트의 텍스처 중 일부를 잘라내어, 
/// 해당 텍스처를 특정 오브젝트의 일부분에만 적용.
/// </summary>
public class PlaneTextureCutter : MonoBehaviour
{
    public GameObject planeObject;  // Plane 오브젝트
    public Vector2 uvStart = new Vector2(0.4f, 0.4f);  // 중앙 부분을 자를 시작 UV 좌표
    public Vector2 uvEnd = new Vector2(0.6f, 0.6f);    // 중앙 부분을 자를 끝 UV 좌표

    [SerializeField] Material planeMaterial;
    [SerializeField] MeshRenderer objRender;
    [SerializeField] MeshFilter objMeshFiter;
    private Mesh objMesh;
    private Material objMaterial;

    void Start()
    {

        // 이 스크립트가 할당된 오브젝트의 Mesh와 Material을 가져옴
        objMaterial = objRender.material;
        objMesh = objMeshFiter.mesh;

        // Plane과 동일한 텍스처 적용
        ApplyTextureToNewObject();

        // UV 매핑 강제 덮어쓰기
        ForceUVMapping();
    }

    void ApplyTextureToNewObject()
    {
        if (planeMaterial != null)
        {
            // Plane 텍스처를 새로운 오브젝트의 Material에 적용
            objMaterial.mainTexture = planeMaterial.mainTexture;
        }
    }

    void ForceUVMapping()
    {
        if (objMesh != null)
        {
            Vector3[] vertices = objMesh.vertices;
            Vector2[] uv = new Vector2[vertices.Length];

            // 모든 정점에 대해 Plane 텍스처의 중앙 부분을 강제로 UV 좌표로 설정
            for (int i = 0; i < vertices.Length; i++)
            {
                uv[i] = new Vector2(
                    Mathf.Lerp(uvStart.x, uvEnd.x, vertices[i].x),
                    Mathf.Lerp(uvStart.y, uvEnd.y, vertices[i].z)
                );
            }

            // 메시의 UV 좌표 덮어쓰기
            objMesh.uv = uv;
        }
    }
}
