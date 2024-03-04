using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WFWSupervisorsDashboard
{

    public class Shift : INotifyPropertyChanged
    {
        
        // **INotifyPropertyChanged Implementation**
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _shiftID;
        public int ShiftID
        {
            get => _shiftID;
            set
            {
                _shiftID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShiftID)));
            }
        }

        private string _shiftName;
        public string ShiftName
        {
            get => _shiftName;
            set
            {
                _shiftName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShiftName)));
            }
        }

        private string _startTime;
        public string StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartTime)));
            }
        }

        private string _endTime;
        public string EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndTime)));
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
            }
        }
    }
}

