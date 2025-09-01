using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class PrintMapper
    {
        public static List<PrintVM> Map(IList<PrintDTO> printDTOs)
        {
            if (printDTOs == null || !printDTOs.Any())
            {
                return new List<PrintVM>();
            }
            List<PrintVM> printVMs = printDTOs
                .Select(printDTO => new PrintVM()
                { 
                    BarCode = printDTO.BarCode,
                    DelevaryReport = printDTO.DelevaryReport,
                    Ticket = printDTO.Ticket
                }).ToList();

            return printVMs;
        }
        public static List<PrintDTO> Map(IList<PrintVM> printVMs)
        {
            if (printVMs == null || !printVMs.Any())
            {
                return new List<PrintDTO>();
            }
            List<PrintDTO> printDTOs = printVMs
                .Select(printVM => new PrintDTO()
                {
                    BarCode = printVM.BarCode,
                    DelevaryReport = printVM.DelevaryReport,
                    Ticket = printVM.Ticket
                }).ToList();

            return printDTOs;
        }


    }
}