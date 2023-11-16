namespace MauiAppHf6
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void slider1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblLabel1.Text = e.NewValue.ToString("F0");
        }
    }
}