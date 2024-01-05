using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace kakunvr.DecoToggleSystem.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DecoToggleSystem))]
    public sealed class DecoToggleSystemEditorMenu : UnityEditor.Editor
    {
        private SerializedProperty toggleParameters;

        private void OnEnable()
        {
            var parameter = serializedObject.FindProperty("_parameter");
            toggleParameters = parameter.FindPropertyRelative("ToggleParameters");

            // メニュー名が空なら自動で入れる

            var decoToggleSystem = target as DecoToggleSystem;
            if (decoToggleSystem == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(decoToggleSystem.Parameter.MenuRootName))
            {
                decoToggleSystem.Parameter.MenuRootName = target.name;
                serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnInspectorGUI()
        {
            var decoToggleSystem = target as DecoToggleSystem;
            if (decoToggleSystem == null)
            {
                return;
            }

            var parameter = decoToggleSystem.Parameter ?? new DTSParameter();

            var isForceCreateMenu = parameter.ToggleParameters.Count >= 8;

            EditorGUI.BeginDisabledGroup(isForceCreateMenu);
            parameter.UseRootMenu = EditorGUILayout.Toggle("Use Root Menu", parameter.UseRootMenu);
            if (isForceCreateMenu)
            {
                parameter.UseRootMenu = true;
            }
            EditorGUI.EndDisabledGroup();
            if (parameter.UseRootMenu)
            {
                parameter.MenuRootName = EditorGUILayout.TextField("Menu Root Name", parameter.MenuRootName);
                parameter.MenuRootIcon =
                    EditorGUILayout.ObjectField("Menu Root Icon", parameter.MenuRootIcon, typeof(Texture2D), false) as
                        Texture2D;
            }

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(toggleParameters, true);
            
            EditorGUILayout.Separator();
            
            if(GUILayout.Button("自動追加"))
            {
                AddAutoParameter();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void AddAutoParameter()
        {
            // ダイアログで確認する
            if (!EditorUtility.DisplayDialog("DecoToggleSystem", "各子供のメニューを自動追加しますか？\n既に追加されてるパラメーターは削除されます。", "OK",
                    "Cancel"))
            {
                return;
            }

            var decoToggleSystem = target as DecoToggleSystem;
            if (decoToggleSystem == null)
            {
                return;
            }

            // 既存のパラメータを削除する
            decoToggleSystem.Parameter.ToggleParameters.Clear();

            var childs = decoToggleSystem.transform.childCount;
            for (var i = 0; i < childs; ++i)
            {
                var child = decoToggleSystem.transform.GetChild(i);
                
                // Armatureは無視
                if (child.name.ToLower().Contains("armature"))
                {
                    continue;
                }

                var toggleParameter = new ToggleParameter()
                {
                    MenuName = child.name,
                    ParameterName = child.name
                        .Replace(" ","")
                        .Replace(".","")
                        .Replace("-","")
                        .Replace("_","")
                        .Replace("(","")
                        .Replace(")",""),
                    Icon = null,
                    DefaultValue = true,
                    SaveValue = true,
                    TargetObjects = new List<GameObject> { child.gameObject }
                };

                decoToggleSystem.Parameter.ToggleParameters.Add(toggleParameter);
            }
        }
    }
}