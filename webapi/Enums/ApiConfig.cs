using System.ComponentModel;

namespace WebApi.Enums
{
    /// <summary>
    /// WebRequestMethod
    /// </summary>
    public enum HttpWebRequestMethod
    {
        GET,
        POST
    }

    public enum ApiResultCode
    {
        [Description("初始化")]
        Ready = 0,

        #region (1000)
        /// <summary>
        /// 呼叫成功，未發生錯誤
        /// </summary>
        [Description("呼叫成功，未發生錯誤")]
        Success = 1000,
        /// <summary>
        /// 未明確定義之錯誤
        /// </summary>
        [Description("未明確定義之錯誤")]
        Error = 1010,
        /// <summary>
        /// 輸入參數錯誤
        /// </summary>
        [Description("輸入參數錯誤")]
        InputError = 1020
        #endregion
    }
}
