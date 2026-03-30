/*****************************************************************************************
This is the UI interface for the Watt Witches Capstone Project. It creates bar graphs to 
compare gears and their operating status. This is in hopes of aiding the operator to 
quickly gleam the issues of the system. It also preforms a Fast Fourier Transform
utilizing FftSharp library and to display the associated graphs.

Please note that the Gear graphs are subject to change; they are currently meant as place
holders.

WattWitchesUI:
Clarity.cs
Clarity.Designer.cs
Program.cs

Veronica Jennings Feb. 2026

******************************************************************************************/

using WinFormsApp5;

namespace WinFormsApp5
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Clarity());
        }

    }
}