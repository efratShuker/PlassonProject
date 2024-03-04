namespace MachineStatusTracker
{
    internal class MachineStatusTrackerBoot
    {
        public MachineStatusTrackerBoot()
        {
            var machineTicketsPanel = new MachineTicketsPanel();

            var mainWindowViewModel = new MainWindowViewModel(machineTicketsPanel);

            var mainWindow = new MainWindow(mainWindowViewModel);
            mainWindow.Show();
        }
    }
}
