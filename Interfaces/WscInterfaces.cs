using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

[ComImport]
[Guid("8C38232E-3A45-4A27-92B0-1A16A975F669")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IWscProduct
{
    #region <IDispatch>
    int GetTypeInfoCount();
    [return: MarshalAs(UnmanagedType.Interface)]
    IntPtr GetTypeInfo([In, MarshalAs(UnmanagedType.U4)] int iTInfo, [In, MarshalAs(UnmanagedType.U4)] int lcid);
    [PreserveSig]
    HRESULT GetIDsOfNames([In] ref Guid riid, [In, MarshalAs(UnmanagedType.LPArray)] string[] rgszNames, [In, MarshalAs(UnmanagedType.U4)] int cNames,
        [In, MarshalAs(UnmanagedType.U4)] int lcid, [Out, MarshalAs(UnmanagedType.LPArray)] int[] rgDispId);
    [PreserveSig]
    HRESULT Invoke(int dispIdMember, [In] ref Guid riid, [In, MarshalAs(UnmanagedType.U4)] int lcid, [In, MarshalAs(UnmanagedType.U4)] int dwFlags,
        [Out, In] DISPPARAMS pDispParams, [Out] out object pVarResult, [Out, In] EXCEPINFO pExcepInfo, [Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] pArgErr);
    #endregion

    HRESULT get_ProductName(out string pVal);
    HRESULT get_ProductState(out WSC_SECURITY_PRODUCT_STATE pVal);
    HRESULT get_SignatureStatus(out WSC_SECURITY_SIGNATURE_STATUS pVal);
    HRESULT get_RemediationPath(out string pVal);
    HRESULT get_ProductStateTimestamp(out string pVal);
}

[ComImport]
[Guid("722A338C-6E8E-4E72-AC27-1417FB0C81C2")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IWSCProductList
{
    #region <IDispatch>
    int GetTypeInfoCount();
    [return: MarshalAs(UnmanagedType.Interface)]
    IntPtr GetTypeInfo([In, MarshalAs(UnmanagedType.U4)] int iTInfo, [In, MarshalAs(UnmanagedType.U4)] int lcid);
    [PreserveSig]
    HRESULT GetIDsOfNames([In] ref Guid riid, [In, MarshalAs(UnmanagedType.LPArray)] string[] rgszNames, [In, MarshalAs(UnmanagedType.U4)] int cNames,
        [In, MarshalAs(UnmanagedType.U4)] int lcid, [Out, MarshalAs(UnmanagedType.LPArray)] int[] rgDispId);
    [PreserveSig]
    HRESULT Invoke(int dispIdMember, [In] ref Guid riid, [In, MarshalAs(UnmanagedType.U4)] int lcid, [In, MarshalAs(UnmanagedType.U4)] int dwFlags,
        [Out, In] DISPPARAMS pDispParams, [Out] out object pVarResult, [Out, In] EXCEPINFO pExcepInfo, [Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] pArgErr);
    #endregion

    HRESULT Initialize(uint provider);
    HRESULT get_Count(out uint pVal);
    HRESULT get_Item(uint index, out IWscProduct pVal);
}