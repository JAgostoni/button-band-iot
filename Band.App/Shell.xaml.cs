using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Band.App
{
    public sealed partial class Shell
    {
        private readonly Frame _rootFrame;

        public Shell(Frame rootFrame)
        {
            _rootFrame = rootFrame;

            InitializeComponent();

            MainSplitView.Content = _rootFrame;
            _rootFrame.Navigated += RootFrame_Navigated;
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            // Update radiobuttons after frame navigates
            var type = _rootFrame.CurrentSourcePageType;
            var navButtons = GetAllNavigationButtons(this);
            foreach (var navButton in navButtons)
            {
                var target = navButton.CommandParameter as NavigationButtonType;
                if (target == null)
                    continue;

                navButton.IsChecked = target.Type == type;
            }
            MainSplitView.IsPaneOpen = false;
        }

        private List<RadioButton> GetAllNavigationButtons(DependencyObject parent)
        {
            var list = new List<RadioButton>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is RadioButton)
                {
                    list.Add(child as RadioButton);
                    continue;
                }
                list.AddRange(GetAllNavigationButtons(child));
            }
            return list;

        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            _rootFrame.Navigate(typeof(AboutPage));
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _rootFrame.Navigate(typeof(MainPage));
        }
    }

}
