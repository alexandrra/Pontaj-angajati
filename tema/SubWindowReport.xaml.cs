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
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;

namespace tema
{
    /// <summary>
    /// Interaction logic for SubWindowReport.xaml
    /// </summary>
    public partial class SubWindowReport : Window
    {
        public SubWindowReport()
        {
            InitializeComponent();
            Pontaj_ATMEntities context = new Pontaj_ATMEntities();
            var result = from p in context.Planficares
                         join a in context.Angajatis
                         on p.ID_Angajat equals a.ID_Angajat
                         select new
                         {
                             p.ID_Planificare,
                             a.Nume,
                             a.Prenume,
                             p.Data,
                             p.ID_Schimb
                         };
            Planning.DataContext = result.ToList();
        }
    }
}
