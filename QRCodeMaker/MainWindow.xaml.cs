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

        private void TooManyCharactersError()
        {
            Input_TXTBX.Text = "";
            MessageBox.Show("This is too many characters to be encoded, please shorten the lenght of the text or choose a smaller encoding level!");
        }

        private void Generate_BTN_Click(object sender, RoutedEventArgs e)
        {
            string text = Input_TXTBX.Text;

            int chosenErrorCorrectionLevel = ErrorCorrection_CMBOX.SelectedIndex;
            QRCodeGenerator.ECCLevel? eccLevel = null;
            switch (chosenErrorCorrectionLevel)
            {
                case 0:
                    if (text.Length < 980)
                        eccLevel = QRCodeGenerator.ECCLevel.L;
                    break;
                case 1:
                    if (text.Length < 775)
                        eccLevel = QRCodeGenerator.ECCLevel.M;
                    break;
                case 2:
                    if (text.Length < 550)
                        eccLevel = QRCodeGenerator.ECCLevel.Q;
                    break;
                case 3:
                    if (text.Length < 415)
                        eccLevel = QRCodeGenerator.ECCLevel.H;
                    break;
            }

            if (eccLevel != null)
            {
                using (PngByteQRCode qrCode = new PngByteQRCode(new QRCodeGenerator().CreateQrCode(text, (QRCodeGenerator.ECCLevel)eccLevel)))
                {
                    QRCode_IMG.Source = ByteArrayToImageSource(qrCode.GetGraphic(20));
                }
            }
            else
            {
                TooManyCharactersError();
            }
        }
    }
}
