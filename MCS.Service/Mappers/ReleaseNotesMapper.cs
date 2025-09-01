using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class ReleaseNotesMapper
    {
        public static ReleaseNote Map(ReleaseNotesDTO releaseNotesDTO)
        {
            if (releaseNotesDTO == null)
                return null;

            ReleaseNote note = new ReleaseNote()
            {
                Id = releaseNotesDTO.Id,
                ReleaseNumber = releaseNotesDTO.ReleaseNumber,
                ReleaseDate = releaseNotesDTO.ReleaseDate,
                DateHj = releaseNotesDTO.DateHj,
                Description = releaseNotesDTO.Description,
                IsActive = releaseNotesDTO.IsActive
            };

            return note;
        }
        public static ReleaseNotesDTO Map(ReleaseNote releaseNote)
        {
            if (releaseNote == null)
                return null;

            ReleaseNotesDTO note = new ReleaseNotesDTO()
            {
                Id = releaseNote.Id,
                ReleaseNumber = releaseNote.ReleaseNumber,
                ReleaseDate = releaseNote.ReleaseDate,
                DateHj = releaseNote.DateHj,
                Description = releaseNote.Description,
                IsActive = releaseNote.IsActive
            };

            return note;
        }


        public static List<ReleaseNotesDTO> Map(IList<ReleaseNote> notesList)
        {
            if (notesList == null || !notesList.Any())
            {
                return new List<ReleaseNotesDTO>();
            }
            List<ReleaseNotesDTO> dtoList = notesList
                .Select(note => new ReleaseNotesDTO()
                {
                    Id = note.Id,
                    ReleaseNumber = note.ReleaseNumber,
                    ReleaseDate = note.ReleaseDate,
                    DateHj = note.DateHj,
                    Description = note.Description,
                    IsActive = note.IsActive
                }).ToList();


            return dtoList;
        }

    }
}