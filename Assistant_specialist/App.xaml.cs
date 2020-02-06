using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Assistant_specialist
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string assemblyName = new AssemblyName(args.Name).Name;
                if (assemblyName == "System.Windows.Interactivity")
                {
                    using (Stream stream = typeof(App).Assembly.GetManifestResourceStream("Assistant_specialist.Resources." + assemblyName + ".dll"))
                    {
                        long bufferSize = stream.Length;
                        byte[] buffer = new byte[bufferSize];
                        stream.Read(buffer, 0, (int)bufferSize);
                        return Assembly.Load(buffer);
                    }
                }
                return null;
            };
        }
    }
}