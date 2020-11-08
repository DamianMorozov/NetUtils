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
    public class PingEntity
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised([CallerMemberName] string memberName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        #endregion

        #region Public fields and properties

        private bool _isTaskWait;
        public bool IsTaskWait
        {
            get => _isTaskWait;
            set
            {
                _isTaskWait = value;
                OnPropertyRaised();
            }
        }

        private bool _isRepeat;
        public bool IsRepeat
        {
            get => _isRepeat;
            set
            {
                _isRepeat = value;
                OnPropertyRaised();
            }
        }

        private bool _isTaskActive;
        public bool IsTaskActive
        {
            get => _isTaskActive;
            private set
            {
                _isTaskActive = value;
                OnPropertyRaised();
            }
        }

        private Task _task;

        private bool _isTimeout;
        public bool IsTimeout
        {
            get => _isTimeout;
            set
            {
                _isTimeout = value;
                OnPropertyRaised();
            }
        }

        private int _timeout;
        public int Timeout
        {
            get => _timeout;
            set
            {
                _timeout = value;
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

        private Dictionary<string, bool> _hosts;
        public Dictionary<string, bool> Hosts
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

        public PingEntity(bool isTimeout, int timeout, bool isRepeat)
        {
            Setup(isTimeout, timeout, isRepeat);
        }

        public void SetupDefault()
        {
            Setup(false, 2_500, false);
        }

        public void Setup(bool isTimeout, int timeout, bool isRepeat)
        {
            IsTimeout = isTimeout;
            Timeout = timeout;
            IsRepeat = isRepeat;
            Status = string.Empty;
            IsTaskWait = true;
            IsTaskActive = false;
            Hosts = new Dictionary<string, bool>();
        }

        #endregion

        #region Public and private methods

        public void OpenTask()
        {
            if (!(_task is null))
            {
                if (_task.Status == TaskStatus.RanToCompletion)
                {
                    _task.Dispose();
                    _task = null;
                }
            }
            _task = Task.Run(async () =>
            {
                await OpenTaskAsync(IsRepeat);
            });
            if (IsTaskWait)
                _task.Wait();
        }

        private async Task OpenTaskAsync(bool isRepeat)
        {
            IsTaskActive = true;
            Status = string.Empty;
            await Task.Delay(TimeSpan.FromMilliseconds(10)).ConfigureAwait(false);
            var host = string.Empty;
            var sw = Stopwatch.StartNew();
            try
            {
                Status += $"[{sw.Elapsed}] Task started." + Environment.NewLine;
                Status += $"[{sw.Elapsed}] IsTaskWait = [{IsTaskWait}]. IsRepeat = [{IsRepeat}]. Timeout = [{Timeout}]." + Environment.NewLine;
                do
                {
                    using (var ping = new Ping())
                    {
                        foreach (var hostDic in Hosts)
                        {
                            try
                            {
                                host = hostDic.Key;
                                var reply = IsTimeout ? ping.Send(host.Trim(), Timeout) : ping.Send(host.Trim());
                                Status += reply != null 
                                    ? $"[{sw.Elapsed}] Host: {host}. Address: {reply.Address}. Status: {reply.Status}." + Environment.NewLine
                                    : $"[{sw.Elapsed}] Reply is null" + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                Status += $"[{sw.Elapsed}] Ping {host} exception: {ex.Message}" + Environment.NewLine;
                                if (!(ex.InnerException is null))
                                    Status += $"[{sw.Elapsed}] Ping {host} inner exception: {ex.InnerException.Message}" + Environment.NewLine;
                            }
                        }
                        Status += $"[{sw.Elapsed}] Waiting {_timeout} milliseconds" + Environment.NewLine;
                        await Task.Delay(TimeSpan.FromMilliseconds(10)).ConfigureAwait(false);
                    }
                } while (IsTaskActive && isRepeat);
                Status += $"[{sw.Elapsed}] Task finished." + Environment.NewLine;
                IsTaskActive = false;
            }
            catch (Exception ex)
            {
                Status += $"[{sw.Elapsed}] Ping {host} exception: {ex.Message}";
                if (!(ex.InnerException is null))
                    Status += $"[{sw.Elapsed}] Ping {host} inner exception: {ex.InnerException.Message}";
            }
            sw.Stop();
        }

        public void Close()
        {
            IsTaskActive = false;
        }

        #endregion
    }
}
