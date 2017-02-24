using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Input;


namespace SecureKeyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public bool IsCode { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            IsCode = false;
        }

        private void BtnCode_Click(object sender, RoutedEventArgs e)
        {
            var username = TxtUsername.Text;
            var password = SecureStringToString(TxtPassword.SecurePassword);
            string result;

            if (GetAuthentication(username, password))
            {
                result = new SecureKey().GetSecureKey(username.ToLower(),password).ToString();
                IsCode = true;
                LabelTip.Content = "Double Click On Code To Copy";
            }
            else
            {
                result = "Incorrect Credentials";
                IsCode = false;
                LabelTip.Content = "";
            }

            TextCode.Text = result ;

        }

        public string SecureStringToString(SecureString value)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        private static bool GetAuthentication(string username, string password)
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:7471/api/") };

            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));

            var loginModel = new { Username = username, Password = password };
            var response = client.PostAsJsonAsync("UsersApi/AuthenticateUser", loginModel).Result;

            return response.IsSuccessStatusCode;
        }

        private void TextCode_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsCode)
            {
                Clipboard.SetText(TextCode.Text);
            }
         
        }
    }
}
