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

        private bool _useStopWatch;
        public bool UseStopWatch
        {
            get => _useStopWatch;
            set
            {
                _useStopWatch = value;
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

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
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

        public PingEntity(int timeoutPing, int timeoutTask, bool useRepeat, bool useStopWatch)
        {
            Setup(timeoutPing, timeoutTask, useRepeat, useStopWatch);
        }

        public void SetupDefault()
        {
            Setup(2_500, 1_000, false, false);
        }

        public void Setup(int timeoutPing, int timeoutRepeat, bool useRepeat, bool useStopWatch)
        {
            TimeoutPing = timeoutPing;
            TimeoutRepeat = timeoutRepeat;
            UseRepeat = useRepeat;
            UseStopWatch = useStopWatch;
            Status = string.Empty;
            IsStop = true;
            Hosts = new HashSet<string>();
        }

        #endregion

        #region Public and private methods

        public void Open()
        {
            lock (_locker)
            {
                Status = "Job started." + Environment.NewLine;
                var sw = Stopwatch.StartNew();
                try
                {
                    Status += (UseStopWatch
                        ? $"[{sw.Elapsed}]. Ping settings: TimeoutPing = [{TimeoutPing}], UseRepeat = [{UseRepeat}], TimeoutRepeat = [{TimeoutRepeat}]."
                        : $"Ping settings: TimeoutPing = [{TimeoutPing}], UseRepeat = [{UseRepeat}], TimeoutRepeat = [{TimeoutRepeat}].") +
                          Environment.NewLine;
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
                                        Status += (UseStopWatch ? $"[{sw.Elapsed}]. Reply is null" : "Reply is null") + Environment.NewLine;
                                    }
                                    else
                                    {
                                        Status += (UseStopWatch
                                                ? $"[{sw.Elapsed}]. Exchange packages. Host: {host}. Address: {reply.Address}. Status: {reply.Status}. Buffer.Length: {reply.Buffer.Length}. RoundtripTime: {reply.RoundtripTime}. TTL: {reply.Options.Ttl}."
                                                : $"Host: {host}. Exchange packages. Address: {reply.Address}. Status: {reply.Status}. Buffer.Length: {reply.Buffer.Length}. RoundtripTime: {reply.RoundtripTime}. TTL: {reply.Options.Ttl}.") + Environment.NewLine;
                                    }
                                }
                                catch (PingException pex)
                                {
                                    Status += (UseStopWatch ? $"[{sw.Elapsed}]. Ping exception: {pex.Message}" : $"Ping exception: {pex.Message}") + Environment.NewLine;
                                    if (!(pex.InnerException is null))
                                        Status += (UseStopWatch ? $"[{sw.Elapsed}]. Ping inner exception: {pex.InnerException.Message}" : $"Ping inner exception: {pex.InnerException.Message}") + Environment.NewLine;
                                }
                            }
                            System.Threading.Thread.Sleep(TimeoutRepeat);
                            if (UseRepeat)
                            Status += (UseStopWatch ? $"[{sw.Elapsed}]. Waiting {TimeoutRepeat} milliseconds" : $"Waiting {TimeoutRepeat} milliseconds") + Environment.NewLine;
                        }
                        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                    } while (UseRepeat);
                    Status += (UseStopWatch ? $"[{sw.Elapsed}]. Job finished." : "Job finished.") + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    Status += (UseStopWatch ? $"[{sw.Elapsed}]. Ping exception: {ex.Message}" : $"Ping exception: {ex.Message}" ) + Environment.NewLine;
                    if (!(ex.InnerException is null))
                        Status += (UseStopWatch ? $"[{sw.Elapsed}]. Ping inner exception: {ex.InnerException.Message}" : $"Ping inner exception: {ex.InnerException.Message}") + Environment.NewLine;
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
            Status += "Job stoped." + Environment.NewLine;
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
