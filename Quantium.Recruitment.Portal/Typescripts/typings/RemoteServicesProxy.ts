


module Quantium.Recruitment.ODataEntities {

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
        public College: string;
        constructor();
        constructor(Id?: number, FirstName?: string, LastName?: string, Email?: string, Mobile?: number, IsActive?: boolean, City?: string, State?: string, Country?: string, College?: string);
        constructor(Id?: number, FirstName?: string, LastName?: string, Email?: string, Mobile?: number, IsActive?: boolean, City?: string, State?: string, Country?: string, College?: string){
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Mobile = Mobile;
            this.IsActive = IsActive;
            this.City = City;
            this.State = State;
            this.Country = Country;
            this.College = College;
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

    export class ChallengeDto { 
        public Id: number;
        public TestId: number;
        public QuestionId: number;
        public StartTime: string;
        public AnsweredTime: string;
        public Question: QuestionDto;
        public CandidateSelectedOptions: CandidateSelectedOptionDto[];
        constructor();
        constructor(Id?: number, TestId?: number, QuestionId?: number, StartTime?: string, AnsweredTime?: string, Question?: QuestionDto, CandidateSelectedOptions?: CandidateSelectedOptionDto[]);
        constructor(Id?: number, TestId?: number, QuestionId?: number, StartTime?: string, AnsweredTime?: string, Question?: QuestionDto, CandidateSelectedOptions?: CandidateSelectedOptionDto[]){
            this.Id = Id;
            this.TestId = TestId;
            this.QuestionId = QuestionId;
            this.StartTime = StartTime;
            this.AnsweredTime = AnsweredTime;
            this.Question = Question;
            this.CandidateSelectedOptions = CandidateSelectedOptions;
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

    export class JobDto { 
        public Id: number;
        public Title: string;
        public Profile: string;
        public Department: DepartmentDto;
        public JobDifficultyLabels: Job_Difficulty_LabelDto[];
        public CandidateJobs: Candidate_JobDto[];
        constructor();
        constructor(Id?: number, Title?: string, Profile?: string, Department?: DepartmentDto, JobDifficultyLabels?: Job_Difficulty_LabelDto[], CandidateJobs?: Candidate_JobDto[]);
        constructor(Id?: number, Title?: string, Profile?: string, Department?: DepartmentDto, JobDifficultyLabels?: Job_Difficulty_LabelDto[], CandidateJobs?: Candidate_JobDto[]){
            this.Id = Id;
            this.Title = Title;
            this.Profile = Profile;
            this.Department = Department;
            this.JobDifficultyLabels = JobDifficultyLabels;
            this.CandidateJobs = CandidateJobs;
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

    export class OptionDto { 
        public Id: number;
        public QuestionId: number;
        public Text: string;
        public ImageUrl: string;
        public IsAnswer: boolean;
        constructor();
        constructor(Id?: number, QuestionId?: number, Text?: string, ImageUrl?: string, IsAnswer?: boolean);
        constructor(Id?: number, QuestionId?: number, Text?: string, ImageUrl?: string, IsAnswer?: boolean){
            this.Id = Id;
            this.QuestionId = QuestionId;
            this.Text = Text;
            this.ImageUrl = ImageUrl;
            this.IsAnswer = IsAnswer;
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

    export class Job_Difficulty_LabelDto { 
        public Id: number;
        public Difficulty: DifficultyDto;
        public Label: LabelDto;
        public QuestionCount: number;
        constructor();
        constructor(Id?: number, Difficulty?: DifficultyDto, Label?: LabelDto, QuestionCount?: number);
        constructor(Id?: number, Difficulty?: DifficultyDto, Label?: LabelDto, QuestionCount?: number){
            this.Id = Id;
            this.Difficulty = Difficulty;
            this.Label = Label;
            this.QuestionCount = QuestionCount;
        }

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
        constructor();
        constructor(Id?: number, Text?: string, ImageUrl?: string, TimeInSeconds?: number, RandomizeOptions?: boolean, QuestionGroup?: QuestionGroupDto, Options?: OptionDto[], Difficulty?: DifficultyDto, Label?: LabelDto);
        constructor(Id?: number, Text?: string, ImageUrl?: string, TimeInSeconds?: number, RandomizeOptions?: boolean, QuestionGroup?: QuestionGroupDto, Options?: OptionDto[], Difficulty?: DifficultyDto, Label?: LabelDto){
            this.Id = Id;
            this.Text = Text;
            this.ImageUrl = ImageUrl;
            this.TimeInSeconds = TimeInSeconds;
            this.RandomizeOptions = RandomizeOptions;
            this.QuestionGroup = QuestionGroup;
            this.Options = Options;
            this.Difficulty = Difficulty;
            this.Label = Label;
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

    export class TestDto { 
        public Id: number;
        public Name: string;
        public Challenges: ChallengeDto[];
        public TestLabels: Test_LabelDto[];
        constructor();
        constructor(Id?: number, Name?: string, Challenges?: ChallengeDto[], TestLabels?: Test_LabelDto[]);
        constructor(Id?: number, Name?: string, Challenges?: ChallengeDto[], TestLabels?: Test_LabelDto[]){
            this.Id = Id;
            this.Name = Name;
            this.Challenges = Challenges;
            this.TestLabels = TestLabels;
        }

    }

}

