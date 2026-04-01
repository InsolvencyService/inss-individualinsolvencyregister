// See https://aka.ms/new-console-template for more information  
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;


// Updated to use Assembly.Location instead of Assembly.CodeBase to fix SYSLIB0012  
var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

// Added null check to fix CS8604  
if (path == null)
{
    throw new InvalidOperationException("The directory path could not be determined.");
}

if (args.Count() < 1)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"At least one argument is required the XML file to validate.");
    Console.ForegroundColor = ConsoleColor.White;

}
else if (!File.Exists(args[0])) 
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Specified file does not exist.");
    Console.ForegroundColor = ConsoleColor.White;
}
else
{
    var sourcefilename = args[0];

    XmlSchemaSet schema = new XmlSchemaSet();
    schema.Add("", Path.Combine(path, "eiirXMLExtractSchema_current.xsd"));
    XmlReader rd = XmlReader.Create(sourcefilename);
    XDocument doc = XDocument.Load(rd);
    doc.Validate(schema, ValidationEventHandler);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Specified file validates against schema.");
    Console.ForegroundColor = ConsoleColor.White;

}


static void ValidationEventHandler(object sender, ValidationEventArgs e)
{
    XmlSeverityType type = XmlSeverityType.Warning;
    if (Enum.TryParse<XmlSeverityType>("Error", out type))
    {
        if (type == XmlSeverityType.Error) throw new Exception(e.Message);
    }
}