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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace tema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        static bool Exists(Guid id)
        {
            using (var context = new Pontaj_ATMEntities())
            {
                var results = from c in context.Angajatis
                              where c.ID_Angajat.Equals(id)
                              orderby c.ID_Angajat
                              select c;
                if (!results.Any())
                    return false;
                else
                    return true;
                /*foreach(var item in results)
                {
                    Console.WriteLine("{0} \t {1} \t {2} \t {3}",
                        item.ID_Angajat.ToString(),
                        item.Nume,
                        item.Prenume,
                        item.Denumire);
                    Console.ReadKey();
                    */
            }
        }

        static void ClockIn(Guid id)
        {
            using (var context = new Pontaj_ATMEntities())
            {
                if (Exists(id))
                {
                    var pontaj = from p in context.Pontajs
                                 where p.ID_Angajat == id
                                 where p.Clock_Out == null
                                 select p;
                    if (!pontaj.Any())
                    {
                        var clkIn = new Pontaj
                        {
                            ID_Angajat = id,
                            Clock_In = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                            Clock_Out = null

                        };
                        context.Pontajs.Add(clkIn);
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
                        MessageBox.Show("Nu ai iesit ca sa poti intra!\nNu mai pasa cartela!");
                    
                }
                else
                    MessageBox.Show("Nu exista acest angajat!");
            }
        }

        static void ClockOut(Guid id)
        {
            using (var context = new Pontaj_ATMEntities())
            {
                if (Exists(id))
                {
                    var pontaj = from p in context.Pontajs
                                 where p.ID_Angajat == id
                                 where p.Clock_Out == null
                                 select p;
                    if (pontaj.Any())
                    {
                        pontaj.First().Clock_Out = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
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
                        MessageBox.Show("Nu ai intrat, ca sa poti iesi!\nNu mai pasa cartela!");
                   
                }
                else
                    MessageBox.Show("Nu exista acest angajat!");
            }
        }

        static bool LoginAsManager(Guid id)
        {
            using (var context = new Pontaj_ATMEntities())
            {
                if (Exists(id))
                {
                    var result = from c in context.Angajatis
                                 join f in context.Functiis
                                 on c.ID_Functie equals f.ID_Functie
                                 where c.ID_Angajat.Equals(id)
                                 where f.Denumire.Equals("manager")
                                 select new
                                 {
                                     c.Nume,
                                     c.Prenume
                                 };
                    if (result.Any())
                        return true;
                }
            }
            return false;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ManagerButton_Click(object sender, RoutedEventArgs e)
        {
            Guid id = new Guid(CodeText.Text);
            if (LoginAsManager(id))
            {
                SubWindow subWindow = new SubWindow();
                subWindow.Show();
            }
            else
                MessageBox.Show("nu esti manager! munceste!");
        }

        private void ClockInButton_Click(object sender, RoutedEventArgs e)
        {
            Guid id = new Guid(CodeText.Text);
            ClockIn(id);
        }

        private void ClockOutButton_Click(object sender, RoutedEventArgs e)
        {
            Guid id = new Guid(CodeText.Text);
            ClockOut(id);
        }

        private void Planning_Click(object sender, RoutedEventArgs e)
        {
            Guid id = new Guid(CodeText.Text);
            using (var context = new Pontaj_ATMEntities())
            {
                var results = from c in context.Angajatis
                              join p in context.Planficares on c.ID_Angajat equals p.ID_Angajat
                              join s in context.Schimburis on p.ID_Schimb equals s.ID_Schimb
                              where c.ID_Angajat == id
                              select new
                              {
                                  p.Data,
                                  s.Interval
                              };
                if (results.Any())
                {
                    foreach (var item in results)
                    {
                        MessageBox.Show(item.Data + " " + item.Interval);
                    }
                }
                else
                    MessageBox.Show("No planning");

            }
        }


    }
}
