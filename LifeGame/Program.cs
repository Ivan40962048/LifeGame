using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var game = new GameModel(1, 704, 704);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(game));
        }
    }
}
