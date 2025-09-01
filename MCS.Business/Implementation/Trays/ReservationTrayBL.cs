using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public class ReservationTrayBL : TrayBaseBL, IReservationTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.Reservation; }
        }

        public override string TrayPermission { get { return UserClaims.Files.Reservation; } }

    }
}
