using System;
using System.Reflection;
using Code.StaticData;
using UnityEditor;
using UnityEngine;

namespace Editor.Code
{
    [CustomEditor(typeof(VersionInfo))]
    public class VersionInfoEditor : UnityEditor.Editor
    {
        private const string VersionFieldName = "_version";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var config = (VersionInfo)target;

            if(GUILayout.Button("Set Version"))
            {
                SetVersion(config);
                EditorUtility.SetDirty(config);
            }
        }

        private void SetVersion(VersionInfo config)
        {
            string version = Application.version;
            
            FieldInfo versionField = typeof(VersionInfo).GetField(
                VersionFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (versionField == null)
            {
                Debug.LogError("Version field name has been changed science last amend to editor tool");
                return;
            }
            versionField.SetValue(config, version);
        }
    }
}