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
		SerializedProperty lifetime;
		SerializedProperty renderMode;
		SerializedProperty material;
		SerializedProperty materials;
		SerializedProperty mesh;
		// script module
		SerializedProperty scriptModule;
		SerializedProperty enableScriptModule;

		private void OnEnable()
		{
			useParticleSystem = serializedObject.FindProperty("useParticleSystem");
			// basic module
			basicModule = serializedObject.FindProperty("basicModule");
			lifetime = basicModule.FindPropertyRelative("lifetime");
			renderMode = basicModule.FindPropertyRelative("renderMode");
			material = basicModule.FindPropertyRelative("material");
			materials = basicModule.FindPropertyRelative("materials");
			mesh = basicModule.FindPropertyRelative("mesh");
			// script module
			scriptModule = serializedObject.FindProperty("scriptModule");
			enableScriptModule = scriptModule.FindPropertyRelative("enabled");
		}

		#region modules
		private void DrawBasicModule()
		{
			EditorGUILayout.PropertyField(lifetime);
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

		private void DrawScriptModule()
		{
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.PropertyField(enableScriptModule, new GUIContent("Script Module"));
			if (enableScriptModule.boolValue)
			{
				var update = scriptModule.FindPropertyRelative("update");
				var fixedUpdate = scriptModule.FindPropertyRelative("fixedUpdate");
				var lateUpdate = scriptModule.FindPropertyRelative("lateUpdate");
				EditorGUILayout.PropertyField(update);
				if (update.stringValue != null && update.stringValue != "")
					EditorGUILayout.PropertyField(scriptModule.FindPropertyRelative("parallelUpdate"));
				EditorGUILayout.PropertyField(fixedUpdate);
				if (fixedUpdate.stringValue != null && fixedUpdate.stringValue != "")
					EditorGUILayout.PropertyField(scriptModule.FindPropertyRelative("parallelFixedUpdate"));
				EditorGUILayout.PropertyField(lateUpdate);
				if (lateUpdate.stringValue != null && lateUpdate.stringValue != "")
					EditorGUILayout.PropertyField(scriptModule.FindPropertyRelative("parallelLateUpdate"));
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(useParticleSystem);
			EditorGUILayout.Space();
			DrawBasicModule();
			DrawScriptModule();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
