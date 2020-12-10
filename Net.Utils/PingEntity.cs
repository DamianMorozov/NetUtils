using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Net.Utils
{
    public class PingEntity : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised([CallerMemberName] string memberName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        #endregion

        #region Public fields and properties

        private bool _useRepeat;
        public bool UseRepeat
        {
            get => _useRepeat;
            set
            {
                _useRepeat = value;
                OnPropertyRaised();
            }
        }

        private bool _isStop;
        public bool IsStop
        {
            get => _isStop;
            private set
            {
                _isStop = value;
                OnPropertyRaised();
            }
        }

        private readonly object _locker = new object();

        private int _timeoutPing;
        public int TimeoutPing
        {
            get => _timeoutPing;
            set
            {
                _timeoutPing = value;
                OnPropertyRaised();
            }
        }

        private int _timeoutRepeat;
        public int TimeoutRepeat
        {
            get => _timeoutRepeat;
            set
            {
                _timeoutRepeat = value;
                OnPropertyRaised();
            }
        }

        private string _settings;
        public string Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyRaised();
            }
        }

        private string _log;
        public string Log
        {
            get => _log;
            set
            {
                _log = value;
                OnPropertyRaised();
            }
        }

        private HashSet<string> _hosts;
        public HashSet<string> Hosts
        {
            get => _hosts;
            set
            {
                _hosts = value;
                OnPropertyRaised();
            }
        }

        #endregion

        #region Constructor and destructor

        public PingEntity()
        {
            SetupDefault();
        }

        public PingEntity(int timeoutPing, int timeoutTask, bool useRepeat)
        {
            Setup(timeoutPing, timeoutTask, useRepeat);
        }

        public void SetupDefault()
        {
            Setup(2_500, 1_000, false);
        }

        public void Setup(int timeoutPing, int timeoutRepeat, bool useRepeat)
        {
            TimeoutPing = timeoutPing;
            TimeoutRepeat = timeoutRepeat;
            UseRepeat = useRepeat;
            Settings = string.Empty;
            Log = string.Empty;
            IsStop = true;
            Hosts = new HashSet<string>();
        }

        #endregion

        #region Public and private methods

        public void Open()
        {
            lock (_locker)
            {
                Settings = string.Empty;
                Log = string.Empty;
                var sw = Stopwatch.StartNew();
                try
                {
                    Settings += $"Ping settings: TimeoutPing = [{TimeoutPing}], UseRepeat = [{UseRepeat}], TimeoutRepeat = [{TimeoutRepeat}]."
                                + Environment.NewLine;
                    IsStop = false;
                    do
                    {
                        using (var ping = new Ping())
                        {
                            foreach (var host in Hosts)
                            {
                                try
                                {
                                    if (IsStop) return;
                                    var reply = ping.Send(host.Trim(), TimeoutPing);
                                    if (reply is null)
                                    {
                                        Log += "Reply is null" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        Log += $"Exchange packages with {host} with {reply.Buffer.Length} bytes" + Environment.NewLine;
                                        Log += $"Reply from {reply.Address}: status = {reply.Status}, roundtrip time = {reply.RoundtripTime} ms, TTL = {reply.Options.Ttl}" + Environment.NewLine;
                                    }
                                }
                                catch (PingException pex)
                                {
                                    Log += $"Ping exception: {pex.Message}" + Environment.NewLine;
                                    //if (!(pex.InnerException is null))
                                    //    Log += $"Ping inner exception: {pex.InnerException.Message}" + Environment.NewLine;
                                }
                            }
                            System.Threading.Thread.Sleep(TimeoutRepeat);
                            if (UseRepeat)
                                Log += $"Waiting {TimeoutRepeat} milliseconds" + Environment.NewLine;
                        }
                        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                    } while (UseRepeat);
                }
                catch (Exception ex)
                {
                    Log += $"Ping exception: {ex.Message}" + Environment.NewLine;
                    if (!(ex.InnerException is null))
                        Log += $"Ping inner exception: {ex.InnerException.Message}" + Environment.NewLine;
                    throw;
                }
                finally
                {
                    IsStop = true;
                }
                sw.Stop();
            }
        }

        public async Task OpenAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            Open();
        }

        public void Close()
        {
            IsStop = true;
        }

        public async Task CloseAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            Close();
        }

        #endregion
    }
}
