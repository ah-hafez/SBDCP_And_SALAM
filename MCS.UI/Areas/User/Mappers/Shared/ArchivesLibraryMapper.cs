using MCS.DTO;
using MCS.UI.Areas.User.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.UI.Areas.User.Mappers.Shared
{
    public class ArchivesLibraryMapper
    {
        public static ArchivesLibraryDTO Map(ArchivesLibrary archivesLibraryVM)
        {
            if (archivesLibraryVM == null)
            {
                return null;
            }
            var archivesLibraryDTO = new ArchivesLibraryDTO();

            archivesLibraryDTO.ConfidentialityLevel = archivesLibraryVM.ConfidentialityLevel;
            archivesLibraryDTO.DocumentNum= archivesLibraryVM.DocumentNum;
            archivesLibraryDTO.Date = archivesLibraryVM.Date;
            archivesLibraryDTO.DocumentType = archivesLibraryVM.DocumentType;   
            archivesLibraryDTO.Keywords = archivesLibraryVM.Keywords;
            archivesLibraryDTO.Operative = archivesLibraryVM.Operative;
            archivesLibraryDTO.PagesNum = archivesLibraryVM.PagesNum;


            return archivesLibraryDTO;

        }
    }
}
