using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace kakunvr.DecoToggleSystem
{
    [Serializable]
    public sealed class DTSParameter
    {
        [SerializeField] public bool UseRootMenu = true;

        [SerializeField] public string MenuRootName = "";
        
        [SerializeField] public Texture2D MenuRootIcon;

        [SerializeField] public List<ToggleParameter> ToggleParameters = new List<ToggleParameter>();
    }

    [Serializable]
    public sealed class ToggleParameter
    {
        [SerializeField] public string MenuName;
        [SerializeField] public string ParameterName;

        [SerializeField] public Texture2D Icon;

        [SerializeField] public bool DefaultValue = true;
        [SerializeField] public bool SaveValue = true;

        [SerializeField] public List<GameObject> TargetObjects;
    }
}