using System;
using UnityEngine;
using VRC.SDKBase;

namespace kakunvr.DecoToggleSystem
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar/Deco Toggle System")]
    public sealed class DecoToggleSystem : MonoBehaviour, IEditorOnly
    {
        [SerializeField] private DTSParameter _parameter = new DTSParameter();

        public DTSParameter Parameter => _parameter;
    }
}
