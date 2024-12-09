using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Xml.Xsl;

namespace XamlPreprocessor
{
    public class PreprocessXamlTask : Task
    {
        [Required]
        public string DefineConstants { get; set; }

        [Required]
        public ITaskItem[] XamlFiles { get; set; }

        public override bool Execute()
        {
            try
            {
                string xsltPath = Path.Combine(
                    Path.GetDirectoryName(typeof(PreprocessXamlTask).Assembly.Location),
                    "preprocess.xslt");

                if (!File.Exists(xsltPath))
                {
                    Log.LogError($"XSLT file not found at {xsltPath}");
                    return false;
                }

                foreach (var xamlFile in XamlFiles)
                {
                    string inputPath = xamlFile.ItemSpec;
                    Log.LogMessage(MessageImportance.Normal, $"Processing {inputPath}");

                    // Create a backup of original file
                    string backupPath = inputPath + ".original";
                    File.Copy(inputPath, backupPath, true);

                    try
                    {
                        // Load XSLT
                        var xslt = new XslCompiledTransform();
                        xslt.Load(xsltPath);

                        // Setup transform parameters
                        var parameters = new XsltArgumentList();
                        parameters.AddParam("define_constants", "", DefineConstants);

                        // Transform the file in place
                        string tempPath = inputPath + ".temp";
                        using (var writer = new StreamWriter(tempPath))
                        {
                            xslt.Transform(inputPath, parameters, writer);
                        }

                        // Replace original with transformed
                        File.Delete(inputPath);
                        File.Move(tempPath, inputPath);
                    }
                    catch (Exception ex)
                    {
                        // Restore from backup on error
                        File.Copy(backupPath, inputPath, true);
                        Log.LogError($"Error processing {inputPath}: {ex.Message}");
                        return false;
                    }
                    finally
                    {
                        // Cleanup backup
                        if (File.Exists(backupPath))
                            File.Delete(backupPath);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error in XAML preprocessing: {ex.Message}");
                return false;
            }
        }
    }
}