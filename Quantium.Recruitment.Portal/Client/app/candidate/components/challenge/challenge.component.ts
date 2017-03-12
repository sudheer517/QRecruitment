import { Component } from '@angular/core';
import { ChallengeService } from '../../services/challenge.service';
import { Router } from '@angular/router';

@Component({
  selector: '[appc-challenge]',
  styleUrls: ['./challenge.component.scss'],
  templateUrl: './challenge.component.html',
})
export class ChallengeComponent {
  constructor(private challengeService: ChallengeService, private router: Router) {
  }

  private getNextChallenge(): any {
        this.challengeService.GetNextChallenge()
            .subscribe(result => {
                if (result.data.Question === undefined && JSON.parse(_.toString(result.data)) === "Finished") {
                    this.router.navigate(["candidateHome"]);
                    return;
                }
                this.currentTestId = result.data.TestId;
                this.$timeout.cancel(this.myTimer);
                this.myTimer = this.$timeout(() => { this.showConfirm() }, (result.data.Question.TimeInSeconds * 1000));
                this.startDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
                this.$scope.selectedQuestionOptions = new SelectedQuestionOptions();
                this.currentChallenge = result.data;
                this.$scope.imageUrl = result.data.Question.ImageUrl;
                var questionGroup = result.data.Question.QuestionGroup;
                this.$scope.questionGroupText = questionGroup ? questionGroup.Description : "";
                this.$scope.challengeId = result.data.Question.Id;
                this.$scope.questionText = result.data.Question.Text;
                this.$scope.options = result.data.Question.Options;
                this.$scope.currentChallenge = result.data.currentChallenge;
                this.$scope.remainingChallenges = result.data.RemainingChallenges;
                this.$scope.challengesAnswered = result.data.ChallengesAnswered;
                this.$scope.totalTestTime = result.data.TotalTestTimeInMinutes;
                this.$scope.remainingTestTime = result.data.RemainingTestTimeInMinutes;
                this.$scope.isRadioQuestion = result.data.Question.IsRadio;
                this.$log.info("new question retrieved");
                this.setTimer(result.data.Question.TimeInSeconds);
            }, reason => {
                this.$log.error("new question retrieval failed");
            });
    }
 }
