using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    MeshFilter meshFilter;

    private float fov = 90f; //field of view angle of vision
    private int rayCount = 50; //rays used
    private float viewDistance = 100f; //view distance
    private Vector3 origin;
    private float startingAngle;
    private Mesh mesh;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
    }

    private Vector3 Angle2Vector(float angle) {
        float angleRad = angle * (Mathf.PI/180f); //to rad
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private float Vector2Angle(Vector3 vec) {
        vec = vec.normalized;
        float n = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        if(n<0) n+=360;
        return n;
    }

    void Start()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    private void LateUpdate() {
        float angle = startingAngle; //current angle used to create the triangles of the mesh and raycast
        float angleIncrease = fov / rayCount; //angle increase for the next raycast

        Vector3[] vertices = new Vector3[rayCount + 2]; //+2 for the borders
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        //Create all the triangles of the mesh
        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i <= rayCount; i++) {
            Vector3 vertex;
            Vector3 direction = Angle2Vector(angle);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, viewDistance, layerMask);
            if(raycastHit2D.collider == null) {
                //No hit
                vertex = origin + (direction * viewDistance);
            }
            else {
                //Hit
                vertex = raycastHit2D.point; //Point where we collided
            }
            vertices[vertexIndex] = vertex;
            if(i > 0) {
                triangles[triangleIndex    ] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease; //we go clockwise
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin) {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection) {
        startingAngle = Vector2Angle(aimDirection) + (fov / 2f);
    }

}
