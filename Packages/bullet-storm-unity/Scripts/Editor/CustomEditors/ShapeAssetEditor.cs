using System;
using System.Collections.Generic;
using System.Globalization;
using CANStudio.BulletStorm.Emission;
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
            const int itemCount = 3;
            var width = EditorGUIUtility.currentViewWidth / (itemCount * 2 + 3);
            EditorGUIUtility.labelWidth = width;
            EditorGUIUtility.fieldWidth = width;
            
            labelContent = (LabelContent)EditorGUILayout.EnumPopup("Label content", labelContent);

            Caches.Instance.shapePreviewMesh =
                EditorGUILayout.ObjectField(Caches.Instance.shapePreviewMesh, typeof(Mesh), false) as Mesh;
            Caches.Instance.shapePreviewMaterial =
                EditorGUILayout.ObjectField(Caches.Instance.shapePreviewMaterial, typeof(Material), false) as Material;
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

            if (!(info is null))
            {
                // move camera
                var transform = previewRenderUtility.camera.transform;
                transform.rotation = Quaternion.Euler(-viewPos.y, -viewPos.x, 0);
                transform.position = transform.forward * -cameraDistance;

                previewRenderUtility.camera.Render();

                Handles.SetCamera(previewRenderUtility.camera);
                DrawGizmos(info);
            }

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
            if (!Caches.Instance.shapePreviewMesh || !Caches.Instance.shapePreviewMaterial)
            {
                return null;
            }

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
                    ? Vector3.one
                    : paramList[i].size;
                var block = new MaterialPropertyBlock();
                block.SetColor(ColorIndex, paramList[i].DefaultColor ? Color.white : paramList[i].color);

                Graphics.DrawMesh(Caches.Instance.shapePreviewMesh, Matrix4x4.TRS(position, lookRotation, size),
                    Caches.Instance.shapePreviewMaterial, 0, previewRenderUtility.camera, 0, block);
                
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
                    case LabelContent.None:
                        text = "";
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
            None,
            Index,
            Speed,
            Position
        }
    }
}