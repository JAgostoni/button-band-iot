using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Band.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Get results from Azure and populate the current button counts
            BlueCount.Text = "24";
            GreenCount.Text = "77";
            YellowCount.Text = "5";
            RedCount.Text = "165";
        }

        /// <summary>
        /// Call this with a title case color like Blue, Green, Yellow, Red.
        /// </summary>
        /// <param name="buttonColorName"></param>
        private void ButtonPressed(string buttonColorName)
        {
            VisualStateManager.GoToState(this, $"{buttonColorName}Pressed", true);
        }
    }
}
