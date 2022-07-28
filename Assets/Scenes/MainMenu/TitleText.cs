using TMPro;
using UnityEngine;

namespace Scenes.MainMenu
{
    public class TitleText : MonoBehaviour
    {
        public float Intensity = 1.0f;

        private float m_Time = 0;
        private TMP_Text textMesh;
        private Mesh mesh;
        private Vector3[] vertices;

        // Start is called before the first frame update
        public void Start()
        {
            textMesh = GetComponent<TMP_Text>();
        }

        // Update is called once per frame
        public void Update()
        {
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i] + GetTransformOffset(vertices[i] - transform.position);
            }

            m_Time += Time.deltaTime;
            m_Time += Time.deltaTime;

            mesh.vertices = vertices;
            textMesh.canvasRenderer.SetMesh(mesh);
        }

        private Vector3 GetTransformOffset(Vector3 pos)
        {
            var offset = new Vector3(pos.y * Intensity * Mathf.Sign(-pos.x) * Mathf.Abs(pos.x / 450), Mathf.Sin(m_Time * 0.5f) * 50, 0);
            offset += new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            return offset;
        }
    }
}
