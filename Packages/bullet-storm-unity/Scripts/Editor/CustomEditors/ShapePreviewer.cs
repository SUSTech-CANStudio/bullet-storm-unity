using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using CANStudio.BulletStorm.XNodes;
using CANStudio.BulletStorm.XNodes.ShapeNodes;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using Rect = UnityEngine.Rect;

namespace CANStudio.BulletStorm.Editor.CustomEditors
{
    public class ShapePreviewer : UnityEditor.Editor
    {
        private IShapeContainer shapeContainer;
        private PreviewRenderUtility previewRenderUtility;
        private Vector2 viewPos;
        private float cameraDistance;
        private BulletStormEditorUtil.LabelContent labelContent;

        private void ValidateData()
        {
            previewRenderUtility = new PreviewRenderUtility();

            previewRenderUtility.camera.farClipPlane = 10e5f;
            previewRenderUtility.camera.clearFlags = CameraClearFlags.Skybox;

            viewPos = new Vector2(0, -10);
            cameraDistance = 30;
        }

        public override void OnPreviewSettings()
        {
            const int itemCount = 3;
            var width = EditorGUIUtility.currentViewWidth / (itemCount * 2 + 3);
            EditorGUIUtility.labelWidth = width;
            EditorGUIUtility.fieldWidth = width;

            labelContent =
                (BulletStormEditorUtil.LabelContent) EditorGUILayout.EnumPopup("Label content", labelContent);

            EditorGUI.BeginChangeCheck();

            Preferences._.shapePreviewMesh =
                EditorGUILayout.ObjectField(Preferences._.shapePreviewMesh, typeof(Mesh), false) as Mesh;
            Preferences._.shapePreviewMaterial =
                EditorGUILayout.ObjectField(Preferences._.shapePreviewMaterial, typeof(Material), false) as Material;

            if (EditorGUI.EndChangeCheck()) Preferences.ApplyChanges();
        }

        protected void OnEnable()
        {
            ValidateData();
            shapeContainer = target as IShapeContainer;
        }

        public override bool HasPreviewGUI() => true;

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            BulletStormEditorUtil.ReceiveInput(r, ref cameraDistance, ref viewPos);
            if (Event.current.type != EventType.Repaint) return;

            previewRenderUtility.BeginPreview(r, background);

            // move camera
            var transform = previewRenderUtility.camera.transform;
            transform.rotation = Quaternion.Euler(-viewPos.y, -viewPos.x, 0);
            transform.position = transform.forward * -cameraDistance;

            BulletStormEditorUtil.DrawShapePreview(shapeContainer.GetShape(), previewRenderUtility.camera,
                labelContent);

            previewRenderUtility.EndAndDrawPreview(r);
        }

        private void OnDisable()
        {
            previewRenderUtility?.Cleanup();
        }
    }
    
    [CustomEditor(typeof(ShapeNode), true)]
    internal class ShapeNodeEditor : ShapePreviewer
    {
    }

    [CustomEditor(typeof(ShapeAsset))]
    internal class ShapeAssetEditor : ShapePreviewer
    {
    }

    [CustomEditor(typeof(ShapeGraph))]
    internal class ShapeGraphEditor : ShapePreviewer
    {
        private ShapeGraph shapeGraph;

        private void Awake()
        {
            shapeGraph = target as ShapeGraph;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Edit"))
            {
                NodeEditorWindow.Open(shapeGraph);
            }

            if (GUILayout.Button("Build"))
            {
                shapeGraph.Build();
            }

            if (GUILayout.Button("Create Asset"))
            {
                var path = AssetDatabase.GetAssetPath(shapeGraph);
                path = path.Substring(0, path.LastIndexOf('.')) + " Asset.asset";
                var shapeAsset = CreateInstance<ShapeAsset>();
                shapeAsset.shape = shapeGraph.shape.Copy();
                AssetDatabase.CreateAsset(shapeAsset, AssetDatabase.GenerateUniqueAssetPath(path));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
