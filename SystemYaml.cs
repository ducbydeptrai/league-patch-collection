using System;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using YamlDotNet.Serialization.NamingConventions;
using System.Text.RegularExpressions;

namespace LeaguePatchCollection;

public static class SystemYamlLive
{
    private static string _gamePath;

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

    private static string GetProductInstallPath()
    {
        try
        {
            string yamlFilePath = GetYamlFilePath();
            if (File.Exists(yamlFilePath))
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .Build();

                using (var reader = new StreamReader(yamlFilePath))
                {
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
        if (OperatingSystem.IsMacOS())
        {
            return "/Users/Shared/Riot Games/Metadata/league_of_legends.live/league_of_legends.live.product_settings.yaml";
        }
        else
        {
            string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            return Path.Combine(programDataPath, "Riot Games", "Metadata", "league_of_legends.live", "league_of_legends.live.product_settings.yaml");
        }
    }

    private static string GetDefaultRiotGamesPath()
    {
        if (OperatingSystem.IsMacOS())
        {
            return "/Applications/League of Legends.app";
        }
        else
        {
            string driveLetter = Environment.GetEnvironmentVariable("SYSTEMDRIVE");
            return Path.Combine(driveLetter ?? "C:", "Riot Games", "League of Legends");
        }
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
            Console.WriteLine("Source system.yaml not found at " + sourceFile);
        }
    }

    public static void ModifySystemYaml(string configFilePath)
    {
        try
        {
            string yamlContent = File.ReadAllText(configFilePath);

            yamlContent = Regex.Replace(yamlContent, @"(?<=lcds_host\s*:\s*)\S+", "\"127.0.0.1\"");
            yamlContent = Regex.Replace(yamlContent, @"(?<=lcds_port\s*:\s*)\d+", "29154");
            yamlContent = Regex.Replace(yamlContent, @"(?<=use_tls\s*:\s*)\btrue\b|\bfalse\b", "false");

            File.WriteAllText(configFilePath, yamlContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error modifying system.yaml: " + ex.Message);
        }
    }
}