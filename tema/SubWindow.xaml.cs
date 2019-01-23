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
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace tema
{
    /// <summary>
    /// Interaction logic for SubWindow.xaml
    /// </summary>
    /// 

    public partial class SubWindow : Window
    {
        public List<Angajati> people = new List<Angajati>();

        public SubWindow()
        {
            using (var context = new Pontaj_ATMEntities())
            {
                people = (from a in context.Angajatis
                          select a).ToList();
            }
            InitializeComponent();
            Humans.ItemsSource = new ObservableCollection<Angajati>(people);

        }

        private void FullPlanning_Click(object sender, RoutedEventArgs e)
        {
            SubWindowReport report = new SubWindowReport();
            report.Show();
        }

        private void SetPlanning_Click(object sender, RoutedEventArgs e)
        {
            Angajati angajat = new Angajati
            {
                Nume = (Humans.SelectedItem as Angajati).Nume,
                Prenume = (Humans.SelectedItem as Angajati).Prenume
            };
            foreach(var item in people)
            {
                if (item.Nume == angajat.Nume && item.Prenume == angajat.Prenume)
                {
                    angajat.ID_Angajat = item.ID_Angajat;
                    angajat.ID_Functie = item.ID_Functie;
                }
            }
            int schimb;
            if (Schimb1.IsChecked == true)
                schimb = 1;
            else
                if (Schimb2.IsChecked == true)
                schimb = 2;
            else
                schimb = 3;

            using (var context = new Pontaj_ATMEntities())
            {
                var result = from f in context.SchimburiFuncties
                             where f.ID_Functie == angajat.ID_Functie
                             where f.ID_Schimb == schimb
                             select f;
                             
                if (result.Any())
                {
                    foreach (var item in Planner.SelectedDates)
                    {
                        string data = item.ToString("dd/MM/yyyy");
                        var result2 = (from p in context.Planficares
                                       where p.ID_Angajat == angajat.ID_Angajat
                                       where p.Data == data
                                       select p).FirstOrDefault();
                                   
                        if (result2 == null)
                        {
                            var nPln = new Planficare
                            {
                                ID_Angajat = angajat.ID_Angajat,
                                ID_Schimb = schimb,
                                Data = data
                            };
                            context.Planficares.Add(nPln);
                        }
                        else
                        {
                            result2.ID_Schimb = schimb;
                        }
                            
                    }

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Succes!");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                        {
                            DbEntityEntry entry = item.Entry;
                            string entityType = entry.Entity.GetType().Name;
                            foreach (DbValidationError subitem in item.ValidationErrors)
                            {
                                string mess = string.Format("{0} {1} {2}", subitem.ErrorMessage, entityType, subitem.PropertyName);
                                MessageBox.Show(mess);
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("nu ai functia potrivita pt acest schimb!!!!!!");
            }  
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            if (Humans.SelectedItem == null)
            {
                MessageBox.Show("Selectati o persoana!");
                return;
            }
            Angajati angajat = new Angajati
            {
                Nume = (Humans.SelectedItem as Angajati).Nume,
                Prenume = (Humans.SelectedItem as Angajati).Prenume
            };
            foreach (var item in people)
            {
                if (item.Nume == angajat.Nume && item.Prenume == angajat.Prenume)
                {
                    angajat.ID_Angajat = item.ID_Angajat;
                    angajat.ID_Functie = item.ID_Functie;
                }
            }
            if (Planner.SelectedDate != null)
            {
                string date = ((DateTime)Planner.SelectedDate).ToString("dd/MM/yyyy");
                Verification report = new Verification(angajat, date);
                report.Show();
            }
            else
                MessageBox.Show("Selectati  data intai");
        }
    }
}
