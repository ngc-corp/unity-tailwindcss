using System.IO;
using UnityEngine;

namespace NGCCorp.TailwindCSS
{
  public static class Settings
  {
    public static string prefsKey = "NGCCorp__Tailwind_UXML_Folders";

    public static string tailwindPath = CombinePaths(Application.dataPath, "TailwindCSS");
    public static string tailwindStylesFile = CombinePaths(Path.GetFullPath("Packages/com.ngc-corp.unity-tailwindcss"), "styles.css");
    public static string tailwindCorePluginsFile = CombinePaths(Path.GetFullPath("Packages/com.ngc-corp.unity-tailwindcss"), "core-plugins.js");

    // public static string tailwindStylesFile = CombinePaths(Application.dataPath, "unity-tailwindcss", "styles.css");
    // public static string tailwindCorePluginsFile = CombinePaths(Application.dataPath, "unity-tailwindcss", "core-plugins.txt");

    public static string tailwindBuildPath = CombinePaths(tailwindPath, "tailwind.uss");
    public static string tailwindConfigFile = CombinePaths(tailwindPath, "tailwind.config.js");
    public static string tailwindConfigUnityFile = CombinePaths(tailwindPath, "tailwind-unity.config.js");

    public static string CombinePaths(params string[] paths)
    {
      return Path.Combine(paths).Replace("\\", "/");
    }
  }
}
