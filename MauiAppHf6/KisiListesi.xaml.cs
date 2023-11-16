using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MauiAppHf6
{
    public partial class KisiListesi : ContentPage
    {
        public KisiListesi()
        {
            InitializeComponent();

            if (File.Exists(dataFile))
            {
                var data = File.ReadAllText(dataFile);
                Kisiler = JsonSerializer.Deserialize<ObservableCollection<Kisi>>(data);
            }

            listKisiler.ItemsSource = Kisiler;
        }

        ObservableCollection<Kisi> Kisiler = new ObservableCollection<Kisi> ();
        string dataFile = Path.Combine( FileSystem.Current.AppDataDirectory, "kisilerData.json");

        private async void KisiEkle(object sender, EventArgs e)
        {
            KisiDetay page = new KisiDetay()
            {
                Title="Kişi Ekle",
                AddMethod = AddKisi
            };
            await Navigation.PushModalAsync(page, true);

        }

        private void AddKisi(Kisi kisi)
        {
            Kisiler.Add(kisi);
        }

        private async void KisiDuzenle(object sender, EventArgs e)
        {
            var ib = sender as ImageButton;
            var kisi = Kisiler.First(o=>o.ID == ib.CommandParameter.ToString());
            KisiDetay page = new KisiDetay(kisi)
            {
                Title="Kişi Ekle",
                AddMethod = AddKisi
            };
            await Navigation.PushModalAsync(page, true);
        }

        private async void KisiSil(object sender, EventArgs e)
        {
            var ib = sender as ImageButton;
            var kisi = Kisiler.First(o=>o.ID == ib.CommandParameter.ToString());
           
            bool answer = await DisplayAlert("Silmeyi Onayla", $"{kisi.AdSoyad} silinsin mi", "Evet", "Hayır");
           
            if(answer)
                Kisiler.Remove(kisi);
        }

        private void SaveList(object sender, EventArgs e)
        {
            var jsonData = JsonSerializer.Serialize<ObservableCollection<Kisi>>(Kisiler);

            File.WriteAllText(dataFile, jsonData);
        }
    }


    public class Kisi : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName]string propName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private string id, ad, soy, tel, mail;
        public string ID {
            get
            {
                if (id == null)
                    id = Guid.NewGuid().ToString();
                return id;
            }
            set { id = value; }
        }

        public string Resim  => "person.png";
        public string Ad
        {
            get => ad;
            set { ad = value; 
                NotifyPropertyChanged(); 
                NotifyPropertyChanged("AdSoyad"); 
            }
        }
        public string Soyad 
        {
            get => soy;
            set
            {
                soy = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("AdSoyad");
            }
        }

        [JsonIgnore]
        public string AdSoyad => Ad + " " + Soyad;

        public string Telefon {
            get => tel;
            set
            {
                tel = value;
                NotifyPropertyChanged();
            }
        }
        public string Mail {
            get => mail;
            set
            {
                mail = value;
                NotifyPropertyChanged();
            }
        }

    }
}
