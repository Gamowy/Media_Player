using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.WAV;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.Effects;
using Media_Player.Model;
using Media_Player.ViewModel.BaseClass;
using Microsoft.Win32;

namespace Media_Player.ViewModel
{
    class EqualizerViewModel : ViewModelBase
    {
        public EqualizerViewModel()
        {

        }

        private const double MaxDB = 20;
        private Equalizer _equalizer;
        private ISoundOut _soundOut;
        private IWaveSource _source;
        public EventHandler? Close;

        #region Properties
        public string Filename { get; set; }
        
        #endregion
        #region Methods
        public void Start()
        {
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                _soundOut = new WasapiOut();
            else
                _soundOut = new DirectSoundOut();

            _source = CodecFactory.Instance.GetCodec(Filename)
                .Loop()
                .ChangeSampleRate(32000)
                .ToSampleSource()
                .AppendSource(Equalizer.Create10BandEqualizer, out _equalizer)
                .ToWaveSource();
            _soundOut.Initialize(_source);
            _soundOut.Play();
        }
        public void Stop()
        {
            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _equalizer.Dispose();
                _soundOut = null;
            }
        }

        public void ChangeFilter(int index, double val)
        {
            double perc = (val / (double)100);
            var value = (float)(perc * MaxDB);
            EqualizerFilter filter = _equalizer.SampleFilters[index];
            filter.AverageGainDB = value;
        }

        private void save_file()
        {
            var sfn = new Microsoft.Win32.SaveFileDialog();
            sfn.Filter = "Audio (*.wav)|*.wav";
            if (sfn.ShowDialog() == true)
            {
                using (var source = CodecFactory.Instance.GetCodec(Filename))
                {
                    var equalizer = Equalizer.Create10BandEqualizer(source.ToSampleSource());
                    for (int i = 0; i < equalizer.SampleFilters.Count; i++)
                    {
                        equalizer.SampleFilters[i].AverageGainDB = _equalizer.SampleFilters[i].AverageGainDB; // Ustawienie wzmocnienia
                    }

                    // Konwersja do formatu wyjściowego
                    try
                    {
                        var waveSource = equalizer.ToWaveSource();
                        using (var waveWriter = new WaveWriter(sfn.FileName, waveSource.WaveFormat))
                        {
                            byte[] buffer = new byte[waveSource.WaveFormat.BytesPerSecond];
                            int read;

                            while ((read = waveSource.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                waveWriter.Write(buffer, 0, read);
                            }
                        }
                        MessageBox.Show("Zapisano pomyślnie", "Zapis", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    catch(Exception e)
                    {
                        MessageBox.Show($"Błąd zapisu:\n{e.Message}", "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                Close(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Commands
        public ICommand SaveFile
        {
            get
            {
                return new RelayCommand(execute => save_file(), canExecute => true);
            }
        }
        #endregion

    }

    public class MyDouble : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _value;
        public double Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
