using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AbisMonitor.UI.Common.Controls
{
    public class NumberTextBox : System.Windows.Controls.TextBox
    {
        public NumberTextBox()
        {
            this.ContextMenu = null;

            System.Windows.DataObject.AddPastingHandler(this, Text_Pasting);

            System.Windows.Input.InputMethod.SetIsInputMethodEnabled(this, false);
        }


        protected override void OnTextChanged(System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                this.Text = null;
            }
            base.OnTextChanged(e);
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
                || (e.Key >= Key.D0 && e.Key <= Key.D9 && e.KeyboardDevice.Modifiers != ModifierKeys.Shift) ||
                e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Enter || e.Key == Key.Tab ||
                e.Key == Key.OemBackTab ||
                ((e.Key == Key.Subtract || e.Key == Key.OemMinus) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
                ||
                ((e.Key == Key.OemPeriod || e.Key == Key.Decimal) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift))
            {
                if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
                {
                    //允许输入负数时，false表示输入可用
                    e.Handled = !AllowNegative;
                }
                else if (e.Key == Key.OemPeriod || e.Key == Key.Decimal)
                {
                    e.Handled = !AllowDecimalPoint;
                }
                else
                {
                    e.Handled = false;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Text_Pasting(object sender, System.Windows.DataObjectPastingEventArgs e)
        {
            //禁止Pasting
            e.CancelCommand();
        }

        public static readonly DependencyProperty AllowNegativeProperty = DependencyProperty.Register(
            "AllowNegative", typeof(bool), typeof(NumberTextBox), new PropertyMetadata(false));

        /// <summary>
        /// 设置是否允许输入负数
        /// </summary>
        public bool AllowNegative
        {
            get { return (bool)GetValue(AllowNegativeProperty); }
            set { SetValue(AllowNegativeProperty, value); }
        }

        public static readonly DependencyProperty AllowDecimalPointProperty = DependencyProperty.Register(
            "AllowDecimalPoint", typeof(bool), typeof(NumberTextBox), new PropertyMetadata(false));

        public bool AllowDecimalPoint
        {
            get { return (bool)GetValue(AllowDecimalPointProperty); }
            set { SetValue(AllowDecimalPointProperty, value); }
        }



    }
}
