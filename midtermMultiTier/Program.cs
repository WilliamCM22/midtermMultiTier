using midtermMultiTier.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace midtermMultiTier
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LivresManager livresManager = new LivresManager();
            livresManager.OpenWindow();
        }
    }
}
