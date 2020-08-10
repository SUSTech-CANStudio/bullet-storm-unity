using BulletStorm.Emission;
using BulletStorm.Util;
using UnityEditor;
using UnityEngine;

namespace BulletStorm.Editor.CustomEditors
{
    [CustomEditor(typeof(ShapeAsset))]
    internal class ShapeAssetEditor : UnityEditor.Editor
    {
        private ShapeAsset shapeAsset;
        private PreviewRenderUtility previewRenderUtility;
        private GameObject previewObject;
        private bool objectChanged = true;
        private Mesh mesh;
        private Material material;
        private Vector2 viewPos;
        private float cameraDistance;
        private static readonly int ColorIndex = Shader.PropertyToID("_Color");

        private void ValidateData()
        {
            previewRenderUtility = new PreviewRenderUtility();

            previewRenderUtility.camera.farClipPlane = 10e5f;

            viewPos = new Vector2(0, -10); 
            cameraDistance = 10;
        }

        public override void OnPreviewSettings()
        {
            EditorGUI.BeginChangeCheck();
            previewObject =
                EditorGUILayout.ObjectField("Preview object", previewObject, typeof(GameObject), false) as GameObject;
            var changed = EditorGUI.EndChangeCheck();
            if (!objectChanged) objectChanged = changed;
        }

        private void OnEnable()
        {
            ValidateData();
            previewObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            previewObject.hideFlags = HideFlags.HideAndDontSave;
            shapeAsset = target as ShapeAsset;
        }

        public override bool HasPreviewGUI() => true;
        
        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            ReceiveInput(r);
            if (Event.current.type != EventType.Repaint) return;
            
            previewRenderUtility.BeginPreview(r, background);
            
            Handles.SetCamera(previewRenderUtility.camera);
            Handles.color = Color.magenta;
            DrawShape();
            
            // move camera
            var transform = previewRenderUtility.camera.transform;
            transform.rotation = Quaternion.Euler(-viewPos.y, -viewPos.x, 0);
            transform.position = transform.forward * -cameraDistance;

            previewRenderUtility.camera.Render();
            previewRenderUtility.EndAndDrawPreview(r);
        }

        private void OnDisable()
        {
            previewRenderUtility?.Cleanup();
        }

        private void DrawShape()
        {
            if (!previewObject) return;
            if (objectChanged)
            {
                mesh = previewObject.GetComponent<MeshFilter>().sharedMesh;
                material = previewObject.GetComponent<MeshRenderer>().sharedMaterial;
                if (!mesh)
                {
                    BulletStormLogger.LogWarning("Preview object mesh not set.");
                    return;
                }
                if (!material)
                {
                    BulletStormLogger.LogWarning("Preview object material not set.");
                    return;
                }
            }

            var paramList = shapeAsset.shape.AsReadOnly();

            for (var i = 0; i < paramList.Count; i++)
            {
                var position = paramList[i].position;
                var speed = paramList[i].velocity.magnitude;
                var rotation = speed == 0
                    ? Quaternion.identity
                    : Quaternion.LookRotation(paramList[i].velocity);
                var size = paramList[i].size.x == 0 || paramList[i].size.y == 0 || paramList[i].size.z == 0
                    ? Vector3.one
                    : paramList[i].size;
                var block = new MaterialPropertyBlock();
                block.SetColor(ColorIndex, paramList[i].color);
                
                // draw meshes
                Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, size), material, 0,
                    previewRenderUtility.camera, 0, block);
                
                // draw gizmos
                Handles.ArrowHandleCap(0, position, rotation, speed / 4, EventType.Repaint);
                //Handles.Label(position, speed.ToString(CultureInfo.CurrentCulture));
            }
        }

        private void ReceiveInput(Rect position)
        {
            var controlID = GUIUtility.GetControlID("Slider".GetHashCode(), FocusType.Passive);
            var current = Event.current;
            switch (current.GetTypeForControl(controlID))
            {
                case EventType.ScrollWheel:
                    if (position.Contains(current.mousePosition))
                    {
                        cameraDistance += current.delta.y;
                        if (cameraDistance < 0) cameraDistance = 0;
                        current.Use();
                        GUI.changed = true;
                    }
                    break;
                case EventType.MouseDown:
                    if (position.Contains(current.mousePosition) && position.width > 50f)
                    {
                        GUIUtility.hotControl = controlID;
                        current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                    }
                    EditorGUIUtility.SetWantsMouseJumping(0);
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        viewPos -= current.delta / Mathf.Min(position.width, position.height) * 140f;
                        viewPos.y = Mathf.Clamp(viewPos.y, -90f, 90f);
                        current.Use();
                        GUI.changed = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}