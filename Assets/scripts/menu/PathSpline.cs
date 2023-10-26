using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PathSpline : MonoBehaviour
{
    private SplineContainer spline;
    public float solid_part = 0.07f;
    public float void_part = 0.03f;
    public float width = 20;
    private float3 toCam = new(0,0,-1);
    void Start()
    {
        spline = GetComponent<SplineContainer>();
        float j = 0;
        float3 right;
        List<float3> m_vertsP1 = new();
        List<float3> m_vertsP2 = new();
        //Находит точки для вершин
        while (j <= 1)
        {
            spline.Evaluate( j, out float3 position, out float3 forward, out _);
            right = Vector3.Cross(forward, toCam).normalized;
            m_vertsP1.Add(position + (right * width));
            m_vertsP2.Add(position - (right * width));
            j += solid_part;
            spline.Evaluate( j, out position, out forward, out _);
            right = Vector3.Cross(forward, toCam).normalized;
            m_vertsP1.Add(position + (right * width));
            m_vertsP2.Add(position - (right * width));
            j += void_part;
        }

        Mesh m = new();
        List<Vector3> verts = new();
        List<int> tris = new();
        int offset;

        int length = m_vertsP2.Count;

        //Заполняет массивы вершин и полигонов
        for (int i = 1; i <= length; i+=2)
        {
            Vector3 p1 = m_vertsP1[i - 1];
            Vector3 p2 = m_vertsP2[i - 1];
            Vector3 p3 = m_vertsP1[i];
            Vector3 p4 = m_vertsP2[i];

            offset = 2 * (i - 1);
            int t1 = offset + 0;
            int t2 = offset + 2;
            int t3 = offset + 3;
            int t4 = offset + 3;
            int t5 = offset + 1;
            int t6 = offset + 0;

            verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
            tris.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });
            
        }

        //Создаёт меш
        m.SetVertices(verts);
        m.SetTriangles(tris, 0);
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        filter.mesh = m;
    }
}
