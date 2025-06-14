using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Core.Application;
using Core.Domain.Entities;

namespace Presentation.Desktop;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly INoteService _noteService;

    [ObservableProperty]
    private string _title = string.Empty;

    public ObservableCollection<Note> Notes { get; } = new();

    public MainWindowViewModel(INoteService noteService)
    {
        _noteService = noteService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        var notes = await _noteService.GetNotesAsync();
        Notes.Clear();
        foreach (var note in notes)
        {
            Notes.Add(note);
        }
    }

    [RelayCommand]
    private async Task AddNoteAsync()
    {
        if (string.IsNullOrWhiteSpace(Title)) return;
        await _noteService.CreateAsync(Title, "");
        Title = string.Empty;
        await LoadAsync();
    }
}
