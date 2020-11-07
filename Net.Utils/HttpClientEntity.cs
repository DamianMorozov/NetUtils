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
                _host = value;
                OnPropertyRaised();
            }
        }

        private StringBuilder _status;
        public StringBuilder Status
        {
            get => _status;
            set
            {
                _status = value;
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

        public void SetupDefault()
        {
            Timeout = 5_000;
            Host = new Uri(@"http://webcode.me");
        }

        #endregion

        #region Public and private methods

        public void OpenTask(string url, ProxyEntity proxy, bool isTimeout)
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
                await OpenTaskAsync(url, proxy, isTimeout);
            });
        }

        public async Task OpenTaskAsync(string url, ProxyEntity proxy, bool isTimeout)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100)).ConfigureAwait(true);
            Status.Clear();
            var sw = Stopwatch.StartNew();
            try
            {
                Status.Append($"[{sw.Elapsed}] Get started. Use proxy = [{proxy.Use}]. Timeout = [{Timeout}].");
                Status.Append($"[{sw.Elapsed}] Url = [{url}]");
                using (var httpClient = GetHttpClient(proxy))
                {
                    if (isTimeout)
                        httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout);
                    var response = await httpClient.GetAsync(url);
                    Status.Append($"[{sw.Elapsed}] Status code: {response.StatusCode}");
                    var rawContent = await response.Content.ReadAsStringAsync();
                    Status.Append($"[{sw.Elapsed}] Response Content: {rawContent}");
                    Status.Append($"[{sw.Elapsed}] response.IsSuccessStatusCode : {response.IsSuccessStatusCode }");
                }
                Status.Append("[{sw.Elapsed}] Get finished.");
            }
            catch (Exception ex)
            {
                Status.Append($"[{sw.Elapsed}] {ex.Message}");
                Status.Append($"[{sw.Elapsed}] {ex.StackTrace}");
                if (ex.InnerException != null)
                    Status.Append($"[{sw.Elapsed}] {ex.InnerException.Message}");
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
