import { Routes, RouterModule }  from '@angular/router';

import { AdminComponent }  from './admin.component';
import { DashboardComponent }  from './components/dashboard/dashboard.component';
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

const routes: Routes = [
  {
    path: '', component: AdminComponent, children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'addAdmin', component: AddAdminComponent },
      { 
        path: 'questions', 
        component: QuestionsComponent, 
        children: [ 
          //{ path: '', component:  UploadQuestionsComponent },
          //{ path: 'uploadQuestions', component:  UploadQuestionsComponent },
          { path: 'viewQuestions', component:  ViewQuestionsComponent }
        ] 
      },
      { 
        path: 'jobs', 
        component: JobsComponent, 
        children: [ 
          { path: '', component:  CreateJobComponent },
          { path: 'createJob', component:  CreateJobComponent },
          { path: 'viewJobs', component:  ViewJobsComponent }
        ] 
      },
      { 
        path: 'candidates', 
        component: CandidatesComponent, 
        children: [ 
          { path: '', component:  AddCandidatesComponent },
          { path: 'addCandidates', component:  AddCandidatesComponent },
          //{ path: 'bulkUpload', component:  BulkUploadComponent },
          { path: 'viewCandidates', component:  ViewCandidatesComponent }
        ] 
      },
      { 
        path: 'tests', 
        component: TestsComponent, 
        children: [ 
          { path: '', component:  CreateTestComponent },
          { path: 'createTest', component:  CreateTestComponent },
          { path: 'viewTests', component:  ViewTestsComponent },
          { path: 'testResults', component:  TestResultsComponent }
        ] 
      },
      { 
        path: 'surveys', 
        component: SurveysComponent, 
        children: [ 
          { path: '', component:  CreateSurveyComponent },
          { path: 'createSurvey', component:  CreateSurveyComponent },
          { path: 'viewSurveys', component:  ViewSurveysComponent },
          { path: 'surveyResults', component:  SurveyResultsComponent }
        ] 
      }
    ]
  }
];

export const routing = RouterModule.forChild(routes);
