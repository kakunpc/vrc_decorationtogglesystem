using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ExportPackage
{
    [MenuItem("kakunvr/ExportPackage")]
    public static void CreatePackage()
    {
        AssetDatabase.ExportPackage(new[]
        {
            "Assets/kakunvr/DecoToggleSystem",
        }, "ExportedPackage.unitypackage", ExportPackageOptions.Recurse);
    }
}
