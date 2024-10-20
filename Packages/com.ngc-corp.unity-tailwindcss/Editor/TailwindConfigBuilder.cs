using System.Collections.Generic;
using System.IO;

namespace NGCCorp.TailwindCSS
{
  public static class TailwindConfigBuilder
  {
    public static void AddCorePlugins()
    {
      string tailwindConfigFileText = File.ReadAllText(Settings.assetsUnityConfigFile);

      if (!tailwindConfigFileText.Contains("corePlugins:"))
      {
        string tailwindCorePluginsFileText = File.ReadAllText(Settings.packageCorePluginsFile);

        // Find the position of the closing bracket in module.exports
        int closingBracketIndex = tailwindConfigFileText.LastIndexOf('}');

        // Insert corePlugins before the closing bracket
        tailwindConfigFileText = tailwindConfigFileText.Insert(closingBracketIndex, $"corePlugins: {tailwindCorePluginsFileText},");

        File.WriteAllText(Settings.assetsUnityConfigFile, tailwindConfigFileText);
      }
    }

    public static void AddPlugins()
    {
      string tailwindConfigFileText = File.ReadAllText(Settings.assetsUnityConfigFile);

      if (tailwindConfigFileText.Contains("plugins: []"))
      {
        string[] files = Directory.GetFiles(Settings.pluginsPath, "*.js");
        List<string> pluginsToAdd = new();

        foreach (string file in files)
        {
          string content = File.ReadAllText(file);

          pluginsToAdd.Add(content);
        }

        string pluginList = "plugins: [\n" + string.Join(",\n", pluginsToAdd) + "\n  ]";

        tailwindConfigFileText = tailwindConfigFileText.Replace("plugins: []", pluginList);

        File.WriteAllText(Settings.assetsUnityConfigFile, tailwindConfigFileText);
      }
    }

    public static void AddSeperator()
    {
      string tailwindConfigFileText = File.ReadAllText(Settings.assetsUnityConfigFile);

      if (!tailwindConfigFileText.Contains("separator:"))
      {
        int closingBracketIndex = tailwindConfigFileText.LastIndexOf('}');

        tailwindConfigFileText = tailwindConfigFileText.Insert(closingBracketIndex, $"\nseparator: '_',");

        File.WriteAllText(Settings.assetsUnityConfigFile, tailwindConfigFileText);
      }
    }

    public static void AddContent() {
      if (!TailwindBuilder.HasTailwindConfig())
      {
        Logger.LogError("Tailwind config file not found. Use Tools/Tailwind/Init Tailwind to create it.");
        return;
      }

      List<string> uxmlFolderPaths = PersistentData.LoadPersistentConfig();

      if (uxmlFolderPaths.Count == 0)
      {
        Logger.LogError("No UXML folders selected.");
        return;
      }

      List<string> contentPaths = new();

      foreach (var path in uxmlFolderPaths)
      {
        string absoultePath = Path.GetFullPath(path);
        contentPaths.Add($"\"{absoultePath.Replace("\\", "/")}/**/*.{{uxml,cs}}\"");
      }

      string newContent = $"content: [{string.Join(", ", contentPaths)}],";
      string configContent = File.ReadAllText(Settings.assetsUnityConfigFile);
      string updatedConfigContent = System.Text.RegularExpressions.Regex.Replace(
        configContent,
        @"content: \[.*?\],",
        newContent
      );

      File.WriteAllText(Settings.assetsUnityConfigFile, updatedConfigContent);
    }
  }
}
