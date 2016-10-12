


module Quantium.Recruitment.ODataEntities {

    export class AdminDto { 
        public Id: number;
        public FirstName: string;
        public LastName: string;
        public Email: string;
        public Mobile: number;
        public IsActive: boolean;
        public DepartmentId: number;
        constructor();
        constructor(Id?: number, FirstName?: string, LastName?: string, Email?: string, Mobile?: number, IsActive?: boolean, DepartmentId?: number);
        constructor(Id?: number, FirstName?: string, LastName?: string, Email?: string, Mobile?: number, IsActive?: boolean, DepartmentId?: number){
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Mobile = Mobile;
            this.IsActive = IsActive;
            this.DepartmentId = DepartmentId;
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
        public Jobs: JobDto[];
        public Admins: AdminDto[];
        constructor();
        constructor(Id?: number, Name?: string, Jobs?: JobDto[], Admins?: AdminDto[]);
        constructor(Id?: number, Name?: string, Jobs?: JobDto[], Admins?: AdminDto[]){
            this.Id = Id;
            this.Name = Name;
            this.Jobs = Jobs;
            this.Admins = Admins;
        }

    }

    export class JobDto { 
        public Id: number;
        public Title: string;
        public Profile: string;
        public Department: DepartmentDto;
        constructor();
        constructor(Id?: number, Title?: string, Profile?: string, Department?: DepartmentDto);
        constructor(Id?: number, Title?: string, Profile?: string, Department?: DepartmentDto){
            this.Id = Id;
            this.Title = Title;
            this.Profile = Profile;
            this.Department = Department;
        }

    }

    export class LabelDto { 
        public Id: number;
        public Name: string;
        public DifficultyLabels: Question_Label_DifficultyDto[];
        constructor();
        constructor(Id?: number, Name?: string, DifficultyLabels?: Question_Label_DifficultyDto[]);
        constructor(Id?: number, Name?: string, DifficultyLabels?: Question_Label_DifficultyDto[]){
            this.Id = Id;
            this.Name = Name;
            this.DifficultyLabels = DifficultyLabels;
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
        public IsAnswer: boolean;
        constructor();
        constructor(Id?: number, QuestionId?: number, Text?: string, IsAnswer?: boolean);
        constructor(Id?: number, QuestionId?: number, Text?: string, IsAnswer?: boolean){
            this.Id = Id;
            this.QuestionId = QuestionId;
            this.Text = Text;
            this.IsAnswer = IsAnswer;
        }

    }

    export class DifficultyDto { 
        public Id: number;
        public Name: string;
        public DifficultyLabels: Question_Label_DifficultyDto[];
        constructor();
        constructor(Id?: number, Name?: string, DifficultyLabels?: Question_Label_DifficultyDto[]);
        constructor(Id?: number, Name?: string, DifficultyLabels?: Question_Label_DifficultyDto[]){
            this.Id = Id;
            this.Name = Name;
            this.DifficultyLabels = DifficultyLabels;
        }

    }

    export class Question_Label_DifficultyDto { 
        public Id: number;
        public Question: QuestionDto;
        public Label: LabelDto;
        public Difficulty: DifficultyDto;
        constructor();
        constructor(Id?: number, Question?: QuestionDto, Label?: LabelDto, Difficulty?: DifficultyDto);
        constructor(Id?: number, Question?: QuestionDto, Label?: LabelDto, Difficulty?: DifficultyDto){
            this.Id = Id;
            this.Question = Question;
            this.Label = Label;
            this.Difficulty = Difficulty;
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
        constructor();
        constructor(Id?: number, Text?: string, ImageUrl?: string, TimeInSeconds?: number, RandomizeOptions?: boolean, QuestionGroup?: QuestionGroupDto, Options?: OptionDto[]);
        constructor(Id?: number, Text?: string, ImageUrl?: string, TimeInSeconds?: number, RandomizeOptions?: boolean, QuestionGroup?: QuestionGroupDto, Options?: OptionDto[]){
            this.Id = Id;
            this.Text = Text;
            this.ImageUrl = ImageUrl;
            this.TimeInSeconds = TimeInSeconds;
            this.RandomizeOptions = RandomizeOptions;
            this.QuestionGroup = QuestionGroup;
            this.Options = Options;
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

