using Stylet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wormwood.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>
    {
        public StartViewModel StartVM { get; }
        public IWindowManager WindowManager;
        public ShellViewModel(IWindowManager windowManager)
        {
            this.WindowManager = windowManager;
            StartVM = new StartViewModel(this);
            this.ActivateItem(StartVM);
        }


    }
}
