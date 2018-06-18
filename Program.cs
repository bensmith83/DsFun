using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;



// so much from pinvoke
// http://www.pinvoke.net/default.aspx/ntdsapi/DsGetDomainControllerInfo.html

namespace DsFun
{

    [StructLayout(LayoutKind.Sequential)]
    public class GuidClass
    {
        public Guid TheGuid;
    }

    class NtdsHelper
    {
        [DllImport("ntdsapi.dll", CharSet = CharSet.Auto)]
        static public extern uint DsBind(
            string DomainControllerName,
            string DnsDomainName, out IntPtr phDS
        );

        [DllImport("Ntdsapi.dll", CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode, EntryPoint = "DsBindWithCredW", SetLastError = true)]
        public static extern UInt32 DsBindWithCred(
            [MarshalAs(UnmanagedType.LPWStr)] String DomainControllerName,
            [MarshalAs(UnmanagedType.LPWStr)] String DnsDomainName,
            IntPtr AuthIdentity,
            out IntPtr phDS
        );

        [DllImport("Ntdsapi.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode,
            EntryPoint = "DsMakePasswordCredentialsW", SetLastError = true)]
        public static extern UInt32 DsMakePasswordCredentials(
             [MarshalAs(UnmanagedType.LPWStr)] String User,
             [MarshalAs(UnmanagedType.LPWStr)] String Domain,
             [MarshalAs(UnmanagedType.LPWStr)] String Password,
             out IntPtr AuthIdentity
        );

        [DllImport("ntdsapi.dll", CharSet = CharSet.Auto)]
        static public extern uint DsUnBind(ref IntPtr phDS);

        [StructLayout(LayoutKind.Sequential)]
        public struct GUID
        {
            public int a;
            public short b;
            public short c;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] d;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DS_DOMAIN_CONTROLLER_INFO_2
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string NetbiosName;        // SDBAD10004
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DnsHostName;        // sdbad10004.dom1.ad.sys
            [MarshalAs(UnmanagedType.LPTStr)]
            public string SiteName;           // BAD1
            [MarshalAs(UnmanagedType.LPTStr)]
            public string SiteObjectName;     // CN=BAD1,CN=Sites,CN=Configuration,DC=ad,DC=sys
            [MarshalAs(UnmanagedType.LPTStr)]
            public string ComputerObjectName;     // CN=SDBAD10004,OU=Domain Controllers,DC=dom1,DC=ad,DC=sys
            [MarshalAs(UnmanagedType.LPTStr)]
            public string ServerObjectName;       // CN=SDBAD10004,CN=Servers,CN=BAD1,CN=Sites,CN=Configuration,DC=ad,DC=sys  
            [MarshalAs(UnmanagedType.LPTStr)]
            public string NtdsDsaObjectName;      // CN=NTDS Settings,CN=SDBAD10004,CN=Servers,CN=BAD1,CN=Sites,CN=Configuration,DC=ad,DC=sys
            [MarshalAs(UnmanagedType.Bool)]
            public bool fIsPdc;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fDsEnabled;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fIsGc;
            public GUID SiteObjectGuid;
            public GUID ComputerObjectGuid;
            public GUID ServerObjectGuid;
            public GUID NtdsDsaObjectGuid;
        }

        [DllImport("ntdsapi.dll", CharSet = CharSet.Auto)]
        public static extern uint DsGetDomainControllerInfo(
            IntPtr hDs,
            string DomainName,
            uint InfoLevel,
            out uint InfoCount,
            out IntPtr pInf);

        [DllImport("ntdsapi.dll", CharSet = CharSet.Auto)]
        public static extern void DsFreeDomainControllerInfo(
            uint InfoLevel,
            uint cInfo,
            IntPtr pInf);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DOMAIN_CONTROLLER_INFO
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DomainControllerName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DomainControllerAddress;
            public uint DomainControllerAddressType;
            public Guid DomainGuid;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DomainName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DnsForestName;
            public uint Flags;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DcSiteName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string ClientSiteName;
        }

        [DllImport("Netapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int DsGetDcName
          (
            [MarshalAs(UnmanagedType.LPTStr)]
    string ComputerName,
            [MarshalAs(UnmanagedType.LPTStr)]
    string DomainName,
            [In] int DomainGuid,
            [MarshalAs(UnmanagedType.LPTStr)]
    string SiteName,
            [MarshalAs(UnmanagedType.U4)]
    DSGETDCNAME_FLAGS flags,
            out IntPtr pDOMAIN_CONTROLLER_INFO
          );

        [DllImport("Netapi32.dll", SetLastError = true)]
        public static extern int NetApiBufferFree(IntPtr Buffer);

        [Flags]
        public enum DSGETDCNAME_FLAGS : uint
        {
            DS_FORCE_REDISCOVERY = 0x00000001,
            DS_DIRECTORY_SERVICE_REQUIRED = 0x00000010,
            DS_DIRECTORY_SERVICE_PREFERRED = 0x00000020,
            DS_GC_SERVER_REQUIRED = 0x00000040,
            DS_PDC_REQUIRED = 0x00000080,
            DS_BACKGROUND_ONLY = 0x00000100,
            DS_IP_REQUIRED = 0x00000200,
            DS_KDC_REQUIRED = 0x00000400,
            DS_TIMESERV_REQUIRED = 0x00000800,
            DS_WRITABLE_REQUIRED = 0x00001000,
            DS_GOOD_TIMESERV_PREFERRED = 0x00002000,
            DS_AVOID_SELF = 0x00004000,
            DS_ONLY_LDAP_NEEDED = 0x00008000,
            DS_IS_FLAT_NAME = 0x00010000,
            DS_IS_DNS_NAME = 0x00020000,
            DS_RETURN_DNS_NAME = 0x40000000,
            DS_RETURN_FLAT_NAME = 0x80000000
        }

        //DsEnumerateDomainTrusts
        [DllImport("Netapi32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint DsEnumerateDomainTrusts(string ServerName,
                            uint Flags,
                            out IntPtr Domains,
                            out uint DomainCount);

        [Flags]
        public enum DS_DOMAIN_TRUST_TYPE : uint
        {
            DS_DOMAIN_IN_FOREST = 0x0001,  // Domain is a member of the forest
            DS_DOMAIN_DIRECT_OUTBOUND = 0x0002,  // Domain is directly trusted
            DS_DOMAIN_TREE_ROOT = 0x0004,  // Domain is root of a tree in the forest
            DS_DOMAIN_PRIMARY = 0x0008,  // Domain is the primary domain of queried server
            DS_DOMAIN_NATIVE_MODE = 0x0010,  // Primary domain is running in native mode
            DS_DOMAIN_DIRECT_INBOUND = 0x0020   // Domain is directly trusting
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DS_DOMAIN_TRUSTS
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string NetbiosDomainName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DnsDomainName;
            public uint Flags;
            public uint ParentIndex;
            public uint TrustType;
            public uint TrustAttributes;
            public IntPtr DomainSid;
            public Guid DomainGuid;
        }

        [DllImport("NetApi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern System.Int32 DsGetDcSiteCoverage(
        [MarshalAs(UnmanagedType.LPTStr)]
        string ServerName,
        out System.Int64 EntryCount,
        out IntPtr SiteNames
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct SOCKET_ADDRESS
        {
            public IntPtr lpSockaddr;
            public int iSockaddrLength;
        }

        [DllImport("netapi32.dll", CharSet = CharSet.Auto)]
        public static extern int DsAddressToSiteNames(string computerName, int entryCount, SOCKET_ADDRESS[] socketAddresses, ref IntPtr siteNames);


    }


    class Program
    {
        [DllImport("Netapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int DsGetDcName
        (
            [MarshalAs(UnmanagedType.LPTStr)]
        string ComputerName,
            [MarshalAs(UnmanagedType.LPTStr)]
        string DomainName,
            //[In] Guid DomainGuid,
            [In] GuidClass DomainGuid,
            [MarshalAs(UnmanagedType.LPTStr)]
        string SiteName,
            int Flags,
            out IntPtr pDOMAIN_CONTROLLER_INFO
        );


        static void Main(string[] args)
        {
            int option = 0;
            string DomainControllerName = "192.168.0.180";
            string DnsDomainName = "";
            string computername = "";
            string domain = "";
            while (true)
            {
                Console.WriteLine("Select a DS option");
                option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        Console.WriteLine("DsGetDcName");
                        // Get a Nearby DC
                        Console.WriteLine("Enter DC Name: ");
                        computername = Console.ReadLine();
                        DomainControllerName = computername;
                        Console.WriteLine("Enter a domain name (opt): ");
                        domain = Console.ReadLine();
                        IntPtr out_dc = (IntPtr)0;
                        GuidClass guid = null;
                        NtdsHelper.DOMAIN_CONTROLLER_INFO domainInfo;

                        uint ret = (uint)Program.DsGetDcName(computername, domain, guid, null, 0, out out_dc);
                        if (ret != 0)
                        {
                            Console.WriteLine("[*] DsGetDcName failed. ret:" + ret.ToString());
                        }
                        else
                        {
                            Console.WriteLine("[*] DsGetDcName: ");
                            domainInfo = (NtdsHelper.DOMAIN_CONTROLLER_INFO)Marshal.PtrToStructure(out_dc, typeof(NtdsHelper.DOMAIN_CONTROLLER_INFO));

                            string msg = "Forest : " + domainInfo.DnsForestName + "\r\n";
                            msg += "DC-Site: " + domainInfo.DomainControllerName + "\r\n";
                            msg += " Client: " + domainInfo.ClientSiteName + "\r\n";
                            Console.WriteLine(msg);
                        }
                        break;
                    case 2:
                        // DsEnumerateDomainTrusts
                        Console.WriteLine("DsEnumerateDomainTrusts");
                        // What trust types are we interested in ?
                        uint trustTypes = (uint)(NtdsHelper.DS_DOMAIN_TRUST_TYPE.DS_DOMAIN_PRIMARY | NtdsHelper.DS_DOMAIN_TRUST_TYPE.DS_DOMAIN_DIRECT_OUTBOUND);

                        IntPtr buf = new IntPtr();
                        uint numDomains = 0;
                        NtdsHelper.DS_DOMAIN_TRUSTS[] trusts = new NtdsHelper.DS_DOMAIN_TRUSTS[0];

                        // Make the call - not doing anything special with the result value here
                        uint result = NtdsHelper.DsEnumerateDomainTrusts(null,
                                              trustTypes,
                                              out buf,
                                              out numDomains);
                        
                        try
                        {
                            if ((numDomains > 0) && (result == 0))
                            {
                                // Marshal the received buffer to managed structs

                                trusts = new NtdsHelper.DS_DOMAIN_TRUSTS[numDomains];

                                IntPtr iter = buf;

                                for (int i = 0; i < numDomains; i++)
                                {
                                    trusts[i] = (NtdsHelper.DS_DOMAIN_TRUSTS)Marshal.PtrToStructure(iter, typeof(NtdsHelper.DS_DOMAIN_TRUSTS));
                                    iter = (IntPtr)(iter.ToInt64() + (long)Marshal.SizeOf(typeof(NtdsHelper.DS_DOMAIN_TRUSTS)));
                                    Console.WriteLine("[*] found:\n    trust domain: "+trusts[i].DnsDomainName+"\n    trust type: "+trusts[i].TrustType+"\n    trust attributes: "+trusts[i].TrustAttributes+"\n    trust parent index: "+trusts[i].ParentIndex);
                                    Console.WriteLine("[*] iter\n    : " + iter.ToString());
                                }
                            }
                        }
                        finally
                        {
                            // Make sure we free the buffer whatever happens
                            NtdsHelper.NetApiBufferFree(buf);
                        }

                        break;
                    case 3:
                        // DsGetForestTrustInformationW
                        break;
                    case 4:
                        // DsGetDCSiteCoverage
                        long lEntryCount = 0;
                        IntPtr pSiteNames;
                        Console.WriteLine("Enter DC Name: ");
                        computername = Console.ReadLine();
                        DomainControllerName = computername;
                        int j = NtdsHelper.DsGetDcSiteCoverage(DomainControllerName, out lEntryCount, out pSiteNames);

                        Console.WriteLine("Status: " + j.ToString());
                        Console.WriteLine("Entries: " + lEntryCount.ToString());

                        for (int jData = 0; jData < lEntryCount; jData++)
                        {
                            Console.WriteLine(Marshal.PtrToStringAuto(Marshal.ReadIntPtr(pSiteNames, jData * IntPtr.Size)));
                        }

                        NtdsHelper.NetApiBufferFree(pSiteNames);
                        break;
                    case 5:
                        // DsValidateSubnetName
                        break;
                    case 6:
                        // DsAddressToSiteNames(ex)9

                        //WSADATA data = new WSADATA();
                        //SockAddr sockAddr = new SockAddr();
                        //IntPtr pSockAddr = IntPtr.Zero;
                        //IntPtr pSites = IntPtr.Zero;
                        //SOCKET_ADDRESS[] SocketAddresses = new SOCKET_ADDRESS[1];
                        //string siteName = string.Empty;

                        break;
                    case 0:
                        // DsGetDomainControllerInfo
                        Console.WriteLine("DsGetDomainConterollerInfo");

                        break;
                    case 7:

                        // DsListSites
                        break;
                    case 8:

                        // DsListServersForDomainInSite
                        break;
                    case 9:

                        // DsListDomainsInSite
                        break;
                    case 10:

                        // DsReplicaSync
                        break;
                    case 11:

                        // DsBackupOpenFile
                        break;
                    case 12:

                        // DsBackupRead
                        break;
                    case 13:

                        // DsCrackNames
                        break;
                    case 14:

                        // DRSReplicaSync
                        break;
                    case 15:

                        // DRSGetNCChanges
                        break;
                    case 16:

                        // DRSGetMemberships
                        break;
                    case 17:

                        // DRSGetMemberships2
                        break;
                    case 18:

                        // DRSDomainControllerInfo
                        break;
                    case 19:

                        // DRSExecuteKCC - validates replication interconnections of DCs
                        break;
                    case 20:

                        // DRSAddCloneDC - creates a new DC object by copying DC attributes
                        break;
                    case 21:

                        // DRSBind & DRSUnbind
                        break;
                    case 22:
                        break;
                    case 23:
                        // DsBind & DsUnbind & DsGetDomainControllerInfo    
                        Console.WriteLine("binding to DC");
                        Console.WriteLine("Enter DC Name: ");
                        computername = Console.ReadLine();
                        DomainControllerName = computername;
                        Console.WriteLine("Enter a domain name (opt): ");
                        domain = Console.ReadLine();
                        DnsDomainName = domain;
                        Int64 dc = 0;
                        IntPtr DCHandle = (IntPtr)dc;
                        ret = NtdsHelper.DsBind(DomainControllerName, DnsDomainName, out DCHandle);
                        if (ret != 0)
                        {
                            Console.WriteLine("[*] cannot bind to " + DomainControllerName + " on " + DnsDomainName + " ret: " + ret.ToString());
                            break;
                        }
                        NtdsHelper.DS_DOMAIN_CONTROLLER_INFO_2[] DcInfos;
                        uint InfoLvl = 2;
                        IntPtr DCInfosPtr;
                        uint nInfo;
                        ret = NtdsHelper.DsGetDomainControllerInfo(DCHandle, DnsDomainName, InfoLvl, out nInfo, out DCInfosPtr);
                        if (ret != 0)
                        {
                            Console.WriteLine("[*] cannot get DC Info for " + DomainControllerName + " on " + DnsDomainName + " ret: " + ret.ToString());
                            break;
                        }
                        DcInfos = new NtdsHelper.DS_DOMAIN_CONTROLLER_INFO_2[nInfo];
                        IntPtr CurrentInfoPtr = DCInfosPtr;
                        NtdsHelper.DS_DOMAIN_CONTROLLER_INFO_2 OneInfo;
                        for (uint i = 0; i < nInfo; i++)
                        {
                            OneInfo = (NtdsHelper.DS_DOMAIN_CONTROLLER_INFO_2)Marshal.PtrToStructure(CurrentInfoPtr, typeof(NtdsHelper.DS_DOMAIN_CONTROLLER_INFO_2));
                            DcInfos[i] = OneInfo;
                            
                            Console.WriteLine("[*] Found: \n    Computer Object Name: \n" + DcInfos[i].ComputerObjectName + "\n    Server Object Name: \n" + DcInfos[i].ServerObjectName + "\n    DnsHostName: \n" + DcInfos[i].DnsHostName);
                            CurrentInfoPtr = (IntPtr)((int)CurrentInfoPtr + Marshal.SizeOf(typeof(NtdsHelper.DS_DOMAIN_CONTROLLER_INFO_2)));

                        }
                        NtdsHelper.DsFreeDomainControllerInfo(InfoLvl, nInfo, DCInfosPtr);
                        NtdsHelper.DsUnBind(ref DCHandle);
                        Console.WriteLine("[*] DC Unbound");
                        break;
                    case 24:
                        Console.WriteLine("getting creds");
                        Console.WriteLine("user name: ");
                        String user = Console.ReadLine();
                        Console.WriteLine("domain: ");
                        domain = Console.ReadLine();
                        Console.WriteLine("password: ");
                        String password = Console.ReadLine();
                        IntPtr AuthIdentity = IntPtr.Zero;
                        NtdsHelper.DsMakePasswordCredentials(user, domain, password, out AuthIdentity);
                        Console.WriteLine("binding to DC");
                        ret = NtdsHelper.DsBindWithCred(DomainControllerName, DnsDomainName, AuthIdentity, out DCHandle);
                        if (ret != 0)
                        {
                            Console.WriteLine("[*] cannot bind to " + DomainControllerName + " on " + DnsDomainName + " ret: " + ret.ToString());
                            break;
                        }
                        break;
                    default:
                        break;
                }
            }
            Console.ReadLine(); // pause before exit

        }

    }
}
