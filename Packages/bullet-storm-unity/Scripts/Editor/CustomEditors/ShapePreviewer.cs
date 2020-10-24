using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.XNodes.ShapeNodes;
using UnityEditor;
using UnityEngine;

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

            Caches.Instance.shapePreviewMesh =
                EditorGUILayout.ObjectField(Caches.Instance.shapePreviewMesh, typeof(Mesh), false) as Mesh;
            Caches.Instance.shapePreviewMaterial =
                EditorGUILayout.ObjectField(Caches.Instance.shapePreviewMaterial, typeof(Material), false) as Material;
        }

        private void OnEnable()
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
}
