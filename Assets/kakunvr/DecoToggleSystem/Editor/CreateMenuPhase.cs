using System.Collections.Generic;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace kakunvr.DecoToggleSystem.Editor
{
    internal sealed class CreateMenuPhase : Pass<CreateMenuPhase>
    {
        protected override void Execute(BuildContext context)
        {
            var state = context.GetState<DecoToggleSystemState>();

            foreach (var stateAnimatorController in state.AnimatorControllers)
            {
                var decoToggleSystem = stateAnimatorController.Key;
                var controller = stateAnimatorController.Value;

                // MergeAnimatorでアニメーションを統合させる
                var mam = context.AvatarRootObject.AddComponent<ModularAvatarMergeAnimator>();
                mam.animator = controller;
                mam.pathMode = MergeAnimatorPathMode.Relative;
                mam.matchAvatarWriteDefaults = true;
                mam.deleteAttachedAnimator = true;
                
                // メニューを生成する
                var firstMenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                VRCExpressionsMenu lastMenu = firstMenu;

                // 8個までしか表示できないので、8個以上の場合は分割する
                var addMenu = new List<VRCExpressionsMenu.Control>();

                for (var i = 0; i < decoToggleSystem.Parameter.ToggleParameters.Count; ++i)
                {
                    var toggleParameter = decoToggleSystem.Parameter.ToggleParameters[i];
                    var menuName = toggleParameter.MenuName;
                    if (string.IsNullOrWhiteSpace(toggleParameter.MenuName))
                    {
                        menuName = toggleParameter.ParameterName;
                    }

                    if (string.IsNullOrWhiteSpace(toggleParameter.ParameterName))
                    {
                        continue;
                    }

                    var control = new VRCExpressionsMenu.Control()
                    {
                        icon = toggleParameter.Icon,
                        name = menuName,
                        type = VRCExpressionsMenu.Control.ControlType.Toggle,
                        parameter = new VRCExpressionsMenu.Control.Parameter()
                        {
                            name = $"DTS_{toggleParameter.ParameterName}"
                                .Replace(" ", "")
                                .Replace(".", "")
                        }
                    };
                    if (decoToggleSystem.Parameter.ToggleParameters.Count > 8)
                    {
                        if (addMenu.Count >= 7)
                        {
                            // 既に7個あるので、新しいメニューを作成しNextを追加
                            var nextMenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();

                            addMenu.Add(new VRCExpressionsMenu.Control()
                            {
                                name = "Next",
                                type = VRCExpressionsMenu.Control.ControlType.SubMenu,
                                subMenu = nextMenu
                            });
                            lastMenu.controls = addMenu;
                            addMenu = new List<VRCExpressionsMenu.Control>();
                            lastMenu = nextMenu;
                        }
                    }

                    addMenu.Add(control);
                }

                lastMenu.controls = addMenu;

                // ルートメニューを生成？
                if (decoToggleSystem.Parameter.UseRootMenu)
                {
                    // RootMenuを作成
                    var rootMenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                    rootMenu.name = decoToggleSystem.Parameter.MenuRootName;
                    var controls = new List<VRCExpressionsMenu.Control>
                    {
                        new VRCExpressionsMenu.Control()
                        {
                            icon = decoToggleSystem.Parameter.MenuRootIcon,
                            name = decoToggleSystem.Parameter.MenuRootName,
                            type = VRCExpressionsMenu.Control.ControlType.SubMenu,
                            subMenu = firstMenu
                        }
                    };
                    rootMenu.controls = controls;
                    firstMenu = rootMenu;
                }

                var maMenu =
                    VRC.Core.ExtensionMethods
                        .GetOrAddComponent<ModularAvatarMenuInstaller>(decoToggleSystem.gameObject);
                maMenu.menuToAppend = firstMenu;

                // パラメーター同期を追加
                var parameters =
                    VRC.Core.ExtensionMethods.GetOrAddComponent<ModularAvatarParameters>(decoToggleSystem.gameObject);
                foreach (var toggleParameter in decoToggleSystem.Parameter.ToggleParameters)
                {
                    parameters.parameters.Add(new ParameterConfig()
                    {
                        nameOrPrefix = $"DTS_{toggleParameter.ParameterName}".Replace(" ", "").Replace(".", ""),
                        syncType = ParameterSyncType.Bool,
                        saved = toggleParameter.SaveValue,
                        internalParameter = false,
                        defaultValue = toggleParameter.DefaultValue ? 1 : 0
                    });

                }

                // 処理完了ということで消す
                if (EditorApplication.isPlaying)
                {
                    Object.Destroy(decoToggleSystem);
                }
            }
        }
    }
}
