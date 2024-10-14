using System.IO;
using UnityEditor;
using UnityEngine;

namespace NGCCorp.TailwindCSS
{
  public static class Settings
  {
    public static string prefsKey = "NGCCorp__Tailwind_UXML_Folders";
    private static bool isInitialized = false;

    // Package
    // public static string packagePath = Path.GetFullPath("Packages/com.ngc-corp.unity-tailwindcss");
    public static string packagePath = CombinePaths(Application.dataPath, "unity-tailwindcss", "Packages", "com.ngc-corp.unity-tailwindcss");
    public static string packageTailwindPath = CombinePaths(packagePath, "Tailwind");
    public static string packageStylesFile = CombinePaths(packagePath, "Tailwind", "styles.css");
    public static string packageCorePluginsFile = CombinePaths(packagePath, "Tailwind", "core-plugins.txt");
    public static string packageJSONFile = CombinePaths(packageTailwindPath, "package.json");

    // Assets
    public static string assetsPath = CombinePaths(Application.dataPath, "TailwindCSS");
    public static string assetsStylesFile = CombinePaths(assetsPath, "styles.css");
    public static string assetsUSSFile = CombinePaths(assetsPath, "tailwind.uss");
    public static string assetsConfigFile = CombinePaths(assetsPath, "tailwind.config.js");
    public static string assetsUnityConfigFile = CombinePaths(assetsPath, "tailwind-unity.config.js");

    // Temp
    public static string tempPath;
    public static string tempNodeModulesPath;
    public static string tempBinaryPath;
    public static string tempBinaryFile;
    public static string tempPackageJSONFile;
    public static string tempNodeModulesTailwindPluginFile;

    // Plugins
    public static string pluginsPath = CombinePaths(packageTailwindPath, "Plugins");

    static Settings()
    {
      if (!isInitialized)
      {
        Initialize();
        isInitialized = true;
      }
    }

    private static void Initialize()
    {
      // Create unique temp path
      tempPath = Path.GetFullPath(FileUtil.GetUniqueTempPathInProject());

      // Ensure it's treated as a directory by creating it
      if (!Directory.Exists(tempPath))
      {
        Directory.CreateDirectory(tempPath);
      }

      tempNodeModulesPath = CombinePaths(tempPath, "node_modules");
      tempNodeModulesTailwindPluginFile = CombinePaths(tempPath, "node_modules", "tailwindcss", "plugin");
      tempBinaryPath = CombinePaths(tempNodeModulesPath, ".bin");
      tempBinaryFile = CombinePaths(tempNodeModulesPath, ".bin", "tailwindcss");
      tempPackageJSONFile = CombinePaths(tempPath, "package.json");
    }

    public static string CombinePaths(params string[] paths)
    {
      return Path.Combine(paths).Replace("\\", "/");
    }
  }
}
