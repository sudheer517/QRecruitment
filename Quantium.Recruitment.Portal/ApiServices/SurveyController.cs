using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Quantium.Recruitment.Models;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    public class SurveyController : Controller
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IJobRepository _jobRepository;
        private readonly ISurveyChallengeRepository _surveyChallengeRepository;
        private readonly ICandidateJobRepository _candidateJobRepository;
        private readonly ISurveyQuestionRepository _surveyQuestionRepository;

        public SurveyController(
            ICandidateRepository candidateRepository,
            ISurveyRepository surveyRepository, 
            ISurveyChallengeRepository surveyChallengeRepository,
            ICandidateJobRepository candidateJobRepository,
            IJobRepository jobRepository,
            ISurveyQuestionRepository surveyQuestionRepository)
        {
            _candidateRepository = candidateRepository;
            _surveyRepository = surveyRepository;
            _surveyChallengeRepository = surveyChallengeRepository;
            _candidateJobRepository = candidateJobRepository;
            jobRepository = _jobRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
        }

        [HttpPost]
        public IActionResult GenerateSurveys([FromBody]List<Candidate_SurveyDto> candidateSurveysDto)
        {
            try
            {


                var candidatesSurveys = Mapper.Map<List<Candidate_Survey>>(candidateSurveysDto);
                var job = _jobRepository.FindById(candidateSurveysDto.First().Job.Id);

                foreach (var candidateSurveyDto in candidateSurveysDto)
                {
                    var candidate = _candidateRepository.FindById(candidateSurveyDto.Candidate.Id);

                    var newCandidateJob = new Candidate_Job
                    {
                        Job = job,
                        Candidate = candidate
                    };

                    var candidateJob = candidate.CandidateJobs.FirstOrDefault(cj => cj.CandidateId == candidate.Id && cj.JobId == job.Id);

                    if (candidateJob == null)
                        _candidateJobRepository.Add(newCandidateJob);

                    Survey newSurvey = new Survey
                    {
                        Name = job.Title + candidate.FirstName,
                        Candidate = candidate,
                        Job = job,
                        CreatedUtc = DateTime.UtcNow
                    };

                    var activeSurvey = candidate.Surveys.FirstOrDefault(s => s.IsFinished != true && s.Job.Id == job.Id);

                    if (activeSurvey == null)
                    {
                        _surveyRepository.Add(newSurvey);
                    }
                    else
                    {
                        continue;
                    }

                    IList<SurveyQuestion> surveyQuestions = new List<SurveyQuestion>();
                    surveyQuestions = _surveyQuestionRepository.GetAll().ToList();
                    var availableQuestionCount = surveyQuestions.Count();

                    foreach (var surveyQuestion in surveyQuestions)
                    {
                        SurveyChallenge newChallenge = new SurveyChallenge
                        {
                            Survey = activeSurvey == null ? newSurvey : activeSurvey,
                            SurveyQuestion = surveyQuestion
                        };
                        _surveyChallengeRepository.Add(newChallenge);

                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult HasActiveSurveyForCandidate(string email)
        {
            var activeSurvey = _surveyRepository.FindActiveSurveyByCandidateEmail(email);

            if (activeSurvey == null)
                return Ok(false);
            else
                return Ok(true);
        }

        [HttpPost]
        public IActionResult ArchiveSurveys([FromBody] long[] surveyIds)
        {
            try
            {
                foreach (var surveyId in surveyIds)
                {
                    var survey = _surveyRepository.FindById(surveyId);
                    survey.IsArchived = true;
                    _surveyRepository.Update(survey);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetFinishedSurveys()
        {
            var finishedSurveys = _surveyRepository.GetAll().Where(s => s.IsFinished == true && s.IsArchived != true).OrderByDescending(t => t.FinishedDate).ToList();
            var finishedSurveyDtos = Mapper.Map<List<SurveyDto>>(finishedSurveys);
            finishedSurveyDtos.ForEach(finishedSurveyDto =>
            {
                var finishedSurvey = finishedSurveys.SingleOrDefault(t => t.Id == finishedSurveyDto.Id);               
            });

            return Ok(finishedSurveyDtos);
        }

        [HttpGet]
        public IActionResult GetAllSurveys()
        {
            var allSurveys = _surveyRepository.GetAll().Where(t => t.IsArchived != true).OrderByDescending(t => t.FinishedDate).ToList();
            var allSurveyDtos = Mapper.Map<List<TestDto>>(allSurveys);

            return Ok(allSurveyDtos);
        }
        public IActionResult GetSurveyById(long id)
        {
            var surveyDto = Mapper.Map<TestDto>(_surveyRepository.GetAll().SingleOrDefault(s => s.Id == id));
            return Ok(surveyDto);
        }

        [HttpGet]
        public IActionResult GetFinishedSurveyById(long id)
        {
            var finishedSurvey = _surveyRepository.GetAll().SingleOrDefault(s => s.Id == id && s.IsFinished == true);

            if (finishedSurvey == null)
                throw new Exception("Finished survey not found");

            var finishedTestDto = Mapper.Map<TestDto>(finishedSurvey);            
            return Ok(finishedTestDto);
        }

        [HttpPost]
        public IActionResult FinishSurvey([FromBody]long id)
        {
            try
            {
                var survey = _surveyRepository.FindById(id);
                survey.IsFinished = true;
                survey.FinishedDate = DateTime.UtcNow;
                _surveyRepository.Update(survey);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpPost]
        public HttpResponseMessage ArchiveSurvey(long id)
        {
            try
            {
                var survey = _surveyRepository.FindById(id);
                survey.IsArchived = true;
                _surveyRepository.Update(survey);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
