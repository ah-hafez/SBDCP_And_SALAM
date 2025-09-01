using MCS.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common;

namespace MCS.UI.Areas.User.Models
{

    public class VIPTextEditorViewModel
    {
        public EditorType EditorType { get; set; }
        public object Content { get; set; }
        public bool IsSigned { get; set; }
        public bool ReadOnly { get; set; }
        public bool IsScanning { get; set; } = true;
        public string OfficeFileId { get; set; }
        public string SignatureBehalfEmployeeName { get; set; }
        public bool IsShowWordAddIn { get; set; }
        public string OldDocumentBase64String { get; set; }
        public int OldDocumentId { get; set; }

    }
}
