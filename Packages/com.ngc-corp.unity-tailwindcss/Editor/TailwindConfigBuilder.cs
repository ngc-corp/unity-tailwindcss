using System.IO;

namespace NGCCorp.TailwindCSS
{
  public static class TailwindConfigBuilder
  {
    public static void AddCorePlugins()
    {
      string tailwindConfigFileText = File.ReadAllText(Settings.assetsConfigFile);

      if (!tailwindConfigFileText.Contains("corePlugins:"))
      {
        string tailwindCorePluginsFileText = File.ReadAllText(Settings.packageCorePluginsFile);

        // Find the position of the closing bracket in module.exports
        int closingBracketIndex = tailwindConfigFileText.LastIndexOf('}');

        // Insert corePlugins before the closing bracket
        tailwindConfigFileText = tailwindConfigFileText.Insert(closingBracketIndex, $"corePlugins: {tailwindCorePluginsFileText},");

        File.WriteAllText(Settings.assetsConfigFile, tailwindConfigFileText);
      }
    }

    public static void AddPlugins()
    {
      string tailwindConfigFileText = File.ReadAllText(Settings.assetsConfigFile);

      if (tailwindConfigFileText.Contains("plugins: []"))
      {
        string pluginTextAlign = File.ReadAllText(Settings.CombinePaths(Settings.pluginsPath, "text-align.js"));
        string[] pluginsToAdd = new string[]
        {
          pluginTextAlign,
        };
        string pluginList = "plugins: [\n" + string.Join(",\n", pluginsToAdd) + "\n  ]";

        tailwindConfigFileText = tailwindConfigFileText.Replace("plugins: []", pluginList);

        File.WriteAllText(Settings.assetsConfigFile, tailwindConfigFileText);
      }
    }

    public static void AddSeperator()
    {
      string tailwindConfigFileText = File.ReadAllText(Settings.assetsConfigFile);

      if (!tailwindConfigFileText.Contains("separator:"))
      {
        int closingBracketIndex = tailwindConfigFileText.LastIndexOf('}');

        tailwindConfigFileText = tailwindConfigFileText.Insert(closingBracketIndex, $"\nseparator: '_',");

        File.WriteAllText(Settings.assetsConfigFile, tailwindConfigFileText);
      }
    }
  }
}
