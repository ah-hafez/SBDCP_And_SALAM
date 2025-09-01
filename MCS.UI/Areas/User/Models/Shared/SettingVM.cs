namespace MCS.UI.Areas.User.Models.Shared
{
    public class SettingVM
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int ModelId { get; set; }
        public string ResourceId { get; set; }
    }
}