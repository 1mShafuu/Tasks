using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchAreaMesh : MonoBehaviour
{
    [SerializeField] private float _angle;
    [SerializeField] private Color meshColor = Color.red;

    private float _distance;
    private Mesh _mesh;
    private MeshFilter _meshRenderer;
    private CatchArea _catchArea;

    private void Awake()
    {
        _catchArea = GetComponent<CatchArea>();
        _meshRenderer = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        _distance = _catchArea.Radius;
        _mesh = CreateWedgeMesh();
        _meshRenderer.mesh = _mesh;
    }

    private Mesh CreateWedgeMesh()
    {
        const int defaultNumberInOneTriangle = 3;
        Mesh mesh = new Mesh();
        int segments = 20;
        int verticesNumber = segments * defaultNumberInOneTriangle;
        Vector3[] vertices = new Vector3[verticesNumber];
        int[] triangles = new int[verticesNumber];
        Vector3 center = Vector3.zero;
        Vector3 farLeft = Quaternion.Euler(0, -_angle, 0) * Vector3.forward * _distance;
        Vector3 farRight = Quaternion.Euler(0, _angle, 0) * Vector3.forward * _distance;
        int currentVerticy = 0;
        float currentAngle = -_angle;
        float deltaAngle = (_angle * 2) / segments;
        
        for (int index = 0; index < segments; index++)
        {
            currentAngle += deltaAngle;
            farLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _distance;
            farRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * _distance;
            vertices[currentVerticy++] = center;
            vertices[currentVerticy++] = farLeft;
            vertices[currentVerticy++] = farRight;
        }

        for (int index = 0; index < verticesNumber; index++)
        {
            triangles[index] = index;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
