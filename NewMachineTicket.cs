using System;

namespace MachineStatusTracker
{
    public class NewMachineTicket:MachineTicket
    {
        Action _disableAddButton;
        Action<string, string, MachineStatusEnum, string> _addMachine;

        public NewMachineTicket(Action<string, string, MachineStatusEnum, string> addMachine, Action disableAddButton)
            :base(Guid.Empty, k => { },()=> { }, string.Empty, string.Empty, MachineStatusEnum.Offline, string.Empty,true)
        {
            _addMachine = addMachine;
            _disableAddButton = disableAddButton;
        }

        protected override void SaveMachine()
        {
            if (string.IsNullOrEmpty(Name))
            {
                AddError(nameof(Name), "name is requierd");
                OnErrorsChanged(nameof(Name));
            }
            if(GetErrors(nameof(Name)) == null) 
            {
                _addMachine(Name, Description , SelectedStatus, Note);
                Clear();
                _disableAddButton();
            }
        }

        protected override void DeleteMachine()
        {
            Clear();
            _disableAddButton();
        }

        private void Clear()
        {
            Name = string.Empty;
            Description = string.Empty;
            SelectedStatus = MachineStatusEnum.Offline;
            Note = string.Empty;
            ClearErrors(nameof(Name));
        }
    }
}
