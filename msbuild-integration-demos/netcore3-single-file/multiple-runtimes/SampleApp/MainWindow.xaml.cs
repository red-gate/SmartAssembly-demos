using System;
using System.Reflection;
using System.Windows;

namespace SampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // You may want to move this to a separate ViewModel
        public string ObfuscatedTypeName => typeof(SecretClass).FullName;
        public string AssemblyBitness => typeof(MainWindow).Assembly.GetName().ProcessorArchitecture switch
        {
            ProcessorArchitecture.None => "Unknown",
            ProcessorArchitecture.MSIL => "AnyCPU",
            ProcessorArchitecture.X86 => "32-bit",
            ProcessorArchitecture.IA64 => "IA-64",
            ProcessorArchitecture.Amd64 => "64-bit",
            ProcessorArchitecture.Arm => "ARM",
            _ => throw new ArgumentOutOfRangeException()
        };

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
