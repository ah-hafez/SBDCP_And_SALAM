using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class BarcodeRepository : BaseRepository<Barcode>, IBarcodeRepository
    {
        #region Attributes

        

        #endregion Attributes

        #region Constructors

        public BarcodeRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
            
        }

        #endregion Constructors

        #region Methods
        public int AddBarcode(Barcode barcode)
        {
            try
            {
                _oMCSDbContext.Barcodes.Add(barcode);

                _oMCSDbContext.SaveChanges();

                return barcode.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }

        }

        public void UpdateBarcode(Barcode barcode)
        {
            try
            {
                _oMCSDbContext.Entry(barcode).State = System.Data.Entity.EntityState.Modified;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteBarcode(int id)
        {
            try
            {
                Barcode barcode = _oMCSDbContext.Barcodes.Where(l => l.Id == id).FirstOrDefault();

                if (barcode != null)
                {
                    _oMCSDbContext.Barcodes.Remove(barcode);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Barcode> GetBarcode(Expression<Func<Barcode, bool>> @where)
        {
            try
            {
                return _oMCSDbContext.Barcodes.Where(@where).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Barcode GetBarcodeById(int barecodeId)
        {
            try
            {
                return this.FindBy(l => l.Id == barecodeId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int AddBarcodeDesign(BarcodeDesign barcodeDesign)
        {
            try
            {
                _oMCSDbContext.BarcodeDesigns.Add(barcodeDesign);
                _oMCSDbContext.SaveChanges();

                return barcodeDesign.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int UpdateBarcodeDesign(BarcodeDesign barcodeDesign)
        {
            try
            {
                BarcodeDesign oldDesign = 
                    _oMCSDbContext.BarcodeDesigns.Where(b => b.Id == barcodeDesign.Id).FirstOrDefault();

                if (oldDesign != null)
                {
                    oldDesign.Html = barcodeDesign.Html;
                    oldDesign.Width = barcodeDesign.Width;
                    oldDesign.Height = barcodeDesign.Height;
                    oldDesign.AttachmentHtml = barcodeDesign.AttachmentHtml;

                    _oMCSDbContext.SaveChanges();

                    return oldDesign.Id;
                }

                return -1;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public BarcodeDesign GetBarcodeDesign(Expression<Func<BarcodeDesign, bool>> where)
        {
            try
            {
                return _oMCSDbContext.BarcodeDesigns.Where(where).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
