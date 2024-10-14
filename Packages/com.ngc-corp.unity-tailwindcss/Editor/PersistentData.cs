using UnityEditor;
using System.Collections.Generic;

namespace NGCCorp.TailwindCSS
{
  public static class PersistentData
  {
    public static void SavePersistentConfig(List<string> uxmlFolderPaths)
    {
      EditorPrefs.SetString(Settings.prefsKey, string.Join(";", uxmlFolderPaths.ToArray()));
    }

    public static List<string> LoadPersistentConfig()
    {
      if (EditorPrefs.HasKey(Settings.prefsKey))
      {
        string savedPaths = EditorPrefs.GetString(Settings.prefsKey);

        return new List<string>(savedPaths.Split(';'));
      }

      return null;
    }
  }
}
