using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Pandora.Core;

namespace Pandora.MVVM.Model
{
    public class NemesisModInfo : IModInfo
    {

        public bool Active { get; set; } = false;

        public DirectoryInfo Folder { get; private set;  }

        public Dictionary<string, string> StringProperties { get; private set; } = new Dictionary<string, string>();

        public string Name { get; set; } = "Default";

        public string Author { get; set; } = "Default";
        public string URL { get; set; } = "Default";

        public string Code { get; set; } = "Default";

        public Version Version { get; } = new Version(1,0,0);

        //internal string Auto { get; set; } = "Default";
        //internal string RequiredFile { get; set; } = "Default";
        //internal string FileToCopy { get; set; } = "Default";
        //internal bool Hidden { get; set; } = false;
        public bool Valid { get; set; } = false;

        public NemesisModInfo()
        {
            Valid = false;
            Folder = new DirectoryInfo(Directory.GetCurrentDirectory()); 
        }
        public NemesisModInfo(DirectoryInfo folder, string name, string author, string url, Dictionary<string, string> properties)
        { 
            Folder = folder;
            Name = name;
            Author = author; 
            URL  = url;
            StringProperties = properties;  
            Code = Folder.Name;
            Valid = true; 
        }
        public static NemesisModInfo ParseMetadata(DirectoryInfo folder)
        {

            StringBuilder pathBuilder = new StringBuilder(folder.FullName);
            pathBuilder.Append('\\');
            pathBuilder.Append("info.ini");
			Dictionary<string, string> properties = new Dictionary<string, string>();

			string infoPath = pathBuilder.ToString();

            

            if (!File.Exists(infoPath))
            {
                return new NemesisModInfo();
            }
			using (StreamReader reader = new StreamReader(infoPath))
			{
				string s;
				string[] args;

				while ((s = reader.ReadLine()!) != null)
				{
					args = s.Split("=");
					if (args.Length > 1)
					{
                        properties.Add(args[0].ToLower().Trim(), args[1].Trim()); 
					}
				}
			}
            string? name;
            string? author;
            string? url; 
            properties.TryGetValue("name", out name); 
            properties.TryGetValue("author", out author);
            properties.TryGetValue("site", out url);

            return (name != null && author != null && url != null) ? new NemesisModInfo(folder, name, author, url, properties) : new NemesisModInfo();
			//add metadata later
		}
    }
}
