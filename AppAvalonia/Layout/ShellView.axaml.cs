//Layout/ShellView.axaml.cs

using Avalonia.Controls;
using System;

namespace AppAvalonia.Views;

public partial class ShellView : UserControl
{
    public ShellView()
    {
        InitializeComponent();
        Console.WriteLine("ShellView construido");
    }
}