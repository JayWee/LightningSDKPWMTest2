using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.IoT.Lightning.Providers;
using Windows.Devices;
using Windows.Devices.Pwm;
using Windows.Devices.Gpio;



namespace LightningSDKPWMTest2
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            InitGpio();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            //Brightness.ValueChanged += Brightness_ValueChanged;
        }

        private void Timer_Tick(object sender, object e)
        {
            int i = 0;
            if (i < 100)
            {
                pwmled[0].SetActiveDutyCyclePercentage(i / 100);
                pwmled[0].Start();
                i++;
            }
            else
            {
                pwmled[0].SetActiveDutyCyclePercentage(i / 100);
                pwmled[0].Start();
                i = 0;
            }
        }

        //private void Brightness_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        //{
        //    double Percentage = e.NewValue / 100;
        //    pwmled[0].SetActiveDutyCyclePercentage(Percentage);
        //    pwmled[0].Start();

        //    Output.Text = Percentage * 100 + "%";
        //}

        private async void InitGpio()
        {
            if (LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
            }

            var gpio = await GpioController.GetDefaultAsync();

            var PWM = await PwmController.GetControllersAsync(LightningPwmProvider.GetPwmProvider());
            var pwm = PWM[1];
            pwm.SetDesiredFrequency(50);

            for (int i = 0; i <= (Pins - 1); i++)
            {
                pwmled[i] = pwm.OpenPin(PWMLED[i]);
                pwmled[i].SetActiveDutyCyclePercentage(0.0);
                pwmled[i].Start();
            }

            if (gpio == null)
            {
                Error.Text = "No GPIO-Device Conected";
                return;
            }
        }

        private const int Pins = 1;
        private static readonly int[] PWMLED = {4};
        private PwmPin[] pwmled = new PwmPin[Pins];
        private DispatcherTimer timer;
    }
}
