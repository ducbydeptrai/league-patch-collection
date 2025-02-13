using System;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using YamlDotNet.Serialization.NamingConventions;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace LeaguePatchCollection;

public static partial class SystemYamlLive
{
    private static string? _gamePath;

    public static string LoadProductInstallPath()
    {
        _gamePath = GetProductInstallPath();
        if (string.IsNullOrEmpty(_gamePath))
        {
            Console.WriteLine("Using fallback product install path.");
            _gamePath = GetDefaultRiotGamesPath();
        }

        string configPath = Path.Combine(_gamePath, "Config", "system.yaml");
        CopySystemYaml(_gamePath, configPath);

        return _gamePath;
    }

    private static string? GetProductInstallPath()
    {
        try
        {
            string yamlFilePath = GetYamlFilePath();
            if (File.Exists(yamlFilePath))
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .Build();

                using var reader = new StreamReader(yamlFilePath);
                var yamlContent = deserializer.Deserialize<dynamic>(reader);
                if (yamlContent.ContainsKey("product_install_full_path"))
                {
                    string productInstallFullPath = yamlContent["product_install_full_path"];
                    return productInstallFullPath;
                }
                else
                {
                    Console.WriteLine("Product Install Full Path not found in the file.");
                }
            }
            else
            {
                Console.WriteLine("Product settings YAML file not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading or parsing the file: " + ex.Message);
        }

        return null;
    }

    private static string GetYamlFilePath()
    {
        string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        return Path.Combine(programDataPath, "Riot Games", "Metadata", "league_of_legends.live", "league_of_legends.live.product_settings.yaml");
    }

    private static string GetDefaultRiotGamesPath()
    {
        string? driveLetter = Environment.GetEnvironmentVariable("SYSTEMDRIVE");
        return Path.Combine(driveLetter ?? "C:", "Riot Games", "League of Legends");
    }

    public static void CopySystemYaml(string sourcePath, string destinationPath)
    {
        if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
        }

        string sourceFile = Path.Combine(sourcePath, "system.yaml");

        if (File.Exists(sourceFile))
        {
            File.Copy(sourceFile, destinationPath, true);
            ModifySystemYaml(destinationPath);
        }
        else
        {
            Trace.WriteLine("[WARN] Source system.yaml not found at " + sourceFile);
        }
    }

    public static void ModifySystemYaml(string configFilePath)
    {
        try
        {
            string yamlContent = File.ReadAllText(configFilePath);

            yamlContent = lcdsHost().Replace(yamlContent, "\"127.0.0.1\"");
            yamlContent = lcdsPort().Replace(yamlContent, "29154");
            yamlContent = lcdsTls().Replace(yamlContent, "false");

            File.WriteAllText(configFilePath, yamlContent);
        }
        catch (Exception ex)
        {
            Trace.WriteLine("[ERROR] Error modifying system.yaml: " + ex.Message);
        }
    }

    [GeneratedRegex(@"(?<=lcds_host\s*:\s*)\S+")]
    private static partial Regex lcdsHost();
    [GeneratedRegex(@"(?<=lcds_port\s*:\s*)\d+")]
    private static partial Regex lcdsPort();
    [GeneratedRegex(@"(?<=use_tls\s*:\s*)\btrue\b|\bfalse\b")]
    private static partial Regex lcdsTls();
}