using RouteBookingSystem;
using System.Windows;
using System.Windows.Markup;

public static class LocalizationManager
{
    private static ResourceDictionary GetDictionary(string culture)
    {
        return new ResourceDictionary()
        {
            Source = new Uri($"/Languages/Strings.{culture}.xaml", UriKind.RelativeOrAbsolute)
        };
    }

    public static void SwitchLanguage(string culture)
    {
        try
        {
            var dict = GetDictionary(culture);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.IsActive && window is AdminWindow adminWindow)
                    {
                        if (adminWindow.DataContext is AdminViewModel vm)
                        {
                            vm.RefreshTransportTypes();
                            window.Language = XmlLanguage.GetLanguage(culture);
                        }
                    }
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Language switch error: {ex.Message}");
        }
    }
}
  