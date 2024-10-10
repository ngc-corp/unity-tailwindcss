using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NGCCorp.TailwindCSS
{
  public class Converter
  {
    // Conversion factor for rem to px (Assuming 1rem = 16px)
    private const double remToPxFactor = 16.0;

    public static void ConvertRem()
    {
      // Regular expression to find rem values
      string remRegex = @"'(\d*\.?\d+)rem'";

      // Read the Tailwind config file
      string configFile = File.ReadAllText(Settings.tailwindConfigFile);

      // Use Regex to replace all rem values with px values
      string updatedConfigFile = Regex.Replace(configFile, remRegex, new MatchEvaluator(RemToPx));

      // Write the updated content back to the Tailwind config file
      File.WriteAllText(Settings.tailwindConfigUnityFile, updatedConfigFile);
    }

    public static void ConvertEm()
    {
      // Regular expression to find rem values
      string remRegex = @"(\d*\.?\d+)em";

      // Read the Tailwind config file
      string configFile = File.ReadAllText(Settings.tailwindConfigUnityFile);

      // Use Regex to replace all rem values with px values
      string updatedConfigFile = Regex.Replace(configFile, remRegex, new MatchEvaluator(EmToPx));

      // Write the updated content back to the Tailwind config file
      File.WriteAllText(Settings.tailwindConfigUnityFile, updatedConfigFile);
    }

    // This function takes a rem match and converts it to px
    static string RemToPx(Match match)
    {
      // Extract the rem value (the number part)
      double remValue = double.Parse(match.Groups[1].Value);

      // Convert rem to px (1rem = 16px)
      double pxValue = remValue * remToPxFactor;

      // Return the value in px as a string
      return $"'{pxValue}px'";
    }

    // This function takes a em match and converts it to px
    static string EmToPx(Match match)
    {
      // Extract the rem value (the number part)
      double emValue = double.Parse(match.Groups[1].Value);

      // Convert em to px (1em = 1px)
      double pxValue = emValue;

      // Return the value in px as a string
      return $"{pxValue}px";
    }
  }
}