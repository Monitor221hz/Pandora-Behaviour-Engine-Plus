using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.MVVM.Model
{
    public class NemesisModInfo
    {

        public bool Active { get; set; } = false;
        internal string FolderPath { get; set; }

        public Dictionary<string, string> Properties { get; private set; } = new Dictionary<string, string>();

        public string Name { get; set; } = "Default";

        public string Author { get; set; } = "Default";
        public string URL { get; set; } = "Default";

        internal string Code { get; set; } = "Default";

        //internal string Auto { get; set; } = "Default";
        //internal string RequiredFile { get; set; } = "Default";
        //internal string FileToCopy { get; set; } = "Default";
        //internal bool Hidden { get; set; } = false;
        public bool Valid { get; set; } = false;
        internal NemesisModInfo(string path)
        {
            StringBuilder pathBuilder = new StringBuilder(path);
            pathBuilder.Append('\\');
            pathBuilder.Append("info.ini");

            string infoPath = pathBuilder.ToString();
            FolderPath = path;
            Code = Path.GetFileName(path);

            if (!File.Exists(infoPath))
            {
                Valid = false;
                return; 
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
                        Properties.Add(args[0].ToLower().Trim(), args[1].Trim()); 
					}
				}
			}
            string name;
            string author;
            string url; 
            Properties.TryGetValue("name", out name); 
            Properties.TryGetValue("author", out author);
            Properties.TryGetValue("url", out url);

            if (name != null) { Name = name; }
            if (author != null) { Author = author; }
            if (url != null) { URL  = url; }
            Valid = true;
			//add metadata later
		}
    }
}
