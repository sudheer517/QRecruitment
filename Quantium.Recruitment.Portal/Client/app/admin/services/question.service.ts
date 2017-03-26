import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType, URLSearchParams } from '@angular/http';
import { QuestionDto, Question_Difficulty_LabelDto, PagedQuestionDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class QuestionService{
    private questionApiUrl: string = '/Question/';

    constructor(private http: Http) { }

    public PreviewQuestions(file: any): Observable<QuestionDto[]> {
        return this.http.post(`${this.questionApiUrl}PreviewQuestions`, file).map(
            (response: Response) => {
                return response.json();
            });
    }

    public AddQuestions(file: any): Observable<Number> {
        return this.http.post(`${this.questionApiUrl}AddQuestions`, file).map(
            (response: Response) => {
                return response.status;
            });
    }

    public GetAllQuestions(): Observable<QuestionDto[]> {
        return this.http.get(`${this.questionApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetAllQuestionsByPaging(pageNumber, questionsPerPage, labelId, difficultyId): Observable<PagedQuestionDto> {
        let params = new URLSearchParams();
        params.set('paging', 'true');
        params.set('pageNumber', pageNumber);
        params.set('questionsPerPage', questionsPerPage);
        params.set('labelId', labelId);
        params.set('difficultyId', difficultyId);

        return this.http.get(`${this.questionApiUrl}GetAll`, { search: params }).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetQuestionsByLabelAndDifficulty(): Observable<Question_Difficulty_LabelDto[]> {
        return this.http.get(`${this.questionApiUrl}GetQuestionsByLabelAndDifficulty`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public DeleteQuestion(questionId: number): Observable<Number> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        console.log(questionId);
        return this.http.post(`${this.questionApiUrl}MarkQuestionInActive`, questionId, options).map(
            (response: Response) => {
                return response.status;
            });
    }
}

