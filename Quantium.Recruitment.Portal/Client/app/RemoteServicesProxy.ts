


    export class AdminDto { 
        public Id: number;
        public FirstName: string;
        public LastName: string;
        public Email: string;
        public Mobile: number;
        public IsActive: boolean;
        public DepartmentId: number;
        public Department: DepartmentDto;
        constructor();
        constructor(Id?: number, FirstName?: string, LastName?: string, Email?: string, Mobile?: number, IsActive?: boolean, DepartmentId?: number, Department?: DepartmentDto);
        constructor(Id?: number, FirstName?: string, LastName?: string, Email?: string, Mobile?: number, IsActive?: boolean, DepartmentId?: number, Department?: DepartmentDto){
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Mobile = Mobile;
            this.IsActive = IsActive;
            this.DepartmentId = DepartmentId;
            this.Department = Department;
        }

    }

    export class CandidateDto { 
        public Id: number;
        public FirstName: string;
        public LastName: string;
        public Email: string;
        public Mobile: number;
        public IsActive: boolean;
        public City: string;
        public State: string;
        public Country: string;
        public Branch: string;
        public College: string;
        public PassingYear: string;
        public CGPA: number;
        public ExperienceInYears: number;
        public CurrentCompany: string;
        public IsInformationFilled: boolean;
        public CreatedUtc: string;
        public Password: string;
        constructor();
        constructor(Id?: number, FirstName?: string, LastName?: string, Email?: string, Mobile?: number, IsActive?: boolean, City?: string, State?: string, Country?: string, Branch?: string, College?: string, PassingYear?: string, CGPA?: number, ExperienceInYears?: number, CurrentCompany?: string, IsInformationFilled?: boolean, CreatedUtc?: string, Password?: string);
        constructor(Id?: number, FirstName?: string, LastName?: string, Email?: string, Mobile?: number, IsActive?: boolean, City?: string, State?: string, Country?: string, Branch?: string, College?: string, PassingYear?: string, CGPA?: number, ExperienceInYears?: number, CurrentCompany?: string, IsInformationFilled?: boolean, CreatedUtc?: string, Password?: string) {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Mobile = Mobile;
            this.IsActive = IsActive;
            this.City = City;
            this.State = State;
            this.Country = Country;
            this.Branch = Branch;
            this.College = College;
            this.PassingYear = PassingYear;
            this.CGPA = CGPA;
            this.ExperienceInYears = ExperienceInYears;
            this.CurrentCompany = CurrentCompany;
            this.IsInformationFilled = IsInformationFilled;
            this.CreatedUtc = CreatedUtc;
            this.Password = Password;
        }

    }

    export class CandidateSelectedOptionDto { 
        public Id: number;
        public ChallengeId: number;
        public OptionId: number;
        constructor();
        constructor(Id?: number, ChallengeId?: number, OptionId?: number);
        constructor(Id?: number, ChallengeId?: number, OptionId?: number){
            this.Id = Id;
            this.ChallengeId = ChallengeId;
            this.OptionId = OptionId;
        }

    }

    export class Candidate_JobDto { 
        public Id: number;
        public Candidate: CandidateDto;
        public Job: JobDto;
        public IsFinished: boolean;
        constructor();
        constructor(Id?: number, Candidate?: CandidateDto, Job?: JobDto, IsFinished?: boolean);
        constructor(Id?: number, Candidate?: CandidateDto, Job?: JobDto, IsFinished?: boolean){
            this.Id = Id;
            this.Candidate = Candidate;
            this.Job = Job;
            this.IsFinished = IsFinished;
        }

    }

    export class ChallengeDto { 
        public Id: number;
        public TestId: number;
        public QuestionId: number;
        public StartTime: string;
        public AnsweredTime: string;
        public RemainingChallenges: number;
        public currentChallenge: number;
        public Question: QuestionDto;
        public ChallengesAnswered: Boolean[];
        public CandidateSelectedOptions: CandidateSelectedOptionDto[];
        public TotalTestTimeInMinutes: string;
        public RemainingTestTimeInMinutes: string;
        constructor();
        constructor(Id?: number, TestId?: number, QuestionId?: number, StartTime?: string, AnsweredTime?: string, RemainingChallenges?: number, currentChallenge?: number, Question?: QuestionDto, ChallengesAnswered?: Boolean[], CandidateSelectedOptions?: CandidateSelectedOptionDto[], TotalTestTimeInMinutes?: string, RemainingTestTimeInMinutes?: string);
        constructor(Id?: number, TestId?: number, QuestionId?: number, StartTime?: string, AnsweredTime?: string, RemainingChallenges?: number, currentChallenge?: number, Question?: QuestionDto, ChallengesAnswered?: Boolean[], CandidateSelectedOptions?: CandidateSelectedOptionDto[], TotalTestTimeInMinutes?: string, RemainingTestTimeInMinutes?: string){
            this.Id = Id;
            this.TestId = TestId;
            this.QuestionId = QuestionId;
            this.StartTime = StartTime;
            this.AnsweredTime = AnsweredTime;
            this.RemainingChallenges = RemainingChallenges;
            this.currentChallenge = currentChallenge;
            this.Question = Question;
            this.ChallengesAnswered = ChallengesAnswered;
            this.CandidateSelectedOptions = CandidateSelectedOptions;
            this.TotalTestTimeInMinutes = TotalTestTimeInMinutes;
            this.RemainingTestTimeInMinutes = RemainingTestTimeInMinutes;
        }

    }

    export class DepartmentDto { 
        public Id: number;
        public Name: string;
        constructor();
        constructor(Id?: number, Name?: string);
        constructor(Id?: number, Name?: string){
            this.Id = Id;
            this.Name = Name;
        }

    }

    export class DifficultyDto { 
        public Id: number;
        public Name: string;
        public JobDifficultyLabels: Job_Difficulty_LabelDto[];
        constructor();
        constructor(Id?: number, Name?: string, JobDifficultyLabels?: Job_Difficulty_LabelDto[]);
        constructor(Id?: number, Name?: string, JobDifficultyLabels?: Job_Difficulty_LabelDto[]){
            this.Id = Id;
            this.Name = Name;
            this.JobDifficultyLabels = JobDifficultyLabels;
        }

    }

    export class FeedbackTypeDto { 
        public Id: number;
        public Name: string;
    }

    export class FeedbackDto { 
        public Id: number;
        public Description: string;
        public FeedbackTypeId: number;
        public TestId: number;
        public CandidateId: number;
    }

    export class JobDto { 
        public Id: number;
        public Title: string;
        public Profile: string;
        public Department: DepartmentDto;
        public JobDifficultyLabels: Job_Difficulty_LabelDto[];
        public CreatedByUserId: number;
        constructor();
        constructor(Id?: number, Title?: string, Profile?: string, Department?: DepartmentDto, JobDifficultyLabels?: Job_Difficulty_LabelDto[], CreatedByUserId?: number);
        constructor(Id?: number, Title?: string, Profile?: string, Department?: DepartmentDto, JobDifficultyLabels?: Job_Difficulty_LabelDto[], CreatedByUserId?: number){
            this.Id = Id;
            this.Title = Title;
            this.Profile = Profile;
            this.Department = Department;
            this.JobDifficultyLabels = JobDifficultyLabels;
            this.CreatedByUserId = CreatedByUserId;
        }

    }

    export class Job_Difficulty_LabelDto { 
        public Id: number;
        public Difficulty: DifficultyDto;
        public Label: LabelDto;
        public DisplayQuestionCount: number;
        public PassingQuestionCount: number;
        public AnsweredCount: number;
        constructor();
        constructor(Id?: number, Difficulty?: DifficultyDto, Label?: LabelDto, DisplayQuestionCount?: number, PassingQuestionCount?: number, AnsweredCount?: number);
        constructor(Id?: number, Difficulty?: DifficultyDto, Label?: LabelDto, DisplayQuestionCount?: number, PassingQuestionCount?: number, AnsweredCount?: number){
            this.Id = Id;
            this.Difficulty = Difficulty;
            this.Label = Label;
            this.DisplayQuestionCount = DisplayQuestionCount;
            this.PassingQuestionCount = PassingQuestionCount;
            this.AnsweredCount = AnsweredCount;
        }

    }

    export class LabelDto { 
        public Id: number;
        public Name: string;
        constructor();
        constructor(Id?: number, Name?: string);
        constructor(Id?: number, Name?: string){
            this.Id = Id;
            this.Name = Name;
        }

    }

    export class OptionDto { 
        public Id: number;
        public QuestionId: number;
        public Text: string;
        public ImageUrl: string;
        public IsAnswer: boolean;
        public IsCandidateSelected: boolean;
        constructor();
        constructor(Id?: number, QuestionId?: number, Text?: string, ImageUrl?: string, IsAnswer?: boolean, IsCandidateSelected?: boolean);
        constructor(Id?: number, QuestionId?: number, Text?: string, ImageUrl?: string, IsAnswer?: boolean, IsCandidateSelected?: boolean){
            this.Id = Id;
            this.QuestionId = QuestionId;
            this.Text = Text;
            this.ImageUrl = ImageUrl;
            this.IsAnswer = IsAnswer;
            this.IsCandidateSelected = IsCandidateSelected;
        }

    }

    export class PagedQuestionDto {
        questions: QuestionDto[];
        totalPages: number;
        totalQuestions : number;
    }

    export class QuestionDto { 
        public Id: number;
        public Text: string;
        public ImageUrl: string;
        public TimeInSeconds: number;
        public RandomizeOptions: boolean;
        public QuestionGroup: QuestionGroupDto;
        public Options: OptionDto[];
        public Difficulty: DifficultyDto;
        public Label: LabelDto;
        public IsRadio: boolean;
        constructor();
        constructor(Id?: number, Text?: string, ImageUrl?: string, TimeInSeconds?: number, RandomizeOptions?: boolean, QuestionGroup?: QuestionGroupDto, Options?: OptionDto[], Difficulty?: DifficultyDto, Label?: LabelDto, IsRadio?: boolean);
        constructor(Id?: number, Text?: string, ImageUrl?: string, TimeInSeconds?: number, RandomizeOptions?: boolean, QuestionGroup?: QuestionGroupDto, Options?: OptionDto[], Difficulty?: DifficultyDto, Label?: LabelDto, IsRadio?: boolean){
            this.Id = Id;
            this.Text = Text;
            this.ImageUrl = ImageUrl;
            this.TimeInSeconds = TimeInSeconds;
            this.RandomizeOptions = RandomizeOptions;
            this.QuestionGroup = QuestionGroup;
            this.Options = Options;
            this.Difficulty = Difficulty;
            this.Label = Label;
            this.IsRadio = IsRadio;
        }

    }

    export class QuestionGroupDto { 
        public Id: number;
        public Description: string;
        constructor();
        constructor(Id?: number, Description?: string);
        constructor(Id?: number, Description?: string){
            this.Id = Id;
            this.Description = Description;
        }

    }

    export class Question_Difficulty_LabelDto { 
        public DifficultyId: number;
        public LabelId: number;
        public QuestionCount: number;
        constructor();
        constructor(DifficultyId?: number, LabelId?: number, QuestionCount?: number);
        constructor(DifficultyId?: number, LabelId?: number, QuestionCount?: number){
            this.DifficultyId = DifficultyId;
            this.LabelId = LabelId;
            this.QuestionCount = QuestionCount;
        }

    }

    export class LabelDiffAnswers {
        [key: string]: string;
    }

    export class TestDto { 
        public Id: number;
        public Name: string;
        public Challenges: ChallengeDto[];
        public TestLabels: Test_LabelDto[];
        public Candidate: CandidateDto;
        public Job: JobDto;
        public IsFinished: boolean;
        public FinishedDate: string;
        public TotalRightAnswers: number;
        public TotalChallengesDisplayed: number;
        public TotalChallengesAnswered: number;
        public IsTestPassed: boolean;
        public CreatedByUserId: number;
        public CreatedUtc: string;
        public LabelDiffAnswers: LabelDiffAnswers;
        constructor();
        constructor(Id?: number, Name?: string, Challenges?: ChallengeDto[], TestLabels?: Test_LabelDto[], Candidate?: CandidateDto, Job?: JobDto, IsFinished?: boolean, FinishedDate?: string, TotalRightAnswers?: number, TotalChallengesDisplayed?: number, TotalChallengesAnswered?: number, IsTestPassed?: boolean, CreatedByUserId?: number, CreatedUtc?: string);
        constructor(Id?: number, Name?: string, Challenges?: ChallengeDto[], TestLabels?: Test_LabelDto[], Candidate?: CandidateDto, Job?: JobDto, IsFinished?: boolean, FinishedDate?: string, TotalRightAnswers?: number, TotalChallengesDisplayed?: number, TotalChallengesAnswered?: number, IsTestPassed?: boolean, CreatedByUserId?: number, CreatedUtc?: string){
            this.Id = Id;
            this.Name = Name;
            this.Challenges = Challenges;
            this.TestLabels = TestLabels;
            this.Candidate = Candidate;
            this.Job = Job;
            this.IsFinished = IsFinished;
            this.FinishedDate = FinishedDate;
            this.TotalRightAnswers = TotalRightAnswers;
            this.TotalChallengesDisplayed = TotalChallengesDisplayed;
            this.TotalChallengesAnswered = TotalChallengesAnswered;
            this.IsTestPassed = IsTestPassed;
            this.CreatedByUserId = CreatedByUserId;
            this.CreatedUtc = CreatedUtc;
        }

    }

    export class Test_LabelDto { 
        public Id: number;
        public Test: TestDto;
        public Label: LabelDto;
        constructor();
        constructor(Id?: number, Test?: TestDto, Label?: LabelDto);
        constructor(Id?: number, Test?: TestDto, Label?: LabelDto){
            this.Id = Id;
            this.Test = Test;
            this.Label = Label;
        }

    }
    
    export class SurveyQuestionDto {
        public Id: number;
        public Text: string;
    }

    export class SurveyChallengeDto{
        public Id: number;
        public ChallengeId: number;
        public SurveyQuestionId: number;
        public SurveyQuestion: SurveyQuestionDto;
        public CandidateAnswer: string;
        public TotalTestTimeInMinutes: string;
    }

    export class SurveyDto{
        public Id: number;
        public Name: string;
        public SurveyChallenges: SurveyChallengeDto[];
        public Candidate: CandidateDto;
    }

    export class TestResultDto {
        public Id: number;
        public Candidate: string;
        public Email: string;
        public JobApplied: string;
        public FinishedDate: string;
        public Result: string;
        public College: string;
        public CGPA: number;
        public TotalRightAnswers: number;
        public IsFinished: boolean;
        public RecruiterBoxUrl: string;
    }

   

