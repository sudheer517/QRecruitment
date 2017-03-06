import { NgModule } from '@angular/core';

import { AdminComponent } from './admin.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AddAdminComponent } from './components/addAdmin/addAdmin.component';

import { QuestionsComponent } from './components/questions/questions.component';
import { UploadQuestionsComponent } from './components/questions/uploadQuestions/uploadQuestions.component';
import { ViewQuestionsComponent } from './components/questions/viewQuestions/viewQuestions.component';

import { JobsComponent } from './components/jobs/jobs.component';
import { CreateJobComponent } from './components/jobs/createJob/createJob.component';
import { ViewJobsComponent } from './components/jobs/viewJobs/viewJobs.component';

import { CandidatesComponent } from './components/candidates/candidates.component';
import { AddCandidatesComponent } from './components/candidates/addCandidates/addCandidates.component';
import { BulkUploadComponent } from './components/candidates/bulkUpload/bulkUpload.component';
import { ViewCandidatesComponent } from './components/candidates/viewCandidates/viewCandidates.component';

import { TestsComponent } from './components/tests/tests.component';
import { CreateTestComponent } from './components/tests/createTest/createTest.component';
import { ViewTestsComponent } from './components/tests/viewTests/viewTests.component';
import { TestResultsComponent } from './components/tests/testResults/testResults.component';

import { SurveysComponent } from './components/surveys/surveys.component';
import { CreateSurveyComponent } from './components/surveys/createSurvey/createSurvey.component';
import { ViewSurveysComponent } from './components/surveys/viewSurveys/viewSurveys.component';
import { SurveyResultsComponent } from './components/surveys/surveyResults/surveyResults.component';

import { routing } from './admin.routes';
import { SharedModule } from '../shared/shared.module';
import { ChartModule } from 'angular2-highcharts';

import { AdminService } from './services/admin.service';
import { DepartmentService } from './services/department.service';
import { QuestionService } from './services/question.service';
import { CandidateService } from './services/candidate.service';
import { JobService } from './services/job.service';
import { LabelService } from './services/label.service';
import { DifficultyService } from './services/difficulty.service';

import { Ng2FileInputModule } from 'ng2-file-input'; 
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
    imports: [routing, SharedModule, ChartModule.forRoot(require('highcharts')), Ng2FileInputModule.forRoot(), ReactiveFormsModule],
    declarations: [
        AdminComponent, 
        DashboardComponent, 
        AddAdminComponent,
        QuestionsComponent, 
        UploadQuestionsComponent,
        ViewQuestionsComponent,
        JobsComponent,
        CreateJobComponent,
        ViewJobsComponent,
        CandidatesComponent,
        AddCandidatesComponent,
        BulkUploadComponent,
        ViewCandidatesComponent,
        TestsComponent,
        CreateTestComponent,
        ViewTestsComponent,
        TestResultsComponent,
        SurveysComponent,
        CreateSurveyComponent,
        ViewSurveysComponent,
        SurveyResultsComponent
    ],
    providers: [ AdminService, DepartmentService, QuestionService, CandidateService, JobService, LabelService, DifficultyService ]
})
export class AdminModule { }
