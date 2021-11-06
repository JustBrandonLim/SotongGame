using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TinyStudent.Models;

namespace TinyStudent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string BASE_ADDRESS = "http://localhost:9000/";

        private static readonly HttpClient httpClient = new HttpClient();

        private static int currentQuestionIndex = 0;

        private Question currentQuestion = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "TinyStudentServer.exe"));
        }

        private async void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> options = new List<string>() 
            { 
                OptionOneTextBox.Text,
                OptionTwoTextBox.Text,
                OptionThreeTextBox.Text,
                OptionFourTextBox.Text
            };

            Question questionToAdd = new Question(ContentTextBox.Text, options, AnswerListBox.SelectedIndex);

            var postContent = JsonConvert.SerializeObject(questionToAdd);

            var buffer = Encoding.UTF8.GetBytes(postContent);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var postRequest = await httpClient.PostAsync(BASE_ADDRESS + "/api/question/", byteContent);

            if (postRequest.IsSuccessStatusCode)
                MessageBox.Show("Question has been added!", "TinyStudent", MessageBoxButton.OK);
        }

        private async void ShowAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            AnswerTextBlock.Text = "The answer is: " + currentQuestion.Options[currentQuestion.Answer];
        }

        private async void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var getRequest = await httpClient.GetAsync(BASE_ADDRESS + "/api/question/" + currentQuestionIndex.ToString());

#pragma warning disable CS8601 // Possible null reference assignment.
            currentQuestion = JsonConvert.DeserializeObject<Question>(await getRequest.Content.ReadAsStringAsync());
#pragma warning restore CS8601 // Possible null reference assignment.

            currentQuestionIndex++;

            QuestionContentTextBlock.Text = currentQuestion.Content;
            QuestionNumberTextBlock.Text = "Question No: " + currentQuestionIndex;

            AnswerTextBlock.Text = "";
        }
    }
}
