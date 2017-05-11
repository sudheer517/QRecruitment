import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewSurveyQuestionsComponent } from './view-survey-questions.component';

describe('ViewSurveyQuestionsComponent', () => {
  let component: ViewSurveyQuestionsComponent;
  let fixture: ComponentFixture<ViewSurveyQuestionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewSurveyQuestionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewSurveyQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
