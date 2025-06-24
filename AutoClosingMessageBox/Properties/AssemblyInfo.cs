using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyVersion("1.0.0.7")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyTitle("AutoClosingMessageBox")]
[assembly: AssemblyDescription("MessageBox with auto-closing functionality")]
[assembly: AssemblyCompany("Dmitry Garavsky(https://github.com/DmitryGaravsky)")]
#if WPF
[assembly: AssemblyProduct("Microsoft® .NET Framework Extension for WPF")]
#else
[assembly: AssemblyProduct("Microsoft® .NET Framework Extension for Windows Forms")]
#endif
[assembly: AssemblyCopyright("Dmitry Garavsky, ©2024")]
[assembly: AssemblyFileVersion("1.0.0.7")]
[assembly: StringFreezing]
[assembly: ComVisible(false)]