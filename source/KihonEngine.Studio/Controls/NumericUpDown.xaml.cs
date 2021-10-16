using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for TemplateNumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        private int _lastValue;
        private bool _autoValidate;

        public NumericUpDown()
        {
            InitializeComponent();

            _lastValue = 0;
            UseBoundValues = false;
            MinValue = 0;
            MaxValue = 100;
        }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public bool UseBoundValues { get; set; }

        public string Text
        {
            get { return tbWrapped.Text; }
            set { tbWrapped.Text = value; }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            var number = 0;
            if (!string.IsNullOrEmpty(tbWrapped.Text))
            {
                number = Convert.ToInt32(tbWrapped.Text);
            }

            if (!UseBoundValues || (UseBoundValues && number < MaxValue))
            {
                _autoValidate = true;
                tbWrapped.Text = Convert.ToString(number + 1);
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            var number = 0;
            if (!string.IsNullOrEmpty(tbWrapped.Text))
            {
                number = Convert.ToInt32(tbWrapped.Text);
            }

            if (!UseBoundValues || (UseBoundValues && number > MinValue))
            {
                _autoValidate = true;
                tbWrapped.Text = Convert.ToString(number - 1);
            }
        }

        private void tbWrapped_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                btnUp.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(btnUp, new object[] { true });
            }

            if (e.Key == Key.Down)
            {
                btnDown.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(btnDown, new object[] { true });
            }
        }

        private void tbWrapped_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(btnUp, new object[] { false });
            }

            if (e.Key == Key.Down)
            {
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(btnDown, new object[] { false });
            }
        }

        private void tbWrapped_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (!string.IsNullOrEmpty(tbWrapped.Text))
            {
                if (!int.TryParse(tbWrapped.Text, out number))
                {
                    tbWrapped.Text = _lastValue.ToString();
                }
                else
                {
                    _lastValue = number;
                }
            }

            if (UseBoundValues && number > MaxValue)
            {
                tbWrapped.Text = MaxValue.ToString();
            }

            if (UseBoundValues && number < MinValue)
            {
                tbWrapped.Text = MinValue.ToString();
            }

            if (_autoValidate)
            {
                tbWrapped.SelectionStart = tbWrapped.Text.Length;

                _autoValidate = false;
                tbWrapped.RaiseEvent(
                    new KeyEventArgs(
                      Keyboard.PrimaryDevice,
                      PresentationSource.FromVisual(tbWrapped),
                      0,
                      Key.Enter)
                    { RoutedEvent = Keyboard.KeyUpEvent }
                  );
            }
        }
    }
}
