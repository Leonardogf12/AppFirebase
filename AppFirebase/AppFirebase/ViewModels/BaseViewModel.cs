﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AppFirebase.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        
    }
}
