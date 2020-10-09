using System;
using System.Security.Policy;
using System.Text;

namespace uFR
{
    using System.Runtime.InteropServices;
    using UFR_HANDLE = System.UIntPtr;

    enum CARD_SAK
    {
        UNKNOWN = 0x00,
        MIFARE_CLASSIC_1k = 0x08,
        MF1ICS50 = 0x08,
        SLE66R35 = 0x88,
        MIFARE_CLASSIC_4k = 0x18,
        MF1ICS70 = 0x18,
        MIFARE_CLASSIC_MINI = 0x09,
        MF1ICS20 = 0x09,
    }

    enum DLOGIC_CARD_TYPE
    {
         DL_NO_CARD                     = 0x00,
         DL_MIFARE_ULTRALIGHT			= 0x01,
         DL_MIFARE_ULTRALIGHT_EV1_11	= 0x02,
         DL_MIFARE_ULTRALIGHT_EV1_21	= 0x03,
         DL_MIFARE_ULTRALIGHT_C			= 0x04,
         DL_NTAG_203					= 0x05,
         DL_NTAG_210					= 0x06,
         DL_NTAG_212					= 0x07,
         DL_NTAG_213					= 0x08,
         DL_NTAG_215					= 0x09,
         DL_NTAG_216					= 0x0A,
         DL_MIKRON_MIK640D				= 0x0B,
         NFC_T2T_GENERIC				= 0x0C,
         DL_MIFARE_MINI					= 0x20,
         DL_MIFARE_CLASSIC_1K		    = 0x21,
         DL_MIFARE_CLASSIC_4K			= 0x22,
         DL_MIFARE_PLUS_S_2K_SL0		= 0x23,
         DL_MIFARE_PLUS_S_4K_SL0		= 0x24,
         DL_MIFARE_PLUS_X_2K_SL0		= 0x25,
         DL_MIFARE_PLUS_X_4K_SL0		= 0x26,
         DL_MIFARE_DESFIRE				= 0x27,
         DL_MIFARE_DESFIRE_EV1_2K		= 0x28,
         DL_MIFARE_DESFIRE_EV1_4K		= 0x29,
         DL_MIFARE_DESFIRE_EV1_8K		= 0x2A,
         DL_MIFARE_DESFIRE_EV2_2K		= 0x2B,
         DL_MIFARE_DESFIRE_EV2_4K		= 0x2C,
         DL_MIFARE_DESFIRE_EV2_8K		= 0x2D,
         DL_MIFARE_PLUS_S_2K_SL1		= 0x2E,
         DL_MIFARE_PLUS_X_2K_SL1		= 0x2F,
         DL_MIFARE_PLUS_EV1_2K_SL1      = 0x30,
         DL_MIFARE_PLUS_X_2K_SL2		= 0x31,
         DL_MIFARE_PLUS_S_2K_SL3		= 0x32,
         DL_MIFARE_PLUS_X_2K_SL3		= 0x33,
         DL_MIFARE_PLUS_EV1_2K_SL3      = 0x34,
         DL_MIFARE_PLUS_S_4K_SL1		= 0x35,
         DL_MIFARE_PLUS_X_4K_SL1		= 0x36,
         DL_MIFARE_PLUS_EV1_4K_SL1      = 0x37,
         DL_MIFARE_PLUS_X_4K_SL2		= 0x38,
         DL_MIFARE_PLUS_S_4K_SL3		= 0x39,
         DL_MIFARE_PLUS_X_4K_SL3		= 0x3A,
         DL_MIFARE_PLUS_EV1_4K_SL3      = 0x3B,
         DL_UNKNOWN_ISO_14443_4			= 0x40,
         DL_GENERIC_ISO14443_4			= 0x40,
         DL_GENERIC_ISO14443_4_TYPE_B	= 0x41,
         DL_GENERIC_ISO14443_3_TYPE_B	= 0x42,
         DL_IMEI_UID					= 0x80

}
    // MIFARE CLASSIC Authentication Modes:
    enum MIFARE_AUTHENTICATION
    {
        MIFARE_AUTHENT1A = 0x60,
        MIFARE_AUTHENT1B = 0x61
    }

    //MIFARE PLUS AES Authentication Modes:
    enum MIFARE_PLUS_AES_AUTHENTICATION
    {
        MIFARE_PLUS_AES_AUTHENT1A = 0x80,
        MIFARE_PLUS_AES_AUTHENT1B = 0x81
    }

    // DLJavaCardSignerCardTypes:
    enum JCDL_SIGNER_CARDS
    {
        DLSigner81 = 0xA0,
        DLSigner22 = 0xA1,
        DLSigner30 = 0xA2,
        DLSigner10 = 0xA3,
        DLSigner145 = 0xAA
    }
    // DLJavaCardSignerAlgorithmTypes:
    enum JCDL_SIGNER_CIPHERS
    {
        SIG_CIPHER_RSA = 0,
        SIG_CIPHER_ECDSA
    };
    enum JCDL_SIGNER_PADDINGS
    {
        PAD_NULL = 0,
        PAD_PKCS1
    };
    enum JCDL_SIGNER_DIGESTS
    {
        ALG_NULL = 0,
        ALG_SHA,
        ALG_SHA_256,
        ALG_SHA_384,
        ALG_SHA_512,
        ALG_SHA_224
    };
    enum JCDL_KEY_TYPES
    {
        TYPE_RSA_PRIVATE = 0,
        TYPE_RSA_CRT_PRIVATE,
        TYPE_EC_F2M_PRIVATE,
        TYPE_EC_FP_PRIVATE
    };
    enum JCDS_EC_KEY_DESIGNATOR
    {
        EC_KEY_DSG_K1 = 0,
        EC_KEY_DSG_R1,
        EC_KEY_DSG_R2,
        EC_KEY_DSG_RFU
    }
    // API Status Codes Type:
    public enum DL_STATUS
    {
        UFR_OK = 0x00,

        UFR_COMMUNICATION_ERROR = 0x01,
        UFR_CHKSUM_ERROR = 0x02,
        UFR_READING_ERROR = 0x03,
        UFR_WRITING_ERROR = 0x04,
        UFR_BUFFER_OVERFLOW = 0x05,
        UFR_MAX_ADDRESS_EXCEEDED = 0x06,
        UFR_MAX_KEY_INDEX_EXCEEDED = 0x07,
        UFR_NO_CARD = 0x08,
        UFR_COMMAND_NOT_SUPPORTED = 0x09,
        UFR_FORBIDEN_DIRECT_WRITE_IN_SECTOR_TRAILER = 0x0A,
        UFR_ADDRESSED_BLOCK_IS_NOT_SECTOR_TRAILER = 0x0B,
        UFR_WRONG_ADDRESS_MODE = 0x0C,
        UFR_WRONG_ACCESS_BITS_VALUES = 0x0D,
        UFR_AUTH_ERROR = 0x0E,
        UFR_PARAMETERS_ERROR = 0x0F,
        UFR_MAX_SIZE_EXCEEDED = 0x10,
        UFR_UNSUPPORTED_CARD_TYPE = 0x11,

        UFR_WRITE_VERIFICATION_ERROR = 0x70,
        UFR_BUFFER_SIZE_EXCEEDED = 0x71,
        UFR_VALUE_BLOCK_INVALID = 0x72,
        UFR_VALUE_BLOCK_ADDR_INVALID = 0x73,
        UFR_VALUE_BLOCK_MANIPULATION_ERROR = 0x74,
        UFR_WRONG_UI_MODE = 0x75,
        UFR_KEYS_LOCKED = 0x76,
        UFR_KEYS_UNLOCKED = 0x77,
        UFR_WRONG_PASSWORD = 0x78,
        UFR_CAN_NOT_LOCK_DEVICE = 0x79,
        UFR_CAN_NOT_UNLOCK_DEVICE = 0x7A,
        UFR_DEVICE_EEPROM_BUSY = 0x7B,
        UFR_RTC_SET_ERROR = 0x7C,

        ANTI_COLLISION_DISABLED = 0x7D,

        UFR_COMMUNICATION_BREAK = 0x50,
        UFR_NO_MEMORY_ERROR = 0x51,
        UFR_CAN_NOT_OPEN_READER = 0x52,
        UFR_READER_NOT_SUPPORTED = 0x53,
        UFR_READER_OPENING_ERROR = 0x54,
        UFR_READER_PORT_NOT_OPENED = 0x55,
        UFR_CANT_CLOSE_READER_PORT = 0x56,

        UFR_TIMEOUT_ERR = 0x90,

        UFR_FT_STATUS_ERROR_1 = 0xA0,
        UFR_FT_STATUS_ERROR_2 = 0xA1,
        UFR_FT_STATUS_ERROR_3 = 0xA2,
        UFR_FT_STATUS_ERROR_4 = 0xA3,
        UFR_FT_STATUS_ERROR_5 = 0xA4,
        UFR_FT_STATUS_ERROR_6 = 0xA5,
        UFR_FT_STATUS_ERROR_7 = 0xA6,
        UFR_FT_STATUS_ERROR_8 = 0xA7,
        UFR_FT_STATUS_ERROR_9 = 0xA8,

        //NDEF error codes
        UFR_WRONG_NDEF_CARD_FORMAT = 0x80,
        UFR_NDEF_MESSAGE_NOT_FOUND = 0x81,
        UFR_NDEF_UNSUPPORTED_CARD_TYPE = 0x82,
        UFR_NDEF_CARD_FORMAT_ERROR = 0x83,
        UFR_MAD_NOT_ENABLED = 0x84,
        UFR_MAD_VERSION_NOT_SUPPORTED = 0x85,       
    

    	// multiple units - return from the functions with ReaderList_ prefix in name
    	UFR_DEVICE_WRONG_HANDLE = 0x100,
    	UFR_DEVICE_INDEX_OUT_OF_BOUND,
    	UFR_DEVICE_ALREADY_OPENED,
    	UFR_DEVICE_ALREADY_CLOSED,
    	UFR_DEVICE_IS_NOT_CONNECTED,
    
    	// Originality Check Error Codes:
    	UFR_NOT_NXP_GENUINE = 0x200,
    	UFR_OPEN_SSL_DYNAMIC_LIB_FAILED,
    	UFR_OPEN_SSL_DYNAMIC_LIB_NOT_FOUND,

        UFR_NOT_IMPLEMENTED = 0x1000,
        UFR_COMMAND_FAILED,

        //MIFARE PLUS error codes
        UFR_MFP_COMMAND_OVERFLOW = 0xB0,
        UFR_MFP_INVALID_MAC = 0xB1,
        UFR_MFP_INVALID_BLOCK_NR = 0xB2,
        UFR_MFP_NOT_EXIST_BLOCK_NR = 0xB3,
        UFR_MFP_COND_OF_USE_ERROR = 0xB4,
        UFR_MFP_LENGTH_ERROR = 0xB5,
        UFR_MFP_GENERAL_MANIP_ERROR = 0xB6,
        UFR_MFP_SWITCH_TO_ISO14443_4_ERROR = 0xB7,
        UFR_MFP_ILLEGAL_STATUS_CODE = 0xB8,
        UFR_MFP_MULTI_BLOCKS_READ = 0xB9,

        // APDU Error Codes:
        UFR_APDU_TRANSCEIVE_ERROR = 0xAE,
        UFR_APDU_JC_APP_NOT_SELECTED = 0x6000,
        UFR_APDU_JC_APP_BUFF_EMPTY,
        UFR_APDU_WRONG_SELECT_RESPONSE,
        UFR_APDU_WRONG_KEY_TYPE,
        UFR_APDU_WRONG_KEY_SIZE,
        UFR_APDU_WRONG_KEY_PARAMS,
        UFR_APDU_WRONG_SIGNING_ALGORITHM,
        UFR_APDU_PLAIN_TEXT_SIZE_EXCEEDED,
        UFR_APDU_UNSUPPORTED_KEY_SIZE,
        UFR_APDU_UNSUPPORTED_ALGORITHMS,
        UFR_APDU_PKI_OBJECT_NOT_FOUND,
        UFR_APDU_SW_TAG = 0x0A0000,

        MAX_UFR_STATUS = 0x7FFFFFFF
    };

    public enum DL_SECURE_CODE
    {
        USER_PIN = 0,
        SO_PIN,
        USER_PUK,
        SO_PUK
    };

    public enum DESFIRE_CARD_STATUS_CODES
    {
        READER_ERROR = 2999,
        CARD_OPERATION_OK = 3001,
        WRONG_KEY_TYPE = 3002,
        KEY_AUTH_ERROR = 3003,
        CARD_CRYPTO_ERROR = 3004,
        READER_CARD_COMM_ERROR = 3005,
        PC_READER_COMM_ERROR = 3006,
        COMMIT_TRANSACTION_NO_REPLY = 3007,
        COMMIT_TRANSACTION_ERROR = 3008,
        DESFIRE_CARD_NO_CHANGES = 0x0C0C,
        DESFIRE_CARD_OUT_OF_EEPROM_ERROR = 0x0C0E,
        DESFIRE_CARD_ILLEGAL_COMMAND_CODE = 0x0C1C,
        DESFIRE_CARD_INTEGRITY_ERROR = 0x0C1E,
        DESFIRE_CARD_NO_SUCH_KEY = 0x0C40,
        DESFIRE_CARD_LENGTH_ERROR = 0x0C7E,
        DESFIRE_CARD_PERMISSION_DENIED = 0x0C9D,
        DESFIRE_CARD_PARAMETER_ERROR = 0x0C9E,
        DESFIRE_CARD_APPLICATION_NOT_FOUND = 0x0CA0,
        DESFIRE_CARD_APPL_INTEGRITY_ERROR = 0x0CA1,
        DESFIRE_CARD_AUTHENTICATION_ERROR = 0x0CAE,
        DESFIRE_CARD_ADDITIONAL_FRAME = 0x0CAF,
        DESFIRE_CARD_BOUNDARY_ERROR = 0x0CBE,
        DESFIRE_CARD_PICC_INTEGRITY_ERROR = 0x0CC1,
        DESFIRE_CARD_COMMAND_ABORTED = 0x0CCA,
        DESFIRE_CARD_PICC_DISABLED_ERROR = 0x0CCD,
        DESFIRE_CARD_COUNT_ERROR = 0x0CCE,
        DESFIRE_CARD_DUPLICATE_ERROR = 0x0CDE,
        DESFIRE_CARD_EEPROM_ERROR_DES = 0x0CEE,
        DESFIRE_CARD_FILE_NOT_FOUND = 0x0CF0,
        DESFIRE_CARD_FILE_INTEGRITY_ERROR = 0x0CF1
    };

    public enum DESFIRE_KEY_SETTINGS
    {
        DESFIRE_KEY_SET_CREATE_WITH_AUTH_SET_CHANGE_KEY_CHANGE = 0x09,
        DESFIRE_KEY_SET_CREATE_WITHOUT_AUTH_SET_CHANGE_KEY_CHANGE = 0x0F,
        DESFIRE_KEY_SET_CREATE_WITH_AUTH_SET_NOT_CHANGE_KEY_CHANGE = 0x01,
        DESFIRE_KEY_SET_CREATE_WITHOUT_AUTH_SET_NOT_CHANGE_KEY_CHANGE = 0x07,
        DESFIRE_KEY_SET_CREATE_WITH_AUTH_SET_CHANGE_KEY_NOT_CHANGE = 0x08,
        DESFIRE_KEY_SET_CREATE_WITHOUT_AUTH_SET_CHANGE_KEY_NOT_CHANGE = 0x0E,
        DESFIRE_KEY_SET_CREATE_WITH_AUTH_SET_NOT_CHANGE_KEY_NOT_CHANGE = 0x00,
        DESFIRE_KEY_SET_CREATE_WITHOUT_AUTH_SET_NOT_CHANGE_KEY_NOT_CHANGE = 0x06
    };

    public static class uFCoder
    {
        public const uint JCAPP_MIN_PIN_LENGTH = 4;
        public const uint JCAPP_MAX_PIN_LENGTH = 8;
        public const uint JCAPP_PUK_LENGTH = 8;
        public const uint SIG_MAX_PLAIN_DATA_LEN = 255;
        public const string JCDL_AID_RID_PLUS = "F0 44 4C 6F 67 69 63";
        public const string JCDL_AID_PIX = "00 01";
        public const string JCDL_AID = JCDL_AID_RID_PLUS + JCDL_AID_PIX;

        const UInt32 MIN_UFR_LIB_VERSION = 0x05000003; // bytes from left to right: MSB=MajorVer, MidSB_H=MinorVer, MidSB_L=0, LSB=BuildNum
        const UInt32 MIN_UFR_FW_VERSION = 0x05000007; // bytes from left to right: MSB=MajorVer, MidSB_H=MinorVer, MidSB_L=0, LSB=BuildNum

        //...
        //...
        //...
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

#if WIN64
        const string DLL_PATH = ""; //@"..\..\..\ufr-lib\windows\x86_64\";
        const string NAME_DLL = "uFCoder-x86_64.dll";

#else
        const string DLL_PATH = "";//; @"..\..\ufr-lib\windows\x86\";
        const string NAME_DLL = "uFCoder-x86.dll";
#endif
        const string DLL_NAME = DLL_PATH + NAME_DLL;

        public static void uFrOpen()
        {
            DL_STATUS status;
            UInt32 version = 0;
            byte version_major;
            byte version_minor;
            UInt16 lib_build;
            byte fw_build;
            String uFR_NotOpenedMessage;


#if WIN64
            string DllPath = @"lib\windows\x86_64"; // for x64 target
            string name = "uFCoder-x86_64.dll";
#else
            string DllPath = @"lib\windows\x86"; // for x86 target
            string name = "uFCoder-x86.dll";
#endif
            string dll_name = DllPath + name;
            int repeat_cnt = 0;
            bool repeat = true;
            do
            {
                DllPath = @"..\" + DllPath;
                SetDllDirectory(DllPath);
                try
                {
                    version = uFCoder.GetDllVersion();
                    repeat = false;
                }
                catch (System.Exception)
                {
                    ++repeat_cnt;
                }

            } while (repeat && repeat_cnt < 3); // relative path upper folders level search

            if (repeat)
                throw new Exception("Can't find " + dll_name + ".\r\nYou will not be able to work with DL Signer cards.");

            // Check lib version:
            version_major = (byte)version;
            version_minor = (byte)(version >> 8);
            lib_build = (ushort)(version >> 16);

            version = ((UInt32)version_major << 24) | ((UInt32)version_minor << 16) | (UInt32)lib_build;
            if (version < MIN_UFR_LIB_VERSION)
            {
                uFR_NotOpenedMessage = "Wrong uFCoder library version.\r\n"
                    + "You can't work with DL Signer cards.\r\n\r\nUse uFCoder library "
                    + (MIN_UFR_LIB_VERSION >> 24) + "." + ((MIN_UFR_LIB_VERSION >> 16) & 0xFF)
                    + "." + (MIN_UFR_LIB_VERSION & 0xFFFF) + " or higher.";
                throw new Exception("Wrong uFCoder library version.\r\n"
                    + "You can't work with DL Signer cards.\r\n\r\nUse uFCoder library "
                    + (MIN_UFR_LIB_VERSION >> 24) + "." + ((MIN_UFR_LIB_VERSION >> 16) & 0xFF)
                    + "." + (MIN_UFR_LIB_VERSION & 0xFFFF) + " or higher.");
            }

            status = ReaderOpen();
            if (status != (UInt32)uFR.DL_STATUS.UFR_OK)
            {
                uFR_NotOpenedMessage = "uFR reader not opened.\r\nYou can't work with Desfire cards."
                    + "\r\n\r\nTry to connect uFR reader and restart application.";
                throw new Exception("Can't open uFR reader.\r\nYou will not be able to work with Desfire cards.");
            }

            // Check firmware version:
            status = GetReaderFirmwareVersion(out version_major, out version_minor);
            if (status != (UInt32)uFR.DL_STATUS.UFR_OK)
            {
                uFCoder.ReaderClose();
                throw new Exception("Can't open uFR reader.\r\nYou will not be able to work with Desfire cards.");
            }
            status = GetBuildNumber(out fw_build);
            if (status != (UInt32)uFR.DL_STATUS.UFR_OK)
            {
                uFCoder.ReaderClose();
                throw new Exception("Can't open uFR reader.\r\nYou will not be able to work with Desfire cards.");
            }

            version = ((UInt32)version_major << 24) | ((UInt32)version_minor << 16) | (UInt32)fw_build;
            if (version < MIN_UFR_FW_VERSION)
            {
                uFCoder.ReaderClose();
                uFR_NotOpenedMessage = "Wrong uFR firmware version.\r\n"
                    + "You can't work with Desfire cards.\r\n\r\nPlease update firmware to "
                    + (MIN_UFR_FW_VERSION >> 24) + "." + ((MIN_UFR_FW_VERSION >> 16) & 0xFF)
                    + "." + (MIN_UFR_FW_VERSION & 0xFFFF) + " or higher.";
                throw new Exception("Wrong uFR firmware version.\r\n"
                    + "Some functions with DESFIRE CARDS will be unavailable.\r\n\r\nPlease update firmware to "
                    + (MIN_UFR_FW_VERSION >> 24) + "." + ((MIN_UFR_FW_VERSION >> 16) & 0xFF)
                    + "." + (MIN_UFR_FW_VERSION & 0xFFFF) + " or higher and restart application.");
            }


        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderOpen")]
        public static extern DL_STATUS ReaderOpen();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderOpenEx")]
        private static extern DL_STATUS ReaderOpenEx(UInt32 reader_type, [In] byte[] port_name, UInt32 port_interface, [In] byte[] arg);
        public static DL_STATUS ReaderOpenEx(UInt32 reader_type, string port_name, UInt32 port_interface, string arg)
        {

            byte[] port_name_p = Encoding.ASCII.GetBytes(port_name);
            byte[] port_name_param = new byte[port_name_p.Length + 1];
            Array.Copy(port_name_p, 0, port_name_param, 0, port_name_p.Length);
            port_name_param[port_name_p.Length] = 0;

            byte[] arg_p = Encoding.ASCII.GetBytes(arg);
            byte[] arg_param = new byte[arg_p.Length + 1];
            Array.Copy(arg_p, 0, arg_param, 0, arg_p.Length);
            arg_param[arg_p.Length] = 0;

            return ReaderOpenEx(reader_type, port_name_param, port_interface, arg_param);
        }

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderClose")]
        public static extern DL_STATUS ReaderClose();

        //--------------------------------------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderType")]
        public static extern DL_STATUS GetReaderType(out UInt32 lpulReaderType);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderSerialDescription")]
        private static extern DL_STATUS getReaderSerialDescription(StringBuilder pSerialDescription);
        public static DL_STATUS GetReaderSerialDescription(out string SerialDescription)
        {
            StringBuilder pSerialDescription = new StringBuilder(7);
            DL_STATUS status = DL_STATUS.UFR_OK;

            status = getReaderSerialDescription(pSerialDescription);
            if (status == DL_STATUS.UFR_OK)
                SerialDescription = pSerialDescription.ToString();
            else
                SerialDescription = "";
            return status;
        }

        //---------------------------------------------------------------------
        // Card emulation:
        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "TagEmulationStart")]
        public static extern DL_STATUS TagEmulationStart();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "TagEmulationStop")]
        public static extern DL_STATUS TagEmulationStop();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "CombinedModeEmulationStart")]
        public static extern DL_STATUS CombinedModeEmulationStart();
        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetDlogicCardType")]
        public static extern DL_STATUS GetDlogicCardType(out byte lpucCardType);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_UpdateAndGetCount")]
        public static extern DL_STATUS ReaderList_UpdateAndGetCount(out UInt32 NumberOfDevices);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetSerialByIndex")]
        public static extern DL_STATUS ReaderList_GetSerialByIndex(Int32 DeviceIndex, out UInt32 lpulSerialNumber);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetSerialDesByIndex")]
        public static extern DL_STATUS ReaderList_GetSerialDescriptionByIndex(Int32 DeviceIndex, out byte pSerialDescription);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetTypeByIndex")]
        public static extern DL_STATUS ReaderList_GetTypeByIndex(Int32 DeviceIndex, out UInt32 lpulReaderType);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetFTDISerialByIndex")]
        private static extern DL_STATUS GetFTDISerialByIndex(UInt32 DeviceIndex, out IntPtr DeviceSerial);
        public static DL_STATUS ReaderList_GetFTDISerialByIndex(UInt32 DeviceIndex, out string DeviceSerial)
        {
            IntPtr ptr;
            DL_STATUS status = DL_STATUS.UFR_OK;

            status = GetFTDISerialByIndex(DeviceIndex, out ptr);
            DeviceSerial = Marshal.PtrToStringAnsi(ptr);
            return status;
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetFTDIDescriptionByIndex")]
        private static extern DL_STATUS GetFTDIDescriptionByIndex(UInt32 DeviceIndex, out IntPtr DeviceDescription);
        public static DL_STATUS ReaderList_GetFTDIDescriptionByIndex(UInt32 DeviceIndex, out string DeviceDescription)
        {
            IntPtr ptr;
            DL_STATUS status = DL_STATUS.UFR_OK;

            status = GetFTDIDescriptionByIndex(DeviceIndex, out ptr);
            DeviceDescription = Marshal.PtrToStringAnsi(ptr);
            return status;
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_OpenByIndex")]
        public static extern DL_STATUS ReaderList_OpenByIndex(Int32 DeviceIndex, out UFR_HANDLE hndUFR);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetDllVersion")]
        public static extern UInt32 GetDllVersion();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderHardwareVersion")]
        public static extern DL_STATUS GetReaderHardwareVersion(out byte version_major, out byte version_minor);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderFirmwareVersion")]
        public static extern DL_STATUS GetReaderFirmwareVersion(out byte version_major, out byte version_minor);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetBuildNumber")]
        public static extern DL_STATUS GetBuildNumber(out byte build);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "SectorTrailerWriteUnsafe_PK")]
        public static extern DL_STATUS SectorTrailerWriteUnsafe_PK(byte addressing_mode, byte address, out byte sector_trailer,
                                                  byte auth_mode, ref byte key);
        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "LinearWrite_PK")]
        public static extern DL_STATUS LinearWrite_PK([In] byte[] data, ushort linear_address, ushort length, out ushort bytes_written,
                                     byte auth_mode, [In] byte[] key);
        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "EE_Lock")]
        private static extern DL_STATUS Linkage_EE_Lock(StringBuilder password, UInt32 locked);
        public static DL_STATUS EE_Lock(String password, UInt32 locked)
        {
            if (password.Length != 8)
                return DL_STATUS.UFR_PARAMETERS_ERROR;

            StringBuilder ptr_password = new StringBuilder(password);
            return Linkage_EE_Lock(ptr_password, locked);
        }

        //--------------------------------------------------------------------------------------------------------------

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderOpen")]
        //public static extern DL_STATUS ReaderOpen();

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderClose")]
        //public static extern DL_STATUS ReaderClose();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ReaderReset")]
        public static extern DL_STATUS ReaderReset();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ReaderSoftRestart")]
        public static extern DL_STATUS ReaderSoftRestart();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "UfrEnterSleepMode")]
        public static extern DL_STATUS UfrEnterSleepMode();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "UfrLeaveSleepMode")]
        public static extern DL_STATUS UfrLeaveSleepMode();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "AutoSleepSet")]
        public static extern DL_STATUS AutoSleepSet(byte seconds_wait);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "AutoSleepGet")]
        public static extern DL_STATUS AutoSleepGet(out byte seconds_wait);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "GetReaderType")]
        //public static extern DL_STATUS GetReaderType(out UInt32 get_reader_type);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderSerialDescription")]
        //public static extern DL_STATUS GetReaderSerialDescription(out byte pSerialDescription);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "GetReaderSerialNumber")]
        public static extern DL_STATUS GetReaderSerialNumber(out UInt32 serial_number);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ReaderKeyWrite")]
        public static extern DL_STATUS ReaderKeyWrite([In] byte[] aucKey, byte ucKeyIndex);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ReaderUISignal")]
        public static extern DL_STATUS ReaderUISignal(int light_mode, int sound_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ReadUserData")]
        public static extern DL_STATUS ReadUserData(out byte aucData);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "WriteUserData")]
        public static extern DL_STATUS WriteUserData(out byte aucData);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "GetReaderHardwareVersion")]
        //public static extern DL_STATUS GetReaderHardwareVersion(out byte bVerMajor,
        //                                                        out byte bVerMinor);
        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "GetReaderFirmwareVersion")]
        //public static extern DL_STATUS GetReaderFirmwareVersion(out byte bVerMajor,
        //                                                        out byte bVerMinor);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "SetDisplayData")]
        public static extern DL_STATUS SetDisplayData(out byte display_data, byte data_length);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "SetSpeakerFrequency")]
        public static extern DL_STATUS SetSpeakerFrequency(short fhz);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "SetDisplayIntensity")]
        public static extern DL_STATUS SetDisplayIntensity(Byte intensity); // 0 to 100 [%]

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "GetDisplayIntensity")]
        public static extern DL_STATUS GetDisplayIntensity(out Byte intensity); // 0 to 100 [%]

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "GetCardIdEx")]
        public static extern DL_STATUS GetCardIdEx(out byte bCardType,
                                                   [In, Out] byte[] nfc_uid, // NFC_UID_MAX_LEN = 10
                                                   out byte bUidSize);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetDlogicCardType")]
        //public static extern DL_STATUS GetDlogicCardType(out byte lpucCardType);

        //--------------------------------------------------------------------------------------------------------------

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_UpdateAndGetCount")]
        //public static extern DL_STATUS ReaderList_UpdateAndGetCount(Int32* NumberOfDevices);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetSerialByIndex")]
        //public static extern DL_STATUS ReaderList_GetSerialByIndex(Int32 DeviceIndex, UInt32* lpulSerialNumber);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetSerialDesByIndex")]
        //public static extern DL_STATUS ReaderList_GetSerialDescriptionByIndex(Int32 DeviceIndex, char* pSerialDescription);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetTypeByIndex")]
        //public static extern DL_STATUS ReaderList_GetTypeByIndex(Int32 DeviceIndex, UInt32* lpulReaderType);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetFTDISerialByIndex")]
        //public static extern DL_STATUS ReaderList_GetFTDISerialByIndex(Int32 DeviceIndex, char** Device_Serial);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetFTDIDescriptionByIndex")]
        //public static extern DL_STATUS ReaderList_GetFTDIDescriptionByIndex(Int32 DeviceIndex, char** Device_Description);

        //--------------------------------------------------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearRead")]
        public static extern DL_STATUS LinearRead([Out] byte[] aucData,
                                                  ushort linear_address,
                                                  ushort data_len,
                                                  out UInt16 bytes_written,
                                                  byte auth_mode,
                                                  byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearRead_AKM1")]
        public static extern DL_STATUS LinearRead_AKM1([Out] byte[] aucData,
                                                   ushort linear_address,
                                                   ushort data_len,
                                                   out UInt16 bytes_written,
                                                   byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearRead_AKM2")]
        public static extern DL_STATUS LinearRead_AKM2([Out] byte[] aucData,
                                                   ushort linear_address,
                                                   ushort data_len,
                                                   out UInt16 bytes_written,
                                                   byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearRead_PK")]
        public static extern DL_STATUS LinearRead_PK([Out] byte[] aucData,
                                                   ushort linear_address,
                                                   ushort data_len,
                                                   out UInt16 bytes_written,
                                                   byte auth_mode,
                                                   [In] byte[] pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearWrite")]
        public static extern DL_STATUS LinearWrite([In] byte[] aucData,
                                                    ushort linear_address,
                                                    ushort data_len,
                                                    out UInt16 bytes_written,
                                                    byte auth_mode,
                                                    byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearWrite_AKM1")]
        public static extern DL_STATUS LinearWrite_AKM1([In] byte[] aucData,
                                                    ushort linear_address,
                                                    ushort data_len,
                                                    out UInt16 bytes_written,
                                                    byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearWrite_AKM2")]
        public static extern DL_STATUS LinearWrite_AKM2([In] byte[] aucData,
                                                    ushort linear_address,
                                                    ushort data_len,
                                                    out UInt16 bytes_written,
                                                    byte auth_mode);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearWrite_PK")]
        //public static extern DL_STATUS LinearWrite_PK(out byte aucData,
        //                                           ushort linear_address,
        //                                           ushort data_len,
        //                                           out UInt16 bytes_written,
        //                                           byte key_mode,
        //                                           out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockRead")]
        public static extern DL_STATUS BlockRead([Out] byte[] data,
                                                  UInt16 block_address,
                                                  byte auth_mode,
                                                  UInt16 key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockRead_AKM1")]
        public static extern DL_STATUS BlockRead_AKM1([Out] byte[] data,
                                                      UInt16 block_address,
                                                      byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockRead_AKM2")]
        public static extern DL_STATUS BlockRead_AKM2([Out] byte[] data,
                                                      UInt16 block_address,
                                                      byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockRead_PK")]
        public static extern DL_STATUS BlockRead_PK([Out] byte[] data,
                                                    UInt16 block_address,
                                                    byte auth_mode,
                                                    [In] byte[] pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockWrite")]
        public static extern DL_STATUS BlockWrite([In] byte[] data,
                                                  UInt16 block_address,
                                                  byte auth_mode,
                                                  byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockWrite_AKM1")]
        public static extern DL_STATUS BlockWrite_AKM1([In] byte[] data,
                                                       UInt16 block_address,
                                                       byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockWrite_AKM2")]
        public static extern DL_STATUS BlockWrite_AKM2([In] byte[] data,
                                                       UInt16 block_address,
                                                       byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockWrite_PK")]
        public static extern DL_STATUS BlockWrite_PK([In] byte[] data,
                                                     UInt16 block_address,
                                                     byte auth_mode,
                                                     [In] byte[] pk_key);



        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockInSectorRead")]
        public static extern DL_STATUS BlockInSectorRead([Out] byte[] data,
                                                         byte sector_address,
                                                         byte block_in_sector_address,
                                                         byte auth_mode, byte key_index);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockInSectorRead_AKM1")]
        public static extern DL_STATUS BlockInSectorRead_AKM1([Out] byte[] data,
                                                              byte sector_address,
                                                              byte block_in_sector_address,
                                                              byte auth_mode);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockInSectorRead_AKM2")]
        public static extern DL_STATUS BlockInSectorRead_AKM2([Out] byte[] data,
                                                              byte sector_address,
                                                              byte block_in_sector_address,
                                                              byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockInSectorRead_PK")]
        public static extern DL_STATUS BlockInSectorRead_PK([Out] byte[] data,
                                                            byte sector_address,
                                                            byte block_in_sector_address,
                                                            byte auth_mode,
                                                            [In] byte[] pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockInSectorWrite")]
        public static extern DL_STATUS BlockInSectorWrite([In] byte[] data,
                                                          byte sector_address,
                                                          byte block_in_sector_address,
                                                          byte auth_mode,
                                                          byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockInSectorWrite_AKM1")]
        public static extern DL_STATUS BlockInSectorWrite_AKM1([In] byte[] data,
                                                               byte sector_address,
                                                               byte block_in_sector_address,
                                                               byte auth_mode);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockInSectorWrite_AKM2")]
        public static extern DL_STATUS BlockInSectorWrite_AKM2([In] byte[] data,
                                                               byte sector_address,
                                                               byte block_in_sector_address,
                                                               byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "BlockInSectorWrite_PK")]
        public static extern DL_STATUS BlockInSectorWrite_PK([In] byte[] data,
                                                             byte sector_address,
                                                             byte block_in_sector_address,
                                                             byte auth_mode,
                                                             [In] byte[] pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockRead")]
        public static extern DL_STATUS ValueBlockRead(out Int32 value,
                                                        out byte value_addr,
                                                        UInt16 block_address,
                                                        byte auth_mode,
                                                        byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockRead_AKM1")]
        public static extern DL_STATUS ValueBlockRead_AKM1(out Int32 value,
                                                           out byte value_addr,
                                                           UInt16 block_address,
                                                           byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockRead_AKM2")]
        public static extern DL_STATUS ValueBlockRead_AKM2(out Int32 value,
                                                           out byte value_addr,
                                                           UInt16 block_address,
                                                           byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockRead_PK")]
        public static extern DL_STATUS ValueBlockRead_PK(out Int32 value,
                                                         out byte value_addr,
                                                         UInt16 block_address,
                                                         byte auth_mode,
                                                         out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockWrite")]
        public static extern DL_STATUS ValueBlockWrite(int value,
                                                        byte value_addr,
                                                        UInt16 block_address,
                                                        byte auth_mode,
                                                        byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockWrite_AKM1")]
        public static extern DL_STATUS ValueBlockWrite_AKM1(int value,
                                                        byte value_addr,
                                                        UInt16 block_address,
                                                        byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockWrite_AKM2")]
        public static extern DL_STATUS ValueBlockWrite_AKM2(int value,
                                                        byte value_addr,
                                                        UInt16 block_address,
                                                        byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockWrite_PK")]
        public static extern DL_STATUS ValueBlockWrite_PK(int value,
                                                        byte value_addr,
                                                        UInt16 block_address,
                                                        byte auth_mode,
                                                        out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockIncrement")]
        public static extern DL_STATUS ValueBlockIncrement(int increment_value,
                                                            UInt16 block_address,
                                                            byte auth_mode,
                                                            byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockIncrement_AKM1")]
        public static extern DL_STATUS ValueBlockIncrement_AKM1(int increment_value,
                                                                UInt16 block_address,
                                                                byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockIncrement_AKM2")]
        public static extern DL_STATUS ValueBlockIncrement_AKM2(int increment_value,
                                                                UInt16 block_address,
                                                                byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockIncrement_PK")]
        public static extern DL_STATUS ValueBlockIncrement_PK(int increment_value,
                                                                UInt16 block_address,
                                                                byte auth_mode,
                                                                out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockDecrement")]
        public static extern DL_STATUS ValueBlockDecrement(int increment_value,
                                                           UInt16 block_address,
                                                           byte auth_mode,
                                                             byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockDecrement_AKM1")]
        public static extern DL_STATUS ValueBlockDecrement_AKM1(int increment_value,
                                                                UInt16 block_address,
                                                                byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockDecrement_AKM2")]
        public static extern DL_STATUS ValueBlockDecrement_AKM2(int increment_value,
                                                                UInt16 block_address,
                                                                byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockDecrement_PK")]
        public static extern DL_STATUS ValueBlockDecrement_PK(int increment_value,
                                                              UInt16 block_address,
                                                              byte auth_mode,
                                                              out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorRead")]
        public static extern DL_STATUS ValueBlockInSectorRead(out Int32 value,
                                                                 out byte value_addr,
                                                                 byte sector_address,
                                                                 byte block_in_sector_address,
                                                                 byte auth_mode,
                                                                 byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorRead_AKM1")]
        public static extern DL_STATUS ValueBlockInSectorRead_AKM1(out Int32 value,
                                                                 out byte value_addr,
                                                                 byte sector_address,
                                                                 byte block_in_sector_address,
                                                                 byte auth_mode);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorRead_AKM2")]
        public static extern DL_STATUS ValueBlockInSectorRead_AKM2(out Int32 value,
                                                                 out byte value_addr,
                                                                 byte sector_address,
                                                                 byte block_in_sector_address,
                                                                 byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorRead_PK")]
        public static extern DL_STATUS ValueBlockInSectorRead_PK(out Int32 value,
                                                                 out byte value_addr,
                                                                 byte sector_address,
                                                                 byte block_in_sector_address,
                                                                 byte auth_mode,
                                                                 out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorWrite")]
        public static extern DL_STATUS ValueBlockInSectorWrite(Int32 value,
                                                               byte value_addr,
                                                               byte sector_address,
                                                               byte block_in_sector_address,
                                                               byte auth_mode, byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorWrite_AKM1")]
        public static extern DL_STATUS ValueBlockInSectorWrite_AKM1(Int32 value,
                                                               byte value_addr,
                                                               byte sector_address,
                                                               byte block_in_sector_address,
                                                               byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorWrite_AKM2")]
        public static extern DL_STATUS ValueBlockInSectorWrite_AKM2(Int32 value,
                                                               byte value_addr,
                                                               byte sector_address,
                                                               byte block_in_sector_address,
                                                               byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorWrite_PK")]
        public static extern DL_STATUS ValueBlockInSectorWrite_PK(Int32 value,
                                                               byte value_addr,
                                                               byte sector_address,
                                                               byte block_in_sector_address,
                                                               byte auth_mode,
                                                               out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorIncrement")]
        public static extern DL_STATUS ValueBlockInSectorIncrement(Int32 increment_value,
                                                                    byte sector_address,
                                                                    byte block_in_sector_address,
                                                                    byte auth_mode,
                                                                    byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorIncrement_AKM1")]
        public static extern DL_STATUS ValueBlockInSectorIncrement_AKM1(Int32 increment_value,
                                                                    byte sector_address,
                                                                    byte block_in_sector_address,
                                                                    byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorIncrement_AKM2")]
        public static extern DL_STATUS ValueBlockInSectorIncrement_AKM2(Int32 increment_value,
                                                                    byte sector_address,
                                                                    byte block_in_sector_address,
                                                                    byte auth_mode);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorIncrement_PK")]
        public static extern DL_STATUS ValueBlockInSectorIncrement_PK(Int32 increment_value,
                                                                    byte sector_address,
                                                                    byte block_in_sector_address,
                                                                    byte auth_mode,
                                                                    out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorDecrement")]
        public static extern DL_STATUS ValueBlockInSectorDecrement(Int32 decrement_value,
                                                                    byte sector_address,
                                                                    byte block_in_sector_address,
                                                                    byte auth_mode,
                                                                    byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorDecrement_AKM1")]
        public static extern DL_STATUS ValueBlockInSectorDecrement_AKM1(Int32 decrement_value,
                                                                    byte sector_address,
                                                                    byte block_in_sector_address,
                                                                    byte auth_mode);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorDecrement_AKM2")]
        public static extern DL_STATUS ValueBlockInSectorDecrement_AKM2(Int32 decrement_value,
                                                                    byte sector_address,
                                                                    byte block_in_sector_address,
                                                                    byte auth_mode);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ValueBlockInSectorDecrement_PK")]
        public static extern DL_STATUS ValueBlockInSectorDecrement_PK(Int32 decrement_value,
                                                                    byte sector_address,
                                                                    byte block_in_sector_address,
                                                                    byte auth_mode,
                                                                    out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "SectorTrailerWrite")]
        public static extern DL_STATUS SectorTrailerWrite(byte addressing_mode,
                                        byte address,
                                        out byte new_key_A,
                                        byte block0_access_bits,
                                        byte block1_access_bits,
                                        byte block2_access_bits,
                                        byte sector_trailer_access_bits,
                                        byte sector_trailer_byte9,
                                        out byte new_key_B,
                                        byte auth_mode,
                                        byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "SectorTrailerWrite_AKM1")]
        public static extern DL_STATUS SectorTrailerWrite_AKM1(byte addressing_mode,
                                        byte address,
                                        out byte new_key_A,
                                        byte block0_access_bits,
                                        byte block1_access_bits,
                                        byte block2_access_bits,
                                        byte sector_trailer_access_bits,
                                        byte sector_trailer_byte9,
                                        out byte new_key_B,
                                        byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "SectorTrailerWrite_AKM2")]
        public static extern DL_STATUS SectorTrailerWrite_AKM2(byte addressing_mode,
                                        byte address,
                                        out byte new_key_A,
                                        byte block0_access_bits,
                                        byte block1_access_bits,
                                        byte block2_access_bits,
                                        byte sector_trailer_access_bits,
                                        byte sector_trailer_byte9,
                                        out byte new_key_B,
                                        byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "SectorTrailerWrite_PK")]
        public static extern DL_STATUS SectorTrailerWrite_PK(byte addressing_mode,
                                        byte address,
                                        out byte new_key_A,
                                        byte block0_access_bits,
                                        byte block1_access_bits,
                                        byte block2_access_bits,
                                        byte sector_trailer_access_bits,
                                        byte sector_trailer_byte9,
                                        out byte new_key_B,
                                        byte auth_mode,
                                        out byte pk_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearFormatCard")]
        public static extern DL_STATUS LinearFormatCard(out byte new_key_A,
                                                         byte blocks_access_bits,
                                                         byte sector_trailers_access_bits,
                                                         byte sector_trailers_byte9,
                                                         out byte new_key_B,
                                                         out byte sectors_formatted,
                                                         byte auth_mode,
                                                         byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearFormatCard_AKM1")]
        public static extern DL_STATUS LinearFormatCard_AKM1(out byte new_key_A,
                                                         byte blocks_access_bits,
                                                         byte sector_trailers_access_bits,
                                                         byte sector_trailers_byte9,
                                                         out byte new_key_B,
                                                         out byte sectors_formatted,
                                                         byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearFormatCard_AKM2")]
        public static extern DL_STATUS LinearFormatCard_AKM2(out byte new_key_A,
                                                         byte blocks_access_bits,
                                                         byte sector_trailers_access_bits,
                                                         byte sector_trailers_byte9,
                                                         out byte new_key_B,
                                                         out byte sectors_formatted,
                                                         byte auth_mode);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "LinearFormatCard_PK")]
        public static extern DL_STATUS LinearFormatCard_PK(out byte new_key_A,
                                                         byte blocks_access_bits,
                                                         byte sector_trailers_access_bits,
                                                         byte sector_trailers_byte9,
                                                         out byte new_key_B,
                                                         out byte sectors_formatted,
                                                         byte auth_mode,
                                                         out byte pk_key);

        //----------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "SetISO14443_4_Mode")]
        public static extern DL_STATUS SetISO14443_4_Mode();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "s_block_deselect")]
        public static extern DL_STATUS s_block_deselect(byte timeout);
        //----------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppSelectByAid")]
        public static extern DL_STATUS JCAppSelectByAid([In] byte[] aid, byte aid_len, [Out] byte[] selection_response);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppPutPrivateKey")]
        public static extern DL_STATUS JCAppPutPrivateKey(byte key_type, byte key_index,
                                                          [In] byte[] key, UInt16 key_bit_len,
                                                          [In] byte[] key_param, UInt16 key_parm_len);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGenerateKeyPair")]
        public static extern DL_STATUS JCAppGenerateKeyPair(byte key_type, byte key_index, byte key_designator,
                                                            UInt16 key_bit_len, [In] byte[] param, UInt16 param_size);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppDeleteRsaKeyPair")]
        public static extern DL_STATUS JCAppDeleteRsaKeyPair(byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppDeleteEcKeyPair")]
        public static extern DL_STATUS JCAppDeleteEcKeyPair(byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppSignatureBegin")]
        public static extern DL_STATUS JCAppSignatureBegin(byte cipher, byte digest, byte padding, byte key_index,
                                                           [In] byte[] chunk, UInt16 chunk_len,
                                                           [In] byte[] alg_param, UInt16 alg_parm_len);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppSignatureUpdate")]
        public static extern DL_STATUS JCAppSignatureUpdate([In] byte[] chunk, UInt16 chunk_len);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppSignatureEnd")]
        private static extern DL_STATUS JCAppSignatureEnd(out UInt16 sig_len);
        public static DL_STATUS JCAppSignatureEnd(out byte[] sig)
        {
            DL_STATUS status;
            UInt16 sig_len;

            status = JCAppSignatureEnd(out sig_len);
            if (status != DL_STATUS.UFR_OK)
            {
                sig = null;
                return status;
            }

            sig = new byte[sig_len];
            return JCAppGetSignature(sig, sig_len);
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGenerateSignature")]
        private static extern DL_STATUS JCAppGenerateSignature(byte cipher, byte digest, byte padding, byte key_index,
                                                                [In] byte[] plain_data, UInt16 plain_data_len,
                                                                out UInt16 sig_len,
                                                                [In] byte[] alg_param, UInt16 alg_parm_len);

        public static DL_STATUS JCAppGenerateSignature(byte cipher, byte digest, byte padding, byte key_index,
                                                       [In] byte[] plain_data, UInt16 plain_data_len,
                                                       out byte[] sig,
                                                       [In] byte[] alg_param, UInt16 alg_parm_len)
        {
            DL_STATUS status;
            UInt16 sig_len;

            status = JCAppGenerateSignature(cipher, digest, padding, key_index, plain_data, plain_data_len, out sig_len, alg_param, alg_parm_len);
            if (status != DL_STATUS.UFR_OK)
            {
                sig = null;
                return status;
            }

            sig = new byte[sig_len];
            return JCAppGetSignature(sig, sig_len);
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGetSignature")]
        private static extern DL_STATUS JCAppGetSignature([Out] byte[] sig, UInt16 sig_len);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppPutObj")]
        public static extern DL_STATUS JCAppPutObj(byte obj_type, byte obj_index, [In] byte[] obj, UInt16 obj_size, [In] byte[] id, byte id_size);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppPutObjSubject")]
        public static extern DL_STATUS JCAppPutObjSubject(byte obj_type, byte obj_index, [In] byte[] subject, byte size);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppInvalidateCert")]
        public static extern DL_STATUS JCAppInvalidateCert(byte obj_type, byte obj_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGetObjId")]
        private static extern DL_STATUS JCAppGetObjId(byte obj_type, byte obj_index, [Out] byte[] id, out UInt16 id_size);
        public static DL_STATUS JCAppGetObjId(byte obj_type, byte obj_index, out byte[] id, out UInt16 size)
        {
            DL_STATUS status;

            size = 0;
            id = null;
            status = JCAppGetObjId(obj_type, obj_index, id, out size);
            if (status == DL_STATUS.UFR_OK)
            {
                id = new byte[size];
                status = JCAppGetObjId(obj_type, obj_index, id, out size);
            }
            return status;
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGetObjSubject")]
        private static extern DL_STATUS JCAppGetObjSubject(byte obj_type, byte obj_index, [Out] byte[] subject, out byte subject_size);
        public static DL_STATUS JCAppGetObjSubject(byte obj_type, byte obj_index, out byte[] subject, out byte size)
        {
            DL_STATUS status;

            size = 0;
            subject = null;
            status = JCAppGetObjSubject(obj_type, obj_index, subject, out size);
            if (status == DL_STATUS.UFR_OK)
            {
                subject = new byte[size];
                status = JCAppGetObjSubject(obj_type, obj_index, subject, out size);
            }
            return status;
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGetObj")]
        private static extern DL_STATUS JCAppGetObj(byte obj_type, byte obj_index, [Out] byte[] obj, UInt16 size);
        public static DL_STATUS JCAppGetObj(byte obj_type, byte obj_index, byte[] obj)
        {
            return JCAppGetObj(obj_type, obj_index, obj, (UInt16)obj.Length);
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGetErrorDescription")]
        private static extern IntPtr GetErrorDescription(UInt32 apdu_error_status);
        public static string JCAppGetErrorDescription(UInt32 apdu_error_status)
        {
            IntPtr str_ret = GetErrorDescription(apdu_error_status);
            return Marshal.PtrToStringAnsi(str_ret);
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppLogin")]
        public static extern DL_STATUS JCAppLogin(byte SO, [In] byte[] pin, byte pinSize);
        public static DL_STATUS JCAppLogin(bool SO, string pin)
        {
            if (pin.Length > byte.MaxValue)
                return DL_STATUS.UFR_PARAMETERS_ERROR;

            byte[] pin_param = Encoding.ASCII.GetBytes(pin);
            return JCAppLogin(SO ? (byte)1 : (byte)0, pin_param, (byte)pin_param.Length);
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGetPinTriesRemaining")]
        public static extern DL_STATUS JCAppGetPinTriesRemaining(DL_SECURE_CODE secureCodeType, out UInt16 triesRemaining);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppPinChange")]
        public static extern DL_STATUS JCAppPinChange(DL_SECURE_CODE secureCodeType, [In] byte[] newPin, byte newPinSize);
        public static DL_STATUS JCAppPinChange(DL_SECURE_CODE secureCodeType, string newPin)
        {
            if (newPin.Length > byte.MaxValue)
                return DL_STATUS.UFR_PARAMETERS_ERROR;

            byte[] newPin_param = Encoding.ASCII.GetBytes(newPin);
            return JCAppPinChange(secureCodeType, newPin_param, (byte)newPin_param.Length);
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppPinUnblock")]
        public static extern DL_STATUS JCAppPinUnblock(byte SO, [In] byte[] puk, byte pukSize);
        public static DL_STATUS JCAppPinUnblock(bool SO, string puk)
        {
            if (puk.Length > byte.MaxValue)
                return DL_STATUS.UFR_PARAMETERS_ERROR;

            byte[] puk_param = Encoding.ASCII.GetBytes(puk);
            return JCAppPinUnblock(SO ? (byte)1 : (byte)0, puk_param, (byte)puk_param.Length);
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGetRsaPublicKey")]
        private static extern DL_STATUS JCAppGetRsaPublicKey(byte key_index, [Out] byte[] modulus, out UInt16 modulus_size,
                [Out] byte[] exponent, out UInt16 exponent_size);
        public static DL_STATUS JCAppGetRsaPublicKey(byte key_index, out byte[] modulus, out byte[] exponent)
        {
            DL_STATUS status;

            UInt16 modulus_size = 0;
            UInt16 exponent_size = 0;
            modulus = null;
            exponent = null;
            status = JCAppGetRsaPublicKey(key_index, modulus, out modulus_size, exponent, out exponent_size);
            if (status == DL_STATUS.UFR_OK)
            {
                modulus = new byte[modulus_size];
                exponent = new byte[exponent_size];
                status = JCAppGetRsaPublicKey(key_index, modulus, out modulus_size, exponent, out exponent_size);
            }
            return status;
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGetEcPublicKey")]
        private static extern DL_STATUS JCAppGetEcPublicKey(byte key_index, [Out] byte[] keyW, out UInt16 keyW_size,
                [Out] byte[] field, out UInt16 field_size, [Out] byte[] ab, out UInt16 ab_size,
                [Out] byte[] g, out UInt16 g_size, [Out] byte[] r, out UInt16 r_size, out UInt16 k, out UInt16 key_size_bits, out UInt16 key_designator);
        public static DL_STATUS JCAppGetEcPublicKey(byte key_index, out byte[] keyW, out byte[] field, out byte[] a, out byte[] b, out byte[] g, out byte[] r,
                out UInt16 k, out UInt16 key_size_bits, out UInt16 key_designator)
        {
            DL_STATUS status;

            UInt16 keyW_size = 0;
            UInt16 field_size = 0;
            UInt16 ab_size = 0;
            UInt16 g_size = 0;
            UInt16 r_size = 0;
            byte[] ab = null;
            keyW = field = a = b = g = r = null;
            k = key_size_bits = key_designator = 0;
            status = JCAppGetEcPublicKey(key_index, keyW, out keyW_size, field, out field_size, ab, out ab_size, g, out g_size,
                    r, out r_size, out k, out key_size_bits, out key_designator);
            if (status == DL_STATUS.UFR_OK)
            {
                keyW = new byte[keyW_size];
                field = new byte[field_size];
                ab = new byte[ab_size];
                g = new byte[g_size];
                r = new byte[r_size];
                status = JCAppGetEcPublicKey(key_index, keyW, out keyW_size, field, out field_size, ab, out ab_size, g, out g_size,
                        r, out r_size, out k, out key_size_bits, out key_designator);

                if (keyW_size != (UInt16)(((key_size_bits + 7) / 8) * 2 + 1))
                    return DL_STATUS.UFR_APDU_WRONG_KEY_SIZE;
                if ((field_size != (UInt16)((key_size_bits + 7) / 8)) && (field_size != 6) && (field_size != 2))
                    return DL_STATUS.UFR_APDU_WRONG_KEY_SIZE;
                if (ab_size != (UInt16)(((key_size_bits + 7) / 8) * 2))
                    return DL_STATUS.UFR_APDU_WRONG_KEY_SIZE;
                if (g_size != (UInt16)(((key_size_bits + 7) / 8) * 2 + 1))
                    return DL_STATUS.UFR_APDU_WRONG_KEY_SIZE;
                if ((r_size != (UInt16)((key_size_bits + 7) / 8)) && (r_size != (UInt16)(((key_size_bits + 7) / 8) + 1)))
                    return DL_STATUS.UFR_APDU_WRONG_KEY_SIZE;
                a = new byte[(key_size_bits + 7) / 8];
                b = new byte[(key_size_bits + 7) / 8];
                Array.Copy(ab, 0, a, 0, (key_size_bits + 7) / 8);
                Array.Copy(ab, (key_size_bits + 7) / 8, b, 0, (key_size_bits + 7) / 8);
            }
            return status;
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "UFR_Status2String")]
        private static extern IntPtr UFR_Status2String(DL_STATUS status);
        public static string status2str(DL_STATUS status)
        {
            IntPtr str_ret = UFR_Status2String(status);
            return Marshal.PtrToStringAnsi(str_ret);
        }

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "JCAppGetEcKeySizeBits")]
        public static extern DL_STATUS JCAppGetEcKeySizeBits(byte key_index, out UInt16 key_size_bits, out UInt16 key_designator);
        //----------------------------------------------------------------------

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "UfrXrcLockOn")]
        public static extern DL_STATUS UfrXrcLockOn(UInt16 pulse_duration);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "UfrXrcRelayState")]
        public static extern DL_STATUS UfrXrcRelayState(byte state);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "UfrXrcGetIoState")]
        public static extern DL_STATUS UfrXrcGetIoState(out byte intercom, out byte dig_in, out byte relay_state);

        //----------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderOpenM")]
        public static extern DL_STATUS ReaderOpenM(UFR_HANDLE hndUFR);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderCloseM")]
        public static extern DL_STATUS ReaderCloseM(UFR_HANDLE hndUFR);

        //----------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetCardIdExM")]
        public static extern DL_STATUS GetCardIdExM(UFR_HANDLE hndUFR,
                                                    out byte bCardType,
                                                    [Out] byte nfc_uid, // NFC_UID_MAX_LEN = 10
                                                    out byte bUidSize);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderTypeM")]
        //public static extern DL_STATUS GetReaderTypeM(UInt32* get_reader_type);

        //----------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "BlockRead_PKM")]
        public static extern DL_STATUS BlockRead_PKM(UFR_HANDLE hndUFR,
                                                  out byte data,
                                                  byte block_address,
                                                  byte auth_mode,
                                                  ref byte key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "BlockWrite_PKM")]
        public static extern DL_STATUS BlockWrite_PKM(UFR_HANDLE hndUFR,
                                                    out byte data,
                                                    byte block_address,
                                                    byte auth_mode,
                                                    ref byte key);
        //----------------------------------------------------------------------

        //Desfire
        //----------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireWriteAesKey")]
        public static extern DL_STATUS uFR_int_DesfireWriteAesKey(byte aes_key_no,
                                                                  [In] byte[] aes_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_GetDesfireUid")]
        public static extern DL_STATUS uFR_int_GetDesfireUid(byte aes_key_nr,
                                                             UInt32 aid,
                                                             byte aid_key_nr,
                                                             [Out] byte[] card_uid,
                                                             out byte card_uid_len,
                                                             out UInt16 card_status,
                                                             out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_GetDesfireUid_PK")]
        public static extern DL_STATUS uFR_int_GetDesfireUid_PK([In] byte[] aes_key_ext,
                                                                UInt32 aid,
                                                                byte aid_key_nr,
                                                                [Out] byte[] card_uid,
                                                                out byte card_uid_len,
                                                                out UInt16 card_status,
                                                                out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireFreeMem")]
        public static extern DL_STATUS uFR_int_DesfireFreeMem(out UInt32 free_mem_byte,
                                                              out UInt16 card_status,
                                                              out UInt16 exec_time);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireFormatCard")]
        public static extern DL_STATUS uFR_int_DesfireFormatCard(byte aes_key_nr,
                                                                 out UInt16 card_status,
                                                                 out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireFormatCard_PK")]
        public static extern DL_STATUS uFR_int_DesfireFormatCard_PK([In] byte[] aes_key_ext,
                                                                    out UInt16 card_status,
                                                                    out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireSetConfiguration")]
        public static extern DL_STATUS uFR_int_DesfireSetConfiguration(byte aes_key_nr,
                                                                       byte random_uid,
                                                                       byte format_disable,
                                                                       out UInt16 card_status,
                                                                       out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireSetConfiguration_PK")]
        public static extern DL_STATUS uFR_int_DesfireSetConfiguration_PK([In] byte[] aes_key_ext,
                                                                          byte random_uid,
                                                                          byte format_disable,
                                                                          out UInt16 card_status,
                                                                          out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireGetKeySettings")]
        public static extern DL_STATUS uFR_int_DesfireGetKeySettings(byte aes_key_nr,
                                                                     UInt32 aid,
                                                                     out byte settings,
                                                                     out byte max_key_no,
                                                                     out UInt16 card_status,
                                                                     out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireGetKeySettings_PK")]
        public static extern DL_STATUS uFR_int_DesfireGetKeySettings_PK([In] byte[] aes_key_ext,
                                                                        UInt32 aid,
                                                                        out byte settings,
                                                                        out byte max_key_no,
                                                                        out UInt16 card_status,
                                                                        out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireChangeKeySettings")]
        public static extern DL_STATUS uFR_int_DesfireChangeKeySettings(byte aes_key_nr,
                                                                        UInt32 aid,
                                                                        byte settings,
                                                                        out UInt16 card_status,
                                                                        out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireChangeKeySettings_PK")]
        public static extern DL_STATUS uFR_int_DesfireChangeKeySettings_PK([In] byte[] aes_key_ext,
                                                                           UInt32 aid,
                                                                           byte settings,
                                                                           out UInt16 card_status,
                                                                           out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireChangeAesKey")]
        public static extern DL_STATUS uFR_int_DesfireChangeAesKey(byte aes_key_nr,
                                                                   UInt32 aid,
                                                                   byte aid_key_nr_auth,
                                                                   [In] byte[] new_aes_key,
                                                                   byte aid_key_no,
                                                                   [In] byte[] old_aes_key,
                                                                   out UInt16 card_status,
                                                                   out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireChangeAesKey_PK")]
        public static extern DL_STATUS uFR_int_DesfireChangeAesKey_PK([In] byte[] aes_key_ext,
                                                                      UInt32 aid,
                                                                      byte aid_key_nr_auth,
                                                                      [In] byte[] new_aes_key,
                                                                      byte aid_key_no,
                                                                      [In] byte[] old_aes_key,
                                                                      out UInt16 card_status,
                                                                      out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireCreateAesApplication")]
        public static extern DL_STATUS uFR_int_DesfireCreateAesApplication(byte aes_key_nr,
                                                                           UInt32 aid_nr,
                                                                           byte setting,
                                                                           byte max_key_no,
                                                                           out UInt16 card_status,
                                                                           out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireCreateAesApplication_PK")]
        public static extern DL_STATUS uFR_int_DesfireCreateAesApplication_PK([In] byte[] aes_key_ext,
                                                                              UInt32 aid_nr,
                                                                              byte setting,
                                                                              byte max_key_no,
                                                                              out UInt16 card_status,
                                                                              out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireCreateAesApplication_no_auth")]
        public static extern DL_STATUS uFR_int_DesfireCreateAesApplication_no_auth(UInt32 aid_nr,
                                                                                   byte setting,
                                                                                   byte max_key_no,
                                                                                   out UInt16 card_status,
                                                                                   out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireDeleteApplication")]
        public static extern DL_STATUS uFR_int_DesfireDeleteApplication(byte aes_key_nr,
                                                                        UInt32 aid_nr,
                                                                        out UInt16 card_status,
                                                                        out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireDeleteApplication_PK")]
        public static extern DL_STATUS uFR_int_DesfireDeleteApplication_PK([In] byte[] aes_key_ext,
                                                                           UInt32 aid_nr,
                                                                           out UInt16 card_status,
                                                                           out UInt16 exec_time);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireCreateStdDataFile")]
        public static extern DL_STATUS uFR_int_DesfireCreateStdDataFile(byte aes_key_nr,
                                                                        UInt32 aid,
                                                                        byte file_id,
                                                                        UInt32 file_size,
                                                                        byte read_key_no,
                                                                        byte write_key_no,
                                                                        byte read_write_key_no,
                                                                        byte change_key_no,
                                                                        byte communication_settings,
                                                                        out UInt16 card_status,
                                                                        out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireCreateStdDataFile_PK")]
        public static extern DL_STATUS uFR_int_DesfireCreateStdDataFile_PK([In] byte[] aes_key_ext,
                                                                           UInt32 aid,
                                                                           byte file_id,
                                                                           UInt32 file_size,
                                                                           byte read_key_no,
                                                                           byte write_key_no,
                                                                           byte read_write_key_no,
                                                                           byte change_key_no,
                                                                           byte communication_settings,
                                                                           out UInt16 card_status,
                                                                           out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireCreateStdDataFile_no_auth")]
        public static extern DL_STATUS uFR_int_DesfireCreateStdDataFile_no_auth(UInt32 aid,
                                                                                byte file_id,
                                                                                UInt32 file_size,
                                                                                byte read_key_no,
                                                                                byte write_key_no,
                                                                                byte read_write_key_no,
                                                                                byte change_key_no,
                                                                                byte communication_settings,
                                                                                out UInt16 card_status,
                                                                                out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireDeleteFile")]
        public static extern DL_STATUS uFR_int_DesfireDeleteFile(byte aes_key_nr,
                                                                 UInt32 aid,
                                                                 byte file_id,
                                                                 out UInt16 card_status,
                                                                 out UInt16 exec_time);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireDeleteFile_PK")]
        public static extern DL_STATUS uFR_int_DesfireDeleteFile_PK([In] byte[] aes_key_ext,
                                                                    UInt32 aid,
                                                                    byte file_id,
                                                                    out UInt16 card_status,
                                                                    out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireDeleteFile_no_auth")]
        public static extern DL_STATUS uFR_int_DesfireDeleteFile_no_auth(UInt32 aid,
                                                                         byte file_id,
                                                                         out UInt16 card_status,
                                                                         out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireReadStdDataFile")]
        public static extern DL_STATUS uFR_int_DesfireReadStdDataFile(byte aes_key_nr,
                                                                      UInt32 aid,
                                                                      byte aid_key_nr,
                                                                      byte file_id,
                                                                      UInt16 offset,
                                                                      UInt16 data_length,
                                                                      byte communication_settings,
                                                                      [Out] byte[] data,
                                                                      out UInt16 card_status,
                                                                      out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireReadStdDataFile_PK")]
        public static extern DL_STATUS uFR_int_DesfireReadStdDataFile_PK([In] byte[] aes_key_ext,
                                                                         UInt32 aid,
                                                                         byte aid_key_nr,
                                                                         byte file_id,
                                                                         UInt16 offset,
                                                                         UInt16 data_length,
                                                                         byte communication_settings,
                                                                         [Out] byte[] data,
                                                                         out UInt16 card_status,
                                                                         out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireReadStdDataFile_no_auth")]
        public static extern DL_STATUS uFR_int_DesfireReadStdDataFile_no_auth(UInt32 aid,
                                                                              byte aid_key_nr,
                                                                              byte file_id,
                                                                              UInt16 offset,
                                                                              UInt16 data_length,
                                                                              byte communication_settings,
                                                                              [Out] byte[] data,
                                                                              out UInt16 card_status,
                                                                              out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireWriteStdDataFile")]
        public static extern DL_STATUS uFR_int_DesfireWriteStdDataFile(byte aes_key_nr,
                                                                       UInt32 aid,
                                                                       byte aid_key_nr,
                                                                       byte file_id,
                                                                       UInt16 offset,
                                                                       UInt16 data_length,
                                                                       byte communication_settings,
                                                                       [In] byte[] data,
                                                                       out UInt16 card_status,
                                                                       out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireWriteStdDataFile_PK")]
        public static extern DL_STATUS uFR_int_DesfireWriteStdDataFile_PK([In] byte[] aes_key_ext,
                                                                          UInt32 aid,
                                                                          byte aid_key_nr,
                                                                          byte file_id,
                                                                          UInt16 offset,
                                                                          UInt16 data_length,
                                                                          byte communication_settings,
                                                                          [In] byte[] data,
                                                                          out UInt16 card_status,
                                                                          out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireWriteStdDataFile_no_auth")]
        public static extern DL_STATUS uFR_int_DesfireWriteStdDataFile_no_auth(UInt32 aid,
                                                                          byte aid_key_nr,
                                                                          byte file_id,
                                                                          UInt16 offset,
                                                                          UInt16 data_length,
                                                                          byte communication_settings,
                                                                          [In] byte[] data,
                                                                          out UInt16 card_status,
                                                                          out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "DES_to_AES_key_type")]
        public static extern DL_STATUS DES_to_AES_key_type();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "AES_to_DES_key_type")]
        public static extern DL_STATUS AES_to_DES_key_type();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "SetSpeedPermanently")]
        public static extern DL_STATUS SetSpeedPermanently(byte tx_speed, byte rx_speed);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetSpeedParameters")]
        public static extern DL_STATUS GetSpeedParameters(out byte tx_speed, out byte rx_speed);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireCreateValueFile")]
        public static extern DL_STATUS uFR_int_DesfireCreateValueFile(byte aes_key_nr,
                                                                      UInt32 aid,
                                                                      byte file_id,
                                                                      Int32 lower_limit,
                                                                      Int32 upper_limit,
                                                                      Int32 value,
                                                                      byte limited_credit_enabled,
                                                                      byte read_key_no,
                                                                      byte write_key_no,
                                                                      byte read_write_key_no,
                                                                      byte change_key_no,
                                                                      byte communication_settings,
                                                                      out UInt16 card_status,
                                                                      out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireCreateValueFile_PK")]
        public static extern DL_STATUS uFR_int_DesfireCreateValueFile_PK([In] byte[] aes_key_ext,
                                                                         UInt32 aid,
                                                                         byte file_id,
                                                                         Int32 lower_limit,
                                                                         Int32 upper_limit,
                                                                         Int32 value,
                                                                         byte limited_credit_enabled,
                                                                         byte read_key_no,
                                                                         byte write_key_no,
                                                                         byte read_write_key_no,
                                                                         byte change_key_no,
                                                                         byte communication_settings,
                                                                         out UInt16 card_status,
                                                                         out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireCreateValueFile_no_auth")]
        public static extern DL_STATUS uFR_int_DesfireCreateValueFile_no_auth(UInt32 aid,
                                                                              byte file_id,
                                                                              Int32 lower_limit,
                                                                              Int32 upper_limit,
                                                                              Int32 value,
                                                                              byte limited_credit_enabled,
                                                                              byte read_key_no,
                                                                              byte write_key_no,
                                                                              byte read_write_key_no,
                                                                              byte change_key_no,
                                                                              byte communication_settings,
                                                                              out UInt16 card_status,
                                                                              out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireReadValueFile")]
        public static extern DL_STATUS uFR_int_DesfireReadValueFile(byte aes_key_nr,
                                                                    UInt32 aid,
                                                                    byte aid_key_nr,
                                                                    byte file_id,
                                                                    byte communication_settings,
                                                                    out Int32 value,
                                                                    out UInt16 card_status,
                                                                    out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireReadValueFile_PK")]
        public static extern DL_STATUS uFR_int_DesfireReadValueFile_PK([In] byte[] aes_key_ext,
                                                                       UInt32 aid,
                                                                       byte aid_key_nr,
                                                                       byte file_id,
                                                                       byte communication_settings,
                                                                       out Int32 value,
                                                                       out UInt16 card_status,
                                                                       out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireReadValueFile_no_auth")]
        public static extern DL_STATUS uFR_int_DesfireReadValueFile_no_auth(UInt32 aid,
                                                                            byte aid_key_nr,
                                                                            byte file_id,
                                                                            byte communication_settings,
                                                                            out Int32 value,
                                                                            out UInt16 card_status,
                                                                            out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireIncreaseValueFile")]
        public static extern DL_STATUS uFR_int_DesfireIncreaseValueFile(byte aes_key_nr,
                                                                        UInt32 aid,
                                                                        byte aid_key_nr,
                                                                        byte file_id,
                                                                        byte communication_settings,
                                                                        Int32 value,
                                                                        out UInt16 card_status,
                                                                        out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireIncreaseValueFile_PK")]
        public static extern DL_STATUS uFR_int_DesfireIncreaseValueFile_PK([In] byte[] aes_key_ext,
                                                                           UInt32 aid,
                                                                           byte aid_key_nr,
                                                                           byte file_id,
                                                                           byte communication_settings,
                                                                           Int32 value,
                                                                           out UInt16 card_status,
                                                                           out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireIncreaseValueFile_no_auth")]
        public static extern DL_STATUS uFR_int_DesfireIncreaseValueFile_no_auth(UInt32 aid,
                                                                                byte aid_key_nr,
                                                                                byte file_id,
                                                                                byte communication_settings,
                                                                                Int32 value,
                                                                                out UInt16 card_status,
                                                                                out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireDecreaseValueFile")]
        public static extern DL_STATUS uFR_int_DesfireDecreaseValueFile(byte aes_key_nr,
                                                                        UInt32 aid,
                                                                        byte aid_key_nr,
                                                                        byte file_id,
                                                                        byte communication_settings,
                                                                        Int32 value,
                                                                        out UInt16 card_status,
                                                                        out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireDecreaseValueFile_PK")]
        public static extern DL_STATUS uFR_int_DesfireDecreaseValueFile_PK([In] byte[] aes_key_ext,
                                                                           UInt32 aid,
                                                                           byte aid_key_nr,
                                                                           byte file_id,
                                                                           byte communication_settings,
                                                                           Int32 value,
                                                                           out UInt16 card_status,
                                                                           out UInt16 exec_time);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireDecreaseValueFile_no_auth")]
        public static extern DL_STATUS uFR_int_DesfireDecreaseValueFile_no_auth(UInt32 aid,
                                                                                byte aid_key_nr,
                                                                                byte file_id,
                                                                                byte communication_settings,
                                                                                Int32 value,
                                                                                out UInt16 card_status,
                                                                                out UInt16 exec_time);

        //---------------------------------------------------------------------                                                     

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderKeysLock")]
        public static extern DL_STATUS ReaderKeysLock([In] char[] password);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderKeysUnlock")]
        public static extern DL_STATUS ReaderKeysUnlock([In] char[] password);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderDescription")]
        private static extern IntPtr GetReaderDescription();
        public static string GetDescription()
        {
            IntPtr str_ret = GetReaderDescription();
            return Marshal.PtrToStringAnsi(str_ret);
        }

        //-------------------------------------------- MIFARE PLUS -------------------------------------------------//


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_PersonalizationMinimal")]
        public static extern DL_STATUS MFP_PersonalizationMinimal([In] byte[] master_key, [In] byte[] config_key, [In] byte[] l2_sw_key, [In] byte[] l3_sw_key,
                                                                  [In] byte[] l1_auth_key, [In] byte[] sel_vc_key, [In] byte[] prox_chk_key, [In] byte[] vc_poll_enc_key,
                                                                  [In] byte[] vc_poll_mac_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_AesAuthSecurityLevel1_PK")]
        public static extern DL_STATUS MFP_AesAuthSecurityLevel1_PK([In] byte[] sl1_auth_key);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_AesAuthSecurityLevel1")]
        public static extern DL_STATUS MFP_AesAuthSecurityLevel1(byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_SwitchToSecurityLevel3_PK")]
        public static extern DL_STATUS MFP_SwitchToSecurityLevel3_PK([In] byte[] sl3_auth_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_SwitchToSecurityLevel3")]
        public static extern DL_STATUS MFP_SwitchToSecurityLevel3(byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeMasterKey_PK")]
        public static extern DL_STATUS MFP_ChangeMasterKey_PK([In] byte[] old_master_key, [In] byte[] new_master_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeMasterKey")]
        public static extern DL_STATUS MFP_ChangeMasterKey(byte key_index, [In] byte[] new_master_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeConfigurationKey_PK")]
        public static extern DL_STATUS MFP_ChangeConfigurationKey_PK([In] byte[] old_config_key, [In] byte[] new_config_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeConfigurationKey")]
        public static extern DL_STATUS MFP_ChangeConfigurationKey(byte key_index, [In] byte[] new_config_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeSectorKey_PK")]
        public static extern DL_STATUS MFP_ChangeSectorKey_PK(byte sector_nr, byte auth_mode, [In] byte[] old_sector_key, [In] byte[] new_sector_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeSectorKey")]
        public static extern DL_STATUS MFP_ChangeSectorKey(byte sector_nr, byte auth_mode, byte key_index, [In] byte[] new_sector_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_FieldConfigurationSet_PK")]
        public static extern DL_STATUS MFP_FieldConfigurationSet_PK([In] byte[] configuration_key, byte rid_use, byte prox_check_use);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_FieldConfigurationSet")]
        public static extern DL_STATUS MFP_FieldConfigurationSet(byte configuration_key_index, byte rid_use, byte prox_check_use);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_GetUid_PK")]
        public static extern DL_STATUS MFP_GetUid_PK([In] byte[] vc_poll_enc_key, [In] byte[] vc_poll_mac_key, [In, Out] byte[] nfc_uid, out byte uid_len);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_GetUid")]
        public static extern DL_STATUS MFP_GetUid(byte key_index_vc_poll_enc_key, byte key_index_vc_poll_mac_key, [In, Out] byte[] nfc_uid, out byte uid_len);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeVcPollingEncKey_PK")]
        public static extern DL_STATUS MFP_ChangeVcPollingEncKey_PK([In] byte[] configuration_key, [In] byte[] new_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeVcPollingEncKey")]
        public static extern DL_STATUS MFP_ChangeVcPollingEncKey(byte configuration_key_index, [In] byte[] new_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeVcPollingMacKey_PK")]
        public static extern DL_STATUS MFP_ChangeVcPollingMacKey_PK([In] byte[] configuration_key, [In] byte[] new_key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFP_ChangeVcPollingMacKey")]
        public static extern DL_STATUS MFP_ChangeVcPollingMacKey(byte configuration_key_index, [In] byte[] new_key);
        

    }
    
}
