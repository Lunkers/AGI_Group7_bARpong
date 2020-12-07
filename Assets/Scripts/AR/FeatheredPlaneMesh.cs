using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Place a feathering effect on the edges of the detected planes
/// To reduce impression of hard edges
/// </summary>

[RequireComponent(typeof(ARPlaneMeshVisualizer), typeof(MeshRenderer), typeof(ARPlane))]
public class FeatheredPlaneMesh : MonoBehaviour
{
    [SerializeField]
    float _featheringWidth = 0.2f;

    public float featheringWidth
    {
        get { return _featheringWidth; }
        set { _featheringWidth = value; }
    }

    void Awake()
    {
        _planeMeshVisualizer = GetComponent<ARPlaneMeshVisualizer>();
        _planeMaterial = GetComponent<MeshRenderer>().material;
        _plane = GetComponent<ARPlane>();
    }

    void OnEnable()
    {
        _plane.boundaryChanged += ARPlane_boundaryUpdated;
    }

    void OnDisable() {
        _plane.boundaryChanged -= ARPlane_boundaryUpdated;
    }

    void ARPlane_boundaryUpdated(ARPlaneBoundaryChangedEventArgs eventArgs) {
        GenerateBoundaryUVs(_planeMeshVisualizer.mesh);
    }


    //Generate UVs to mark the boundary vertices and feathering coords
    void GenerateBoundaryUVs(Mesh mesh)
    {
        int vertexCount = mesh.vertexCount;

        // Reuse the list of UVs
        _FeatheringUVs.Clear();
        if (_FeatheringUVs.Capacity < vertexCount) { _FeatheringUVs.Capacity = vertexCount; }

        mesh.GetVertices(_vertices);

        Vector3 centerInPlaneSpace = _vertices[_vertices.Count - 1];
        Vector3 uv = new Vector3(0, 0, 0);
        float shortestUVMapping = float.MaxValue;

        // Assume the last vertex is the center vertex.
        for (int i = 0; i < vertexCount - 1; i++)
        {
            float vertexDist = Vector3.Distance(_vertices[i], centerInPlaneSpace);

            // Remap the UV so that a UV of "1" marks the feathering boudary.
            // The ratio of featherBoundaryDistance/edgeDistance is the same as featherUV/edgeUV.
            // Rearrange to get the edge UV.
            float uvMapping = vertexDist / Mathf.Max(vertexDist - featheringWidth, 0.001f);
            uv.x = uvMapping;

            // All the UV mappings will be different. In the shader we need to know the UV value we need to fade out by.
            // Choose the shortest UV to guarentee we fade out before the border.
            // This means the feathering widths will be slightly different, we again rely on a fairly uniform plane.
            if (shortestUVMapping > uvMapping) { shortestUVMapping = uvMapping; }

            _FeatheringUVs.Add(uv);
        }

        _planeMaterial.SetFloat("_ShortestUVMapping", shortestUVMapping);

        // Add the center vertex UV
        uv.Set(0, 0, 0);
        _FeatheringUVs.Add(uv);

        mesh.SetUVs(1, _FeatheringUVs);
        mesh.UploadMeshData(false);
    }

    static List<Vector3> _FeatheringUVs = new List<Vector3>();
    static List<Vector3> _vertices = new List<Vector3>();

    ARPlaneMeshVisualizer _planeMeshVisualizer;
    ARPlane _plane;
    Material _planeMaterial;
}