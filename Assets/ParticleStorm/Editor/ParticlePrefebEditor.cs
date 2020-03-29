using ParticleStorm;
using UnityEngine;

namespace UnityEditor
{
	[CustomEditor(typeof(ParticlePrefeb))]
	class ParticlePrefebEditor : Editor
	{
		SerializedProperty useParticleSystem;
		// basic module
		SerializedProperty basicModule;
		SerializedProperty renderMode;
		SerializedProperty material;
		SerializedProperty materials;
		SerializedProperty mesh;

		private void OnEnable()
		{
			useParticleSystem = serializedObject.FindProperty("useParticleSystem");
			// basic module
			basicModule = serializedObject.FindProperty("basicModule");
			renderMode = basicModule.FindPropertyRelative("renderMode");
			material = basicModule.FindPropertyRelative("material");
			materials = basicModule.FindPropertyRelative("materials");
			mesh = basicModule.FindPropertyRelative("mesh");
		}

		private void DrawBasicModule()
		{
			if (materials.arraySize == 0)
				EditorGUILayout.PropertyField(material);
			EditorGUILayout.PropertyField(materials);

			if (useParticleSystem.boolValue)
			{
				EditorGUILayout.PropertyField(renderMode);
				if (renderMode.enumValueIndex == (int)ParticleSystemRenderMode.Mesh)
					EditorGUILayout.PropertyField(mesh);
			}
			else
				EditorGUILayout.PropertyField(mesh);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(useParticleSystem);
			EditorGUILayout.Space();
			DrawBasicModule();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
