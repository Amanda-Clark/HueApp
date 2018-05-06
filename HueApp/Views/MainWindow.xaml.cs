using System;
using System.Windows;
using MyToolkit.Mvvm;
using HueApp.ViewModels;
using System.Linq;
using HueApp.Models;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Windows.Media;



namespace HueApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> lightDict = new Dictionary<string, string>();
        ConcurrentDictionary<string, LightState> Lights = new ConcurrentDictionary<string, LightState>();
        public bool OnLoad = true;
        public MainWindow()
        {
            InitializeComponent();
           
            ViewModelHelper.RegisterViewModel(Model, this);
           
        }

       
        public MainWindowModel Model
        {
            get { return (MainWindowModel)Resources["ViewModel"]; }
        }

        private void BriSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (OnLoad)
            {
                return;
            }
            else
            {
                string lightSelected = LightsCombo.SelectedValue.ToString();
                Model.SetLightState(slBri.Value.ToString(), lightSelected, "Bri");
                Lights = Model.GetLightStates();
                var newRGB = Helpers.RgbHelper.GetRGB(slHue.Value, slSat.Value, slBri.Value);
                ColorPreview.IsEnabled = false;
                ColorPreview.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(newRGB.R), Convert.ToByte(newRGB.G), Convert.ToByte(newRGB.B)));
            }
            
        }
        private void HueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (OnLoad)
            {
                return;
            }
            else
            {
                string lightSelected = LightsCombo.SelectedValue.ToString();
                Model.SetLightState(slHue.Value.ToString(), lightSelected, "Hue");
                Lights = Model.GetLightStates();
                var newRGB = Helpers.RgbHelper.GetRGB(slHue.Value, slSat.Value, slBri.Value);
                ColorPreview.IsEnabled = false;
                ColorPreview.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(newRGB.R), Convert.ToByte(newRGB.G), Convert.ToByte(newRGB.B)));
            }
            
        }
        private void SatSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (OnLoad)
            {
                return;
            }
            else
            {
                string lightSelected = LightsCombo.SelectedValue.ToString();
                Model.SetLightState(slSat.Value.ToString(), lightSelected, "Sat");
                Lights = Model.GetLightStates();
                var newRGB = Helpers.RgbHelper.GetRGB(slHue.Value, slSat.Value, slBri.Value);
                ColorPreview.IsEnabled = false;
                ColorPreview.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(newRGB.R), Convert.ToByte(newRGB.G), Convert.ToByte(newRGB.B)));
            }
            
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
             Lights = Model.GetLightStates();
             if (Lights != null && Lights.Count > 0)
             {
                 SetInitialSliderState(Lights);
             }
            
        }

        public void SetInitialSliderState(ConcurrentDictionary<string, LightState> Lights)
        {

            foreach (KeyValuePair<string, LightState> pair in Lights)
            {
                lightDict.Add(pair.Key, pair.Value.LampName);


            }
            RGB newRGB = new RGB();
            LightsCombo.ItemsSource = lightDict;
            LightsCombo.DisplayMemberPath = "Value";
            LightsCombo.SelectedValuePath = "Key";
            var firstKey = lightDict.Take(1).Select(d => d.Key).First();
            LightsCombo.SelectedIndex = 0;
            slHue.Value = Lights[firstKey].HueState.Hue;
            slSat.Value = Lights[firstKey].HueState.Sat;
            slBri.Value = Lights[firstKey].HueState.Bri;
            var isOn = Lights[firstKey].HueState.On;
            if (!isOn)
            {
                slBri.IsEnabled = false;
                slHue.IsEnabled = false;
                slSat.IsEnabled = false;
                StatusLbl.Visibility = Visibility.Visible;
                LightOff.IsChecked = false;
               

            }
            else
            {
                slBri.IsEnabled = true;
                slHue.IsEnabled = true;
                slSat.IsEnabled = true;
                StatusLbl.Visibility = Visibility.Hidden;
                LightOff.IsChecked = true;

            }
            newRGB = Helpers.RgbHelper.GetRGB(slHue.Value, slSat.Value, slBri.Value);
            ColorPreview.IsEnabled = false;
            ColorPreview.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(newRGB.R), Convert.ToByte(newRGB.G), Convert.ToByte(newRGB.B)));
            
            OnLoad = false;
            
        }

        public void LightsCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OnLoad)
            {
                return;
            }
            else
            {
                string lightSelected = LightsCombo.SelectedValue.ToString();
                var isOn = Lights[lightSelected].HueState.On;
                if (!isOn)
                {
                    slBri.IsEnabled = false;
                    slHue.IsEnabled = false;
                    slSat.IsEnabled = false;
                    StatusLbl.Visibility = Visibility.Visible;
                    LightOff.IsChecked = false;
                   
                }
                else
                {
                    slHue.Value = Lights[lightSelected].HueState.Hue;
                    slSat.Value = Lights[lightSelected].HueState.Sat;
                    slBri.Value = Lights[lightSelected].HueState.Bri;
                    StatusLbl.Visibility = Visibility.Hidden;
                    LightOff.IsChecked = true;
                    var newRGB = Helpers.RgbHelper.GetRGB(slHue.Value, slSat.Value, slBri.Value);
                    ColorPreview.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(newRGB.R), Convert.ToByte(newRGB.G), Convert.ToByte(newRGB.B)));
                   
                }
               
            }

        }

        private void LightOff_Checked(object sender, RoutedEventArgs e)
        {
            string lightSelected = LightsCombo.SelectedValue.ToString();
            if (LightOff.IsChecked == true)
            {
                Model.TurnLightOn(lightSelected);
                StatusLbl.Visibility = Visibility.Hidden;
            }
            else
            {
                Model.TurnLightOff(lightSelected);
                StatusLbl.Visibility = Visibility.Visible;
            }
        }
    }
}
