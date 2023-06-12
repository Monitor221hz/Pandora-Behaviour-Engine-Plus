using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pandora.MVVM.Model
{
    public class ModInfo
    {

        public bool Active { get; set; } = false;
        internal string FolderPath { get; set; }

        public string Name { get; set; } = "Default";

        public string Author { get; set; } = "Default";
        public string URL { get; set; } = "Default";

        internal string Code { get; set; } = "Default";

        internal string Auto { get; set; } = "Default";
        internal string RequiredFile { get; set; } = "Default";
        internal string FileToCopy { get; set; } = "Default";
        internal bool Hidden { get; set; } = false;
        public bool Valid { get; set; } = false;
        internal ModInfo(string path)
        {
            StringBuilder pathBuilder = new StringBuilder(path);
            pathBuilder.Append('\\');
            pathBuilder.Append("info.ini");

            string infoPath = pathBuilder.ToString();
            FolderPath = path;
            Code = Path.GetFileName(path);
            if (File.Exists(infoPath))
            {
                using (StreamReader reader = new StreamReader(infoPath))
                {
                    string s;
                    string[] args;
                    string val;
                    while ((s = reader.ReadLine()!) != null)
                    {
                        args = s.Split("=");
                        if (args.Length > 1)
                        {
                            val = args[1].Trim();

                            switch (args[0].Trim().ToLower())
                            {
                                case ("name"):
                                    Name = val;
                                    break;

                                case ("author"):
                                    Author = val;
                                    break;

                                case ("site"):
                                    URL = val;
                                    break;

                                case ("auto"):
                                    Auto = val;
                                    break;
                                case ("required"):
                                    RequiredFile = val;
                                    break;
                                case ("copyfile"):
                                    FileToCopy = val;
                                    break;
                                case ("hidden"):
                                    Hidden = bool.Parse(val);
                                    break;
                            }


                            if (Name != "") Valid = true;
                        }

                    }
                }

            }

            //add metadata later
        }
    }
}
