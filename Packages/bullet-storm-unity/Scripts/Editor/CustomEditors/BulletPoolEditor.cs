using CANStudio.BulletStorm.BulletSystem;
using UnityEditor;
using UnityEngine;

namespace CANStudio.BulletStorm.Editor.CustomEditors
{
    [CustomEditor(typeof(BulletPool))]
    internal class BulletPoolEditor : UnityEditor.Editor
    {
        private BulletPool self;
        private string bullets;
        private string inheritedBullets;

        private void OnEnable()
        {
            self = target as BulletPool;
            if (self == null) return;
            bullets = self.BulletsToString();
            inheritedBullets = self.InheritedBulletsToString();
        }

        protected override bool ShouldHideOpenButton() => true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button(new GUIContent("Detect", "Detect bullets in the folder.")))
            {
                self.Detect();
                bullets = self.BulletsToString();
                inheritedBullets = self.InheritedBulletsToString();
            }

            if (bullets != "")
            {
                EditorGUILayout.LabelField("Bullets in this pool");
                EditorGUILayout.HelpBox(bullets, MessageType.Info);
            }

            if (inheritedBullets != "")
            {
                EditorGUILayout.LabelField("Inherited bullets in this pool");
                EditorGUILayout.HelpBox(inheritedBullets, MessageType.Info);
            }
        }
        
    }
}