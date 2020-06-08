using ParticleStorm;
using UnityEngine;

namespace UnityEditor
{
	[CustomEditor(typeof(ParticlePrefeb))]
	class ParticlePrefebEditor : Editor
	{
		// basic module
		SerializedProperty basicModule;
		SerializedProperty renderMode;
		SerializedProperty material;
		SerializedProperty materials;
		SerializedProperty mesh;
		SerializedProperty defaultParams;
		// normal modules
		SerializedProperty colorBySpeedModule;
		SerializedProperty colorOverLifetimeModule;
		SerializedProperty rotationBySpeedModule;
		SerializedProperty rotationOverLifetimeModule;
		SerializedProperty sizeBySpeedModule;
		SerializedProperty sizeOverLifetimeModule;
		SerializedProperty velocityOverLifetimeModule;
		// script module
		SerializedProperty scriptModule;
		// collision module
		SerializedProperty collisionModule;

		private void OnEnable()
		{
			// basic module
			basicModule = serializedObject.FindProperty("basicModule");
			renderMode = basicModule.FindPropertyRelative("renderMode");
			material = basicModule.FindPropertyRelative("material");
			materials = basicModule.FindPropertyRelative("materials");
			mesh = basicModule.FindPropertyRelative("mesh");
			defaultParams = basicModule.FindPropertyRelative("defaultParams");
			// normal modules
			colorBySpeedModule = serializedObject.FindProperty("colorBySpeedModule");
			colorOverLifetimeModule = serializedObject.FindProperty("colorOverLifetimeModule");
			rotationBySpeedModule = serializedObject.FindProperty("rotationBySpeedModule");
			rotationOverLifetimeModule = serializedObject.FindProperty("rotationOverLifetimeModule");
			sizeBySpeedModule = serializedObject.FindProperty("sizeBySpeedModule");
			sizeOverLifetimeModule = serializedObject.FindProperty("sizeOverLifetimeModule");
			velocityOverLifetimeModule = serializedObject.FindProperty("velocityOverLifetimeModule");
			// script module
			scriptModule = serializedObject.FindProperty("scriptModule");
			// collicion module
			collisionModule = serializedObject.FindProperty("collisionModule");
		}

		#region modules
		private void DrawBasicModule()
		{
			if (materials.arraySize == 0)
			{
				EditorGUILayout.PropertyField(material);
			}
			EditorGUILayout.PropertyField(materials);
			EditorGUILayout.PropertyField(renderMode);
			if (renderMode.enumValueIndex == (int)ParticleSystemRenderMode.Mesh)
			{
				EditorGUILayout.PropertyField(mesh);
			}
			EditorGUILayout.PropertyField(defaultParams);
		}
	 
		private void DrawModule(SerializedProperty module, int propertyCount)
		{
			SerializedProperty sp = module.FindPropertyRelative("enabled");
			
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.PropertyField(sp, new GUIContent(module.displayName, sp.tooltip));
			if (sp.boolValue)
			{
				for (int i = 1; i < propertyCount; i++)
				{
					sp.NextVisible(false);
					EditorGUILayout.PropertyField(sp);
				}
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawBasicModule();
			DrawModule(colorBySpeedModule, 3);
			DrawModule(colorOverLifetimeModule, 2);
			DrawModule(rotationBySpeedModule, 8);
			DrawModule(rotationOverLifetimeModule, 7);
			DrawModule(sizeBySpeedModule, 4);
			DrawModule(sizeOverLifetimeModule, 3);
			DrawModule(velocityOverLifetimeModule, 3);
			DrawModule(scriptModule, 2);
			DrawModule(collisionModule, 5);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
