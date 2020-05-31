using Stylet;
using System;
using System.Collections.Generic;
using System.Text;
using Wormwood.Views;

namespace Wormwood.ViewModels
{
    public class StartViewModel : Screen
    {
        private ShellViewModel Shell;
        public static string Title = "Wormwood: Start";

        public StartViewModel(ShellViewModel shell)
        {
            Shell = shell;
        }
        public void OpenEncrypt()
        {
            var encryptVM = new EncryptViewModel(Shell);
            Shell.ActivateItem(encryptVM);
        }

        public void OpenLocker()
        {
            var lockerVM = new LockerViewModel(Shell);
            Shell.ActivateItem(lockerVM);
        }
    }
}
