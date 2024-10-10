using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NGCCorp.TailwindCSS
{
  public class RemToPxConverter
  {
    public static void Convert()
    {
      // Regular expression to find rem values
      string remRegex = @"(\d*\.?\d+)rem";

      // Read the Tailwind config file
      string configFile = File.ReadAllText(Settings.tailwindConfigFile);

      // Use Regex to replace all rem values with px values
      string updatedConfigFile = Regex.Replace(configFile, remRegex, new MatchEvaluator(RemToPx));

      // Write the updated content back to the Tailwind config file
      File.WriteAllText(Settings.tailwindConfigUnityFile, updatedConfigFile);
    }

    // This function takes a rem match and converts it to px
    static string RemToPx(Match match)
    {
      // Extract the rem value (the number part)
      float remValue = float.Parse(match.Groups[1].Value);

      // Convert rem to px (1rem = 16px)
      float pxValue = remValue * 16;

      // Return the value in px as a string
      return $"{pxValue}px";
    }
  }
}
