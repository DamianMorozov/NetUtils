using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace NetUtils
{
    public class HttpClientEntity : INotifyPropertyChanged
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
                if (!value.ToString().Contains("http://") && !value.ToString().Contains("https://"))
                    value = new Uri("http://" + value);
                _host = value;
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

        private bool _isStop;
        public bool IsStop
        {
            get => _isStop;
            set
            {
                _isStop = value;
                OnPropertyRaised();
            }
        }

        private object _locker;

        #endregion

        #region Constructor and destructor

        public HttpClientEntity()
        {
            SetupDefault();
        }

        public HttpClientEntity(int timeout, Uri host)
        {
            Setup(timeout, host);
        }

        public void SetupDefault()
        {
            Setup(500, new Uri(@"http://localhost"));
        }

        public void Setup(int timeout, Uri host)
        {
            Timeout = timeout;
            Host = host;
            Content = string.Empty;
            IsStop = true;

            Settings = $"Timeout = [{Timeout}]. Url = [{Host}]";
            Log = string.Empty;
        }

        #endregion

        #region Public and private methods

        public async Task<HttpStatusCode> OpenAsync(ProxyEntity proxy = null)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            HttpStatusCode statusCode = HttpStatusCode.NotFound;
            if (_locker is null)
            {
                _locker = new object();
                Settings += Environment.NewLine + $"Use proxy = [{proxy?.Use}]. " + Environment.NewLine;
                IsStop = false;
                try
                {
                    using (var httpClient = await GetHttpClient(proxy))
                    {
                        if (IsStop) return statusCode;
                        httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout);
                        var response = await httpClient.GetAsync(Host).ConfigureAwait(false);
                        if (IsStop) return statusCode;
                        Log += $"Status code: {response.StatusCode}" + Environment.NewLine;
                        statusCode = response.StatusCode;
                        Content = await response.Content.ReadAsStringAsync();
                        Log += $"response.IsSuccessStatusCode : {response.IsSuccessStatusCode}" + Environment.NewLine;
                    }

                    Log += "Task finished." + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    Log += $"{ex.Message}" + Environment.NewLine;
                    Log += $"{ex.StackTrace}" + Environment.NewLine;
                    if (ex.InnerException != null)
                        Log += $"{ex.InnerException.Message}" + Environment.NewLine;
                }
                finally
                {
                    IsStop = true;
                    _locker = null;
                }
            }
            return statusCode;
        }

        public async Task CloseAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            IsStop = true;
        }

        public async Task<HttpClient> GetHttpClient(ProxyEntity proxy = null)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            if (proxy is null)
            {
                proxy = new ProxyEntity();
            }
            if (!proxy.Use)
            {
                return new HttpClient(new HttpClientHandler { UseProxy = false });
            }

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
