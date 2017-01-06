using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedMesh : MonoBehaviour
{
    //In radians
    public int subdivisions = 12;
    //In unity units
    public float radius = 5f;   //Inner radius
    public float width = 1;

    public float angleRadians = 2 * Mathf.PI;

    private MeshFilter filter;
    private MeshRenderer rend;

    public Material mat;

    public void SetMaterial(Material s)
    {
        mat = s;
        rend.sharedMaterial = s;
    }


    public void InitObjects()
    {
        if (filter == null || rend == null)
        {

            if ((MeshFilter)gameObject.GetComponent(typeof(MeshFilter)) != null)
                filter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
            else
            {
                filter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
                //filter.sharedMesh = new Mesh ();
            }

            if ((MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer)) != null)
                rend = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));
            else
                rend = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
        }

        if (filter.sharedMesh == null)
        {
            filter.sharedMesh = new Mesh();
        }
        if (rend.sharedMaterial == null)
        {
            rend.sharedMaterial = new Material(Shader.Find("Diffuse"));
        }
    }

    // Use this for initialization
    void Start()
    {

        RebuildMesh();

    }

    public void RebuildMesh()
    {

        InitObjects();

        if (mat != null)
            SetMaterial(mat);

        float angleDivision = angleRadians / subdivisions;

        List<Vector3> vertsList = new List<Vector3>();
        List<Vector2> uvList = new List<Vector2>();
        List<int> trisList = new List<int>();


        float angleTotal = -angleRadians / 2f;

        float rad1 = radius;
        float rad2 = radius + width;

        for (int i = 0; i < subdivisions + 1; i++)
        {
            vertsList.Add(
                new Vector3(Mathf.Sin(angleTotal) * rad1, 0, Mathf.Cos(angleTotal) * rad1));
            vertsList.Add(
                new Vector3(Mathf.Sin(angleTotal) * rad2, 0, Mathf.Cos(angleTotal) * rad2));

            uvList.Add(new Vector2((angleTotal + angleRadians / 2f) / angleRadians, 0));
            uvList.Add(new Vector2((angleTotal + angleRadians / 2f) / angleRadians, 1));

            angleTotal += angleDivision;
        }



        for (int i = 0; i < subdivisions * 2; i += 1)
        {


                bool sides = i % 2 == 1;

                if (sides)
                {
                    trisList.Add(i);
                    trisList.Add(i + 1);
                    trisList.Add(i + 2);

                }
                else
                {
                    trisList.Add(i + 2);
                    trisList.Add(i + 1);
                    trisList.Add(i);
                }

    

            angleTotal += angleDivision;
        }

        filter.sharedMesh.Clear();

        filter.sharedMesh.vertices = vertsList.ToArray();
        filter.sharedMesh.uv = uvList.ToArray();
        filter.sharedMesh.triangles = trisList.ToArray();

        filter.sharedMesh.RecalculateNormals();
        ;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
               RebuildMesh();
        }

        transform.parent.LookAt(Camera.main.transform);
    }
}