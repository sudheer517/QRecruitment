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
import { TestResultsComponent } from './components/tests/testResults/testResults.component';
import { ArchivedTestsComponent } from './components/tests/archivedTests/archivedTests.component';


import { TestDetailComponent } from './components/tests/testDetail/testDetail.component';

import { FilterCandidatesPipe } from './pipes/filterCandidates.pipe';

import { routing } from './admin.routes';
import { SharedModule } from '../shared/shared.module';

import { AdminService } from './services/admin.service';
import { DepartmentService } from './services/department.service';
import { QuestionService } from './services/question.service';
import { CandidateService } from './services/candidate.service';
import { JobService } from './services/job.service';
import { LabelService } from './services/label.service';
import { DifficultyService } from './services/difficulty.service';
import { TestService } from './services/test.service';
import { DatePipe } from '@angular/common';
import { TableModule } from './directives/data-table/ng-table-module';

@NgModule({
    imports: [
        routing, 
        SharedModule,
        TableModule
],
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
        TestResultsComponent,
        TestDetailComponent,
        ArchivedTestsComponent,
        FilterCandidatesPipe
    ],
    providers: [ AdminService, DepartmentService, QuestionService, CandidateService, JobService, LabelService, DifficultyService, TestService, DatePipe ]
})
export class AdminModule { }
