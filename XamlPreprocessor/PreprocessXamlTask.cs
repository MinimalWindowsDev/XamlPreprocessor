using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace XamlPreprocessor
{
    public class PreprocessXamlTask : Task
    {
        [Required]
        public string DefineConstants { get; set; }

        [Required]
        public ITaskItem[] XamlFiles { get; set; }

        [Required]
        public string XsltPath { get; set; }

        public override bool Execute()
        {
            try
            {
                if (!File.Exists(XsltPath))
                {
                    Log.LogError($"XSLT file not found at {XsltPath}");
                    return false;
                }

                var xslt = new XslCompiledTransform();
                xslt.Load(XsltPath);
                var parameters = new XsltArgumentList();
                parameters.AddParam("define_constants", "", DefineConstants);

                foreach (var xamlFile in XamlFiles)
                {
                    string inputPath = xamlFile.ItemSpec;
                    Log.LogMessage(MessageImportance.Normal, $"Processing {inputPath}");

                    try
                    {
                        // Create UTF-8 encoding with BOM
                        var utf8WithBom = new UTF8Encoding(true);

                        using (var memoryStream = new MemoryStream())
                        using (var writer = new XmlTextWriter(memoryStream, utf8WithBom))
                        {
                            writer.Formatting = Formatting.Indented;
                            xslt.Transform(inputPath, parameters, writer);
                            writer.Flush();

                            // Write the processed content to file
                            File.WriteAllBytes(inputPath, memoryStream.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogError($"Error processing {inputPath}: {ex.Message}");
                        return false;
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