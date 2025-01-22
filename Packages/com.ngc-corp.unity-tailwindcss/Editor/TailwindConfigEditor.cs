using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace NGCCorp.TailwindCSS
{
  public class TailwindConfigEditor : EditorWindow
  {
    private List<string> UxmlFolderPaths = new();
    private bool Debug = true;

    [MenuItem("Tools/Tailwind/Configure Tailwind", validate = true)]
    public static bool ValidateShowWindow()
    {
      // Disable the menu if the config file does not exist
      return File.Exists(Settings.assetsConfigFile);
    }

    [MenuItem("Tools/Tailwind/Configure Tailwind")]
    public static void ShowWindow()
    {
      TailwindConfigEditor window = GetWindow<TailwindConfigEditor>("Tailwind Config Editor");
      window.LoadPersistentConfig();
    }

    private void OnGUI()
    {
      GUILayout.Label("Configure Tailwind", EditorStyles.boldLabel);
      Debug = GUILayout.Toggle(Debug, "Show debug output");

      if (GUI.changed)
      {
        SavePersistentConfig();
      }

      GUILayout.Label("Configure Tailwind Scanning", EditorStyles.boldLabel);

      // Button to select new UXML folder
      if (GUILayout.Button("Add Folder"))
      {
        string selectedFolder = EditorUtility.OpenFolderPanel("Select Folder", "", "");

        if (!string.IsNullOrEmpty(selectedFolder))
        {
          if (!UxmlFolderPaths.Contains(selectedFolder))
          {
            string relativeFolderPath = selectedFolder.Replace(Application.dataPath, "Assets");
            UxmlFolderPaths.Add(relativeFolderPath);
            SavePersistentConfig();
          }
        }
      }

      // Display already chosen UXML folders
      GUILayout.Label("Chosen Folders:", EditorStyles.boldLabel);

      for (int i = 0; i < UxmlFolderPaths.Count; i++)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(UxmlFolderPaths[i]);

        if (GUILayout.Button("Remove"))
        {
          UxmlFolderPaths.RemoveAt(i);
          SavePersistentConfig();
        }

        GUILayout.EndHorizontal();
      }

      // Button to update the Tailwind config file
      if (GUILayout.Button("Update Tailwind Config"))
      {
        UpdateTailwindConfig();
      }
    }

    private void SavePersistentConfig()
    {
      PersistentData.SavePersistentConfig(Settings.prefsKeyFolders, string.Join(";", UxmlFolderPaths.ToArray()));
      PersistentData.SavePersistentConfig(Settings.prefsKeyDebug, Debug);

      Settings.showDebug = Debug;
    }

    private void LoadPersistentConfig()
    {
      string folders = PersistentData.LoadPersistentConfig<string>(Settings.prefsKeyFolders);

      if (!folders.Equals(default))
      {
        UxmlFolderPaths = folders.Split(";").ToList();
      }

      bool debug = PersistentData.LoadPersistentConfig<bool>(Settings.prefsKeyDebug);

      if (!debug.Equals(default))
      {
        Settings.showDebug = Debug;
        Debug = debug;
      }
    }

    private void UpdateTailwindConfig()
    {
      TailwindBuilder.BuildCSS();
    }
  }
}
