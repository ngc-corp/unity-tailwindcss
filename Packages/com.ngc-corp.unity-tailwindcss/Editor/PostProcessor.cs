using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace NGCCorp.TailwindCSS {
  public class UXMLPostprocessor : AssetPostprocessor
  {
    // This method is called when any asset is changed, imported, deleted, or moved
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
      // Retrieve the list of monitored folders from EditorPrefs
      List<string> monitoredFolders = GetMonitoredFolders();

      // Check imported assets
      foreach (string assetPath in importedAssets)
      {
        if (IsMonitoredAsset(assetPath, monitoredFolders))
        {
          // Trigger Tailwind CSS regeneration
          TailwindBuilder.BuildCSS();

          break; // Rebuild only once per set of changes
        }
      }
    }

    // Retrieves monitored folders from EditorPrefs
    private static List<string> GetMonitoredFolders()
    {
      if (EditorPrefs.HasKey(Settings.prefsKey))
      {
        string savedPaths = EditorPrefs.GetString(Settings.prefsKey);
        return new List<string>(savedPaths.Split(';'));
      }
      return new List<string>();
    }

    // Checks if the asset is a UXML or C# file and is within the monitored folders
    private static bool IsMonitoredAsset(string assetPath, List<string> monitoredFolders)
    {
      string extension = Path.GetExtension(assetPath);
      bool isUXMLorCS = extension == ".uxml" || extension == ".cs";

      // Check if the asset path starts with any of the monitored folder paths
      bool isInMonitoredFolder = monitoredFolders.Exists(folder => assetPath.StartsWith(folder.Replace("\\", "/")));

      return isUXMLorCS && isInMonitoredFolder;
    }
  }
}
