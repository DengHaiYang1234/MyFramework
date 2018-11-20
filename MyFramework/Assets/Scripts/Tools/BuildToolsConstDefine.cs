using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Res;

public static class BuildToolsConstDefine
{
    public const string ExportDependsAtlasTextureAssetFolder = "_source";
    public const string AssetRootFolder = "Assets";
    public const string AssetSuffix = ".asset";

    public const string BuildAtlasFolder = "atlas";

    public const string BundleName = ".assetbundle";

    public static string GetBuildingFolderByResType(EResType type)
    {
        switch (type)
        {
            case EResType.Atlas:
                return BuildAtlasFolder;
        }

        return null;
    }

}
