using System.IO.Pipes;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using SoundMeterLib;

namespace SoundMeter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AudioService audioService = new AudioService();

        public delegate void AdjustVolume(int vol);
        public event AdjustVolume AdjustVolumeEvent;
        public Storyboard opacityShow;
        public Storyboard opacityHide;
        public DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            KeyUp += MainWindow_OnKeyUp;
            int vol = audioService.GetVolume();
            lblVolumeNum.Content = vol;
            prgVolume.Value = vol;
            System.Threading.Tasks.Task.Run(PipeServerThread);
            AdjustVolumeEvent += (int vol) => SetVolume(vol);
            const double opacityDuration = 0.5;
            opacityShow = BuildWindowAnimation(1.0, TimeSpan.FromSeconds(opacityDuration));
            opacityHide = BuildWindowAnimation(0.0, TimeSpan.FromSeconds(opacityDuration));
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(5.0);
            timer.Start();
            opacityHide.Completed += (object sender, EventArgs e) => Visibility = Visibility.Hidden;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            opacityHide.Begin();
        }

        protected override void OnActivated(EventArgs e)
        {
            Opacity = 0.0;
            opacityShow.Begin();

            base.OnActivated(e);
        }

        private Storyboard BuildWindowAnimation(double to, TimeSpan duration)
        {
            DoubleAnimation a = new DoubleAnimation(to, duration);
            Storyboard.SetTarget(a, this);
            Storyboard.SetTargetProperty(a, new PropertyPath("Opacity"));

            Storyboard s = new Storyboard();
            s.Children.Add(a);
            return s;
        }

        private void PipeServerThread()
        {
            while (true)
            {
                using (NamedPipeServerStream server = new NamedPipeServerStream(SharedConstants.NAMEDPIPENAME, PipeDirection.In))
                {
                    server.WaitForConnection();
                    Span<byte> buffer;
                    BinaryReader br = new BinaryReader(server);
                    buffer = br.ReadBytes(2);
                    byte command = buffer[0];

                    switch (command)
                    {
                        case SharedConstants.VOLUP:
                            int vol = buffer[1];
                            Dispatcher.Invoke(() =>
                                {
                                    Visibility = Visibility.Visible;
                                    timer.Stop();
                                    opacityShow.Begin();
                                    timer.Start();
                                    AdjustVolumeEvent(vol);
                                }
                            );
                            break;
                        case SharedConstants.VOLDOWN:
                            int volDown = buffer[1];
                            Dispatcher.Invoke(() =>
                                {
                                    Visibility = Visibility.Visible;
                                    timer.Stop();
                                    opacityShow.Begin();
                                    timer.Start();
                                    AdjustVolumeEvent(-volDown);
                                }
                            );
                            break;
                    }
                    server.Disconnect();
                }
            }

        }

        private void SetVolume(int volIncrementNumber)
        {
            int vol = audioService.GetVolume();
            vol += volIncrementNumber;
            vol = audioService.SetVolume(vol);
            lblVolumeNum.Content = $"{vol}%";
            prgVolume.Value = vol;
            timer.Stop();
            timer.Start();
        }


        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            int increment = 10;
            opacityShow.Begin();

            if (e.Key == Key.Up)
            {
                SetVolume(increment);
            }

            if (e.Key == Key.Down)
            {
                SetVolume(-increment);
            }

            if (e.Key == Key.Q)
            {
                Close();
            }
        }
    }
}
