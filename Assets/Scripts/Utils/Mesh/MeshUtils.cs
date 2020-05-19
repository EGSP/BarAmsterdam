using UnityEngine;

namespace Gasanov.SpeedUtils.MeshUtilities
{
    public static class MeshUtils
    {
        public static Mesh CreateEmptyMesh()
        {
            Mesh mesh = new Mesh();
            return mesh;
        }

        /// <summary>
        /// Создает меш квадрата с заданными шириной и высостой
        /// </summary>
        public static Mesh CreateQuadMesh(float width, float height)
        {
            Vector3[] vertices;
            Vector2[] uv;
            int[] triangles;
            
            CreateEmptyQuadData(1,out vertices,out uv,out triangles);

            vertices[0] = Vector3.zero;
            vertices[1] = new Vector3(0,height);
            vertices[2] = new Vector3(width,height);
            vertices[3] = new Vector3(width,0);
            
            uv[0] = Vector2.zero;
            uv[1] = Vector2.up;
            uv[2] = Vector2.one;
            uv[3] = Vector2.right;

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;

            var mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            return mesh;
        }
        
        /// <summary>
        /// Создает пустые массивы данных меша в зависимости от количества квадратов
        /// </summary>
        public static void CreateEmptyQuadData(int quadCount,out Vector3[] vertices, out Vector2[] uv, out int[] triangles)
        {
            vertices = new Vector3[4*quadCount];
            uv = new Vector2[4*quadCount];
            triangles = new int[6*quadCount];
        }

        /// <summary>
        /// Создает игровой объект квадрат без материала
        /// </summary>
        public static GameObject CreateQuadObject(float width, float height)
        {
            var mesh = CreateQuadMesh(width, height);
            var meshObject = new GameObject("QuadObject");

            var meshFilter = meshObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            
            return meshObject;
        }

        /// <summary>
        /// Создает игровой объект квадрат с материалом
        /// </summary>
        public static GameObject CreateQuadObject(float width, float height, Material material)
        {
            var meshObject = CreateQuadObject(width, height);
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;

            return meshObject;
        }

        /// <summary>
        /// Изменяет размер квадрата
        /// </summary>
        /// <param name="quad">Меш квадрата</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        public static void ChangeSizeQuadMesh(Mesh quad, float width, float height)
        {
            Vector3[] vertices;

            vertices = quad.vertices;

            vertices[0] = Vector3.zero;
            vertices[1] = Vector3.up * height;
            vertices[2] = new Vector3(width, height);
            vertices[3] = Vector3.right * width;

            quad.vertices = vertices;
        }
    }
}