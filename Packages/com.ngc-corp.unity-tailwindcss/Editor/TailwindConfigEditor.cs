using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace NGCCorp.TailwindCSS
{
  public class TailwindConfigEditor : EditorWindow
  {
    private List<string> uxmlFolderPaths = new();

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
      GUILayout.Label("Configure Tailwind Scanning", EditorStyles.boldLabel);

      // Button to select new UXML folder
      if (GUILayout.Button("Add Folder"))
      {
        string selectedFolder = EditorUtility.OpenFolderPanel("Select Folder", "", "");

        if (!string.IsNullOrEmpty(selectedFolder))
        {
          if (!uxmlFolderPaths.Contains(selectedFolder))
          {
            string relativeFolderPath = selectedFolder.Replace(Application.dataPath, "Assets");
            uxmlFolderPaths.Add(relativeFolderPath);
            SavePersistentConfig();
          }
        }
      }

      // Display already chosen UXML folders
      GUILayout.Label("Chosen Folders:", EditorStyles.boldLabel);

      for (int i = 0; i < uxmlFolderPaths.Count; i++)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(uxmlFolderPaths[i]);

        if (GUILayout.Button("Remove"))
        {
          uxmlFolderPaths.RemoveAt(i);
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
      PersistentData.SavePersistentConfig(uxmlFolderPaths);
    }

    private void LoadPersistentConfig()
    {
      uxmlFolderPaths = PersistentData.LoadPersistentConfig();
    }

    private void UpdateTailwindConfig()
    {
      TailwindBuilder.BuildCSS();
    }
  }
}
