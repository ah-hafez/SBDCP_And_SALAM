using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class BarcodeBL : BaseBL, IBarcodeBL
    {
        public int AddBarcode(Barcode barcode)
        {
            try
            {
                IBarcodeRepository barcodeRepository = IoC.Resolve<BarcodeRepository>();

                return barcodeRepository.AddBarcode(barcode);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateBarcode(Barcode barcode)
        {
            try
            {
                {
                    IBarcodeRepository barcodeRepository = IoC.Resolve<BarcodeRepository>();
                    barcodeRepository.UpdateBarcode(barcode);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void DeleteBarcodes(IList<int> ids)
        {
            try
            {
                IBarcodeRepository barcodeRepository = IoC.Resolve<BarcodeRepository>();

                foreach (int id in ids)
                {
                    barcodeRepository.DeleteBarcode(id);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Barcode GetBarcodeById(int barcodeId)
        {
            try
            {
                IBarcodeRepository barcodeRepository = IoC.Resolve<BarcodeRepository>();

                return barcodeRepository.GetBarcodeById(barcodeId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Barcode> GetBarcode(Expression<Func<Barcode, bool>> @where)
        {
            try
            {
                IBarcodeRepository barcodeRepository = IoC.Resolve<BarcodeRepository>();

                return barcodeRepository.GetBarcode(@where);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public int AddOrUpdateBarcodeDesign(BarcodeDesign barcodeDesign)
        {
            try
            {
                IBarcodeRepository barcodeRepository = IoC.Resolve<BarcodeRepository>();

                if (barcodeDesign.Id == 0)
                {
                    return barcodeRepository.AddBarcodeDesign(barcodeDesign);
                }

                return barcodeRepository.UpdateBarcodeDesign(barcodeDesign);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public BarcodeDesign GetBarcodeDesign(bool isGeneral, int typeId)
        {
            try
            {
                IBarcodeRepository barcodeRepository = IoC.Resolve<BarcodeRepository>();

                return barcodeRepository.GetBarcodeDesign(b => b.Type.Id == typeId && b.IsGeneral == isGeneral);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public BarcodeDesign GetAttachmentBarcodeDesign()
        {
            try
            {
                IBarcodeRepository barcodeRepository = IoC.Resolve<BarcodeRepository>();

                return barcodeRepository.GetBarcodeDesign(b => b.Type.Id == BarcodeDesignType.Attachment.LookupIdentity(LookupCategory.BarcodeDesignType, string.Empty));
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public BarcodeDesign GetBarcodeDesign(BarcodeDesignType barcodeDesignType, int OrgUnitId)
        {
            try
            {
                IBarcodeRepository barcodeRepository = IoC.Resolve<BarcodeRepository>();
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                BarcodeDesign barcodeDesign = null;

                OrgUnit OrgUnit = OrgUnitBL.GetOrgUnitById(OrgUnitId);

                if (OrgUnit != null && OrgUnit.BarcodeDesigns.Count > 0)
                {
                    barcodeDesign = OrgUnit.BarcodeDesigns.Where(b => b.TypeId == barcodeDesignType
                    .LookupIdentity(LookupCategory.BarcodeDesignType, string.Empty)).FirstOrDefault();

                    if (barcodeDesign != null)
                    {
                        return barcodeDesign;
                    }
                }
                int value = barcodeDesignType.LookupIdentity(LookupCategory.BarcodeDesignType, string.Empty);
                barcodeDesign =
                    barcodeRepository.GetBarcodeDesign(b => b.IsGeneral == true && b.TypeId == value);


                return barcodeDesign;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
