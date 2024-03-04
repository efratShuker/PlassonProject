namespace MachineStatusTracker
{
    public class MainWindowViewModel : NotificationObject
    {
        MachineTicketsPanel _machineTicketsPanel;

        public MainWindowViewModel(MachineTicketsPanel machineTicketsPanel)
        {
            _machineTicketsPanel = machineTicketsPanel;
        }

        public MachineTicketsPanel MachineTicketsPanel 
        {
            get => _machineTicketsPanel;
            set
            {
                _machineTicketsPanel = value;
                OnNotifyPropertyChanged(nameof(MachineTicketsPanel));
            }
        }
    }
}
