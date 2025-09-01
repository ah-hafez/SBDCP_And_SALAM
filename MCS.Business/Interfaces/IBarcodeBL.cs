using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface IBarcodeBL
    {
        int AddBarcode(Barcode barcode);
        void UpdateBarcode(Barcode barcode);
        void DeleteBarcodes(IList<int> ids);
        IList<Barcode> GetBarcode(Expression<Func<Barcode, bool>> @where);
        Barcode GetBarcodeById(int id);
        int AddOrUpdateBarcodeDesign(BarcodeDesign barcodeDesign);
        BarcodeDesign GetBarcodeDesign(BarcodeDesignType barcodeDesignType, int organizationUnitId);
        BarcodeDesign GetBarcodeDesign(bool isGeneral, int typeId);
        BarcodeDesign GetAttachmentBarcodeDesign();
    }
}
