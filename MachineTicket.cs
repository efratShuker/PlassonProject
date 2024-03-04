using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MachineStatusTracker
{
    public class MachineTicket : NotificationObject, INotifyDataErrorInfo
    {
        bool _editMachineState, _load;
        string _name, _description, _note;
        Action<Guid> _deleteMachine;
        Action _refreshFilter;
        MachineStatusEnum _selectedStatus;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public MachineTicket(Guid key, Action<Guid> deleteMachine, Action refreshFilter,
            string name, string description, MachineStatusEnum machineStatus, string note, bool editMachineState)
        {
            Key = key;
            Name = name; 
            Description = description;
            SelectedStatus = machineStatus;
            Note = note;
            _deleteMachine = deleteMachine;
            EditMachineState = editMachineState;
            _refreshFilter = refreshFilter;
            _load = false;
            DeleteMachineCommand = new RelayCommand(e => DeleteMachine());
            SaveMachineCommand = new RelayCommand(e => SaveMachine());
            EditMachineCommand = new RelayCommand(e => EditMachine());
        }

        private void EditMachine()
        {
            EditMachineState = true;
        }

        protected virtual void SaveMachine()
        {
            if (string.IsNullOrEmpty(Name))
            {
                AddError(nameof(Name), "name is requierd");
                OnErrorsChanged(nameof(Name));
            }
            if (GetErrors(nameof(Name)) == null)
            {
                EditMachineState = false;
                _refreshFilter();
                MessageBox.Show("Success Update Machine Status");
            }
        }

        protected virtual void DeleteMachine() => _deleteMachine(Key);

        public Guid Key { get; set; }
        public bool EditMachineState 
        {
            get => _editMachineState;
            set
            { 
                _editMachineState = value;
                OnNotifyPropertyChanged(nameof(EditMachineState));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                ValidateName();
                OnNotifyPropertyChanged(nameof(Name));
                _load = true;
            }
        } 
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnNotifyPropertyChanged(nameof(Description));
            }
        } 
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnNotifyPropertyChanged(nameof(Note));
            }
        }

        public MachineStatusEnum SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnNotifyPropertyChanged(nameof(SelectedStatus));
            }
        }
        public ICommand DeleteMachineCommand { get; set; }
        public ICommand SaveMachineCommand { get; set; }
        public ICommand EditMachineCommand { get; set; }

        public bool HasErrors => _errorsByPropertyName.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;

        }
        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ValidateName()
        {
            ClearErrors(nameof(Name));
            if (_load && string.IsNullOrWhiteSpace(Name))
                AddError(nameof(Name), "Name cannot be empty.");
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
