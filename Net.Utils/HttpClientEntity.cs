using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Net.Utils
{
    public class HttpClientEntity
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
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

        private bool _isTaskFinished;
        public bool IsTaskFinished
        {
            get => _isTaskFinished;
            private set
            {
                _isTaskFinished = value;
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

        private Uri _host;
        public Uri Host
        {
            get => _host;
            set
            {
                if (!value.ToString().Contains("http://") && !value.ToString().Contains("https://"))
                    value = new Uri("http://" + value);
                _host = value;
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

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyRaised();
            }
        }

        private Task _task;

        #endregion

        #region Constructor and destructor

        public HttpClientEntity()
        {
            SetupDefault();
        }

        public HttpClientEntity(bool isTimeout, int timeout, Uri host)
        {
            Setup(isTimeout, timeout, host);
        }

        public void SetupDefault()
        {
            Setup(false, 2_500, new Uri(@"http://localhost"));
        }

        public void Setup(bool isTimeout, int timeout, Uri host)
        {
            IsTaskWait = true;
            IsTimeout = isTimeout;
            Timeout = timeout;
            Host = host;
            Status = string.Empty;
            Content = string.Empty;
            IsTaskFinished = false;
        }

        #endregion

        #region Public and private methods

        public void OpenTask(ProxyEntity proxy)
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
                await OpenTaskAsync(proxy);
            });
            if (IsTaskWait)
                _task.Wait();
        }

        public async Task OpenTaskAsync(ProxyEntity proxy)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10)).ConfigureAwait(false);
            IsTaskFinished = false;
            Status = string.Empty;
            var sw = Stopwatch.StartNew();
            try
            {
                Status += $"[{sw.Elapsed}] Get started. Use proxy = [{proxy.Use}]. Timeout = [{Timeout}]." + Environment.NewLine;
                Status += $"[{sw.Elapsed}] Url = [{Host}]" + Environment.NewLine;
                using (var httpClient = GetHttpClient(proxy))
                {
                    if (IsTimeout)
                        httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout);
                    var response = await httpClient.GetAsync(Host);
                    Status += $"[{sw.Elapsed}] Status code: {response.StatusCode}" + Environment.NewLine;
                    Content = await response.Content.ReadAsStringAsync();
                    Status += $"[{sw.Elapsed}] response.IsSuccessStatusCode : {response.IsSuccessStatusCode }" + Environment.NewLine;
                }
                Status += "[{sw.Elapsed}] Get finished." + Environment.NewLine;
                IsTaskFinished = true;
            }
            catch (Exception ex)
            {
                Status += $"[{sw.Elapsed}] {ex.Message}" + Environment.NewLine;
                Status += $"[{sw.Elapsed}] {ex.StackTrace}" + Environment.NewLine;
                if (ex.InnerException != null)
                    Status += $"[{sw.Elapsed}] {ex.InnerException.Message}" + Environment.NewLine;
            }
            sw.Stop();
        }

        public HttpClient GetHttpClient(ProxyEntity proxy)
        {
            if (!proxy.Use)
            {
                return new HttpClient(new HttpClientHandler { UseProxy = false });
            }

            if (proxy is null)
                throw new ArgumentException("Poxy is empty!", nameof(proxy));
            var handler = new HttpClientHandler()
            {
                UseProxy = true,
                Proxy = new WebProxy(proxy.Host),
            };
            var httpClient = new HttpClient(handler);
            if (proxy.UseDefaultCredentials)
            {
                handler.UseDefaultCredentials = true;
            }
            else if (!string.IsNullOrWhiteSpace(proxy.Username) && !string.IsNullOrWhiteSpace(proxy.Password))
            {
                handler.PreAuthenticate = false;
                handler.UseDefaultCredentials = false;
                handler.Credentials = !string.IsNullOrWhiteSpace(proxy.Domain)
                    ? new NetworkCredential(proxy.Username, proxy.Password, proxy.Domain)
                    : new NetworkCredential(proxy.Username, proxy.Password);
            }
            return httpClient;
        }

        #endregion
    }
}
