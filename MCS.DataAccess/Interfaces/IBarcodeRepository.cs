using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IBarcodeRepository : IRepository<Barcode>
    {
        int AddBarcode(Barcode barcode);
        void UpdateBarcode(Barcode barcode);
        void DeleteBarcode(int id);
        IList<Barcode> GetBarcode(Expression<Func<Barcode, bool>> @where);
        Barcode GetBarcodeById(int id);
        int UpdateBarcodeDesign(BarcodeDesign barcodeDesign);
        int AddBarcodeDesign(BarcodeDesign barcodeDesign);
        BarcodeDesign GetBarcodeDesign(Expression<Func<BarcodeDesign, bool>> where);
    }
}