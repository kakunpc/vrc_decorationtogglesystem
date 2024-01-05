using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace kakunvr.DecoToggleSystem.Editor
{
    public class DecoToggleSystemState
    {
        public Dictionary<DecoToggleSystem, AnimatorController> AnimatorControllers =
            new Dictionary<DecoToggleSystem, AnimatorController>();
    }
}
