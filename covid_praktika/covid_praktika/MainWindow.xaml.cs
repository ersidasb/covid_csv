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

namespace covid_praktika
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            //cia tiesiog paleidzia UI langa,cia default buna sitas
            InitializeComponent();

            //start mygtukas isjungiamas nes default textBox tuscias ir comboBox not selected
            btnStart.IsEnabled = false;

            //comboBox 0 = not selected
            cbxOutput.SelectedIndex = 0;
        }

        //paspaudi mygtuka
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //sukuria nauja CSV objekta, kad galetum naudotis metodais
            CSV csv = new CSV(tbxCountryCode.Text);

            //combobox 1 = CSV
            if (cbxOutput.SelectedIndex == 1)
                csv.csvToCsv();
            //combobox 2 = JSON
            else if (cbxOutput.SelectedIndex == 2)
                csv.csvToJson();
            //combobox 3 = DB
            else
                ;//cia imesi csv.csvToDB(); bet paciam reiks metoda pasidaryt
                 //dar patarciau pasidaryt Repository klase, kurioj bus metodai skirti komunikacijai su DB, pvz insertCountry() ar kas nors
                 //nu realiai ten tik insert tereiks tai tebus vienas metodas xd
        }

        //textBox tekstas pasikeite
        private void tbxCountryCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkIfFieldsFilled();
        }

        //comboBox pasirinkimas pasikeite
        private void cbxOutput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkIfFieldsFilled();
        }

        //metodas isjungia mygtuka jei comboBoxe nera pasirinktas elementas arba textBox tuscias
        private void checkIfFieldsFilled()
        {
            if (cbxOutput.SelectedIndex != 0 && !tbxCountryCode.Text.Trim(' ').Equals(""))
                btnStart.IsEnabled = true;
            else
                btnStart.IsEnabled = false;
        }
    }
}
