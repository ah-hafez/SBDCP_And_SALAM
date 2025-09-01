using System.Collections.Generic;

namespace MCS.DTO
{
    public  class EditUserCategoryTrayDTO
    {
        public int UserCategoryId { get; set; }

        public List<int> TraysIds { get; set; }
    }
}
