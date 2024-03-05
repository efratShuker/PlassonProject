using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineStatusTracker
{
    public class MachineTicketsPanel : NotificationObject
    {
        ObservableCollection<NotificationObject> _machineTickets;
        ListCollectionView _machineTicketsList;
        KeyValuePair<int,string> _selectedStatusFilter;
        Dictionary<int, string> _statuses;
        private bool _addMachineTicketEnable;
        private NewMachineTicket _newMachineTicket;

        public MachineTicketsPanel()
        {
            _machineTickets = new ObservableCollection<NotificationObject>();
            _machineTicketsList = new ListCollectionView(_machineTickets);
            NewMachineTicket = new NewMachineTicket(AddMachine, DisableAddMachineTicket);
            MachineTicketsList.Filter = new Predicate<object>(m => Filter(m));
            _statuses = Enum.GetValues<MachineStatusEnum>().Cast<MachineStatusEnum>().ToDictionary(m => (int)m, m => m.ToString());
            _statuses.Add(-1, "Without FIlter");
            AddMachineTicketEnable = false;
            SelectedStatusFilter = new KeyValuePair<int, string>(-1, "Without FIlter");
            AddMachineCommand = new RelayCommand(e => DisplayAddMachineTicket());
        }

        private void DisplayAddMachineTicket() => AddMachineTicketEnable = true;
        private void DisableAddMachineTicket() => AddMachineTicketEnable = false;
        private void AddMachine(string name, string description, MachineStatusEnum machineStatus, string note)
        {
            _machineTickets.Add(new MachineTicket(Guid.NewGuid(), DeleteMachine, () => MachineTicketsList.Refresh(), name, description, machineStatus, note, false));
        }

        private void DeleteMachine(Guid key)
        {
            var machineToDelete = _machineTickets.FirstOrDefault(m => (m as MachineTicket)?.Key == key);
            _machineTickets.Remove(machineToDelete);
        }

        private bool Filter(object item)
        {
            if (SelectedStatusFilter.Key == -1)
                return true;
            if (item is MachineTicket machineTicket)
                return (int)machineTicket?.SelectedStatus == SelectedStatusFilter.Key;
            return false;
        }

        public bool AddMachineTicketEnable
        {
            get => _addMachineTicketEnable;
            set
            {
                _addMachineTicketEnable = value;
                OnNotifyPropertyChanged(nameof(AddMachineTicketEnable));
            }
        }

        public KeyValuePair<int,string> SelectedStatusFilter
        {
            get => _selectedStatusFilter;
            set
            {
                _selectedStatusFilter = value;
                OnNotifyPropertyChanged(nameof(SelectedStatusFilter));
                MachineTicketsList.Refresh();
            }
        }

        public ListCollectionView MachineTicketsList
        {
            get => _machineTicketsList;
            set
            {
                _machineTicketsList = value;
                OnNotifyPropertyChanged(nameof(MachineTicketsList));
            }
        }

        public NewMachineTicket NewMachineTicket
        {
            get => _newMachineTicket;
            set
            {
                _newMachineTicket = value;
                OnNotifyPropertyChanged(nameof(NewMachineTicket));
            }
        }
        public ICommand AddMachineCommand { get; set; }

        public Dictionary<int, string> Statuses {  get => _statuses; set { } }
    }
}
