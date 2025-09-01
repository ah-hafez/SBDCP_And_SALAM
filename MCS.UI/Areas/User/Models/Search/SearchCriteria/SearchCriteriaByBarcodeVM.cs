using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByBarcodeVM
    {
        [CustomDisplayName("User.BarcodeSearch.Barcode")]
        [CustomRequired("User.BarcodeSearch.BarcodeRequired")]
        [CustomStringLength("User.BarcodeSearch.BarcodeLength", 20, 0)]
        public string Barcode { get; set; }
    }
}