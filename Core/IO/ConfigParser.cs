using System;
using System.IO;
using System.Xml.Linq;

namespace Core.IO
{
    public sealed  class ConfigParser
    {
        private XDocument ConfigFile = null;

        /// <summary>
        /// Enter the file name only, without extension.The file must be in the same directory as the server
        /// </summary>
        /// <param name="_fileName"></param>
        public ConfigParser(string _fileName)
        {
            _fileName = _fileName + ".xml";
            string _path = Path.Combine(Environment.CurrentDirectory, _fileName);

            ConfigFile = LoadXMLFile(_path);

            if (ConfigFile == null)
                throw new System.IO.FileNotFoundException("Could not load Config File... File not found");
        }


        private bool IsXMLFile(string f) //TODO: This is absolute innecesary... Really... you are adding the fucking  .xml just above!!!
        {
            return (f != null && f.EndsWith(".xml", StringComparison.Ordinal));
        }

        private XDocument LoadXMLFile(string _filePath)
        {
            if (IsXMLFile(_filePath))
            {
                XDocument File = XDocument.Load(_filePath);
                return File;
            }
            return null;
        }

        /// <summary>
        /// Reads the first key found in ServerConfig whose parents are section and a server
        /// </summary>
        /// <param name="_server"></param>
        /// <param name="_section"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string Read(string _server, string _section, string _key)
        {
            string _result = String.Empty;

            if (ConfigFile != null)
            {
                try
                {
                    _result = ConfigFile.Element("Config").Element(_server).Element(_section).Element(_key).Value.ToString();
                }
                catch
                {
                    throw new System.Exception("Could not read requested XML tree:" + _server + '/' + _section + '/' + _key);
                }
          
            }
            return _result;            
        }


    }
}