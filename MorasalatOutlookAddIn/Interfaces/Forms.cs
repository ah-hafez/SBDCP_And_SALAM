using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorasalatOutlookAddIn.Interfaces
{
    public class Forms
    {
        public interface ITreeForm
        {
            void FillTreeSelectedValue(int selectedId, string selectedText);

        }
    }
}
