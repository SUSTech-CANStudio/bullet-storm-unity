using System;
using System.Collections.Generic;
using System.Globalization;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace CANStudio.BulletStorm.Editor.CustomEditors
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
        private LabelContent labelContent;
        private static readonly int ColorIndex = Shader.PropertyToID("_Color");

        private void ValidateData()
        {
            previewRenderUtility = new PreviewRenderUtility();

            previewRenderUtility.camera.farClipPlane = 10e5f;
            previewRenderUtility.camera.clearFlags = CameraClearFlags.Skybox;

            viewPos = new Vector2(0, -10); 
            cameraDistance = 10;
        }

        public override void OnPreviewSettings()
        {
            const int itemCount = 2;
            var width = EditorGUIUtility.currentViewWidth / (itemCount * 2 + 3);
            EditorGUIUtility.labelWidth = width;
            EditorGUIUtility.fieldWidth = width;
            
            labelContent = (LabelContent)EditorGUILayout.EnumPopup("Label content", labelContent);
            
            EditorGUI.BeginChangeCheck();
            previewObject =
                EditorGUILayout.ObjectField("Preview object", previewObject, typeof(GameObject), true) as GameObject;
            var changed = EditorGUI.EndChangeCheck();
            if (!objectChanged) objectChanged = changed;
        }

        private void OnEnable()
        {
            ValidateData();
            shapeAsset = target as ShapeAsset;
        }

        public override bool HasPreviewGUI() => true;
        
        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            ReceiveInput(r);
            if (Event.current.type != EventType.Repaint) return;
            
            previewRenderUtility.BeginPreview(r, background);
            
            var info = DrawShape();
            
            // move camera
            var transform = previewRenderUtility.camera.transform;
            transform.rotation = Quaternion.Euler(-viewPos.y, -viewPos.x, 0);
            transform.position = transform.forward * -cameraDistance;

            previewRenderUtility.camera.Render();
            
            Handles.SetCamera(previewRenderUtility.camera);
            DrawGizmos(info);
            
            previewRenderUtility.EndAndDrawPreview(r);
        }

        private void OnDisable()
        {
            previewRenderUtility?.Cleanup();
        }

        /// <summary>
        /// Draw preview meshes.
        /// </summary>
        /// <returns>Information to draw gizmos.</returns>
        private IEnumerable<Tuple<Vector3, Quaternion, float>> DrawShape()
        {
            if (!previewObject)    // Use default preview object if not set.
            {
                previewObject = BulletStormEditorUtil.LoadDefaultAsset<GameObject>("PreviewCube.prefab");
                objectChanged = true;
            }
            if (objectChanged)    // Get mesh and material from preview object.
            {
                mesh = previewObject.GetComponent<MeshFilter>().sharedMesh;
                material = previewObject.GetComponent<MeshRenderer>().sharedMaterial;
                if (!mesh)
                {
                    BulletStormLogger.LogWarning("Preview object mesh not set.");
                    return null;
                }
                if (!material)
                {
                    BulletStormLogger.LogWarning("Preview object material not set.");
                    return null;
                }
            }

            var scale = previewObject.transform.localScale;
            var rotation = previewObject.transform.rotation;
            var paramList = shapeAsset.shape;
            var returnValue = new List<Tuple<Vector3, Quaternion, float>>();

            for (var i = 0; i < paramList.Count; i++)
            {
                var position = paramList[i].position;
                var speed = paramList[i].velocity.magnitude;
                var lookRotation = speed == 0
                    ? Quaternion.identity
                    : Quaternion.LookRotation(paramList[i].velocity);
                var size = paramList[i].DefaultSize
                    ? scale
                    : Vector3.Scale(paramList[i].size, scale);
                var block = new MaterialPropertyBlock();
                block.SetColor(ColorIndex, paramList[i].DefaultColor ? Color.white : paramList[i].color);
                
                Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, lookRotation * rotation, size), material, 0,
                    previewRenderUtility.camera, 0, block);
                
                returnValue.Add(new Tuple<Vector3, Quaternion, float>(position, lookRotation, speed));
            }

            return returnValue;
        }

        /// <summary>
        /// Draw gizmos on preview.
        /// </summary>
        /// <param name="prs">List of position, rotation and speed.</param>
        private void DrawGizmos(IEnumerable<Tuple<Vector3, Quaternion, float>> prs)
        {
            var i = 0;
            foreach (var (position, rotation, speed) in prs)
            {
                Handles.zTest = CompareFunction.Greater;
                Handles.color = Color.red;
                Handles.ArrowHandleCap(0, position, rotation, speed / 4, EventType.Repaint);
                Handles.zTest = CompareFunction.LessEqual;
                Handles.color = Color.green;
                Handles.ArrowHandleCap(0, position, rotation, speed / 4, EventType.Repaint);
                
                Handles.zTest = CompareFunction.Always;
                string text;
                switch (labelContent)
                {
                    case LabelContent.Index:
                        text = i.ToString();
                        break;
                    case LabelContent.Speed:
                        text = speed.ToString(CultureInfo.CurrentCulture);
                        break;
                    case LabelContent.Position:
                        text = position.ToString();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Handles.Label(position, text);
                
                i++;
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
            }
        }

        private enum LabelContent
        {
            Index,
            Speed,
            Position
        }
    }
}