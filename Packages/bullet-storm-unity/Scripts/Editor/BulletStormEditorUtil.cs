using System;
using System.Collections.Generic;
using System.Globalization;
using CANStudio.BulletStorm.Emission;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace CANStudio.BulletStorm.Editor
{
    internal static class BulletStormEditorUtil
    {
        private static readonly int ColorIndex = Shader.PropertyToID("_Color");
        
        /// <summary>
        /// Draw shape preview meshes and gizmos.
        /// </summary>
        /// <param name="shape">The shape to draw.</param>
        /// <param name="camera"></param>
        /// <param name="content"></param>
        /// <returns>Information to draw gizmos.</returns>
        public static void DrawShapePreview(Shape shape, [NotNull] Camera camera, LabelContent content)
        {
            if (shape is null || !Caches.Instance.shapePreviewMesh || !Caches.Instance.shapePreviewMaterial) return;

            var prs = new List<Tuple<Vector3, Quaternion, float>>();

            for (var i = 0; i < shape.Count; i++)
            {
                var position = shape[i].position;
                var speed = shape[i].velocity.magnitude;
                var lookRotation = speed == 0
                    ? Quaternion.identity
                    : Quaternion.LookRotation(shape[i].velocity);
                var size = shape[i].DefaultSize
                    ? Vector3.one
                    : shape[i].size;
                var block = new MaterialPropertyBlock();
                block.SetColor(ColorIndex, shape[i].DefaultColor ? Color.white : shape[i].color);

                Graphics.DrawMesh(Caches.Instance.shapePreviewMesh, Matrix4x4.TRS(position, lookRotation, size),
                    Caches.Instance.shapePreviewMaterial, 0, camera, 0, block);
                
                prs.Add(new Tuple<Vector3, Quaternion, float>(position, lookRotation, speed));
            }

            // clear depth
            camera.Render();

            // draw gizmos
            Handles.SetCamera(camera);
            var index = 0;
            foreach (var (position1, rotation, speed1) in (IEnumerable<Tuple<Vector3, Quaternion, float>>) prs)
            {
                Handles.zTest = CompareFunction.Greater;
                Handles.color = Color.red;
                Handles.ArrowHandleCap(0, position1, rotation, speed1 / 4, EventType.Repaint);
                Handles.zTest = CompareFunction.LessEqual;
                Handles.color = Color.green;
                Handles.ArrowHandleCap(0, position1, rotation, speed1 / 4, EventType.Repaint);
                
                Handles.zTest = CompareFunction.Always;
                string text;
                switch (content)
                {
                    case LabelContent.Index:
                        text = index.ToString();
                        break;
                    case LabelContent.Speed:
                        text = speed1.ToString(CultureInfo.CurrentCulture);
                        break;
                    case LabelContent.Position:
                        text = position1.ToString();
                        break;
                    case LabelContent.None:
                        text = "";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Handles.Label(position1, text);
                
                index++;
            }
        }

        public enum LabelContent
        {
            None,
            Index,
            Speed,
            Position
        }

        /// <summary>
        /// Receives mouse input in given rectangle.
        /// </summary>
        /// <param name="position">Rectangle range to receive input.</param>
        /// <param name="distance">Camera distance, controlled by scroll wheel.</param>
        /// <param name="view">Camera view position, controlled by mouse drag.</param>
        public static void ReceiveInput(Rect position, ref float distance, ref Vector2 view)
        {
            var controlID = GUIUtility.GetControlID("Slider".GetHashCode(), FocusType.Passive);
            var current = Event.current;
            switch (current.GetTypeForControl(controlID))
            {
                case EventType.ScrollWheel:
                    if (position.Contains(current.mousePosition))
                    {
                        distance += current.delta.y;
                        if (distance < 0) distance = 0;
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
                        view -= current.delta / Mathf.Min(position.width, position.height) * 140f;
                        view.y = Mathf.Clamp(view.y, -90f, 90f);
                        current.Use();
                        GUI.changed = true;
                    }
                    break;
            }
        }
    }
}
