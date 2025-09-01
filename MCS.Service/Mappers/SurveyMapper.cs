using MCS.Domain;
using MCS.DTO;
using System.Collections.Generic;
using System.Linq;

namespace MCS.Service.Mappers
{
    public class SurveyMapper
    {
        public static List<SurveyQuestionDTO> Map(List<SurveyQuestion> SurveyQuestions)
        {
            if (SurveyQuestions == null || !SurveyQuestions.Any())
            {
                return new List<SurveyQuestionDTO>();
            }
            List<SurveyQuestionDTO> SurveyQuestionDTOs = SurveyQuestions
                .Select(b => new SurveyQuestionDTO
                {
                    Id = b.Id,
                    QuestionsDesc = b.QuestionsDesc,
                    IsDeleted = b.IsDeleted,
                }).ToList();
            return SurveyQuestionDTOs;
        }
        public static List<SurveyQuestion> Map(List<SurveyQuestionDTO> SurveyQuestionDTOs)
        {
            if (SurveyQuestionDTOs == null || !SurveyQuestionDTOs.Any())
            {
                return new List<SurveyQuestion>();
            }
            List<SurveyQuestion> SurveyQuestions = SurveyQuestionDTOs
                .Select(b => new SurveyQuestion
                {
                    Id = b.Id,
                    QuestionsDesc = b.QuestionsDesc,
                    IsDeleted = b.IsDeleted
                }).ToList();
            return SurveyQuestions;
        }

        public static SurveyQuestionDTO Map(SurveyQuestion b)
        {
            if (b == null)
            {
                return new SurveyQuestionDTO();
            }

            SurveyQuestionDTO surveyQuestionDTO = new SurveyQuestionDTO()
            {
                Id = b.Id,
                QuestionsDesc = b.QuestionsDesc,
                IsDeleted = b.IsDeleted,
            };

            return surveyQuestionDTO;

        }


        public static List<SurveyAnswerDTO> Map(List<SurveyAnswer> SurveyAnswers)
        {
            if (SurveyAnswers == null || !SurveyAnswers.Any())
            {
                return new List<SurveyAnswerDTO>();
            }
            List<SurveyAnswerDTO> SurveyAnswerDTOs = SurveyAnswers
                .Select(b => new SurveyAnswerDTO
                {
                    Id = b.Id,
                    AnswerId = b.AnswerId,
                    QuestionId = b.QuestionId,
                    UserId = b.UserId,
                    OrgUnitId = b.OrgUnitId, 
                    AnswerDate = b.AnswerDate,
                }).ToList();
            return SurveyAnswerDTOs;
        }
        public static List<SurveyAnswer> Map(List<SurveyAnswerDTO> SurveyAnswerDTOs)
        {
            if (SurveyAnswerDTOs == null || !SurveyAnswerDTOs.Any())
            {
                return new List<SurveyAnswer>();
            }
            List<SurveyAnswer> SurveyAnswers = SurveyAnswerDTOs
                .Select(b => new SurveyAnswer
                {
                    Id = b.Id,
                    AnswerId = b.AnswerId,
                    QuestionId = b.QuestionId,
                    UserId = b.UserId,
                    OrgUnitId = b.OrgUnitId, 
                    AnswerDate = b.AnswerDate,
                }).ToList();
            return SurveyAnswers;
        }
        public static SurveyAnswerDTO Map(SurveyAnswer b)
        {
            if (b == null)
            {
                return new SurveyAnswerDTO();
            }

            SurveyAnswerDTO SurveyAnswerDTOs = new SurveyAnswerDTO()
            {
                Id = b.Id,
                AnswerId = b.AnswerId,
                QuestionId = b.QuestionId,
                UserId = b.UserId,
                OrgUnitId = b.OrgUnitId, 
                AnswerDate = b.AnswerDate,
            };

            return SurveyAnswerDTOs;

        }


        public static List<SurveyNoteDTO> Map(List<SurveyNote> SurveyNotes)
        {
            if (SurveyNotes == null || !SurveyNotes.Any())
            {
                return new List<SurveyNoteDTO>();
            }
            List<SurveyNoteDTO> SurveyNoteDTOs = SurveyNotes
                .Select(b => new SurveyNoteDTO
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    OrgUnitId = b.OrgUnitId,
                    Note = b.Note,
                    NoteDate = b.NoteDate,
                }).ToList();
            return SurveyNoteDTOs;
        }

        public static List<SurveyNote> Map(List<SurveyNoteDTO> SurveyNoteDTOs)
        {
            if (SurveyNoteDTOs == null || !SurveyNoteDTOs.Any())
            {
                return new List<SurveyNote>();
            }
            List<SurveyNote> SurveyNotes = SurveyNoteDTOs
                .Select(b => new SurveyNote
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    OrgUnitId = b.OrgUnitId,
                    Note = b.Note,
                    NoteDate = b.NoteDate,
                }).ToList();
            return SurveyNotes;
        }

        public static SurveyNoteDTO Map(SurveyNote b)
        {
            if (b == null)
            {
                return new SurveyNoteDTO();
            }

            SurveyNoteDTO SurveyNoteDTOs = new SurveyNoteDTO()
            {
                Id = b.Id,
                UserId = b.UserId,
                OrgUnitId = b.OrgUnitId,
                Note = b.Note,
                NoteDate = b.NoteDate,
            };

            return SurveyNoteDTOs;

        }

        public static SurveyNote Map(SurveyNoteDTO SurveyNoteDTOs)
        {
            if (SurveyNoteDTOs == null)
            {
                return new SurveyNote();
            }

            SurveyNote surveyNote = new SurveyNote()
            {
                Id = SurveyNoteDTOs.Id,
                UserId = SurveyNoteDTOs.UserId,
                OrgUnitId = SurveyNoteDTOs.OrgUnitId,
                Note = SurveyNoteDTOs.Note,
                NoteDate = SurveyNoteDTOs.NoteDate,
            };

            return surveyNote;

        }
    }


}