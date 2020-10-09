using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uFR;

namespace ufr_mfp_examples_c_sharp_console
{
    using DL_STATUS = UInt32;

    class Functions
    {
        static DL_STATUS status;

        public static void usage()
        {

            Console.Write(" +------------------------------------------------+\n");
            Console.Write(" |         uFR_MFP_console_example                |\n");
            Console.Write(" |              MIFARE PLUS                       |\n");
            Console.Write(" |              version 1.0                       |\n");
            Console.Write(" +------------------------------------------------+\n");
            Console.Write("                              For exit, hit escape.\n");
            Console.Write(" --------------------------------------------------\n");

            Console.Write("  (1) - Card personalization\n");
            Console.Write("  (2) - AES authentication on SL1\n");
            Console.Write("  (3) - Switch to SL3\n");
            Console.Write("  (4) - Change master key\n");
            Console.Write("  (5) - Change configuration key\n");
            Console.Write("  (6) - Change sector keys\n");
            Console.Write("  (7) - Field configuration set\n");
            Console.Write("  (8) - Get card UID\n");
            Console.Write("  (9) - Change VC polling ENC key\n");
            Console.Write("  (a) - Change VC polling MAC key\n");
            Console.Write("  (b) - Data read\n");
            Console.Write("  (c) - Data write\n");
            Console.Write("  (d) - Write keys into reader\n");

            Console.Write(" --------------------------------------------------\n");
        }

        public static void menu(char key)
        {
            switch (key)
            {
                case '1':
                    Card_Personalization();

                    break;
                case '2':
                    AES_authenticate_SL1();
                    break;
                case '3':
                    Switch_to_SL3();
                    break;
                case '4':
                    Change_Master_Key();
                    break;
                case '5':
                    Change_configuration_key();
                    break;
                case '6':
                    Change_Sector_Key();
                    break;
                case '7':
                    Field_configuration_set();
                    break;
                case '8':
                    GetUID();
                    break;
                case '9':
                    Change_VC_polling_ENC_key();
                    break;
                case 'a':
                    Change_VC_polling_MAC_key();
                    break;
                case 'b':
                    Data_read();
                    break;
                case 'c':
                    Data_write();
                    break;
                case 'd':
                    Write_keys();
                    break;

                default:
                    usage();
                    break;
            }
            Console.Write(" --------------------------------------------------\n");
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static void reader_open()
        {
            char mode;

            Console.Write("Select reader opening mode:\n");
            Console.Write(" (1) - Simple Reader Open\n");
            Console.Write(" (2) - Advanced Reader Open\n");
            
            mode = Console.ReadKey(true).KeyChar;

            if (mode == '1') {
                status = (UInt32)uFCoder.ReaderOpen();
            } else if (mode == '2') {
                /* For opening uFR Nano Online UDP mode use:
                # status = ReaderOpenEx(0, "ip_address:port_number", 85, 0)
                #
                # For opening uFR Nano Online TCP/IP mode use:
                # status = ReaderOpenEx(0, "ip address:port_number", 84, 0)
                #
                # For opening uFR Nano Online without reset/RTS on ESP32 - transparent mode 115200 use:
                # status = ReaderOpenEx(2, 0, 0, "UNIT_OPEN_RESET_DISABLE")
                */

                string reader_type = "", port_name = "", port_interface = "", arg = "";

                Console.Write("Enter reader type:\n");
                reader_type = Console.ReadLine();
                Console.Write("Enter port name:\n");
                port_name = Console.ReadLine();
                Console.Write("Enter port interface:\n");
                port_interface = Console.ReadLine();
                Console.Write("Enter additional argument:\n");
                arg = Console.ReadLine();

                UInt32 reader_type_int = Convert.ToUInt32(reader_type);
                UInt32 port_interface_int = 0;
                if (port_interface == "U")
                {
                    port_interface_int = 85; //for passing interface as int to function
                } else if (port_interface == "T")
                {
                    port_interface_int = 84; //for passing interface as int to function
                }
                else { 
                    port_interface_int = Convert.ToUInt32(port_interface);
                }

                status = (UInt32)uFCoder.ReaderOpenEx(reader_type_int, port_name, port_interface_int, arg);
            }
            else
            {
                Console.Write(" Wrong input. Press any key to quit the application...");
                Console.ReadKey();
                Environment.Exit(0);
            }
            if (status == 0)
            {
                Console.Write(" --------------------------------------------------\n");
                Console.Write("        uFR NFC reader successfully opened.\n");
                Console.Write(" --------------------------------------------------\n");
            }
            else
            {
                Console.WriteLine(" Error while opening device, status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                Console.Write("\nPress any key to quit the application...\n");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void Card_Personalization()
        {
            status = 0;
            byte[] master_key = new byte[16];
            byte[] config_key = new byte[16];
            byte[] l2_sw_key = new byte[16];
            byte[] l3_sw_key = new byte[16];
            byte[] l1_auth_key = new byte[16];
            byte[] sel_vc_key = new byte[16];
            byte[] prox_chk_key = new byte[16];
            byte[] vc_poll_enc_key = new byte[16];
            byte[] vc_poll_mac_key = new byte[16];
            byte dl_card_type = 0;
            string key_input = "";


            Console.Write(" -------------------------------------------------------------------\n");
            Console.Write("                     CARD PERSONALIZATON                            \n");
            Console.Write("              MIFARE PLUS CARD MUST BE IN SL0 MODE					\n");
            Console.Write("      ALL AES SECTOR KEYS WILL BE FACTORY DEFAULT 16 x 0xFF         \n");
            Console.Write(" -------------------------------------------------------------------\n");

            status = (UInt32)uFCoder.GetDlogicCardType(out dl_card_type);

            if (status > 0)
            {
                Console.Write("\nCommunication with card failed \n");
                return;
            }

            //check if card type S in SL0

            if (!((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL0 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_X_4K_SL0))
            {
                Console.Write("\nCard is not in security level 0 mode.\n");
                return;
            }

            Console.Write("\nEnter card master key:\n");
            key_input = Console.ReadLine();

            if (key_input.Length != 32)
            {
                Console.Write("Key entered must be 16 bytes long!\n");
                return;
            }
            master_key = StringToByteArray(key_input);

            //----------------------------------------------

            Console.Write("\nEnter card configuration key:\n");
            key_input = Console.ReadLine();

            if (key_input.Length != 32)
            {
                Console.Write("Key entered must be 16 bytes long!\n");
                return;
            }
            config_key = StringToByteArray(key_input);

            //----------------------------------------------

            Console.Write("\nEnter level 2 switch key:\n");
            key_input = Console.ReadLine();

            if (key_input.Length != 32)
            {
                Console.Write("Key entered must be 16 bytes long!\n");
                return;
            }
            l2_sw_key = StringToByteArray(key_input);

            //----------------------------------------------

            Console.Write("\nEnter level 3 switch key:\n");
            key_input = Console.ReadLine();

            if (key_input.Length != 32)
            {
                Console.Write("Key entered must be 16 bytes long!\n");
                return;
            }
            l3_sw_key = StringToByteArray(key_input);

            //----------------------------------------------

            Console.Write("\nEnter SL1 card authentication key:\n");
            key_input = Console.ReadLine();

            if (key_input.Length != 32)
            {
                Console.Write("Key entered must be 16 bytes long!\n");
                return;
            }
            l1_auth_key = StringToByteArray(key_input);

            //----------------------------------------------

            if (!((DLOGIC_CARD_TYPE)dl_card_type == DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_X_2K_SL0 || (DLOGIC_CARD_TYPE)dl_card_type == DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_X_4K_SL0))
            {
                //----------------------------------------------

                Console.Write("\nEnter select VC key:\n");
                key_input = Console.ReadLine();

                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }
                sel_vc_key = StringToByteArray(key_input);

                //----------------------------------------------

                Console.Write("\nEnter proximity check key:\n");
                key_input = Console.ReadLine();

                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }
                prox_chk_key = StringToByteArray(key_input);


            }

            //----------------------------------------------

            Console.Write("\nEnter VC polling ENC key:\n");
            key_input = Console.ReadLine();

            if (key_input.Length != 32)
            {
                Console.Write("Key entered must be 16 bytes long!\n");
                return;
            }
            vc_poll_enc_key = StringToByteArray(key_input);

            //----------------------------------------------

            Console.Write("\nEnter VC polling MAC key:\n");
            key_input = Console.ReadLine();

            if (key_input.Length != 32)
            {
                Console.Write("Key entered must be 16 bytes long!\n");
                return;
            }
            vc_poll_mac_key = StringToByteArray(key_input);

            //----------------------------------------------

            status = (UInt32)uFCoder.MFP_PersonalizationMinimal(master_key, config_key, l2_sw_key, l3_sw_key, l1_auth_key,
                                                sel_vc_key, prox_chk_key, vc_poll_enc_key, vc_poll_mac_key);

            if (status != 0)
            {
                Console.Write("\nCard personalization failed\n");
                Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

            }
            else
            {
                Console.Write("\nCard personalization successful\n");
                Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
            }

        }

        //-----------------------------------------------------------------------------------------//

        public static void AES_authenticate_SL1()
        {
            status = 0;

            byte[] sl1_auth_key = new byte[16];
            byte key_index = 0;
            int choice = 0;
            string key_input = "";

            Console.Write(" -------------------------------------------------------------------\n");
            Console.Write("                       AES AUTHENTICATION ON SL1                    	\n");
            Console.Write(" 		       MIFARE PLUS CARD MUST BE IN SL1 MODE	    			\n");
            Console.Write(" -------------------------------------------------------------------\n");

            Console.WriteLine(" (1) - Provided AES key");
            Console.WriteLine(" (2) - Reader AES key");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                //PROVIDED
                Console.Write("\nEnter SL1 card authentication key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                sl1_auth_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_AesAuthSecurityLevel1_PK(sl1_auth_key);

                if (status != 0)
                {
                    Console.Write("\nnSL1 AES authentication failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.Write("\nnSL1 AES authentication successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }

            }
            else if (choice == '2')
            {
                //INTERNAL
                Console.Write("\nEnter reader key index for SL1 card authentication key (0 - 15):\n");
                key_input = Console.ReadLine();
                key_index = Byte.Parse(key_input);

                if (key_index < 0 && key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                status = (UInt32)uFCoder.MFP_AesAuthSecurityLevel1(key_index);
                if (status != 0)
                {
                    Console.Write("\nSL1 AES authentication failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.Write("\nSL1 AES authentication successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
            }
            else
            {
                Console.WriteLine("\nWrong choice");
            }

        }

        //-----------------------------------------------------------------------------------------//

        public static void Switch_to_SL3()
        {
            status = 0;
            int choice = 0;
            byte[] sl3_sw_key = new byte[16];
            byte sl3_sw_key_index = 0;
            string key_input = "";

            Console.Write(" -------------------------------------------------------------------\n");
            Console.Write("                           SWITCH TO SL3                            \n");
            Console.Write(" 		       MIFARE PLUS CARD MUST BE IN SL1 MODE	    			\n");
            Console.Write(" -------------------------------------------------------------------\n");

            Console.WriteLine(" (1) - Provided AES key");
            Console.WriteLine(" (2) - Reader AES key");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                //PROVIDED
                Console.Write("\nEnter level 3 switch key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                sl3_sw_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_SwitchToSecurityLevel3_PK(sl3_sw_key);

                if (status != 0)
                {
                    Console.WriteLine("\nSwitch to security level 3 failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.WriteLine("\nSwitch to security level 3 successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }

            }
            else if (choice == '2')
            {
                //INTERNAL
                Console.Write("\nEnter reader key index for level 3 switch key (0 - 15):\n");
                key_input = Console.ReadLine();
                sl3_sw_key_index = Byte.Parse(key_input);
                if (sl3_sw_key_index < 0 && sl3_sw_key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                status = (UInt32)uFCoder.MFP_SwitchToSecurityLevel3(sl3_sw_key_index);
                if (status != 0)
                {
                    Console.Write("\nSwitch to security level 3 failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.Write("\nSwitch to security level 3 successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }

            }
            else
            {
                Console.WriteLine("\nWrong choice");
            }

        }

        //-----------------------------------------------------------------------------------------//

        public static void Change_Master_Key()
        {
            Console.Write(" -------------------------------------------------------------------\n");
            Console.Write("                      CHANGE CARD MASTER KEY                        \n");
            Console.Write(" 		       MIFARE PLUS CARD MUST BE IN SL3 MODE	    			\n");
            Console.Write(" -------------------------------------------------------------------\n");

            status = 0;

            byte[] old_master_key = new byte[16];
            byte[] new_master_key = new byte[16];
            byte old_key_index = 0;
            int choice = 0;
            string key_input = "";

            Console.WriteLine(" (1) - Provided AES key");
            Console.WriteLine(" (2) - Reader AES key");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                //PROVIDED
                Console.Write("\nEnter old card master key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                old_master_key = StringToByteArray(key_input);

                Console.Write("\nEnter new card master key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_master_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_ChangeMasterKey_PK(old_master_key, new_master_key);
                if (status != 0)
                {
                    Console.Write("\nMaster key change has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.Write("\nMaster key change successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
            }
            else if (choice == '2')
            {
                //INTERNAL
                Console.Write("\nEnter reader key index for old card master key (0 - 15):\n");
                key_input = Console.ReadLine();
                old_key_index = Byte.Parse(key_input);
                if (old_key_index < 0 && old_key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                Console.Write("\nEnter new master card key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_master_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_ChangeMasterKey(old_key_index, new_master_key);
                if (status != 0)
                {
                    Console.Write("\nMaster key change has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.Write("\nMaster key change successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
            }
            else
            {
                Console.WriteLine("\nWrong choice");
                return;
            }
        }

        //-----------------------------------------------------------------------------------------//

        public static void Change_configuration_key()
        {
            Console.Write(" -------------------------------------------------------------------\n");
            Console.Write("                    CHANGE CARD CONFIGURATION KEY                   \n");
            Console.Write(" 		       MIFARE PLUS CARD MUST BE IN SL3 MODE	    			\n");
            Console.Write(" -------------------------------------------------------------------\n");

            status = 0;

            byte[] old_config_key = new byte[16];
            byte[] new_config_key = new byte[16];
            byte old_key_index = 0;
            int choice = 0;
            string key_input = "";

            Console.WriteLine(" (1) - Provided AES key");
            Console.WriteLine(" (2) - Reader AES key");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                //PROVIDED
                Console.Write("\nEnter old card configuration key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                old_config_key = StringToByteArray(key_input);

                Console.Write("\nEnter new configuration card key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_config_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_ChangeConfigurationKey_PK(old_config_key, new_config_key);
                if (status != 0)
                {
                    Console.Write("\nConfiguration key change has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.Write("\nConfiguration key change successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
            }
            else if (choice == '2')
            {
                //INTERNAL
                Console.Write("\nEnter reader key index for old card master key (0 - 15):\n");
                key_input = Console.ReadLine();
                old_key_index = Byte.Parse(key_input);
                if (old_key_index < 0 && old_key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                Console.Write("\nEnter new master card key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_config_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_ChangeConfigurationKey(old_key_index, new_config_key);
                if (status != 0)
                {
                    Console.Write("\nConfiguration key change has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.Write("\nConfiguration key change successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
            }
            else
            {
                Console.WriteLine("\nWrong choice");
                return;
            }

        }

        //-----------------------------------------------------------------------------------------//

        public static void Change_Sector_Key()
        {
            status = 0;
            int choice = 0;
            byte[] old_sector_key = new byte[16];
            byte[] new_sector_key = new byte[16];
            byte old_key_index = 0, sector_nr = 0, auth_mode = 0;
            string input = "", key_input = "";

            Console.Write(" -------------------------------------------------------------------\n");
            Console.Write("                       CHANGE SECTOR AES KEY                        \n");
            Console.Write(" 		       MIFARE PLUS CARD MUST BE IN SL3 MODE	    			\n");
            Console.Write(" -------------------------------------------------------------------\n");

            Console.WriteLine(" (1) - Provided AES key");
            Console.WriteLine(" (2) - Reader AES key");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                //PROVIDED
                Console.WriteLine("\nEnter sector number (0 - 31 for 2K card) (0 - 39 for 4K card):");
                input = Console.ReadLine();
                sector_nr = Byte.Parse(input);
                if (sector_nr < 0 && sector_nr > 39)
                {
                    Console.Write("Sector number must be between 0 and 39!\n");
                    return;
                }

                Console.WriteLine("\nEnter code for authentication mode:");
                Console.WriteLine(" (1) - AES key A");
                Console.WriteLine(" (2) - AES key B");

                input = Console.ReadLine();
                auth_mode = Byte.Parse(input);
                if (auth_mode == 1)
                {
                    auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1A;
                }
                else if (auth_mode == 2)
                {
                    auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1B;
                }

                Console.Write("\nEnter old AES sector key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                old_sector_key = StringToByteArray(key_input);

                Console.Write("\nEnter new AES sector key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_sector_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_ChangeSectorKey_PK(sector_nr, auth_mode, old_sector_key, new_sector_key);
                if (status != 0)
                {
                    Console.Write("\nAES sector key change has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.Write("\nAES sector key change successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
            }
            else if (choice == '2')
            {
                //INTERNAL
                Console.WriteLine("\nEnter sector number (0 - 31 for 2K card) (0 - 39 for 4K card):");
                input = Console.ReadLine();
                sector_nr = Byte.Parse(input);
                if (sector_nr < 0 && sector_nr > 39)
                {
                    Console.Write("Sector number must be between 0 and 39!\n");
                    return;
                }

                Console.WriteLine("\nEnter code for authentication mode:");
                Console.WriteLine(" (1) - AES key A");
                Console.WriteLine(" (2) - AES key B");

                input = Console.ReadLine();
                auth_mode = Byte.Parse(input);
                if (auth_mode == 1)
                {
                    auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1A;
                }
                else if (auth_mode == 2)
                {
                    auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1B;
                }

                Console.WriteLine("\nEnter reader key index for old AES sector key (0 - 15)");
                input = Console.ReadLine();
                old_key_index = Byte.Parse(input);
                if (old_key_index < 0 && old_key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                Console.Write("\nEnter new AES sector key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_sector_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_ChangeSectorKey(sector_nr, auth_mode, old_key_index, new_sector_key);
                if (status != 0)
                {
                    Console.Write("\nAES sector key change has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.Write("\nAES sector key change successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
            }
            else
            {
                Console.WriteLine("\nWrong choice");
                return;
            }
        }

        //-----------------------------------------------------------------------------------------//

        public static void Field_configuration_set()
        {
            Console.Write(" -------------------------------------------------------------------\n");
            Console.Write("                       FIELD CONFIGURATION SETTING                  \n");
            Console.Write(" 		       MIFARE PLUS CARD MUST BE IN SL3 MODE	    			\n");
            Console.Write(" -------------------------------------------------------------------\n");


            status = 0;

            byte[] config_key = new byte[16];
            byte key_index = 0, rid_use = 0, prox_check_use = 0; //Proximity check for X and EV1 card is not implemented yet
            string key_input = "";
            int choice = 0;
            Console.WriteLine(" (1) - Provided AES key");
            Console.WriteLine(" (2) - Reader AES key");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                //PROVIDED
                Console.WriteLine("\nEnter random ID option");
                Console.WriteLine(" (1) - use random ID");
                Console.WriteLine(" (2) - use UID");

                choice = Console.ReadKey().KeyChar;
                if (choice == '1')
                {
                    rid_use = 1;
                }
                else if (choice == '2')
                {
                    rid_use = 0;
                }
                else
                {
                    Console.WriteLine("\nWrong choice");
                }

                Console.WriteLine("\nEnter configuration key:");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                config_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_FieldConfigurationSet_PK(config_key, rid_use, prox_check_use);
                if (status != 0)
                {
                    Console.Write("\nField configuration block change has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.Write("\nField configuration block change was successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }

            }
            else if (choice == '2')
            {
                Console.WriteLine("\nEnter random ID option");
                Console.WriteLine(" (1) - use random ID");
                Console.WriteLine(" (2) - use UID");

                choice = Console.ReadKey().KeyChar;
                if (choice == '1')
                {
                    rid_use = 1;
                }
                else if (choice == '2')
                {
                    rid_use = 0;
                }
                else
                {
                    Console.WriteLine("\nWrong choice");
                }

                Console.WriteLine("\nEnter reader key index for old configuration key (0 - 15)");
                key_input = Console.ReadLine();
                key_index = Byte.Parse(key_input);
                if (key_index < 0 && key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                status = (UInt32)uFCoder.MFP_FieldConfigurationSet(key_index, rid_use, prox_check_use);
                if (status != 0)
                {
                    Console.Write("\nField configuration block change has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.Write("\nField configuration block change was successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
            }
            else
            {
                Console.WriteLine("\nWrong choice");
            }


        }

        //-----------------------------------------------------------------------------------------//    

        public static void GetUID()
        {

            byte[] vc_enc_key = new byte[16];
            byte[] vc_mac_key = new byte[16];
            byte[] uid = new byte[10];

            string key_input = "";
            byte uid_len = 0;
            int choice = 0;
            byte vc_enc_key_index = 0;
            byte vc_mac_key_index = 0;

            Console.WriteLine(" (1) - Provided AES key");
            Console.WriteLine(" (2) - Reader AES key");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                Console.Write("\nEnter VC polling ENC key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                vc_enc_key = StringToByteArray(key_input);

                Console.Write("\nEnter VC polling MAC key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }
                vc_mac_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_GetUid_PK(vc_enc_key, vc_mac_key, uid, out uid_len);
                if (status != 0)
                {
                    Console.Write("\nReading card UID failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.Write("\nCard UID = " + BitConverter.ToString(uid).Replace("-", ":") + "\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
            }
            else if (choice == '2')
            {
                //INTERNAL

                Console.Write("\nEnter reader key index for VC polling ENC key (0 - 15):\n");
                key_input = Console.ReadLine();
                vc_enc_key_index = Byte.Parse(key_input);
                if (vc_enc_key_index < 0 && vc_enc_key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                Console.Write("\nEnter reader key index for VC polling ENC key (0 - 15):\n");
                key_input = Console.ReadLine();
                vc_enc_key_index = Byte.Parse(key_input);
                if (vc_enc_key_index < 0 && vc_enc_key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                status = (UInt32)uFCoder.MFP_GetUid(vc_enc_key_index, vc_mac_key_index, uid, out uid_len);

                if (status != 0)
                {
                    Console.Write("\nReading card UID failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));

                }
                else
                {
                    Console.Write("\nCard UID = " + BitConverter.ToString(uid).Replace("-", ":") + "\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }

            }
            else
            {
                Console.WriteLine("\nWrong choice");
            }

        }

        //-----------------------------------------------------------------------------------------//

        public static void Change_VC_polling_ENC_key()
        {
            status = 0;

            int choice = 0;

            byte[] config_key = new byte[16];
            byte[] new_vc_enc_key = new byte[16];
            byte key_index = 0;
            string key_input = "";


            Console.WriteLine(" (1) - Provided AES key");
            Console.WriteLine(" (2) - Reader AES key");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                //PROVIDED
                Console.Write("\nEnter card configuration key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                config_key = StringToByteArray(key_input);

                Console.Write("\nEnter new VC polling ENC key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_vc_enc_key = StringToByteArray(key_input);


                status = (UInt32)uFCoder.MFP_ChangeVcPollingEncKey_PK(config_key, new_vc_enc_key);
                if (status != 0)
                {
                    Console.Write("\nChange of VC polling ENC key has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.Write("\nChange of VC polling ENC key was successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }

            }
            else if (choice == '2')
            {
                //INTERNAL
                Console.Write("\nEnter reader key index for configuration key (0 - 15):\n");
                key_input = Console.ReadLine();
                key_index = Byte.Parse(key_input);
                if (key_index < 0 && key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                Console.Write("\nEnter new VC polling ENC key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_vc_enc_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_ChangeVcPollingEncKey(key_index, new_vc_enc_key);
                if (status != 0)
                {
                    Console.Write("\nChange of VC polling ENC key has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.Write("\nChange of VC polling ENC key was successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }

            }
            else
            {
                Console.WriteLine("\nWrong choice");
            }
        }
        //-----------------------------------------------------------------------------------------//

        public static void Change_VC_polling_MAC_key()
        {
            status = 0;

            int choice = 0;

            byte[] config_key = new byte[16];
            byte[] new_vc_mac_key = new byte[16];
            byte key_index = 0;
            string key_input = "";

            Console.WriteLine(" (1) - Provided AES key");
            Console.WriteLine(" (2) - Reader AES key");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                //PROVIDED
                Console.Write("\nEnter card configuration key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                config_key = StringToByteArray(key_input);

                Console.Write("\nEnter new VC polling MAC key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_vc_mac_key = StringToByteArray(key_input);


                status = (UInt32)uFCoder.MFP_ChangeVcPollingMacKey_PK(config_key, new_vc_mac_key);
                if (status != 0)
                {
                    Console.Write("\nChange of VC polling MAC key has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.Write("\nChange of VC polling MAC key was successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }

            }
            else if (choice == '2')
            {
                //INTERNAL
                Console.Write("\nEnter reader key index for configuration key (0 - 15):\n");
                key_input = Console.ReadLine();
                key_index = Byte.Parse(key_input);
                if (key_index < 0 && key_index > 15)
                {
                    Console.Write("Key index must be between 0 and 15!\n");
                    return;
                }

                Console.Write("\nEnter new VC polling MAC key:\n");
                key_input = Console.ReadLine();
                if (key_input.Length != 32)
                {
                    Console.Write("Key entered must be 16 bytes long!\n");
                    return;
                }

                new_vc_mac_key = StringToByteArray(key_input);

                status = (UInt32)uFCoder.MFP_ChangeVcPollingMacKey(key_index, new_vc_mac_key);
                if (status != 0)
                {
                    Console.Write("\nChange of VC polling MAC key has failed\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.Write("\nChange of VC polling MAC key was successful\n");
                    Console.WriteLine("Status is: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }

            }
            else
            {
                Console.WriteLine("\nWrong choice");
            }
        }

        //-----------------------------------------------------------------------------------------//

        public static void Data_read()
        {
            Console.Write(" -------------------------------------------------------------------\n");
            Console.Write("                       READ DATA FROM CARD	                        \n");
            Console.Write(" 		 MIFARE PLUS CARD MUST BE IN SL1 OR SL3 MODE    			\n");
            Console.Write(" -------------------------------------------------------------------\n");

            status = 0;
            int choice = 0;
            string key_input = "";
            byte[] crypto_1_sector_key = new byte[6];
            byte[] aes_sector_key = new byte[16];
            byte[] block_data = new byte[16];
            byte[] linear_data = new byte[3440];

            byte block_nr = 0, sector_nr = 0, auth_mode = 0, key_index = 0;
            byte dl_card_type = 0;

            ushort lin_addr = 0, lin_len = 0, ret_bytes = 0;

            status = (UInt32)uFCoder.GetDlogicCardType(out dl_card_type);
            //Console.WriteLine("Card type: " + Enum.GetName(typeof(DLOGIC_CARD_TYPE), dl_card_type));

            if (status != 0)
            {
                Console.Write("\nCommunication with card failed, status: " + uFCoder.status2str((uFR.DL_STATUS)status));
                return;
            }


            if (!(((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL1) ||
              ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_4K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL1) ||
              ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL3 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL3) ||
              ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL3 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL3)))
            {
                Console.WriteLine("\nCard is not in security level 1 or 3 mode");
                return;
            }


            Console.WriteLine(" (1) - Block read");
            Console.WriteLine(" (2) - Block in sector read");
            Console.WriteLine(" (3) - Linear read");
            choice = Console.ReadKey().KeyChar;


            if (choice == '1')
            {
                Console.WriteLine("\nBlock read selected");
                //if card is using CRYPTO1 key(s)
                if (((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL1)
                    || ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_4K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL1))
                {
                    Console.Write(" (1) - Provided CRYPTO 1 key\n");
                    Console.Write(" (2) - Reader CRYPTO 1 key\n");
                    Console.Write(" (3) - AKM1 CRYPTO 1 key\n");
                    Console.Write(" (4) - AKM2 CRYPTO 1 key\n");

                    choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        key_input = Console.ReadLine();
                        if (key_input.Length != 12)
                        {
                            Console.WriteLine("CRYPTO1 Key must be 6 bytes long");
                            return;
                        }
                        crypto_1_sector_key = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockRead_PK(block_data, block_nr, auth_mode, crypto_1_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else if (choice == '2')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A index");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B index");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        key_input = Console.ReadLine();

                        key_index = Byte.Parse(key_input);

                        status = (UInt32)uFCoder.BlockRead(block_data, block_nr, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else if (choice == '3')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        status = (UInt32)uFCoder.BlockRead_AKM1(block_data, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else if (choice == '4')
                    {

                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        status = (UInt32)uFCoder.BlockRead_AKM2(block_data, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine(" (1) - Provided AES key");
                    Console.WriteLine(" (2) - Reader AES key");
                    Console.WriteLine(" (3) - AKM1 AES key");
                    Console.WriteLine(" (4) - AKM2 AES key");

                    choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        Console.WriteLine("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1A;
                            Console.WriteLine("Enter AES key A:");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1B;
                            Console.WriteLine("Enter AES key B:");

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }
                        key_input = Console.ReadLine();
                        if (key_input.Length != 32)
                        {
                            Console.WriteLine("AES key must be 16 bytes long! ");
                            return;
                        }
                        aes_sector_key = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockRead_PK(block_data, block_nr, auth_mode, aes_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else if (choice == '2')
                    {
                        Console.WriteLine("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("Enter AES key A index:");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("Enter AES key B index:");

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        key_input = Console.ReadLine();
                        key_index = Byte.Parse(key_input);

                        status = (UInt32)uFCoder.BlockRead(block_data, block_nr, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else if (choice == '3')
                    {
                        Console.WriteLine("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;

                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        status = (UInt32)uFCoder.BlockRead_AKM1(block_data, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else if (choice == '4')
                    {

                        Console.WriteLine("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;

                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        status = (UInt32)uFCoder.BlockRead_AKM2(block_data, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }


                }
            }
            else if (choice == '2')
            {
                Console.WriteLine("\nBlock in sector read selected");

                if (((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL1)
                    || ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_4K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL1))
                {
                    Console.Write(" (1) - Provided CRYPTO 1 key\n");
                    Console.Write(" (2) - Reader CRYPTO 1 key\n");
                    Console.Write(" (3) - AKM1 CRYPTO 1 key\n");
                    Console.Write(" (4) - AKM2 CRYPTO 1 key\n");

                    choice = Console.ReadKey().KeyChar;
                    if (choice == '1')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B\n");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }
                        key_input = Console.ReadLine();
                        if (key_input.Length != 12)
                        {
                            Console.WriteLine("CRYPTO1 key must be 6 bytes long");
                            return;
                        }
                        crypto_1_sector_key = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockInSectorRead_PK(block_data, sector_nr, block_nr, auth_mode, crypto_1_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else if (choice == '2')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A index (0 - 31)");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B index (0 - 31)\n");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }
                        key_input = Console.ReadLine();

                        key_index = Byte.Parse(key_input);

                        status = (UInt32)uFCoder.BlockInSectorRead(block_data, sector_nr, block_nr, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else if (choice == '3')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;

                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }
                        status = (UInt32)uFCoder.BlockInSectorRead_AKM1(block_data, sector_nr, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else if (choice == '4')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;

                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }
                        status = (UInt32)uFCoder.BlockInSectorRead_AKM2(block_data, sector_nr, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }
                }
                else
                {

                    Console.Write(" (1) - Provided AES key\n");
                    Console.Write(" (2) - Reader AES key\n");
                    Console.Write(" (3) - AKM1 AES key\n");
                    Console.Write(" (4) - AKM2 AES key\n");

                    choice = Console.ReadKey().KeyChar;
                    if (choice == '1')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1A;
                            Console.WriteLine("Enter AES key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1B;
                            Console.WriteLine("Enter AES key B");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();

                        if (key_input.Length != 32)
                        {
                            Console.WriteLine("AES key must be 16 bytes long");
                            return;
                        }

                        aes_sector_key = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockInSectorRead_PK(block_data, sector_nr, block_nr, auth_mode, aes_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else if (choice == '2')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("Enter AES key A index (0 - 15)");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("Enter AES key B index (0 - 15)");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();

                        key_index = Byte.Parse(key_input);

                        status = (UInt32)uFCoder.BlockInSectorRead(block_data, sector_nr, block_nr, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else if (choice == '3')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;

                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        status = (UInt32)uFCoder.BlockInSectorRead_AKM1(block_data, sector_nr, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else if (choice == '4')
                    {

                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;

                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;

                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        status = (UInt32)uFCoder.BlockInSectorRead_AKM2(block_data, sector_nr, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(block_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }
                }
            }
            else if (choice == '3')
            {
                Console.WriteLine("\nLinear read select key mode");

                if (((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL1)
                    || ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_4K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL1))
                {

                    Console.Write(" (1) - Provided CRYPTO 1 key\n");
                    Console.Write(" (2) - Reader CRYPTO 1 key\n");
                    Console.Write(" (3) - AKM1 CRYPTO 1 key\n");
                    Console.Write(" (4) - AKM2 CRYPTO 1 key\n");

                    choice = Console.ReadKey().KeyChar;
                    if (choice == '1')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.WriteLine("\nEnter number of bytes for reading");
                        key_input = Console.ReadLine();
                        lin_len = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();

                        if (key_input.Length != 12)
                        {
                            Console.WriteLine("\n CRYPTO 1 key must be 6 bytes long");
                            return;
                        }

                        crypto_1_sector_key = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.LinearRead_PK(linear_data, lin_addr, lin_len, out ret_bytes, auth_mode, crypto_1_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            byte[] write_data = new byte[ret_bytes];

                            Array.Copy(linear_data, write_data, ret_bytes);

                            Console.WriteLine("Linear read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(write_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else if (choice == '2')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.WriteLine("\nEnter number of bytes for reading");
                        key_input = Console.ReadLine();
                        lin_len = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A index (0 - 31)");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B index (0 - 31)");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();

                        key_index = Byte.Parse(key_input);

                        status = (UInt32)uFCoder.LinearRead(linear_data, lin_addr, lin_len, out ret_bytes, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            byte[] write_data = new byte[ret_bytes];

                            Array.Copy(linear_data, write_data, ret_bytes);

                            Console.WriteLine("Linear read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(write_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else if (choice == '3')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.WriteLine("\nEnter number of bytes for reading");
                        key_input = Console.ReadLine();
                        lin_len = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        status = (UInt32)uFCoder.LinearRead_AKM1(linear_data, lin_addr, lin_len, out ret_bytes, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            byte[] write_data = new byte[ret_bytes];

                            Array.Copy(linear_data, write_data, ret_bytes);

                            Console.WriteLine("Linear read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(write_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else if (choice == '4')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.WriteLine("\nEnter number of bytes for reading");
                        key_input = Console.ReadLine();
                        lin_len = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        status = (UInt32)uFCoder.LinearRead_AKM2(linear_data, lin_addr, lin_len, out ret_bytes, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            byte[] write_data = new byte[ret_bytes];

                            Array.Copy(linear_data, write_data, ret_bytes);

                            Console.WriteLine("Linear read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(write_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }

                }
                else
                {
                    Console.WriteLine(" (1) - Provided AES key");
                    Console.WriteLine(" (2) - Reader AES key");

                    choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.WriteLine("\nEnter number of bytes for reading");
                        key_input = Console.ReadLine();
                        lin_len = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1A;
                            Console.WriteLine("\nEnter AES key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1B;
                            Console.WriteLine("\nEnter AES key B");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();

                        if (key_input.Length != 32)
                        {
                            Console.WriteLine("\n AES key must be 16 bytes long");
                            return;
                        }

                        aes_sector_key = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.LinearRead_PK(linear_data, lin_addr, lin_len, out ret_bytes, auth_mode, aes_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            byte[] write_data = new byte[ret_bytes];

                            Array.Copy(linear_data, write_data, ret_bytes);

                            Console.WriteLine("Linear read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(write_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else if (choice == '2')
                    {

                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.WriteLine("\nEnter number of bytes for reading");
                        key_input = Console.ReadLine();
                        lin_len = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter AES key A index (0 - 16)");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter AES key B index (0 - 15)");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();

                        key_index = Byte.Parse(key_input);

                        status = (UInt32)uFCoder.LinearRead(linear_data, lin_addr, lin_len, out ret_bytes, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear read failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            byte[] write_data = new byte[ret_bytes];

                            Array.Copy(linear_data, write_data, ret_bytes);

                            Console.WriteLine("Linear read successful");
                            Console.WriteLine("Data = " + BitConverter.ToString(write_data).Replace("-", ":"));
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }

                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }

                }
            }
            else
            {
                Console.WriteLine("\nWrong choice");
                return;
            }
        }

        //-----------------------------------------------------------------------------------------//

        public static void Data_write()
        {
            Console.Write(" -------------------------------------------------------------------\n");
            Console.Write("                       WRITE DATA TO CARD	                        \n");
            Console.Write(" 		 MIFARE PLUS CARD MUST BE IN SL1 OR SL3 MODE    			\n");
            Console.Write(" -------------------------------------------------------------------\n");

            status = 0;
            int choice = 0;
            string key_input = "";
            byte[] crypto_1_sector_key = new byte[6];
            byte[] aes_sector_key = new byte[16];
            byte[] block_data = new byte[16];
            byte[] linear_data = new byte[3440];

            byte block_nr = 0, sector_nr = 0, auth_mode = 0, key_index = 0, dl_card_type = 0;
            ushort lin_addr = 0, lin_len = 0, ret_bytes = 0;

            status = (UInt32)uFCoder.GetDlogicCardType(out dl_card_type);

            if (status != 0)
            {
                Console.WriteLine("\nCommunication with card failed");
                Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status));
                return;
            }

            if (!(((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL1)
            || ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_4K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL1)
            || ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL3 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL3)
            || ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL3 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL3)))
            {
                Console.WriteLine("Card is not in security level 1 or 3 mode\n");
                return;
            }

            Console.WriteLine(" (1) - Block write");
            Console.WriteLine(" (2) - Block in sector write");
            Console.WriteLine(" (3) - Linear write");

            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                Console.WriteLine(Environment.NewLine + "Block write selected" + Environment.NewLine);

                if (((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL1)
                    || ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_4K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL1))
                {
                    Console.WriteLine(" (1) - Provided CRYPTO 1 key");
                    Console.WriteLine(" (2) - Reader CRYPTO 1 key");
                    Console.WriteLine(" (3) - AKM1 CRYPTO 1 key");
                    Console.WriteLine(" (4) - AKM2 CRYPTO 1 key");

                    choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        key_input = Console.ReadLine();
                        if (key_input.Length != 12)
                        {
                            Console.WriteLine("CRYPTO1 Key must be 6 bytes long");
                            return;
                        }

                        crypto_1_sector_key = StringToByteArray(key_input);

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }
                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockWrite_PK(block_data, block_nr, auth_mode, crypto_1_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }

                    }
                    else if (choice == '2')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A index");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B index");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        key_input = Console.ReadLine();

                        key_index = Byte.Parse(key_input);

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }
                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockWrite(block_data, block_nr, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");

                        }
                    }
                    else if (choice == '3')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }
                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockWrite_AKM1(block_data, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else if (choice == '4')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }
                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockWrite_AKM2(block_data, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine(" (1) - Provided AES key");
                    Console.WriteLine(" (2) - Reader AES key");
                    Console.WriteLine(" (3) - AKM1 AES key");
                    Console.WriteLine(" (4) - AKM2 AES key");

                    choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1A;
                            Console.WriteLine("\nEnter AES key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1B;
                            Console.WriteLine("\nEnter AES key B");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        key_input = Console.ReadLine();
                        if (key_input.Length != 32)
                        {
                            Console.WriteLine("AES Key must be 16 bytes long");
                            return;
                        }

                        aes_sector_key = StringToByteArray(key_input);

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }
                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockWrite_PK(block_data, block_nr, auth_mode, aes_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }

                    }
                    else if (choice == '2')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter AES key A index (0 - 15)");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter AES key B index (0 - 15)");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        key_input = Console.ReadLine();
                        key_index = Byte.Parse(key_input);

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }
                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockWrite(block_data, block_nr, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else if (choice == '3')
                    {
                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");

                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }
                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockWrite_AKM1(block_data, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else if (choice == '4')
                    {

                        Console.Write("\nEnter block number (0 - 128 2K card) (0 - 255 4K card)\n");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");

                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice\n");
                            return;
                        }

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }

                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockWrite_AKM2(block_data, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }

                }

            }
            else if (choice == '2')
            {
                Console.Write("\nBlock in sector write selected" + Environment.NewLine);

                if (((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL1)
                    || ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_4K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL1))
                {
                    Console.WriteLine(" (1) - Provided CRYPTO 1 key");
                    Console.WriteLine(" (2) - Reader CRYPTO 1 key");
                    Console.WriteLine(" (3) - AKM1 CRYPTO 1 key");
                    Console.WriteLine(" (4) - AKM2 CRYPTO 1 key");

                    choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B\n");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }
                        key_input = Console.ReadLine();
                        if (key_input.Length != 12)
                        {
                            Console.WriteLine("CRYPTO1 key must be 6 bytes long");
                            return;
                        }
                        crypto_1_sector_key = StringToByteArray(key_input);

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }

                        block_data = StringToByteArray(key_input);


                        status = (UInt32)uFCoder.BlockInSectorWrite_PK(block_data, sector_nr, block_nr, auth_mode, crypto_1_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else if (choice == '2')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A index (0 - 31)");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B index (0 - 31)\n");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }
                        key_input = Console.ReadLine();

                        key_index = Byte.Parse(key_input);

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }

                        block_data = StringToByteArray(key_input);


                        status = (UInt32)uFCoder.BlockInSectorWrite(block_data, sector_nr, block_nr, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else if (choice == '3')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }

                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockInSectorWrite_AKM1(block_data, sector_nr, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else if (choice == '4')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }

                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockInSectorWrite_AKM2(block_data, sector_nr, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }

                }
                else
                {
                    Console.WriteLine(" (1) - Provided AES key");
                    Console.WriteLine(" (2) - Reader AES key");
                    Console.WriteLine(" (3) - AKM1 AES key");
                    Console.WriteLine(" (4) - AKM2 AES key");

                    choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1A;
                            Console.WriteLine("\nEnter AES key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1B;
                            Console.WriteLine("\nEnter AES key B\n");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }
                        key_input = Console.ReadLine();
                        if (key_input.Length != 32)
                        {
                            Console.WriteLine("CRYPTO1 key must be 16 bytes long");
                            return;
                        }
                        aes_sector_key = StringToByteArray(key_input);

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }

                        block_data = StringToByteArray(key_input);


                        status = (UInt32)uFCoder.BlockInSectorWrite_PK(block_data, sector_nr, block_nr, auth_mode, aes_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else if (choice == '2')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter AES key A index (0 - 15)");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter AES key B (0-15)");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }
                        key_input = Console.ReadLine();

                        key_index = Byte.Parse(key_input);

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }

                        block_data = StringToByteArray(key_input);

                        status = (UInt32)uFCoder.BlockInSectorWrite(block_data, sector_nr, block_nr, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else if (choice == '3')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }


                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }

                        block_data = StringToByteArray(key_input);


                        status = (UInt32)uFCoder.BlockInSectorWrite_AKM1(block_data, sector_nr, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                    else if (choice == '4')
                    {
                        Console.WriteLine("\nEnter sector number (0 - 31 2K card) (0 - 39 4K card)");
                        key_input = Console.ReadLine();
                        sector_nr = Byte.Parse(key_input);


                        Console.WriteLine("\nEnter block in sector number (0 - 3 for sectors 0 - 31) (0 - 15 for sectors 32 - 39)");
                        key_input = Console.ReadLine();
                        block_nr = Byte.Parse(key_input);


                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        Console.WriteLine("\nEnter block data");
                        key_input = Console.ReadLine();
                        if (key_input.Length > 32)
                        {
                            Console.WriteLine("Cant't write more than 16 bytes of block data");
                            return;
                        }

                        block_data = StringToByteArray(key_input);


                        status = (UInt32)uFCoder.BlockInSectorWrite_AKM2(block_data, sector_nr, block_nr, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nBlock write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Block write successful");
                        }
                    }
                }

            }
            else if (choice == '3')
            {
                Console.Write("\nLinear write selected" + Environment.NewLine);
                if (((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_2K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_2K_SL1)
                            || ((DLOGIC_CARD_TYPE)dl_card_type >= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_S_4K_SL1 && (DLOGIC_CARD_TYPE)dl_card_type <= DLOGIC_CARD_TYPE.DL_MIFARE_PLUS_EV1_4K_SL1))
                {
                    Console.WriteLine(" (1) - Provided CRYPTO 1 key");
                    Console.WriteLine(" (2) - Reader CRYPTO 1 key");
                    Console.WriteLine(" (3) - AKM1 CRYPTO 1 key");
                    Console.WriteLine(" (4) - AKM2 CRYPTO 1 key");


                    choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();

                        if (key_input.Length != 12)
                        {
                            Console.WriteLine("\n CRYPTO 1 key must be 6 bytes long");
                            return;
                        }

                        crypto_1_sector_key = StringToByteArray(key_input);

                        Console.WriteLine("\nEnter data");
                        key_input = Console.ReadLine();

                        byte[] write_linear_data = new byte[key_input.Length];

                        write_linear_data = StringToByteArray(key_input);

                        lin_len = Convert.ToUInt16(write_linear_data.Length);

                        status = (UInt32)uFCoder.LinearWrite_PK(write_linear_data, lin_addr, lin_len, out ret_bytes, auth_mode, crypto_1_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.Write("\nLinear write succesful");
                        }
                    }
                    else if (choice == '2')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter CRYPTO 1 key A (0 - 31)");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter CRYPTO 1 key B index (0 - 31)");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();


                        key_index = Byte.Parse(key_input);

                        Console.WriteLine("\nEnter data");
                        key_input = Console.ReadLine();

                        byte[] write_linear_data = new byte[key_input.Length];

                        write_linear_data = StringToByteArray(key_input);

                        lin_len = Convert.ToUInt16(write_linear_data.Length);

                        status = (UInt32)uFCoder.LinearWrite(write_linear_data, lin_addr, lin_len, out ret_bytes, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.Write("\nLinear write succesful");
                        }
                    }
                    else if (choice == '3')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        Console.WriteLine("\nEnter data");
                        key_input = Console.ReadLine();

                        byte[] write_linear_data = new byte[key_input.Length];

                        write_linear_data = StringToByteArray(key_input);

                        lin_len = Convert.ToUInt16(write_linear_data.Length);

                        status = (UInt32)uFCoder.LinearWrite_AKM1(write_linear_data, lin_addr, lin_len, out ret_bytes, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.Write("\nLinear write succesful");
                        }
                    }
                    else if (choice == '4')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - CRYPTO 1 KEY A\n");
                        Console.Write(" (2) - CRYPTO 1 KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        Console.WriteLine("\nEnter data");
                        key_input = Console.ReadLine();

                        byte[] write_linear_data = new byte[key_input.Length];

                        write_linear_data = StringToByteArray(key_input);

                        lin_len = Convert.ToUInt16(write_linear_data.Length);

                        status = (UInt32)uFCoder.LinearWrite_AKM2(write_linear_data, lin_addr, lin_len, out ret_bytes, auth_mode);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.Write("\nLinear write succesful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine(" (1) - Provided AES key");
                    Console.WriteLine(" (2) - Reader AES key");

                    choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1A;
                            Console.WriteLine("\nEnter AES key A");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_PLUS_AES_AUTHENTICATION.MIFARE_PLUS_AES_AUTHENT1B;
                            Console.WriteLine("\nEnter AES key B");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();

                        if (key_input.Length != 32)
                        {
                            Console.WriteLine("\n AES key must be 16 bytes long");
                            return;
                        }

                        aes_sector_key = StringToByteArray(key_input);

                        Console.WriteLine("\nEnter data");
                        key_input = Console.ReadLine();

                        byte[] write_linear_data = new byte[key_input.Length];

                        write_linear_data = StringToByteArray(key_input);

                        lin_len = Convert.ToUInt16(write_linear_data.Length);

                        status = (UInt32)uFCoder.LinearWrite_PK(write_linear_data, lin_addr, lin_len, out ret_bytes, auth_mode, aes_sector_key);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("\nLinear write succesful");
                        }
                    }
                    else if (choice == '2')
                    {
                        Console.WriteLine("\nEnter linear address (0 - 1519 2K cards) (0 - 3439 4K cards)");
                        key_input = Console.ReadLine();
                        lin_addr = Convert.ToUInt16(key_input);

                        Console.Write("Enter code for authentication mode\n");
                        Console.Write(" (1) - AES KEY A\n");
                        Console.Write(" (2) - AES KEY B\n");
                        key_input = Console.ReadLine();
                        auth_mode = Byte.Parse(key_input);

                        if (auth_mode == 1)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1A;
                            Console.WriteLine("\nEnter AES key A index (0 - 15)");
                        }
                        else if (auth_mode == 2)
                        {
                            auth_mode = (byte)MIFARE_AUTHENTICATION.MIFARE_AUTHENT1B;
                            Console.WriteLine("\nEnter AES key B  index (0 - 15)");
                        }
                        else
                        {
                            Console.WriteLine("\nWrong choice");
                            return;
                        }

                        key_input = Console.ReadLine();

                        key_index = Byte.Parse(key_input);

                        Console.WriteLine("\nEnter data");
                        key_input = Console.ReadLine();

                        byte[] write_linear_data = new byte[key_input.Length];

                        write_linear_data = StringToByteArray(key_input);

                        lin_len = Convert.ToUInt16(write_linear_data.Length);

                        status = (UInt32)uFCoder.LinearWrite(write_linear_data, lin_addr, lin_len, out ret_bytes, auth_mode, key_index);

                        if (status != 0)
                        {
                            Console.WriteLine("\nLinear write failed");
                            Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("\nLinear write succesful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nWrong choice");
                        return;
                    }
                }

            }
            else
            {
                Console.WriteLine("\nWrong choice");
                return;
            }

        }

        //-----------------------------------------------------------------------------------------//

        public static void Write_keys()
        {
            Console.WriteLine(" -------------------------------------------------------------------");
            Console.WriteLine("                	WRITING KEYS INTO READER                        ");
            Console.WriteLine(" -------------------------------------------------------------------");

            status = 0;
            int choice = 0;
            byte[] crypto_1_key = new byte[6];
            byte[] aes_key = new byte[16];
            char[] password = new char[8];
            string key_input = "";
            byte key_index = 0;



            Console.WriteLine(" (1) - CRIPTO 1 keys");
            Console.WriteLine(" (2) - AES keys");
            Console.WriteLine(" (3) - Unlock reader");
            Console.WriteLine(" (4) - Lock reader");


            choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                Console.WriteLine("\nEnter CRYPTO 1 key");
                key_input = Console.ReadLine();

                if (key_input.Length != 12)

                {
                    Console.WriteLine("\nCRYPTO 1 key must be 6 bytes long");
                    return;
                }

                crypto_1_key = StringToByteArray(key_input);

                Console.WriteLine("Enter key index ( 0 - 31)");
                key_input = Console.ReadLine();


                key_index = Byte.Parse(key_input);



                status = (UInt32)uFCoder.ReaderKeyWrite(crypto_1_key, key_index);
                if (status != 0)
                {
                    Console.WriteLine("Writing key into reader failed");
                    Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.WriteLine("Key written into reader successfully ");
                }
            }
            else if (choice == '2')
            {
                Console.WriteLine("\nEnter AES key");
                key_input = Console.ReadLine();

                if (key_input.Length != 32)

                {
                    Console.WriteLine("\nAES key must be 6 bytes long");
                    return;
                }

                aes_key = StringToByteArray(key_input);

                Console.WriteLine("Enter key index ( 0 - 15)");
                key_input = Console.ReadLine();

                key_index = Byte.Parse(key_input);

                status = (UInt32)uFCoder.uFR_int_DesfireWriteAesKey(key_index, aes_key);
                if (status != 0)
                {
                    Console.WriteLine("Writing key into reader failed");
                    Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.WriteLine("Key written into reader successfully ");
                }
            }
            else if (choice == '3')
            {
                Console.WriteLine("\nEnter password (8 characters)");
                key_input = Console.ReadLine();

                if (key_input.Length != 8)

                {
                    Console.WriteLine("\nPassword nust be 8 characters long");
                    return;
                }

                password = key_input.ToCharArray();

                status = (UInt32)uFCoder.ReaderKeysUnlock(password);
                if (status != 0)
                {
                    Console.WriteLine("\nUnlock keys error");
                    Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.WriteLine("Reader keys are unlocked\n");
                }

                System.Threading.Thread.Sleep(250);

            }
            else if (choice == '4')
            {
                Console.WriteLine("\nEnter password (8 characters)");
                key_input = Console.ReadLine();

                if (key_input.Length != 8)

                {
                    Console.WriteLine("\nPassword nust be 8 characters long");
                    return;
                }

                password = key_input.ToCharArray();

                status = (UInt32)uFCoder.ReaderKeysLock(password);

                if (status != 0)
                {
                    Console.WriteLine("\nLock keys error");
                    Console.WriteLine("Status: " + uFCoder.status2str((uFR.DL_STATUS)status));
                }
                else
                {
                    Console.WriteLine("Reader keys are locked\n");
                    System.Threading.Thread.Sleep(250);
                }
            }
            else
            {
                Console.WriteLine("\nWrong choice");
                return;
            }

        }

        //-----------------------------------------------------------------------------------------//

        public static uFR.DL_STATUS New_card_in_field(byte sak, ref byte[] uid, byte uid_size)
        {
            DL_STATUS status;

            byte dl_card_type = 0;

            status = (UInt32)uFCoder.GetDlogicCardType(out dl_card_type);
            if (status != 0)
            {
                return (uFR.DL_STATUS)status;
            }

            byte[] shorter_uid = new byte[uid_size];
            Array.Copy(uid, shorter_uid, uid_size);

            Console.WriteLine(" \a-------------------------------------------------------------------");
            Console.WriteLine(" Card type: " + (DLOGIC_CARD_TYPE)dl_card_type + ", sak = 0x" + sak.ToString("X2") + ", uid[" + uid_size.ToString() + "] = " + BitConverter.ToString(shorter_uid).Replace("-", ":")); 

            return uFR.DL_STATUS.UFR_OK;
        }
        //-----------------------------------------------------------------------------------------//


    }
}

