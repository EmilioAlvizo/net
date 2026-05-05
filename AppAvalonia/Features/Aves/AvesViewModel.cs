// AvesViewModel.cs

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia.Models;
using AppAvalonia.Services;

namespace AppAvalonia.ViewModels;

public partial class AvesViewModel : ObservableObject
{
    public string Titulo => "Pantalla de Aves 🐔";
}