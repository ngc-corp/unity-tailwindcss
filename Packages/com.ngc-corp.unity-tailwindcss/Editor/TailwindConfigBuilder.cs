using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    public static void AddContent()
    {
      string folders = PersistentData.LoadPersistentConfig<string>(Settings.prefsKeyFolders);

      if (!folders.Equals(default))
      {
        List<string> uxmlFolderPaths = folders.Split(";").ToList();

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
        string tailwindConfigFileText = File.ReadAllText(Settings.assetsUnityConfigFile);
        string updatedTailwindConfigFileText = System.Text.RegularExpressions.Regex.Replace(
          tailwindConfigFileText,
          @"content: \[.*?\],",
          newContent
        );

        File.WriteAllText(Settings.assetsUnityConfigFile, updatedTailwindConfigFileText);
      }
    }

    public static void UpdateTransitionTimingFunction()
    {
      string tailwindConfigFileText = File.ReadAllText(Settings.assetsUnityConfigFile);
      string oldConfigStart = "transitionTimingFunction: {";
      string oldConfigEnd = "},";
      string newConfig = @"
        transitionTimingFunction: {
          DEFAULT: 'ease-in-out',
          linear: 'linear',
          in: 'ease-in',
          out: 'ease-out',
          'in-out': 'ease-in-out',
          'in-sine': 'ease-in-sine',
          'out-sine': 'ease-out-sine',
          'in-out-sine': 'ease-in-out-sine',
          'in-cubic': 'ease-in-cubic',
          'out-cubic': 'ease-out-cubic',
          'in-out-cubic': 'ease-in-out-cubic',
          'in-back': 'ease-in-back',
          'out-back': 'ease-out-back',
          'in-out-back': 'ease-in-out-back',
          'in-bounce': 'ease-in-bounce',
          'out-bounce': 'ease-out-bounce',
          'in-out-bounce': 'ease-in-out-bounce',
        },";

      int startIndex = tailwindConfigFileText.IndexOf(oldConfigStart);
      int endIndex = tailwindConfigFileText.IndexOf(oldConfigEnd, startIndex) + oldConfigEnd.Length;

      if (startIndex != -1 && endIndex > startIndex)
      {
        string oldConfig = tailwindConfigFileText.Substring(startIndex, endIndex - startIndex);
        string updatedTailwindConfigFileText = tailwindConfigFileText.Replace(oldConfig, newConfig);

        File.WriteAllText(Settings.assetsUnityConfigFile, updatedTailwindConfigFileText);
      }
    }

    public static void UpdateTransitionProperty()
    {
      string tailwindConfigFileText = File.ReadAllText(Settings.assetsUnityConfigFile);
      string oldConfigStart = "transitionProperty: {";
      string oldConfigEnd = "},";
      string newConfig = @"
        transitionProperty: {
          none: 'none',
          all: 'all',
          DEFAULT:
            'color, background-color, border-color, opacity',
          colors: 'color, background-color, border-color',
          opacity: 'opacity',
          shadow: 'text-shadow',
        },";

      int startIndex = tailwindConfigFileText.IndexOf(oldConfigStart);
      int endIndex = tailwindConfigFileText.IndexOf(oldConfigEnd, startIndex) + oldConfigEnd.Length;

      if (startIndex != -1 && endIndex > startIndex)
      {
        string oldConfig = tailwindConfigFileText.Substring(startIndex, endIndex - startIndex);
        string updatedTailwindConfigFileText = tailwindConfigFileText.Replace(oldConfig, newConfig);

        File.WriteAllText(Settings.assetsUnityConfigFile, updatedTailwindConfigFileText);
      }
    }
  }
}
