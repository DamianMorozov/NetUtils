using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace NetUtils
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

        private object _locker;

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

        private int _bufferSize;
        public int BufferSize
        {
            get => _bufferSize;
            set
            {
                _bufferSize = value;
                OnPropertyRaised();
            }
        }

        private int _ttl;
        public int Ttl
        {
            get => _ttl;
            set
            {
                _ttl = value;
                OnPropertyRaised();
            }
        }

        private bool _dontFragment;
        public bool DontFragment
        {
            get => _dontFragment;
            set
            {
                _dontFragment = value;
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

        [Obsolete(@"Deprecated method")]
        public PingEntity(int timeoutPing, int timeoutTask, bool useRepeat)
        {
            Setup(timeoutPing, timeoutTask, useRepeat);
        }

        public PingEntity(int timeoutPing, int bufferSize, int ttl, bool dontFragment, int timeoutTask, bool useRepeat)
        {
            Setup(timeoutPing, bufferSize, ttl, dontFragment, timeoutTask, useRepeat);
        }

        public void SetupDefault()
        {
            Setup(2_500, 32, 128, true, 1_000, false);
        }

        [Obsolete(@"Deprecated method")]
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

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="timeoutPing"></param>
        /// <param name="bufferSize">default 32</param>
        /// <param name="ttl">default 128</param>
        /// <param name="dontFragment">default true</param>
        /// <param name="timeoutRepeat"></param>
        /// <param name="useRepeat"></param>
        public void Setup(int timeoutPing, int bufferSize, int ttl, bool dontFragment, int timeoutRepeat, bool useRepeat)
        {
            TimeoutPing = timeoutPing;
            BufferSize = bufferSize;
            Ttl = ttl;
            DontFragment = dontFragment;
            TimeoutRepeat = timeoutRepeat;
            UseRepeat = useRepeat;

            IsStop = true;
            Hosts = new HashSet<string>();

            Settings = $"Ping settings: TimeoutPing = [{TimeoutPing}], BufferSize: [{BufferSize}], Ttl: [{Ttl}], DontFragment:[{DontFragment}], " +
                       $"UseRepeat = [{UseRepeat}], TimeoutRepeat = [{TimeoutRepeat}].";
            Log = string.Empty;
        }

        #endregion

        #region Public and private methods

        public void Open()
        {
            if (_locker is null)
            {
                _locker = new object();
                try
                {
                    IsStop = false;
                    do
                    {
                        foreach (var host in Hosts)
                        {
                            try
                            {
                                using (var ping = new Ping())
                                {
                                    if (IsStop) return;
                                    var buffer = new byte[BufferSize];
                                    var pingOptions = new PingOptions(Ttl, DontFragment);
                                    var reply = ping.Send(host.Trim(), TimeoutPing, buffer, pingOptions);
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
                            }
                            catch (PingException pex)
                            {
                                Log += $"Ping exception: {pex.Message}" + Environment.NewLine;
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
                }
                finally
                {
                    IsStop = true;
                    _locker = null;
                }
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
