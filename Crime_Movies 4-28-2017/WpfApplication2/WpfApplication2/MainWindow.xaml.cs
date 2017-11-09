using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for FunctionalWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public BitmapImage result = null;
        string resultsPath = "C:\\Users\\Denver\\Documents\\COLLEGE\\RIT\\17SPRING\\BigData\\GroupProject\\";
        string batchPath = "C:\\Users\\Denver\\Documents\\COLLEGE\\RIT\\17SPRING\\BigData\\GroupProject\\";
        string scriptPath = "C:\\Users\\Denver\\Documents\\COLLEGE\\RIT\\17SPRING\\BigData\\GroupProject\\";
        string executionPath = "C:\\Program Files\\R\\R-3.3.2\\bin\\";


        public MainWindow()
        {
            InitializeComponent();
        }

        /**
         *  Launches batch file that runs R script, then displays resulting plot as image
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // YEAR FIRST
            string year = "";
            if ((bool)radioButton2012.IsChecked)
            {
                year = "2012";
            }
            else if ((bool)radioButton2013.IsChecked)
            {
                year = "2013";
            }
            else if ((bool)radioButton2014.IsChecked)
            {
                year = "2014";
            }
            else if ((bool)radioButton2015.IsChecked)
            {
                year = "2015";
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a year", "Error");
            }
            if (!year.Equals(""))
            {
                string batName = year + ".bat";
                try
                {
                    // CHANGE TO LOCATION OF BATCH FILES ON PC
                    Process p = Process.Start("" + batchPath + batName);
                    p.WaitForExit(); // Wait for process to end, otherwise image source will not exist
                    if (p.ExitCode == 0)
                    {
                        p.Close();
                        // CHANGE TO LOCATION OF RESULTS IMAGE OUTPUT BY SCRIPT - SHOULD BE SAME FOR EACH SCRIPT
                        ResImage.Source = makeBitmap(new Uri(resultsPath + "res.png"));
                        // DELETE RESULTS IMAGE, USED FOR ERROR CHECKING IF R SCRIPT DOES NOT EXIST
                        File.Delete(resultsPath + "res.png");
                    }
                    else
                    {
                        p.Close();
                        System.Windows.MessageBox.Show("Batch file did not work, R script possibly missing", "Error");
                    }
                }
                catch (Exception exc)
                {
                    System.Windows.MessageBox.Show("File '" + batName + "' not found", "Error");
                }
            }

        }

        /**
         *  Make a bitmap image from a given location on disk, then release lock on file
         *  @param src Location of image on disk
         */
        private static BitmapImage makeBitmap(Uri src)
        {
            try
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = src;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                return bmp;
            }
            catch (Exception exc)
            {
                System.Windows.MessageBox.Show("'res.png' not found", "Error");
                return null;
            }
        }

        /**
         *  Choose directory where res.png will be output
         */
        private void resPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            resultsPath = fbd.SelectedPath + "\\";
        }

        /**
         *  Choose directory containing batch files (.bat)
         */
        private void batchPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            batchPath = fbd.SelectedPath + "\\";
        }

        /**
         * Choose directory containing R scripts
         */
        private void scriptPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            scriptPath = fbd.SelectedPath + "\\";
        }

        /**
         *  Choose directory containing Rscript.exe
         */
        private void execPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            executionPath = fbd.SelectedPath + "\\";
        }

        private void makeBatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(batchPath + "2012.bat"))
            {
                File.Delete(batchPath + "2012.bat");
            }
            if (File.Exists(batchPath + "2013.bat"))
            {
                File.Delete(batchPath + "2013.bat");
            }
            if (File.Exists(batchPath + "2014.bat"))
            {
                File.Delete(batchPath + "2014.bat");
            }
            if (File.Exists(batchPath + "2015.bat"))
            {
                File.Delete(batchPath + "2015.bat");
            }


            StreamWriter writer = new StreamWriter(System.IO.Path.Combine(batchPath, "2012.bat"));
            writer.WriteLine("cd " + executionPath);
            writer.WriteLine("Rscript.exe " + scriptPath + "2012.R");
            writer.Close();

            writer = new StreamWriter(System.IO.Path.Combine(batchPath, "2013.bat"));
            writer.WriteLine("cd " + executionPath);
            writer.WriteLine("Rscript.exe " + scriptPath + "2013.R");
            writer.Close();

            writer = new StreamWriter(System.IO.Path.Combine(batchPath, "2014.bat"));
            writer.WriteLine("cd " + executionPath);
            writer.WriteLine("Rscript.exe " + scriptPath + "2014.R");
            writer.Close();

            writer = new StreamWriter(System.IO.Path.Combine(batchPath, "2015.bat"));
            writer.WriteLine("cd " + executionPath);
            writer.WriteLine("Rscript.exe " + scriptPath + "2015.R");
            writer.Close();
        }
    }
}
