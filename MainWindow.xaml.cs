using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace EasyOCRFlaskAPITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string imagePath;

        public MainWindow()
        {
            InitializeComponent();
        }
        async Task ProcessImageAsItemListAsync()
        {
            var ocrService = new EasyOCRService();
            try
            {
                List<Item> result = await ocrService.GetItemListFromImage(imagePath);

                Dispatcher.Invoke(() =>
                {
                    dgOCRResult.ItemsSource = result;
                });

                Console.WriteLine("OCR Result: ");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private void btnImagePath_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new (); // add file type filtering
            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                Uri fileUri = new (imagePath);
                imgReceiptSource.Source = new BitmapImage(fileUri);
            }
        }
        private void btnGetListFromImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ = ProcessImageAsItemListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        /*
        async Task ProcessImageAsync()
        {
            var ocrService = new EasyOCRService();
            try
            {
                string result = await ocrService.ProcessImageAsStringAsync(imagePath);

                Dispatcher.Invoke(() =>
                {
                    tbOCRResult.Text = result;
                });

                Console.WriteLine("OCR Result: ");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
         * 
        async Task ProcessImageAsync()
        {
            var ocrService = new EasyOCRService();
            try
            {
                string result = await ocrService.ProcessImageAsStringAsync(imagePath);

                Dispatcher.Invoke(() =>
                {
                    tbOCRResult.Text = result;
                });

                Console.WriteLine("OCR Result: ");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
         * 
        private void btnGetTextFromImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ = ProcessImageAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        */
    }
}