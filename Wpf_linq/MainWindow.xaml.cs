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
using System.Collections.ObjectModel;

namespace Wpf_linq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientesModelDataContext dc = new ClientesModelDataContext();
        ObservableCollection<Cliente> cliLista = new ObservableCollection<Cliente>();
        public MainWindow()
        {
            InitializeComponent();
            foreach (var c in dc.Clientes)
            {
                cliLista.Add(c);
            }
            listBox1.ItemsSource = cliLista;
        }

        private void AdicionaTabContato(Contato c)
        {
            TabItem item = new TabItem();
            ContatoControl ctrl = new ContatoControl();
            ctrl.DataContext = c;
            item.Content = ctrl;
            item.Header = c.Id.ToString();
            tabControl1.Items.Add(item);
            tabControl1.SelectedItem = item;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Cliente cli = new Cliente() { Nome = "Não identificado" };
            dc.Clientes.InsertOnSubmit(cli);
            cliLista.Add(cli);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Cliente cli = (Cliente)listBox1.SelectedItem;
                cli.Contato.Clear();
                dc.Clientes.DeleteOnSubmit((Cliente)listBox1.SelectedItem);
                cliLista.Remove((Cliente)listBox1.SelectedItem);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            dc.SubmitChanges();
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tabControl1.Items.Clear();
            if (listBox1.SelectedItem != null)
            {
                Cliente cli = (Cliente)listBox1.SelectedItem;
                foreach (var c in cli.Contato)
                {
                    AdicionaTabContato(c);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Cliente cli = (Cliente)listBox1.SelectedItem;
                Contato con = new Contato() { Cliente = cli.Id, Nome="Não identificado" };
                cli.Contato.Add(con);
                AdicionaTabContato(con);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedItem != null && tabControl1.SelectedItem != null)
            {
                Cliente cli = (Cliente)listBox1.SelectedItem;
                Contato con = (Contato)((ContatoControl)((TabItem)tabControl1.SelectedItem).Content).DataContext;
                cli.Contato.Remove(con);
                tabControl1.Items.Remove(tabControl1.SelectedItem);
                if (tabControl1.Items.Count > 0)
                    tabControl1.SelectedIndex = 0;
            }
        }
    }
}
