using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FixedParamsPub
    {
        public const bool IsHex = true; //默认以16进制显示

        public const int DEFAULT_LAC_LIMIT1 = 1;
        public const int DEFAULT_LAC_LIMIT2 = 65535;
        public const int DEFAULT_LAC_ALL = 65536;
        public const int DEFAULT_LAC_UNKNOWN = 65537;
        public const int DEFAULT_LAC_INVALID = 0;

        public const int DEFAULT_CI_LIMIT1 = 0;
        public const int DEFAULT_CI_LIMIT2 = 65535;
        public const int DEFAULT_CI_ALL = 65536;
        public const int DEFAULT_CI_UNKNOWN = 65537;
        public const int DEFAULT_CI_INVALID = -1;

        public const int DEFAULT_LINENUM_LIMIT1 = 1;
        public const int DEFAULT_LINENUM_LIMIT2 = 65534;
        public const int DEFAULT_LINENUM_ALL = 65536;
        public const int DEFAULT_LINENUM_UNKNOWN = 65537;
        public const int DEFAULT_LINENUM_INVALID = 0;

        public const int DEFAULT_KILO_LIMIT1 = -0x3FFFFF;
        public const int DEFAULT_KILO_LIMIT2 = 0x3FFFFF;
        public const int DEFAULT_KILO_SPECIAL = 9999999;
        public const int DEFAULT_KILO_INVALID = -0x400000;

        public const int DEFAULT_ENGTYPE_LIMIT1 = 1;
        public const int DEFAULT_ENGTYPE_LIMIT2 = 511;
        public const int DEFAULT_ENGTYPE_INVALID = 0;
        public const int DEFAULT_ENGNUM_LIMIT1 = 1;
        public const int DEFAULT_ENGNUM_LIMIT2 = 99999;
        public const int DEFAULT_ENGNUM_INVALID = 0;
        public const int DEFAULT_ENGNUMCOUNT_LIMIT = 1000;

        public const int DEFAULT_CMDPLACENUM_LIMIT1 = 1;
        public const int DEFAULT_CMDPLACENUM_LIMIT2 = 65534;
        public const int DEFAULT_CMDPLACENUM_ALL = 65536;
        public const int DEFAULT_CMDPLACENUM_UNKNOWN = 65537;
        public const int DEFAULT_CMDPLACENUM_INVALID = 0;

        public const int DEFAULT_BUREAUINFO_LIMIT1 = 1;
        public const int DEFAULT_BUREAUINFO_LIMIT2 = 9999;
        public const int DEFAULT_BUREAUINFO_INVALID = 0;



        public const int DEFAULT_ROLEID_LIMIT1 = 1;
        public const int DEFAULT_ROLEID_LIMIT2 = 999;

        public static DateTime DefaultDate = new DateTime(1970, 1, 1);
        public static DateTime DefaultNullDtDb = new DateTime(1970, 1, 1, 7, 0, 0);
        public static DateTime DefaultNullDtRt = new DateTime(1970, 1, 1, 8, 0, 0);
        public const string TIME_FORMAT_YM = "yyyy-MM";
        public const string TIME_FORMAT_YMDHMS = "yyyy-MM-dd HH:mm:ss";
        public const string TIME_FORMAT_YMDHMSF3 = "yyyy-MM-dd HH:mm:ss.fff";
        public const string TIME_FORMAT_YMD = "yyyyMMdd";
        public const string TIME_COMPFORMAT_YMDHMSF6 = "yyyyMMddHHmmssffffff";



        public const double DEFAULT_LONGLAT_LIMIT1 = 0.000001;
        public const double DEFAULT_LONGLAT_LIMIT2 = 180.0;



        public const int DB_DATAMAXCOUNT_User = 500;
        public const int DB_DATAMAXCOUNT_ImsiMap = 50000; //IMSI映射关系


        public const int DB_DATAMAXCOUNT_AccuLocAddr = 100000;
        public const int DB_DATAMAXCOUNT_CallType = 100;
        public const int DB_DATAMAXCOUNT_ROLE = 100000;
        public const int DB_DATAMAXCOUNT_DispatchArea = 10000;
        public const int DB_DATAMAXCOUNT_DaCi = 1000;
        public const int DB_DATAMAXCOUNT_IsdnBlack = 20000;
        public const int DB_DATAMAXCOUNT_LocAddr = 10000;
        public const int DB_DATAMAXCOUNT_LaCi = 1000;
        public const int DB_DATAMAXCOUNT_BureauInfo = 100000;
        public const int DB_DATAMAXCOUNT_CellInfo = 200000;
        public const int DB_DATAMAXCOUNT_LineInfo = 100000;
        public const int DB_DATAMAXCOUNT_HlrGt = 500;
        public const int DB_DATAMAXCOUNT_FnRegState = 999999999;
        public const int DB_DATAMAXCOUNT_GrisInfo = 100000;
        public const int DB_DATAMAXCOUNT_MGrisInfo = 100000;
        public const int DB_DATAMAXCOUNT_SttLnCi = 1000;
        public const int DB_DATAMAXCOUNT_SttLineName = 200;
        public const int DB_DTATMAXCOUNT_IpInfo = 100000;

        public const int DB_DATAMAXCOUNT_LineKiloToGris = 100000;
        public const int DB_DATAMAXCOUNT_LineKiloToMGris = 100000;
        public const int DB_DATAMAXCOUNT_LongLatToGris = 100000;
        public const int DB_DATAMAXCOUNT_LongLatToMGris = 100000;

    }
}
