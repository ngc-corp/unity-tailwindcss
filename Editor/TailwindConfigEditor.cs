using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Zom;

namespace NGCCorp.TailwindCSS
{
  public class TailwindConfigEditor : EditorWindow
  {
    private List<string> uxmlFolderPaths = new();

    [MenuItem("Tools/Tailwind/Configure Tailwind", validate = true)]
    public static bool ValidateShowWindow()
    {
      // Disable the menu if the config file does not exist
      return File.Exists(TailwindBuilder.tailwindConfigFile);
    }

    [MenuItem("Tools/Tailwind/Configure Tailwind")]
    public static void ShowWindow()
    {
      TailwindConfigEditor window = GetWindow<TailwindConfigEditor>("Tailwind Config Editor");
      window.LoadPersistentConfig();
    }

    private void OnGUI()
    {
      GUILayout.Label("Configure Tailwind UXML Scanning", EditorStyles.boldLabel);

      // Button to select new UXML folder
      if (GUILayout.Button("Add UXML Folder"))
      {
        string selectedFolder = EditorUtility.OpenFolderPanel("Select UXML Folder", "", "");

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
      GUILayout.Label("Chosen UXML Folders:", EditorStyles.boldLabel);

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

    // Save chosen directories using EditorPrefs
    private void SavePersistentConfig()
    {
      EditorPrefs.SetString(Settings.prefsKey, string.Join(";", uxmlFolderPaths.ToArray()));
      Logger.LogInfo("UXML folder paths saved.");
    }

    // Load saved directories on window open
    private void LoadPersistentConfig()
    {
      if (EditorPrefs.HasKey(Settings.prefsKey))
      {
        string savedPaths = EditorPrefs.GetString(Settings.prefsKey);
        uxmlFolderPaths = new List<string>(savedPaths.Split(';'));
      }
    }

    private void UpdateTailwindConfig()
    {
      if (uxmlFolderPaths.Count == 0)
      {
        Logger.LogError("No UXML folders selected.");
        return;
      }

      string configFile = TailwindBuilder.tailwindConfigFile;

      if (!File.Exists(configFile))
      {
        Logger.LogError("Tailwind config file not found. Use Tools/Tailwind/Init Tailwind to create it.");
        return;
      }

      // Prepare content section for tailwind.config.js
      List<string> contentPaths = new List<string>();

      foreach (var path in uxmlFolderPaths)
      {
        contentPaths.Add($"\"{path.Replace("\\", "/")}/**/*.{{uxml,cs}}\"");
      }

      string newContent = $"content: [{string.Join(", ", contentPaths)}],";

      // Update tailwind.config.js
      string configContent = File.ReadAllText(configFile);
      string updatedConfigContent = System.Text.RegularExpressions.Regex.Replace(
        configContent,
        @"content: \[.*?\],",
        newContent
      );

      File.WriteAllText(configFile, updatedConfigContent);
      Logger.LogInfo("Tailwind config updated successfully!");

      TailwindBuilder.BuildCSS();
    }
  }
}
