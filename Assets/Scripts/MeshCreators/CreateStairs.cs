using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Handout {
	public class CreateStairs : MonoBehaviour {
		public int numberOfSteps = 10;
		// The dimensions of a single step of the staircase:
		public float width=3;
		public float height=1;
		public float depth=1;

		MeshBuilder builder;

		void Start () {
			builder = new MeshBuilder ();
			CreateShape();
			GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
		}

		/// <summary>
		/// Creates a stairway shape in [builder].
		/// </summary>
		[ContextMenu("Create Shape")]
		void CreateShape() {
			builder.Clear();

			for (int i = 0; i < numberOfSteps; i++) {
				Vector3 offset = new(0, height * i, depth * i);

				Front(offset);
				Top(offset);
				Left(offset);
				Right(offset);
				Back(offset);
			}

			void Front(Vector3 offset) {
                int v1 = builder.AddVertex(offset + new Vector3(width, 0, 0), new Vector2(1, 0));
                int v2 = builder.AddVertex(offset + new Vector3(-width, 0, 0), new Vector2(0, 0));
                int v3 = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(1, 0.5f));
                int v4 = builder.AddVertex(offset + new Vector3(-width, height, 0), new Vector2(0, 0.5f));

                builder.AddTriangle(v1, v2, v3);
                builder.AddTriangle(v2, v4, v3);
            }

            void Top(Vector3 offset) {
                int v1 = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(1, 0));
                int v2 = builder.AddVertex(offset + new Vector3(-width, height, 0), new Vector2(0, 0));
                int v3 = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(1, .5f));
                int v4 = builder.AddVertex(offset + new Vector3(-width, height, depth), new Vector2(0, .5f));

                builder.AddTriangle(v1, v2, v3);
                builder.AddTriangle(v2, v4, v3);
            }
            void Left(Vector3 offset) {
                int v1 = builder.AddVertex(offset + new Vector3(-width, 0, 0), new Vector2(0, 0));
                int v2 = builder.AddVertex(offset + new Vector3(-width, height, 0), new Vector2(0, 1));
                int v3 = builder.AddVertex(offset + new Vector3(-width, height, depth), new Vector2(1, 1));

                builder.AddTriangle(v1, v3, v2);
            }
            void Right(Vector3 offset) {
                int v1 = builder.AddVertex(offset + new Vector3(width, 0, 0), new Vector2(0, 0));
                int v2 = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(0, 1));
                int v3 = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(1, 1));

                builder.AddTriangle(v1, v2, v3);
            }
            void Back(Vector3 offset) {
                int v1 = builder.AddVertex(offset + new Vector3(width, 0, 0), new Vector2(1, 0));
                int v2 = builder.AddVertex(offset + new Vector3(-width, 0, 0), new Vector2(0, 0));
                int v3 = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(1, .5f));
                int v4 = builder.AddVertex(offset + new Vector3(-width, height, depth), new Vector2(0, .5f));

                builder.AddTriangle(v1, v3, v2);
                builder.AddTriangle(v2, v3, v4);
            }
        }
	}
}