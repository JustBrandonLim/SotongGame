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

        private static readonly HttpClient httpClient = new();

        private int currentQuestionIndex = 0;
        private Question currentQuestion;
        private Answer currentAnswer;

        private Random random = new Random();
        
        public MainWindow()
        {
            InitializeComponent();
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

            var postRequest = await httpClient.PostAsync(BASE_ADDRESS + "/api/question/addquestion", byteContent);

            if (postRequest.IsSuccessStatusCode)
                MessageBox.Show("Question has been added!", "TinyStudent", MessageBoxButton.OK);
        }

        private async void ShowSubmissionsButton_Click(object sender, RoutedEventArgs e)
        {
            int optionOneCount = 0;
            int optionTwoCount = 0;
            int optionThreeCount = 0;
            int optionFourCount = 0;

            HttpResponseMessage getSubmissionsRequest = await httpClient.GetAsync(BASE_ADDRESS + "/api/question/getsubmissions/");

            #pragma warning disable CS8601 // Possible null reference assignment.
            currentAnswer = JsonConvert.DeserializeObject<Answer>(await getSubmissionsRequest.Content.ReadAsStringAsync());

            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (int answerSubmitted in currentAnswer.Answers)
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                switch (answerSubmitted)
                {
                    case 1:
                        optionOneCount++;
                        break;
                    case 2:
                        optionTwoCount++;  
                        break;
                    case 3:
                        optionThreeCount++; 
                        break;
                    case 4:
                        optionFourCount++; 
                        break;
                }
            }

            #pragma warning restore CS8601 // Possible null reference assignment.
            OptionOneSubmissionsTextBlock.Text = "Option 1: " + optionOneCount.ToString();
            OptionTwoSubmissionsTextBlock.Text = "Option 2: " + optionTwoCount.ToString();
            OptionThreeSubmissionsTextBlock.Text = "Option 3: " + optionThreeCount.ToString();
            OptionFourSubmissionsTextBlock.Text = "Option 4: " + optionFourCount.ToString();
        }

        private void ShowAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The answer is: " + currentQuestion.Options[currentQuestion.Answer], "TinyStudent", MessageBoxButton.OK);
        }

        private async void GetQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            OptionOneSubmissionsTextBlock.Text = "";
            OptionTwoSubmissionsTextBlock.Text = "";
            OptionThreeSubmissionsTextBlock.Text = "";
            OptionFourSubmissionsTextBlock.Text = "";

            HttpResponseMessage getQuestionIndexRequest = await httpClient.GetAsync(BASE_ADDRESS + "/api/question/getquestionindex/");
            HttpResponseMessage getQuestionRequest = await httpClient.GetAsync(BASE_ADDRESS + "/api/question/getquestion/");

            string questionIndexContent = await getQuestionIndexRequest.Content.ReadAsStringAsync();
            currentQuestionIndex = Convert.ToInt32(questionIndexContent);

            #pragma warning disable CS8601 // Possible null reference assignment.
            currentQuestion = JsonConvert.DeserializeObject<Question>(await getQuestionRequest.Content.ReadAsStringAsync());

            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (currentQuestion.Content == "NO AVAILABLE QUESTIONS")
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                QuestionNumberTextBlock.Text = "Q?";
                OptionOneTextBlock.Text = "Option 1: ";

                OptionTwoTextBlock.Text = "Option 2: ";

                OptionThreeTextBlock.Text = "Option 3: ";

                OptionFourTextBlock.Text = "Option 4: ";
            }
            else
            {
                List<Brush> kahootColours = new()
                {
                    Brushes.OrangeRed,
                    Brushes.PaleGreen,
                    Brushes.SkyBlue,
                    Brushes.Gold
                };

                QuestionNumberTextBlock.Text = "Q" + currentQuestionIndex.ToString();

                OptionOneTextBlock.Text = "Option 1: " + currentQuestion.Options[0];
                int optionOneColour = random.Next(0, 4);
                OptionOneTextBlock.Background = kahootColours[optionOneColour];
                kahootColours.Remove(kahootColours[optionOneColour]);

                OptionTwoTextBlock.Text = "Option 2: " + currentQuestion.Options[1];
                int optionTwoColour = random.Next(0, 3);
                OptionTwoTextBlock.Background = kahootColours[optionTwoColour];
                kahootColours.Remove(kahootColours[optionTwoColour]);

                OptionThreeTextBlock.Text = "Option 3: " + currentQuestion.Options[2];
                int optionThreeColour = random.Next(0, 2);
                OptionThreeTextBlock.Background = kahootColours[optionThreeColour];
                kahootColours.Remove(kahootColours[optionThreeColour]);

                OptionFourTextBlock.Text = "Option 4: " + currentQuestion.Options[3];
                int optionFourColour = random.Next(0, 1);
                OptionFourTextBlock.Background = kahootColours[optionFourColour];
                kahootColours.Remove(kahootColours[optionFourColour]);
            }

            QuestionContentTextBlock.Text = currentQuestion.Content;
            #pragma warning restore CS8601 // Possible null reference assignment.

            ShowSubmissionsButton.IsEnabled = true;
            ShowAnswerButton.IsEnabled = true;
        }
    }
}
