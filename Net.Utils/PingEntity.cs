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

        private bool _taskStop;
        public bool TaskStop
        {
            get => _taskStop;
            private set
            {
                _taskStop = value;
                OnPropertyRaised();
            }
        }

        private Task _task;

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

        private int _timeoutTask;
        public int TimeoutTask
        {
            get => _timeoutTask;
            set
            {
                _timeoutTask = value;
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

        public PingEntity(int timeoutPing, int timeoutTask, bool isRepeat)
        {
            Setup(timeoutPing, timeoutTask, isRepeat);
        }

        public void SetupDefault()
        {
            Setup(2_500, 1_000, false);
        }

        public void Setup(int timeoutPing, int timeoutTask, bool isRepeat)
        {
            TimeoutPing = timeoutPing;
            TimeoutTask = timeoutTask;
            IsRepeat = isRepeat;
            Status = string.Empty;
            TaskStop = true;
            Hosts = new HashSet<string>();
        }

        #endregion

        #region Public and private methods

        public void OpenTask(bool isTaskWait)
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
                await OpenTaskAsync(isTaskWait, IsRepeat);
            });
            if (isTaskWait)
                _task.Wait();
        }

        private async Task OpenTaskAsync(bool isTaskWait, bool isRepeat)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            var sw = Stopwatch.StartNew();
            try
            {
                Status = string.Empty;
                Status += $"[{sw.Elapsed}] Task started." + Environment.NewLine;
                Status +=
                    $"[{sw.Elapsed}] IsTaskWait = [{isTaskWait}]. IsRepeat = [{IsRepeat}]. TimeoutPing = [{TimeoutPing}]. TimeoutTask = [{TimeoutTask}]." +
                    Environment.NewLine;
                TaskStop = false;
                do
                {
                    using (var ping = new Ping())
                    {
                        foreach (var host in Hosts)
                        {
                            if (TaskStop) return;
                            try
                            {
                                var reply = ping.Send(host.Trim(), TimeoutPing);
                                Status += reply != null
                                    ? $"[{sw.Elapsed}] Host: {host}. Address: {reply.Address}. Status: {reply.Status}. RoundtripTime: {reply.RoundtripTime}." +
                                      Environment.NewLine
                                    : $"[{sw.Elapsed}] Reply is null" + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                Status += $"[{sw.Elapsed}] Ping {host} exception: {ex.Message}" + Environment.NewLine;
                                if (!(ex.InnerException is null))
                                    Status +=
                                        $"[{sw.Elapsed}] Ping {host} inner exception: {ex.InnerException.Message}" +
                                        Environment.NewLine;
                            }
                        }

                        await Task.Delay(TimeSpan.FromMilliseconds(TimeoutTask)).ConfigureAwait(false);
                        Status += $"[{sw.Elapsed}] Waiting {TimeoutTask} milliseconds" + Environment.NewLine;
                    }

                    // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                } while (isRepeat);

                Status += $"[{sw.Elapsed}] Task finished." + Environment.NewLine;
            }
            catch (Exception ex)
            {
                Status += $"[{sw.Elapsed}] Ping exception: {ex.Message}";
                if (!(ex.InnerException is null))
                    Status += $"[{sw.Elapsed}] Ping inner exception: {ex.InnerException.Message}";
            }
            finally
            {
                TaskStop = true;
            }
            sw.Stop();
        }

        public void Close()
        {
            Status += "Task stoped." + Environment.NewLine;
            TaskStop = true;
        }

        #endregion
    }
}
