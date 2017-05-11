import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSurveyQuestionsComponent } from './add-survey-questions.component';

describe('AddSurveyQuestionsComponent', () => {
  let component: AddSurveyQuestionsComponent;
  let fixture: ComponentFixture<AddSurveyQuestionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddSurveyQuestionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddSurveyQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
