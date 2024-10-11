using System.IO;

namespace NGCCorp.TailwindCSS
{
  public static class CorePlugins
  {
    public static void AddCorePlugins()
    {
      string tailwindConfigFileText = File.ReadAllText(Settings.tailwindConfigFile);

      if (!tailwindConfigFileText.Contains("corePlugins"))
      {
        string tailwindCorePluginsFileText = File.ReadAllText(Settings.tailwindCorePluginsFile);

        // Find the position of the closing bracket in module.exports
        int closingBracketIndex = tailwindConfigFileText.LastIndexOf('}');

        // Insert corePlugins before the closing bracket
        tailwindConfigFileText = tailwindConfigFileText.Insert(closingBracketIndex, $"corePlugins: {tailwindCorePluginsFileText}");

        File.WriteAllText(Settings.tailwindConfigFile, tailwindConfigFileText);
      }
    }
  }
}
