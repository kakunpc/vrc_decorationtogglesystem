using nadena.dev.ndmf;
using UnityEditor.Animations;
using UnityEngine;

namespace kakunvr.DecoToggleSystem.Editor
{
    internal sealed class CreateAnimationPhase : Pass<CreateAnimationPhase>
    {
        protected override void Execute(BuildContext context)
        {
            var decoToggleSystems = context.AvatarRootObject.GetComponentsInChildren<DecoToggleSystem>();
            var state = context.GetState<DecoToggleSystemState>();
            foreach (var decoToggleSystem in decoToggleSystems)
            {
                var controller = new AnimatorController() { name = "DTS Controller " + decoToggleSystem.name };

                foreach (var toggleParameter in decoToggleSystem.Parameter.ToggleParameters)
                {
                    if (string.IsNullOrWhiteSpace(toggleParameter.ParameterName))
                    {
                        continue;
                    }
                    
                    var onClip = CreateAnimationClip(toggleParameter, context.AvatarRootObject.transform, true);
                    var offClip = CreateAnimationClip(toggleParameter, context.AvatarRootObject.transform, false);
                    var paramName = $"DTS_{toggleParameter.ParameterName}".Replace(" ", "").Replace(".", "");

                    var layer = new AnimatorControllerLayer
                    {
                        name = controller.MakeUniqueLayerName(paramName),
                        defaultWeight = 1f
                    };

                    layer.stateMachine = new AnimatorStateMachine
                    {
                        name = layer.name,
                        hideFlags = HideFlags.HideInHierarchy
                    };
                    var onState = layer.stateMachine.AddState($"{paramName}_ON", new Vector3(0, 300, 0));
                    onState.motion = onClip;
                    var offState = layer.stateMachine.AddState($"{paramName}_OFF", new Vector3(500, 300, 0));
                    offState.motion = offClip;

                    layer.stateMachine.defaultState = onState;

                    controller.AddParameter(paramName, AnimatorControllerParameterType.Bool);

                    // on offの遷移を設定する
                    var transition = onState.AddTransition(offState);
                    transition.exitTime = 0f;
                    transition.hasExitTime = false;
                    transition.hasFixedDuration = true;
                    transition.duration = 0f;
                    transition.AddCondition(AnimatorConditionMode.IfNot, 0, paramName);
                    
                    transition = offState.AddTransition(onState);
                    transition.exitTime = 0f;
                    transition.hasExitTime = false;
                    transition.hasFixedDuration = true;
                    transition.duration = 0f;
                    transition.AddCondition(AnimatorConditionMode.If, 0, paramName);
                    layer.stateMachine.AddAnyStateTransition(onState);

                    controller.AddLayer(layer);
                }

                state.AnimatorControllers[decoToggleSystem] = controller;
            }
        }

        private AnimationClip CreateAnimationClip(ToggleParameter toggleParameter, Transform root, bool state)
        {
            var stateName = state ? "ON" : "OFF";

            var clip = new AnimationClip() { name = (toggleParameter.ParameterName + stateName).Replace(" ", "") };

            foreach (var targetObject in toggleParameter.TargetObjects)
            {
                var child = targetObject;
                var curve = new AnimationCurve();

                var value = child.activeInHierarchy ? 1 : 0;
                // 状態を反転
                if (!state)
                {
                    value = value == 1 ? 0 : 1;
                }

                curve.AddKey(0f, value);
                curve.AddKey(1f / clip.frameRate, value);
                clip.SetCurve(GetRelativePath(root, child.transform), typeof(GameObject), "m_IsActive", curve);
            }

            return clip;
        }
        
        private string GetRelativePath(Transform root, Transform target)
        {
            var path = target.name;
            var parent = target.parent;
            while (parent != null && parent != root)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }

    }
}