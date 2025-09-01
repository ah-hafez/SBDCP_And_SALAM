using MCS.Common;


namespace MCS.Business
{
    public class YesserTrayBL : TrayBaseBL, IYesserTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.YESSER; }
        }

        public override string TrayPermission { get { return UserClaims.Files.YESSER; } }
    }
}
