using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework.Localization;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Survey;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public class SurveyMapper
    {
        
            
        public static List<SurveyQuestionVM> Map(List<SurveyQuestionDTO> SurveyQuestionDTOs)
        {
            if (SurveyQuestionDTOs == null || !SurveyQuestionDTOs.Any())
            {
                return new List<SurveyQuestionVM>();
            }
            List<SurveyQuestionVM> SurveyQuestionVMs = SurveyQuestionDTOs
                .Select(b => new SurveyQuestionVM
                {
                    Id = b.Id,
                    QuestionsDesc = b.QuestionsDesc,
                    IsDeleted = b.IsDeleted,
                }).ToList();
            return SurveyQuestionVMs;
        }
        public static List<SurveyQuestionDTO> Map(List<SurveyQuestionVM> SurveyQuestionVMs)
        {
            if (SurveyQuestionVMs == null || !SurveyQuestionVMs.Any())
            {
                return new List<SurveyQuestionDTO>();
            }
            List<SurveyQuestionDTO> SurveyQuestionDTOs = SurveyQuestionVMs
                .Select(b => new SurveyQuestionDTO
                {
                    Id = b.Id,
                    QuestionsDesc = b.QuestionsDesc,
                    IsDeleted = b.IsDeleted
                }).ToList();
            return SurveyQuestionDTOs;
        }

        public static SurveyQuestionVM Map(SurveyQuestionDTO surveyQuestionDTO)
        {
            if (surveyQuestionDTO == null)
            {
                return new SurveyQuestionVM();
            }

            SurveyQuestionVM SurveyQuestionVM = new SurveyQuestionVM()
            {
                Id = surveyQuestionDTO.Id,
                QuestionsDesc = surveyQuestionDTO.QuestionsDesc,
                IsDeleted = surveyQuestionDTO.IsDeleted,
            };

            return SurveyQuestionVM;

        }

        
        public static List<SurveyAnswerVM> Map(List<SurveyAnswerDTO> SurveyAnswerDTOs)
        {
            if (SurveyAnswerDTOs == null || !SurveyAnswerDTOs.Any())
            {
                return new List<SurveyAnswerVM>();
            }
            List<SurveyAnswerVM> SurveyAnswerVMs = SurveyAnswerDTOs
                .Select(b => new SurveyAnswerVM
                {
                    Id = b.Id,
                    AnswerId = b.AnswerId,
                    QuestionId = b.QuestionId,
                    UserId = b.UserId,
                    OrgUnitId = b.OrgUnitId, 
                    AnswerDate = b.AnswerDate,
                }).ToList();
            return SurveyAnswerVMs;
        }
        public static List<SurveyAnswerDTO> Map(List<SurveyAnswerVM> SurveyAnswerVMs)
        {
            if (SurveyAnswerVMs == null || !SurveyAnswerVMs.Any())
            {
                return new List<SurveyAnswerDTO>();
            }
            List<SurveyAnswerDTO> SurveyAnswerDTOs = SurveyAnswerVMs
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
        public static SurveyAnswerVM Map(SurveyAnswerDTO b)
        {
            if (b == null)
            {
                return new SurveyAnswerVM();
            }

            SurveyAnswerVM SurveyAnswerVMs = new SurveyAnswerVM()
            {
                Id = b.Id,
                AnswerId = b.AnswerId,
                QuestionId = b.QuestionId,
                UserId = b.UserId,
                OrgUnitId = b.OrgUnitId, 
                AnswerDate = b.AnswerDate,
            };

            return SurveyAnswerVMs;
                
        }


        public static List<SurveyNoteVM> Map(List<SurveyNoteDTO> SurveyNoteDTOs)
        {
            if (SurveyNoteDTOs == null || !SurveyNoteDTOs.Any())
            {
                return new List<SurveyNoteVM>();
            }
            List<SurveyNoteVM> SurveyNoteVMs = SurveyNoteDTOs
                .Select(b => new SurveyNoteVM
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    OrgUnitId = b.OrgUnitId,
                    Note = b.Note,
                    NoteDate = b.NoteDate,
                }).ToList();
            return SurveyNoteVMs;
        }

        public static List<SurveyNoteDTO> Map(List<SurveyNoteVM> SurveyNoteVMs)
        {
            if (SurveyNoteVMs == null || !SurveyNoteVMs.Any())
            {
                return new List<SurveyNoteDTO>();
            }
            List<SurveyNoteDTO> SurveyNoteDTOs = SurveyNoteVMs
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

        public static SurveyNoteVM Map(SurveyNoteDTO b)
        {
            if (b == null)
            {
                return new SurveyNoteVM();
            }

            SurveyNoteVM SurveyNoteVMs = new SurveyNoteVM()
            {
                Id = b.Id,
                UserId = b.UserId,
                OrgUnitId = b.OrgUnitId,
                Note = b.Note,
                NoteDate = b.NoteDate,
            };

            return SurveyNoteVMs;

        }

        public static SurveyNoteDTO Map(SurveyNoteVM surveyNoteVM)
        {
            if (surveyNoteVM == null)
            {
                return new SurveyNoteDTO();
            }

            SurveyNoteDTO surveyNoteDTO = new SurveyNoteDTO()
            {
                Id = surveyNoteVM.Id,
                UserId = surveyNoteVM.UserId,
                OrgUnitId = surveyNoteVM.OrgUnitId,
                Note = surveyNoteVM.Note,
                NoteDate = surveyNoteVM.NoteDate,
            };

            return surveyNoteDTO;

        }
    }
}