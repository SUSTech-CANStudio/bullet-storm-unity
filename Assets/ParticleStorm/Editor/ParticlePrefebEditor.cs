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
		SerializedProperty enableScriptModule;
		// collision module
		SerializedProperty collisionModule;
		SerializedProperty enableCollisionModule;

		private void OnEnable()
		{
			useParticleSystem = serializedObject.FindProperty("useParticleSystem");
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
			enableScriptModule = scriptModule.FindPropertyRelative("enabled");
			// collicion module
			collisionModule = serializedObject.FindProperty("collisionModule");
			enableCollisionModule = collisionModule.FindPropertyRelative("enabled");
		}

		#region modules
		private void DrawBasicModule()
		{
			if (materials.arraySize == 0)
			{
				EditorGUILayout.PropertyField(material);
			}
			EditorGUILayout.PropertyField(materials);
			if (useParticleSystem.boolValue)
			{
				EditorGUILayout.PropertyField(renderMode);
				if (renderMode.enumValueIndex == (int)ParticleSystemRenderMode.Mesh)
					EditorGUILayout.PropertyField(mesh);
			}
			else
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

		private void DrawScriptModule()
		{
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.PropertyField(enableScriptModule, new GUIContent(scriptModule.displayName, enableScriptModule.tooltip));
			if (enableScriptModule.boolValue)
			{
				var update = scriptModule.FindPropertyRelative("update");
				var fixedUpdate = scriptModule.FindPropertyRelative("fixedUpdate");
				var lateUpdate = scriptModule.FindPropertyRelative("lateUpdate");
				EditorGUILayout.PropertyField(update);
				if (update.stringValue != null && update.stringValue != "")
				{
					EditorGUILayout.PropertyField(scriptModule.FindPropertyRelative("parallelUpdate"));
				}
				EditorGUILayout.PropertyField(fixedUpdate);
				if (fixedUpdate.stringValue != null && fixedUpdate.stringValue != "")
				{
					EditorGUILayout.PropertyField(scriptModule.FindPropertyRelative("parallelFixedUpdate"));
				}
				EditorGUILayout.PropertyField(lateUpdate);
				if (lateUpdate.stringValue != null && lateUpdate.stringValue != "")
				{
					EditorGUILayout.PropertyField(scriptModule.FindPropertyRelative("parallelLateUpdate"));
				}
			}
			EditorGUILayout.EndVertical();
		}

		private void DrawCollisionModule()
		{
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.PropertyField(enableCollisionModule, new GUIContent(collisionModule.displayName, enableCollisionModule.tooltip));
			if (enableCollisionModule.boolValue)
			{
				EditorGUILayout.PropertyField(collisionModule.FindPropertyRelative("kill"));
				EditorGUILayout.PropertyField(collisionModule.FindPropertyRelative("triggerType"));
				EditorGUILayout.PropertyField(collisionModule.FindPropertyRelative("collisionScript"));
				EditorGUILayout.PropertyField(collisionModule.FindPropertyRelative("quality"));
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
			DrawModule(colorBySpeedModule, 3);
			DrawModule(colorOverLifetimeModule, 2);
			DrawModule(rotationBySpeedModule, 8);
			DrawModule(rotationOverLifetimeModule, 7);
			DrawModule(sizeBySpeedModule, 4);
			DrawModule(sizeOverLifetimeModule, 3);
			DrawModule(velocityOverLifetimeModule, 3);
			DrawScriptModule();
			DrawCollisionModule();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
