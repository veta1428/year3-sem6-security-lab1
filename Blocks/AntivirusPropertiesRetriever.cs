using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SecurityReport.Blocks;
internal class AntivirusPropertiesRetriever : IDataRetriever
{
    public List<Data.Data> RetrieveData()
    {
        List<Data.Data> result = new();
        Guid CLSID_WSCProductList = new Guid("17072F7B-9ABE-4A74-A261-1EB76B55107A");
        Type WSCProductListType = Type.GetTypeFromCLSID(CLSID_WSCProductList, true) ?? throw new Exception($"{nameof(WSCProductListType)} cant be null");
        WSC_SECURITY_PROVIDER antivirus = WSC_SECURITY_PROVIDER.WSC_SECURITY_PROVIDER_ANTIVIRUS;
        object WSCProductList = Activator.CreateInstance(WSCProductListType) ?? throw new Exception($"{nameof(WSCProductList)} cant be null");
        IWSCProductList pWSCProductList = (IWSCProductList)WSCProductList;
        HRESULT hr = pWSCProductList.Initialize((uint)antivirus);
        if (hr == HRESULT.S_OK)
        {
            uint nProductCount = 0;
            hr = pWSCProductList.get_Count(out nProductCount);
            if (hr == HRESULT.S_OK)
            {
                for (uint i = 0; i < nProductCount; i++)
                {
                    IWscProduct pWscProduct;
                    hr = pWSCProductList.get_Item(i, out pWscProduct);
                    if (hr == HRESULT.S_OK)
                    {
                        string sProductName = new string('\0', 260);
                        hr = pWscProduct.get_ProductName(out sProductName);

                        if (hr == HRESULT.S_OK)
                        {
                            result.Add(new Data.Data("Product Name:", sProductName));
                        }

                        WSC_SECURITY_PRODUCT_STATE nProductState = 0;
                        hr = pWscProduct.get_ProductState(out nProductState);
                        if (hr == HRESULT.S_OK)
                        {
                            string sState;
                            if (nProductState == WSC_SECURITY_PRODUCT_STATE.WSC_SECURITY_PRODUCT_STATE_ON)
                            {
                                sState = "On";
                            }
                            else if (nProductState == WSC_SECURITY_PRODUCT_STATE.WSC_SECURITY_PRODUCT_STATE_OFF)
                            {
                                sState = "Off";
                            }
                            else if (nProductState == WSC_SECURITY_PRODUCT_STATE.WSC_SECURITY_PRODUCT_STATE_SNOOZED)
                            {
                                sState = "Snoozed";
                            }
                            else
                            {
                                sState = "Expired";
                            }

                            result.Add(new Data.Data("Product State:", sState));
                        }

                        WSC_SECURITY_SIGNATURE_STATUS nProductStatus;
                        hr = pWscProduct.get_SignatureStatus(out nProductStatus);
                        if (hr == HRESULT.S_OK)
                        {
                            string sStatus;
                            sStatus = (nProductStatus == WSC_SECURITY_SIGNATURE_STATUS.WSC_SECURITY_PRODUCT_UP_TO_DATE) ? "Up-to-date" : "Out-of-date";
                            result.Add(new Data.Data("Signatures status:", sStatus));
                        }

                        string sRemediationPath = new string('\0', 260);
                        hr = pWscProduct.get_RemediationPath(out sRemediationPath);
                        if (hr == HRESULT.S_OK)
                        {
                            result.Add(new Data.Data("Product remediation path:", sRemediationPath));
                        }

                        string sProductStateTimestamp = new string('\0', 260);
                        hr = pWscProduct.get_ProductStateTimestamp(out sProductStateTimestamp);
                        if (hr == HRESULT.S_OK)
                        {
                            result.Add(new Data.Data("Product State timestamp:", sProductStateTimestamp));
                        }
                    }
                }

                Marshal.ReleaseComObject(pWSCProductList);
            }
        }

        return result;
    }
}
