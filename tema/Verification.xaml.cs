using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;
using System.Data;

namespace tema
{
    /// <summary>
    /// Interaction logic for Verification.xaml
    /// </summary>
    public partial class Verification : Window
    {
        public Verification(Angajati angajat, string date)
        {
            
            InitializeComponent();
            using (var context = new Pontaj_ATMEntities())
            {
                var result = from p in context.Pontajs
                             where p.ID_Angajat == angajat.ID_Angajat
                             where p.Clock_In.Substring(0, 10) == date
                             select new
                             {
                                 p.Clock_In,
                                 p.Clock_Out
                             };
                Verificare.DataContext = result.ToList();

            }
            
            
        }
    }

   
}
