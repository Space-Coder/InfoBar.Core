using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InfoBar.Core
{
    public enum InfoBarStatus
    {
        CRITICAL,
        SUCCESS,
        ATTENTION,
        CAUTION
    }

    public enum InfoBarPosition
    {
        TOP,
        BOTTOM
    }
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class InfoBarBox : UserControl
    {
        private readonly CircleEase circleEase = new()
        {
            EasingMode = EasingMode.EaseInOut
        };
        private Timer timer = new();
        private bool isOpenBar = false;
        private InfoBarPosition savedPosition;

        public InfoBarBox()
        {
            InitializeComponent();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                ShowHideAnimation(false, savedPosition);
            });
        }

        /// <summary>
        /// Activate animation for show or hide bar
        /// </summary>
        /// <param name="isOpen">Set bar show or hide</param>
        /// <param name="barPosition">Set bar position</param>
        private void ShowHideAnimation(bool isOpen, InfoBarPosition barPosition = InfoBarPosition.TOP)
        {
            ThicknessAnimation showAnimation = new()
            {
                From = this.Margin,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = circleEase
            };
            if (barPosition == InfoBarPosition.TOP)
            {
                this.VerticalAlignment = VerticalAlignment.Top;
                this.Margin = new Thickness(this.Margin.Left, (-35) - this.ActualHeight, this.Margin.Right, this.Margin.Bottom);
                double newMargin = isOpen == true ? 15 : (-35) - this.ActualHeight;
                showAnimation.To = new Thickness(this.Margin.Left, newMargin, this.Margin.Right, this.Margin.Bottom);
            }
            else
            {
                this.VerticalAlignment = VerticalAlignment.Bottom;
                this.Margin = new Thickness(this.Margin.Left, this.Margin.Top, this.Margin.Right, (-35) - this.ActualHeight);
                double newMargin = isOpen == true ? 15 : (-35) - this.ActualHeight;
                showAnimation.To = new Thickness(this.Margin.Left, this.Margin.Top, this.Margin.Right, newMargin);
            }

            showAnimation.Completed += ShowAnimation_Completed;

            if (isOpen == true)
            {
                timer.Start();
                SystemSounds.Hand.Play();
            }

            isOpenBar = isOpen;
            this.BeginAnimation(MarginProperty, showAnimation);
        }

        private void ShowAnimation_Completed(object sender, EventArgs e)
        {
            if (isOpenBar == false)
            {
                this.Effect = null;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ShowHideAnimation(false, savedPosition);
        }

        /// <summary>
        /// Show bar with message
        /// </summary>
        /// <param name="message">Message value</param>
        /// <param name="status">Message status (Critical, Success, Attention, Caution)</param>
        /// <param name="position">Set bar position</param>
        /// <param name="timerInterval">Set timer interval for hide bar</param>
        public void ShowMessage(string message, InfoBarStatus status, InfoBarPosition position, double timerInterval = 60000)
        {

            TextInfo statusText = new CultureInfo("en-US", false).TextInfo;
            savedPosition = position;
            timer.Interval = timerInterval;
            BackgroundBorder.Background = (SolidColorBrush)Application.Current.Resources[$"Info{statusText.ToTitleCase(status.ToString().ToLower())}Background"];
            IconBackground.Fill = (SolidColorBrush)Application.Current.Resources[$"Info{statusText.ToTitleCase(status.ToString().ToLower())}"];
            Icon.Data = (Geometry)Resources[$"{statusText.ToTitleCase(status.ToString().ToLower())}Icon"];
            CaptionLabel.Content = statusText.ToTitleCase(status.ToString().ToLower());
            this.Effect = (System.Windows.Media.Effects.Effect)Application.Current.Resources["ShadowFlyout"];

            MessageLabel.Text = message;
            if (SetBarHeight(message) < 328)
            {
                this.Height = 55;
            }
            else
            {
                MessageLabel.UpdateLayout();
                this.Height = 55 + (this.MessageLabel.ActualHeight) - 15;
            }
            ShowHideAnimation(true, position);
        }

        /// <summary>
        /// Calculate width of formatted text
        /// </summary>
        /// <param name="message">Text value to be calculated</param>
        /// <returns></returns>
        private double SetBarHeight(string message)
        {
            FormattedText formattedText = new(message,
                new CultureInfo("en-US"),
                FlowDirection.LeftToRight,
                new Typeface(this.MessageLabel.FontFamily, this.MessageLabel.FontStyle, this.MessageLabel.FontWeight, this.MessageLabel.FontStretch),
                this.MessageLabel.FontSize,
                Brushes.Black);
            return formattedText.Width;
        }
    }
}