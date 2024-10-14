using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace NGCCorp.TailwindCSS
{
  public class TailwindBuilder : EditorWindow
  {
    public static string[] commandCheckNodeWindows = { "cmd.exe", "/c node -v" };
    public static string[] commandCheckNodeMacOS = { "/bin/bash", "-c \"node -v\"" };
    public static string[] commandCheckNodeLinux = { "/bin/bash", "-c \"node -v\"" };

    public static string[] commandNPMInstallNodeWindows = { "cmd.exe", "/c npm install" };
    public static string[] commandNPMInstallNodeMacOS = { "/bin/bash", "-c \"npm install\"" };
    public static string[] commandNPMInstallNodeLinux = { "/bin/bash", "-c \"npm install\"" };

    public static string[] commandInitWindows = { "cmd.exe", $"/c \"\"{Settings.tempBinaryFile}\" init --full\"" };
    public static string[] commandInitMacOS = { "/bin/bash", $"-c \"'{Settings.tempBinaryFile}' init --full\"" };
    public static string[] commandInitLinux = { "/bin/bash", $"-c \"'{Settings.tempBinaryFile}' init --full\"" };

    public static string[] commandBuildWindows = { "cmd.exe", $"/c \"tailwindcss -i \"{Settings.assetsStylesFile}\" -o \"{Settings.assetsUSSFile}\" -c \"{Settings.assetsUnityConfigFile}\"\"" };
    public static string[] commandBuildMacOS = { "/bin/bash", $"-c \"tailwindcss -i '{Settings.assetsStylesFile}' -o '{Settings.assetsUSSFile}' -c '{Settings.assetsUnityConfigFile}'\"" };
    public static string[] commandBuildLinux = { "/bin/bash", $"-c \"tailwindcss -i '{Settings.assetsStylesFile}' -o '{Settings.assetsUSSFile}' -c '{Settings.assetsUnityConfigFile}'\"" };

    [MenuItem("Tools/Tailwind/Init Tailwind", validate = true)]
    public static bool ValidateShowWindow()
    {
      // Disable the menu if the config file exists
      return !HasTailwindConfig();
    }

    [MenuItem("Tools/Tailwind/Init Tailwind")]
    public static void BuildTailwindCSS()
    {
      if (!CheckNodeInstallation())
      {
        Logger.LogError("Node.js is not installed or not available in the system PATH.");
        return;
      }

      Logger.LogInfo("Node.js is installed. Proceeding with Tailwind CSS initialization...");

      EnsureTailwindPathExists();
      CopyPackageFilesToTemp();
      CopyPackageFilesToAssets();
      RunNPMInstall();
      InitTailwind();
      TailwindConfigBuilder.AddCorePlugins();
      TailwindConfigBuilder.AddPlugins();
      TailwindConfigBuilder.AddSeperator();
      AddRequireTailwindPlugin();
      BuildCSS();
    }

    public static bool HasTailwindConfig()
    {
      return File.Exists(Settings.assetsConfigFile);
    }

    public static bool HasNodeModulesInTemp()
    {
      return File.Exists(Settings.tempBinaryPath);
    }

    private static void AddRequireTailwindPlugin()
    {
      string jsCode = $@"
        const path = require('path');
        const pluginPath = path.resolve('{Settings.tempNodeModulesTailwindPluginFile}');
        const plugin = require(pluginPath);
      ";

      string originalContent = File.ReadAllText(Settings.assetsConfigFile);
      string updatedContent = jsCode + Environment.NewLine + originalContent;

      File.WriteAllText(Settings.assetsConfigFile, updatedContent);
    }

    private static void UpdateRequireTailwindPlugin()
    {
      string configContent = File.ReadAllText(Settings.assetsConfigFile);
      string pattern = @"const pluginPath = path\.resolve\('([^']+)'\);";
      Match match = Regex.Match(configContent, pattern);

      if (!match.Success)
      {
        Console.WriteLine("Plugin path not found in the config file.");
        return;
      }

      string oldPath = match.Groups[1].Value;
      string updatedConfigContent = configContent.Replace(oldPath, Settings.tempNodeModulesTailwindPluginFile);

      File.WriteAllText(Settings.assetsConfigFile, updatedConfigContent);
    }

    public static void BuildCSS()
    {
      if (!HasTailwindConfig())
      {
        Logger.LogError("Tailwind config file not found. Use Tools/Tailwind/Init Tailwind to create it.");
        return;
      }

      if (!HasNodeModulesInTemp())
      {
        CopyPackageFilesToTemp();
        RunNPMInstall();
        UpdateRequireTailwindPlugin();
      }

      Converter.ConvertRem();
      Converter.ConvertEm();

      string[] command = SystemInfo.operatingSystemFamily switch
      {
        OperatingSystemFamily.Windows => commandBuildWindows,
        OperatingSystemFamily.MacOSX => commandBuildMacOS,
        OperatingSystemFamily.Linux => commandBuildLinux,
        _ => throw new Exception("Unknown Operating System."),
      };

      // Set up the process start information
      ProcessStartInfo processInfo = GetProcessStartInfo(command, Settings.tempBinaryPath);

      // Start the process
      RunProcess(processInfo);

      Converter.ReplaceRgbCalls();

      Logger.LogInfo("Tailwind CSS build complete!");
    }

    public static void RunNPMInstall()
    {
      string[] command = SystemInfo.operatingSystemFamily switch
      {
        OperatingSystemFamily.Windows => commandNPMInstallNodeWindows,
        OperatingSystemFamily.MacOSX => commandNPMInstallNodeMacOS,
        OperatingSystemFamily.Linux => commandNPMInstallNodeLinux,
        _ => throw new Exception("Unknown Operating System."),
      };

      // Set up the process start information
      ProcessStartInfo processInfo = GetProcessStartInfo(command, Settings.tempPath);

      // Start the process
      RunProcess(processInfo);
    }

    public static void InitTailwind()
    {
      Logger.LogInfo($"Init Tailwind in {Settings.assetsPath}");

      string[] command = SystemInfo.operatingSystemFamily switch
      {
        OperatingSystemFamily.Windows => commandInitWindows,
        OperatingSystemFamily.MacOSX => commandInitMacOS,
        OperatingSystemFamily.Linux => commandInitLinux,
        _ => throw new Exception("Unknown Operating System."),
      };

      // Set up the process start information
      ProcessStartInfo processInfo = GetProcessStartInfo(command, Settings.assetsPath);

      // Start the process
      RunProcess(processInfo);
    }

    public static void EnsureTailwindPathExists()
    {
      if (!Directory.Exists(Settings.assetsPath))
      {
        Directory.CreateDirectory(Settings.assetsPath);
      }
    }

    public static void CopyPackageFilesToTemp()
    {
      // Check for existing package.json in temp folder
      if (!File.Exists(Settings.tempPackageJSONFile))
      {
        File.Copy(Settings.packageJSONFile, Settings.tempPackageJSONFile);
      }
    }

    public static void CopyPackageFilesToAssets()
    {
      // Check for existing styles.css in assets
      if (!File.Exists(Settings.assetsStylesFile))
      {
        File.Copy(Settings.packageStylesFile, Settings.assetsStylesFile);
      }
    }

    public static bool CheckNodeInstallation()
    {
      try
      {
        string[] command = SystemInfo.operatingSystemFamily switch
        {
          OperatingSystemFamily.Windows => commandCheckNodeWindows,
          OperatingSystemFamily.MacOSX => commandCheckNodeMacOS,
          OperatingSystemFamily.Linux => commandCheckNodeLinux,
          _ => throw new Exception("Unknown Operating System."),
        };

        // Set up the process start information
        ProcessStartInfo processInfo = GetProcessStartInfo(command, null);

        // Start the process
        return RunProcess(processInfo);
      }
      catch (Exception ex)
      {
        Logger.LogError($"Exception when checking Node.js installation: {ex.Message}");
        return false;
      }
    }

    public static bool RunProcess(ProcessStartInfo processInfo)
    {
      using Process process = new();

      process.StartInfo = processInfo;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.UseShellExecute = false;

      process.OutputDataReceived += (sender, args) => Logger.LogInfo(args.Data);
      process.ErrorDataReceived += (sender, args) => Logger.LogError(args.Data);

      process.Start();
      process.BeginOutputReadLine();
      process.BeginErrorReadLine();

      process.WaitForExit();

      // Check if the process exited with an error
      if (process.ExitCode != 0)
      {
        Logger.LogError($"Process finished with exit code {process.ExitCode}. An error occurred.");

        return false;
      }
      else
      {
        Logger.LogInfo("Process completed successfully.");

        return true;
      }
    }

    public static ProcessStartInfo GetProcessStartInfo(string[] command, string workingDirectory)
    {
      return new(command[0], command[1])
      {
        WorkingDirectory = workingDirectory,
        // Redirect the output to Unity console
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
      };
    }
  }
}
