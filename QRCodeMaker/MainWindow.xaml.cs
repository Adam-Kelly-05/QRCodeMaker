using QRCoder;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QRCodeMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ImageSource ByteArrayToImageSource(byte[] imageBytes)
        {
            var stream = new MemoryStream(imageBytes);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            return image;
        }


        private void Generate_BTN_Click(object sender, RoutedEventArgs e)
        {
            string text = Input_TXTBX.Text;

            int chosenErrorCorrectionLevel = ErrorCorrection_CMBOX.SelectedIndex;
            QRCodeGenerator.ECCLevel eccLevel;
            switch (chosenErrorCorrectionLevel)
            {
                case 0:
                    eccLevel = QRCodeGenerator.ECCLevel.L;
                    break;
                case 1:
                    eccLevel = QRCodeGenerator.ECCLevel.M;
                    break;
                case 2:
                    eccLevel = QRCodeGenerator.ECCLevel.Q;
                    break;
                case 3:
                    eccLevel = QRCodeGenerator.ECCLevel.H;
                    break;
                default:
                    eccLevel = QRCodeGenerator.ECCLevel.L;
                    break;
            }

            using (PngByteQRCode qrCode = new PngByteQRCode(new QRCodeGenerator().CreateQrCode(text, eccLevel)))
            {
                QRCode_IMG.Source = ByteArrayToImageSource(qrCode.GetGraphic(20));
            }
        }
    }
}
