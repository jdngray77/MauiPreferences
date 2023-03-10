using System.Windows.Input;

namespace Prefs.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    public static readonly BindableProperty KeyProperty = BindableProperty.Create(
        "Key",
        typeof(string),
        typeof(MainViewModel),
        "MyKey");

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        "Value",
        typeof(string),
        typeof(MainViewModel),
        "This is a value");

    public static readonly BindableProperty SharedNameProperty = BindableProperty.Create(
        "SharedName",
        typeof(string),
        typeof(MainViewModel),
        string.Empty);

    public static readonly BindableProperty SelectedKeyProperty = BindableProperty.Create(
        "SelectedKey",
        typeof(string),
        typeof(MainViewModel),
        string.Empty);


    public static readonly BindableProperty ValueFromPrefsProperty = BindableProperty.Create(
        "ValueFromPrefs",
        typeof(string),
        typeof(MainViewModel),
        string.Empty);
    
    public static readonly BindableProperty AllKeysProperty = BindableProperty.Create(
        "AllKeys",
        typeof(string[]),
        typeof(MainViewModel),
        Array.Empty<string>());

    private List<string> allKeys = new List<string>();

    public ICommand NewCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand FetchCommand { get; set; }
    public ICommand ClearCommand { get; set; }

    public MainViewModel()
    {
        NewCommand = new RelayCommand(() => Try(New));
        DeleteCommand = new RelayCommand(() => Try(Delete));
        FetchCommand = new RelayCommand(() => Try(Fetch));

        Microsoft.Maui.Storage.Preferences.Default.Clear();
    }

    public string Key
    {
        get => (string) GetValue(KeyProperty);
        set => SetValue(KeyProperty, value ?? string.Empty);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value ?? string.Empty);
    }

    public string SharedName
    {
        get => (string) GetValue(SharedNameProperty);
        set => SetValue(SharedNameProperty, value ?? string.Empty);
    }

    public string SelectedKey
    {
        get => (string)GetValue(SelectedKeyProperty);
        set => SetValue(SelectedKeyProperty, value ?? string.Empty);
    }


    public string ValueFromPrefs
    {
        get => (string) GetValue(ValueFromPrefsProperty);
        set => SetValue(ValueFromPrefsProperty, value ?? string.Empty);
    }

    public string[] AllKeys
    {
        get => (string[])GetValue(AllKeysProperty);
        set => SetValue(AllKeysProperty, value);
    }

    private void New()
    {
        Microsoft.Maui.Storage.Preferences.Default.Set(
            Key,
            Value, 
            (string.IsNullOrWhiteSpace(SharedName) ? null : SharedName));

        AddNewKnownKey(Key);
    }

    private void Delete()
    {
        Microsoft.Maui.Storage.Preferences.Default.Remove(
            Key,
            (string.IsNullOrWhiteSpace(SharedName) ? null : SharedName));
        
        RemoveKnownKey(Key);
    }

    private void Fetch()
    {
        ValueFromPrefs = Microsoft.Maui.Storage.Preferences.Default.Get(
            Key,
            "NO VALUE",
            (string.IsNullOrWhiteSpace(SharedName) ? null : SharedName));
    }

    private void AddNewKnownKey(string key)
    {
        allKeys.Remove(key);
        AllKeys = allKeys.ToArray();
    }

    private void RemoveKnownKey(string key)
    {
        allKeys.Remove(key);
        AllKeys = allKeys.ToArray();
    }

    private void Try(Action it)
    {
        try
        {
            it();
        }
        catch (Exception e)
        {
            DisplayAlert("Exception", e.ToString(), "My bad g");
        }
    }
}
