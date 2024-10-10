using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NGCCorp.TailwindCSS
{
  public class TailwindBuilder : EditorWindow
  {
    public static string[] commandCheckNodeWindows = { "cmd.exe", "/c node -v" };
    public static string[] commandCheckNodeMacOS = { "/bin/bash", "-c \"node -v\"" };
    public static string[] commandCheckNodeLinux = { "/bin/bash", "-c \"node -v\"" };

    public static string[] commandInitWindows = { "cmd.exe", $"/c npx --yes tailwindcss init --full" };
    public static string[] commandInitMacOS = { "/bin/bash", $"-c \"npx --yes tailwindcss init --full\"" };
    public static string[] commandInitLinux = { "/bin/bash", $"-c \"npx --yes tailwindcss init --full\"" };

    public static string[] commandBuildWindows = { "cmd.exe", $"/c npx --yes tailwindcss -i \"{Settings.tailwindStylesFile}\" -o \"{Settings.tailwindBuildPath}\" -c \"{Settings.tailwindConfigUnityFile}\"" };
    public static string[] commandBuildMacOS = { "/bin/bash", $"-c \"npx --yes tailwindcss -i '{Settings.tailwindStylesFile}' -o '{Settings.tailwindBuildPath}' -c '{Settings.tailwindConfigUnityFile}'\"" };
    public static string[] commandBuildLinux = { "/bin/bash", $"-c \"npx --yes tailwindcss -i '{Settings.tailwindStylesFile}' -o '{Settings.tailwindBuildPath}' -c '{Settings.tailwindConfigUnityFile}'\"" };

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
      InitTailwind();
      CorePlugins.AddCorePlugins();
      BuildCSS();
    }

    public static bool HasTailwindConfig() {
      return File.Exists(Settings.tailwindConfigFile);
    }

    public static void BuildCSS() {
      if (!HasTailwindConfig()) {
        Logger.LogError("Tailwind config file not found. Use Tools/Tailwind/Init Tailwind to create it.");
        return;
      }

      RemToPxConverter.Convert();

      string[] command = SystemInfo.operatingSystemFamily switch
      {
        OperatingSystemFamily.Windows => commandBuildWindows,
        OperatingSystemFamily.MacOSX => commandBuildMacOS,
        OperatingSystemFamily.Linux => commandBuildLinux,
        _ => throw new Exception("Unknown Operating System."),
      };

      // Set up the process start information
      ProcessStartInfo processInfo = GetProcessStartInfo(command, null);

      // Start the process
      RunProcess(processInfo);

      Logger.LogInfo("Tailwind CSS build complete!");
    }

    public static void InitTailwind()
    {
      Logger.LogInfo($"Init Tailwind in {Settings.tailwindPath}");

      string[] command = SystemInfo.operatingSystemFamily switch
      {
        OperatingSystemFamily.Windows => commandInitWindows,
        OperatingSystemFamily.MacOSX => commandInitMacOS,
        OperatingSystemFamily.Linux => commandInitLinux,
        _ => throw new Exception("Unknown Operating System."),
      };

      // Set up the process start information
      ProcessStartInfo processInfo = GetProcessStartInfo(command, Settings.tailwindPath);

      // Start the process
      RunProcess(processInfo);
    }

    public static void EnsureTailwindPathExists()
    {
      if (!Directory.Exists(Settings.tailwindPath))
      {
        Directory.CreateDirectory(Settings.tailwindPath);
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

    public static bool RunProcess(ProcessStartInfo processInfo) {
      using Process process = new();

      process.StartInfo = processInfo;

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
