using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GitStart.Services
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, "Возникла ошибка");
            }
        }
    }
}
