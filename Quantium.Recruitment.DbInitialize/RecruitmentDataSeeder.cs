using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;

namespace Quantium.Recruitment.DbInitialize
{
    public interface IDataSeeder
    {
        void Seed();
    }
    public class DataSeeder: IDataSeeder
    {
        private readonly IRecruitmentContext _dbContext;
        public DataSeeder(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            _dbContext.GetDatabase().CreateIfNotExists();

            #region Department
            Department softwareDepartment = new Department() { Name = "Software" };
            Department analyticsDepartment = new Department() { Name = "Analytics" };

            _dbContext.Departments.Add(softwareDepartment);
            _dbContext.Departments.Add(analyticsDepartment);

            #endregion Department

            #region Admin

            Admin admin1 = new Admin()
            {
                FirstName = "Kannan",
                LastName = "Perumal",
                Email = "kannan.perumal@quantium.co.in",
                IsActive = true,
                Mobile = 8886008855,
                Department = softwareDepartment
            };

            Admin admin2 = new Admin()
            {
                FirstName = "Rakesh",
                LastName = "Rohan",
                Email = "Rakesh.Aitipamula@quantium.co.in",
                IsActive = true,
                Mobile = 9052791243,
                Department = softwareDepartment
            };

            Admin admin3 = new Admin()
            {
                FirstName = "Ravi",
                LastName = "Bhaskar",
                Email = "Ravi.Bhaskar@quantium.co.in",
                IsActive = true,
                Mobile = 7799814877,
                Department = softwareDepartment
            };

            Admin admin4 = new Admin()
            {
                FirstName = "Mit",
                LastName = "Suthar",
                Email = "Mit.Suthar@quantium.co.in",
                IsActive = true,
                Mobile = 8886008855,
                Department = softwareDepartment
            };

            Admin admin5 = new Admin()
            {
                FirstName = "Babul",
                LastName = "Reddy",
                Email = "Babul.Yasa@quantium.co.in",
                IsActive = true,
                Mobile = 9618415060,
                Department = softwareDepartment
            };

            Admin admin6 = new Admin()
            {
                FirstName = "Jayaram",
                LastName = "Putineedi",
                Email = "Jayaram.Putineedi@quantium.co.in",
                IsActive = true,
                Mobile = 9542749797,
                Department = softwareDepartment
            };

            Admin admin7 = new Admin()
            {
                FirstName = "Samyuktha",
                LastName = "Kodali",
                Email = "Samyuktha.Kodali@quantium.co.in",
                IsActive = true,
                Mobile = 9651525567,
                Department = softwareDepartment
            };

            Admin admin8 = new Admin()
            {
                FirstName = "Rakesh",
                LastName = "Rohan",
                Email = "rkshrohan@gmail.com",
                IsActive = true,
                Mobile = 9052791243,
                Department = softwareDepartment
            };

            _dbContext.Admins.Add(admin1);
            _dbContext.Admins.Add(admin2);
            _dbContext.Admins.Add(admin3);
            _dbContext.Admins.Add(admin4);
            _dbContext.Admins.Add(admin5);
            _dbContext.Admins.Add(admin6);
            _dbContext.Admins.Add(admin7);
            _dbContext.Admins.Add(admin8);

            #endregion Admin

            #region Job

            Job job1 = new Job()
            {
                Title = "Mid-Level Software Developer",
                Profile = "Software Developer is responsible for requirements analysis and object modelling of new software features and ensuring adequate testing and quality control measures are followed. The ideal candidates should have very strong technical ability and be able to apply programming techniques to solve complex problems. They should also have well developed written and verbal communication skills",
                Department = softwareDepartment
            };

            Job job2 = new Job()
            {
                Title = "Data Analyst",
                Profile = "We are looking for a passionate Data Analyst to turn data into information, information into insight and insight into business decisions.Data analyst responsibilities include conducting full lifecycle activities to include requirements analysis and design, developing analysis and reporting capabilities,and continuously monitoring performance and quality control plans to identify improvements",
                Department = analyticsDepartment
            };

            _dbContext.Jobs.Add(job1);
            _dbContext.Jobs.Add(job2);

            #endregion Job

            #region Candidate

            Candidate candidate1 = new Candidate
            {
                FirstName = "Aman",
                LastName = "Agarwal",
                Email = "aman.agarwal@gmail.com",
                Mobile = 9595959595,
                IsActive = true,
            };

            Candidate candidate2 = new Candidate
            {
                FirstName = "Raj",
                LastName = "Kundal",
                Email = "Raj.Kundal@gmail.com",
                Mobile = 9595958885,
                IsActive = true,
            };

            Candidate candidate3 = new Candidate
            {
                FirstName = "Pooja",
                LastName = "Sharma",
                Email = "Pooja.Sharma41@gmail.com",
                Mobile = 96759987453,
                IsActive = true,
            };

            Candidate candidate4 = new Candidate
            {
                FirstName = "Rakesh1",
                LastName = "Rohan1",
                Email = "rakeshrohan@outlook.com",
                Mobile = 9242526667,
                IsActive = true,
            };

            Candidate candidate5 = new Candidate
            {
                FirstName = "Rakesh2",
                LastName = "Rohan2",
                Email = "rakesh.rohan@outlook.com",
                Mobile = 9010456746,
                IsActive = true,
            };

            Candidate candidate6 = new Candidate
            {
                FirstName = "Firefist",
                LastName = "Ace",
                Email = "0firefist0@gmail.com",
                Mobile = 9052791243,
                IsActive = true,
            };

            _dbContext.Candidates.Add(candidate1);
            _dbContext.Candidates.Add(candidate2);
            _dbContext.Candidates.Add(candidate3);
            _dbContext.Candidates.Add(candidate4);
            _dbContext.Candidates.Add(candidate5);
            _dbContext.Candidates.Add(candidate6);

            #endregion Candidate

            #region Label

            Label label1 = new Label
            {
                Name = "C#",
            };

            Label label2 = new Label
            {
                Name = "SQL",
            };

            Label label3 = new Label
            {
                Name = "R",
            };

            Label label4 = new Label
            {
                Name = "Modelling",
            };

            _dbContext.Labels.Add(label1);
            _dbContext.Labels.Add(label2);
            _dbContext.Labels.Add(label3);
            _dbContext.Labels.Add(label4);

            #endregion Label

            #region Difficulty

            Difficulty diff1 = new Difficulty
            {
                Name = "Easy",
            };

            Difficulty diff2 = new Difficulty
            {
                Name = "Medium",
            };

            Difficulty diff3 = new Difficulty
            {
                Name = "Hard",
            };

            _dbContext.Difficulties.Add(diff1);
            _dbContext.Difficulties.Add(diff2);
            _dbContext.Difficulties.Add(diff3);

            #endregion Difficulty

            //#region Test

            //Test test1 = new Test();
            //test1.Name = "Test1";
            //test1.Candidate = candidate6;
            //test1.Job = job1;

            //Test test2 = new Test();
            //test2.Name = "Test2";
            //test2.Candidate = candidate5;
            //test2.Job = job2;

            //_dbContext.Tests.Add(test1);
            //_dbContext.Tests.Add(test2);

            //#endregion Test

            //#region QuestionGroup

            //QuestionGroup questionGroup = new QuestionGroup
            //{
            //    Description = "This question is related to software programming languages",
            //    ImageUrl = "http://www.troll.me/images/another-boromir-meme/one-does-not-simply-walk-into-mordor.jpg"
            //};

            //_dbContext.QuestionGroups.Add(questionGroup);

            //#endregion QuestionGroup

            //#region Question

            //Question question1 = new Question
            //{
            //    Text = "Which of the following is NOT an Integer?",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    ImageUrl = "http://www.relatably.com/m/img/table-flipping-meme/49389583.jpg",
            //    Difficulty = diff1,
            //    Label = label1
            //};

            //Question question2 = new Question
            //{
            //    Text = "Which of the following is an 8 - byte Integer?",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    ImageUrl = "https://s-media-cache-ak0.pinimg.com/originals/15/ee/b2/15eeb2506bca7f8cc5a686914fd9025c.jpg",
            //    Difficulty = diff2,
            //    Label = label2
            //};

            //Question question3 = new Question
            //{
            //    Text = "Which of the following operator creates a pointer to a variable in C#?",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    ImageUrl = "http://bestanimations.com/Animals/Mammals/Cats/catgif/funny-cat-gif-3.gif",
            //    Difficulty = diff3,
            //    Label = label3
            //};

            //Question question4 = new Question
            //{
            //    Text = "Which of the following keyword is used for including the namespaces in the program in C#?",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    Difficulty = diff1,
            //    Label = label1
            //};

            //Question question5 = new Question
            //{
            //    Text = "Which of the following utilities can be used to compile managed assemblies into processor-specific native code?",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    Difficulty = diff1,
            //    Label = label4
            //};

            //Question question6 = new Question
            //{
            //    Text = "Which of the following jobs are NOT performed by Garbage Collector? 1.Freeing memory on the stack. 2.Avoiding memory leaks. 3.Freeing memory occupied by unreferenced objects. 4.Closing unclosed database collections. 5.Closing unclosed files.",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    Difficulty = diff3,
            //    Label = label1
            //};

            //Question question7 = new Question
            //{
            //    Text = "Which of the following constitutes the .NET Framework? 1.ASP.NET Applications 2.CLR 3.Framework Class Library 4.WinForm Applications 5.Windows Services",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    Difficulty = diff2,
            //    Label = label2
            //};

            //Question question8 = new Question
            //{
            //    Text = "Which of the following.NET components can be used to remove unused references from the managed heap ?",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    Difficulty = diff1,
            //    Label = label1
            //};

            //Question question9 = new Question
            //{
            //    Text = "Which of the following security features can .NET applications avail?",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    Difficulty = diff1,
            //    Label = label3
            //};

            //Question question10 = new Question
            //{
            //    Text = "Which of the following can be declared in an interface?",
            //    TimeInSeconds = 30,
            //    QuestionGroup = questionGroup,
            //    Difficulty = diff3,
            //    Label = label4
            //};

            //_dbContext.Questions.Add(question1);
            //_dbContext.Questions.Add(question2);
            //_dbContext.Questions.Add(question3);
            //_dbContext.Questions.Add(question4);
            //_dbContext.Questions.Add(question5);
            //_dbContext.Questions.Add(question6);
            //_dbContext.Questions.Add(question7);
            //_dbContext.Questions.Add(question8);
            //_dbContext.Questions.Add(question9);
            //_dbContext.Questions.Add(question10);

            //#endregion Question

            //#region Challenge

            //Challenge challenge1 = new Challenge
            //{
            //    StartTime = DateTime.Now,
            //    AnsweredTime = DateTime.Now.AddSeconds(30),
            //    Test = test1,
            //    Question = question1
            //};

            //Challenge challenge2 = new Challenge
            //{
            //    StartTime = DateTime.Now.AddSeconds(30),
            //    AnsweredTime = DateTime.Now.AddSeconds(60),
            //    Test = test1,
            //    Question = question2
            //};

            //Challenge challenge3 = new Challenge
            //{
            //    StartTime = DateTime.Now.AddSeconds(90),
            //    AnsweredTime = DateTime.Now.AddSeconds(100),
            //    Test = test1,
            //    Question = question3
            //};

            //Challenge challenge4 = new Challenge
            //{
            //    StartTime = DateTime.Now.AddSeconds(100),
            //    AnsweredTime = DateTime.Now.AddSeconds(120),
            //    Test = test1,
            //    Question = question4
            //};

            //Challenge challenge5 = new Challenge
            //{
            //    StartTime = DateTime.Now.AddSeconds(120),
            //    AnsweredTime = DateTime.Now.AddSeconds(150),
            //    Test = test1,
            //    Question = question5
            //};

            //Challenge challenge6 = new Challenge
            //{
            //    StartTime = DateTime.Now.AddSeconds(150),
            //    AnsweredTime = DateTime.Now.AddSeconds(155),
            //    Test = test1,
            //    Question = question6
            //};

            //Challenge challenge7 = new Challenge
            //{
            //    StartTime = DateTime.Now.AddSeconds(155),
            //    AnsweredTime = DateTime.Now.AddSeconds(190),
            //    Test = test1,
            //    Question = question7
            //};

            //Challenge challenge8 = new Challenge
            //{
            //    StartTime = DateTime.Now.AddSeconds(190),
            //    AnsweredTime = DateTime.Now.AddSeconds(200),
            //    Test = test1,
            //    Question = question8
            //};

            //Challenge challenge9 = new Challenge
            //{
            //    StartTime = DateTime.Now.AddSeconds(200),
            //    AnsweredTime = DateTime.Now.AddSeconds(230),
            //    Test = test1,
            //    Question = question9
            //};

            //Challenge challenge10 = new Challenge
            //{
            //    StartTime = DateTime.Now.AddSeconds(230),
            //    AnsweredTime = DateTime.Now.AddSeconds(250),
            //    Test = test1,
            //    Question = question10
            //};

            //_dbContext.Challenges.Add(challenge1);
            //_dbContext.Challenges.Add(challenge2);
            //_dbContext.Challenges.Add(challenge3);
            //_dbContext.Challenges.Add(challenge4);
            //_dbContext.Challenges.Add(challenge5);
            //_dbContext.Challenges.Add(challenge6);
            //_dbContext.Challenges.Add(challenge7);
            //_dbContext.Challenges.Add(challenge8);
            //_dbContext.Challenges.Add(challenge9);
            //_dbContext.Challenges.Add(challenge10);

            //#endregion Challenge

            //#region Option

            //#region Question 1 Options

            //Option option1 = new Option
            //{
            //    Text = "Char",
            //    IsAnswer = true,
            //    Question = question1
            //};

            //Option option2 = new Option
            //{
            //    Text = "Byte",
            //    IsAnswer = false,
            //    Question = question1
            //};

            //Option option3 = new Option
            //{
            //    Text = "Integer",
            //    IsAnswer = false,
            //    Question = question1
            //};

            //Option option4 = new Option
            //{
            //    Text = "Short",
            //    IsAnswer = false,
            //    Question = question1
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions1 = new List<Option> { option1 };

            //#endregion

            //#region Question 2 Options

            //option1 = new Option
            //{
            //    Text = "Char",
            //    IsAnswer = false,
            //    Question = question2
            //};

            //option2 = new Option
            //{
            //    Text = "Long",
            //    IsAnswer = true,
            //    Question = question2
            //};

            //option3 = new Option
            //{
            //    Text = "Short",
            //    IsAnswer = false,
            //    Question = question2
            //};

            //option4 = new Option
            //{
            //    Text = "Byte",
            //    IsAnswer = false,
            //    Question = question2
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions2 = new List<Option> { option2 };

            //#endregion Question 2 Options

            //#region Question 3 Options

            //option1 = new Option
            //{
            //    Text = "sizeof",
            //    IsAnswer = false,
            //    Question = question3
            //};

            //option2 = new Option
            //{
            //    Text = "typeof",
            //    IsAnswer = false,
            //    Question = question3
            //};

            //option3 = new Option
            //{
            //    Text = "&",
            //    IsAnswer = false,
            //    Question = question3
            //};

            //option4 = new Option
            //{
            //    Text = "*",
            //    IsAnswer = true,
            //    Question = question3
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions3 = new List<Option> { option4 };

            //#endregion Question 3 Options

            //#region Question 4 Options

            //option1 = new Option
            //{
            //    Text = "imports",
            //    IsAnswer = false,
            //    Question = question4
            //};

            //option2 = new Option
            //{
            //    Text = "using",
            //    IsAnswer = true,
            //    Question = question4
            //};

            //option3 = new Option
            //{
            //    Text = "exports",
            //    IsAnswer = false,
            //    Question = question4
            //};

            //option4 = new Option
            //{
            //    Text = "None of the above",
            //    IsAnswer = false,
            //    Question = question4
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions4 = new List<Option> { option2 };

            //#endregion Question 4 Options

            //#region Question 5 Options

            //option1 = new Option
            //{
            //    Text = "gacutil",
            //    IsAnswer = false,
            //    Question = question5
            //};

            //option2 = new Option
            //{
            //    Text = "ngen",
            //    IsAnswer = true,
            //    Question = question5
            //};

            //option3 = new Option
            //{
            //    Text = "sn",
            //    IsAnswer = false,
            //    Question = question5
            //};

            //option4 = new Option
            //{
            //    Text = "dumpbin",
            //    IsAnswer = false,
            //    Question = question5
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions5 = new List<Option> { option2 };

            //#endregion Question 5 Options

            //#region Question 6 Options

            //option1 = new Option
            //{
            //    Text = "1, 2, 3",
            //    IsAnswer = false,
            //    Question = question6
            //};

            //option2 = new Option
            //{
            //    Text = "3, 5",
            //    IsAnswer = false,
            //    Question = question6
            //};

            //option3 = new Option
            //{
            //    Text = "1, 4, 5",
            //    IsAnswer = true,
            //    Question = question6
            //};

            //option4 = new Option
            //{
            //    Text = "3, 4",
            //    IsAnswer = false,
            //    Question = question6
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions6 = new List<Option> { option3 };

            //#endregion Question 6 Options

            //#region Question 7 Options

            //option1 = new Option
            //{
            //    Text = "1, 2",
            //    IsAnswer = false,
            //    Question = question7
            //};

            //option2 = new Option
            //{
            //    Text = "2, 3",
            //    IsAnswer = true,
            //    Question = question7
            //};

            //option3 = new Option
            //{
            //    Text = "3, 4",
            //    IsAnswer = false,
            //    Question = question7
            //};

            //option4 = new Option
            //{
            //    Text = "2, 5",
            //    IsAnswer = false,
            //    Question = question7
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions7 = new List<Option> { option2 };

            //#endregion Question 7 Options

            //#region Question 8 Options

            //option1 = new Option
            //{
            //    Text = "Common Language Infrastructure",
            //    IsAnswer = false,
            //    Question = question8
            //};

            //option2 = new Option
            //{
            //    Text = "CLR",
            //    IsAnswer = false,
            //    Question = question8
            //};

            //option3 = new Option
            //{
            //    Text = "Garbage Collector",
            //    IsAnswer = true,
            //    Question = question8
            //};

            //option4 = new Option
            //{
            //    Text = "Class Loader",
            //    IsAnswer = false,
            //    Question = question8
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions8 = new List<Option> { option3 };

            //#endregion Question 8 Options

            //#region Question 9 Options

            //option1 = new Option
            //{
            //    Text = "PIN Security",
            //    IsAnswer = false,
            //    Question = question9
            //};

            //option2 = new Option
            //{
            //    Text = "Code Access Security",
            //    IsAnswer = true,
            //    Question = question9
            //};

            //option3 = new Option
            //{
            //    Text = "Role Based Security",
            //    IsAnswer = true,
            //    Question = question9
            //};

            //option4 = new Option
            //{
            //    Text = "Authentication Security",
            //    IsAnswer = false,
            //    Question = question9
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions9 = new List<Option> { option2, option3 };

            //#endregion Question 9 Options

            //#region Question 10 Options

            //option1 = new Option
            //{
            //    Text = "Properties",
            //    IsAnswer = true,
            //    Question = question10
            //};

            //option2 = new Option
            //{
            //    Text = "Methods",
            //    IsAnswer = true,
            //    Question = question10
            //};

            //option3 = new Option
            //{
            //    Text = "Enumerations",
            //    IsAnswer = false,
            //    Question = question10
            //};

            //option4 = new Option
            //{
            //    Text = "Events",
            //    IsAnswer = true,
            //    Question = question10
            //};

            //_dbContext.Options.Add(option1);
            //_dbContext.Options.Add(option2);
            //_dbContext.Options.Add(option3);
            //_dbContext.Options.Add(option4);

            //List<Option> answerOptions10 = new List<Option> { option1, option2, option4 };

            //#endregion Question 10 Options

            //#endregion Option

            //#region CandidateSelectedOption

            //CandidateSelectedOption candidateSelectedOption1 = new CandidateSelectedOption
            //{
            //    Challenge = challenge1,
            //    Option = option1
            //};

            //CandidateSelectedOption candidateSelectedOption2 = new CandidateSelectedOption
            //{
            //    Challenge = challenge2,
            //    Option = option2
            //};

            //CandidateSelectedOption candidateSelectedOption3 = new CandidateSelectedOption
            //{
            //    Challenge = challenge3,
            //    Option = option3
            //};

            ////CandidateSelectedOption candidateSelectedOption4 = new CandidateSelectedOption
            ////{
            ////    Challenge = challenge4,
            ////    Options = answerOptions4
            ////};

            ////CandidateSelectedOption candidateSelectedOption5 = new CandidateSelectedOption
            ////{
            ////    Challenge = challenge5,
            ////    Options = answerOptions5
            ////};

            ////CandidateSelectedOption candidateSelectedOption6 = new CandidateSelectedOption
            ////{
            ////    Challenge = challenge6,
            ////    Options = answerOptions6
            ////};

            ////CandidateSelectedOption candidateSelectedOption7 = new CandidateSelectedOption
            ////{
            ////    Challenge = challenge7,
            ////    Options = answerOptions7
            ////};

            ////CandidateSelectedOption candidateSelectedOption8 = new CandidateSelectedOption
            ////{
            ////    Challenge = challenge8,
            ////    Options = answerOptions8
            ////};

            ////CandidateSelectedOption candidateSelectedOption9 = new CandidateSelectedOption
            ////{
            ////    Challenge = challenge9,
            ////    Options = answerOptions9
            ////};

            ////CandidateSelectedOption candidateSelectedOption10 = new CandidateSelectedOption
            ////{
            ////    Challenge = challenge10,
            ////    Options = answerOptions10
            ////};

            //_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption1);
            //_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption2);
            //_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption3);
            ////_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption4);
            ////_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption5);
            ////_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption6);
            ////_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption7);
            ////_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption8);
            ////_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption9);
            ////_dbContext.CandidateSelectedOptions.Add(candidateSelectedOption10);

            //#endregion CandidateSelectedOption

            //#region Job_Difficulty_Label

            //Job_Difficulty_Label jobDiffLabel1 = new Job_Difficulty_Label
            //{
            //    Job = job1,
            //    Difficulty = diff1,
            //    Label = label1,
            //    QuestionCount = 3
            //};

            //Job_Difficulty_Label jobDiffLabel2 = new Job_Difficulty_Label
            //{
            //    Job = job1,
            //    Difficulty = diff1,
            //    Label = label3,
            //    QuestionCount = 1
            //};

            //Job_Difficulty_Label jobDiffLabel3 = new Job_Difficulty_Label
            //{
            //    Job = job1,
            //    Difficulty = diff1,
            //    Label = label4,
            //    QuestionCount = 1
            //};


            //Job_Difficulty_Label jobDiffLabel4 = new Job_Difficulty_Label
            //{
            //    Job = job1,
            //    Difficulty = diff2,
            //    Label = label2,
            //    QuestionCount = 2
            //};

            //Job_Difficulty_Label jobDiffLabel5 = new Job_Difficulty_Label
            //{
            //    Job = job2,
            //    Difficulty = diff3,
            //    Label = label1,
            //    QuestionCount = 1
            //};

            //Job_Difficulty_Label jobDiffLabel6 = new Job_Difficulty_Label
            //{
            //    Job = job2,
            //    Difficulty = diff3,
            //    Label = label3,
            //    QuestionCount = 1
            //};

            //Job_Difficulty_Label jobDiffLabel7 = new Job_Difficulty_Label
            //{
            //    Job = job2,
            //    Difficulty = diff3,
            //    Label = label4,
            //    QuestionCount = 1
            //};

            //_dbContext.JobDifficultyLabels.Add(jobDiffLabel1);
            //_dbContext.JobDifficultyLabels.Add(jobDiffLabel2);
            //_dbContext.JobDifficultyLabels.Add(jobDiffLabel3);
            //_dbContext.JobDifficultyLabels.Add(jobDiffLabel4);
            //_dbContext.JobDifficultyLabels.Add(jobDiffLabel5);
            //_dbContext.JobDifficultyLabels.Add(jobDiffLabel6);
            //_dbContext.JobDifficultyLabels.Add(jobDiffLabel7);
            //#endregion Job_Difficulty_Label

            _dbContext.SaveChanges();
        }
    }
}
