


module Quantium.Recruitment.ODataEntities {

    export class AdminDto{ 
        public Id: number;
        public FirstName: string;
        public LastName: string;
        public Email: string;
        public Mobile: number;
        public IsActive: boolean;
        public DepartmentId: number;
        constructor();
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

    export class CandidateDto{ 
        public Id: number;
        public FirstName: string;
        public LastName: string;
        public Email: string;
        public Mobile: number;
        public IsActive: boolean;
        public JobId: number;
        public TestId: number;
        constructor();
        constructor(Id?: number, FirstName?: string, LastName?: string, Email?: string, Mobile?: number, IsActive?: boolean, JobId?: number, TestId?: number){
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Mobile = Mobile;
            this.IsActive = IsActive;
            this.JobId = JobId;
            this.TestId = TestId;
        }

    }

    export class CandidateSelectedOptionDto{ 
        public Id: number;
        public ChallengeId: number;
        public OptionId: number;
        constructor();
        constructor(Id?: number, ChallengeId?: number, OptionId?: number){
            this.Id = Id;
            this.ChallengeId = ChallengeId;
            this.OptionId = OptionId;
        }

    }

    export class ChallengeDto{ 
        public Id: number;
        public TestId: number;
        public QuestionId: number;
        public StartTime: string;
        public AnsweredTime: string;
        public Question: Quantium.Recruitment.ApiServices.Models.QuestionDto;
        public CandidateSelectedOptions: Quantium.Recruitment.ApiServices.Models.ChallengeDto[];
        constructor();
        constructor(Id?: number, TestId?: number, QuestionId?: number, StartTime?: string, AnsweredTime?: string, Question?: Quantium.Recruitment.ApiServices.Models.QuestionDto, CandidateSelectedOptions?: Quantium.Recruitment.ApiServices.Models.ChallengeDto[]){
            this.Id = Id;
            this.TestId = TestId;
            this.QuestionId = QuestionId;
            this.StartTime = StartTime;
            this.AnsweredTime = AnsweredTime;
            this.Question = Question;
            this.CandidateSelectedOptions = CandidateSelectedOptions;
        }

    }

    export class DepartmentDto{ 
        public Id: number;
        public Name: string;
        public Jobs: Quantium.Recruitment.ApiServices.Models.DepartmentDto[];
        public Admins: Quantium.Recruitment.ApiServices.Models.DepartmentDto[];
        constructor();
        constructor(Id?: number, Name?: string, Jobs?: Quantium.Recruitment.ApiServices.Models.DepartmentDto[], Admins?: Quantium.Recruitment.ApiServices.Models.DepartmentDto[]){
            this.Id = Id;
            this.Name = Name;
            this.Jobs = Jobs;
            this.Admins = Admins;
        }

    }

    export class Identifiable{ 
        public Id: number;
        constructor();
        constructor(Id?: number){
            this.Id = Id;
        }

    }

    export class JobDto{ 
        public Id: number;
        public Title: string;
        public Profile: string;
        public DepartmentId: number;
        public Candidates: Quantium.Recruitment.ApiServices.Models.JobDto[];
        public Labels: Quantium.Recruitment.ApiServices.Models.JobDto[];
        constructor();
        constructor(Id?: number, Title?: string, Profile?: string, DepartmentId?: number, Candidates?: Quantium.Recruitment.ApiServices.Models.JobDto[], Labels?: Quantium.Recruitment.ApiServices.Models.JobDto[]){
            this.Id = Id;
            this.Title = Title;
            this.Profile = Profile;
            this.DepartmentId = DepartmentId;
            this.Candidates = Candidates;
            this.Labels = Labels;
        }

    }

    export class LabelDto{ 
        public Id: number;
        public Name: string;
        public JobId: number;
        public TestId: number;
        constructor();
        constructor(Id?: number, Name?: string, JobId?: number, TestId?: number){
            this.Id = Id;
            this.Name = Name;
            this.JobId = JobId;
            this.TestId = TestId;
        }

    }

    export class OptionDto{ 
        public Id: number;
        public QuestionId: number;
        public Text: string;
        public IsAnswer: boolean;
        constructor();
        constructor(Id?: number, QuestionId?: number, Text?: string, IsAnswer?: boolean){
            this.Id = Id;
            this.QuestionId = QuestionId;
            this.Text = Text;
            this.IsAnswer = IsAnswer;
        }

    }

    export class QuestionDto{ 
        public Id: number;
        public QuestionGroupId: number;
        public Text: string;
        public TimeInSeconds: number;
        public QuestionGroup: Quantium.Recruitment.ApiServices.Models.QuestionGroupDto;
        public Options: Quantium.Recruitment.ApiServices.Models.QuestionDto[];
        constructor();
        constructor(Id?: number, QuestionGroupId?: number, Text?: string, TimeInSeconds?: number, QuestionGroup?: Quantium.Recruitment.ApiServices.Models.QuestionGroupDto, Options?: Quantium.Recruitment.ApiServices.Models.QuestionDto[]){
            this.Id = Id;
            this.QuestionGroupId = QuestionGroupId;
            this.Text = Text;
            this.TimeInSeconds = TimeInSeconds;
            this.QuestionGroup = QuestionGroup;
            this.Options = Options;
        }

    }

    export class QuestionGroupDto{ 
        public Id: number;
        public Description: string;
        constructor();
        constructor(Id?: number, Description?: string){
            this.Id = Id;
            this.Description = Description;
        }

    }

    export class TestDto{ 
        public Id: number;
        public Name: string;
        public Labels: Quantium.Recruitment.ApiServices.Models.TestDto[];
        public Challenges: Quantium.Recruitment.ApiServices.Models.TestDto[];
        constructor();
        constructor(Id?: number, Name?: string, Labels?: Quantium.Recruitment.ApiServices.Models.TestDto[], Challenges?: Quantium.Recruitment.ApiServices.Models.TestDto[]){
            this.Id = Id;
            this.Name = Name;
            this.Labels = Labels;
            this.Challenges = Challenges;
        }

    }

}

