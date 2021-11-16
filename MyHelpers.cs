using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace DSSGBOAdmin.Utilities
{
    public class MyHelpers
    {
        // L'identifiant de l'admin GBO pour le requette  IdentifiantAdminRequest => OrganizationSystemPrefix
        public static string IdentifiantAdminRequest = "GBOADMIN";

        // Pour l'envoie de Backup vers Server Nas Ou l'admin GBO => l'endroit de la sauvegarde de Backup.
        public static async Task<HttpResponseMessage> SendRequestToServiceAPI(HttpMethod Method, string UrlRequest, HttpContent _Body)
        {
            var cl = new HttpClient();
            cl.BaseAddress = new Uri(UrlRequest);

            //cl.DefaultRequestHeaders.Add("Cookie", $"IdentifiantUserRequest={MyHelpers.IdentifiantAdminRequest}");

            HttpResponseMessage response;

            switch (Method.ToString().ToUpper())
            {
                case "GET":
                    response = await cl.GetAsync(cl.BaseAddress);
                    break;
                case "POST":
                    response = await cl.PostAsync(cl.BaseAddress, _Body);
                    break;
                case "PUT":
                    response = await cl.PutAsync(cl.BaseAddress, _Body);
                    break;
                case "DELETE":
                    response = await cl.DeleteAsync(cl.BaseAddress);
                    break;
                default:
                    throw new NotImplementedException();
            }


            return response;
        }



        //public static string GetIdentifiantUserRequest(IRequestCookieCollection CookiesCollection)
        //{
        //    return (CookiesCollection["IdentifiantUserRequest"] ?? " ");
        //}

        // ALL Method For Disk Mangement File using DllImport("kernel32.dll")
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern bool GetDiskFreeSpace([MarshalAs(UnmanagedType.LPTStr)] string rootPathName,
        ref int sectorsPerCluster, ref int bytesPerSector, ref int numberOfFreeClusters, ref int totalNumbeOfClusters);

        public struct DiskInfo
        {
            public string RootPathName;
            public int SectorsPerCluster;
            public int BytesPerSector;
        }

        public static DiskInfo GetDiskInfo(string rootPathName)
        {
            DiskInfo diskInfo = new DiskInfo();
            int sectorsPerCluster = 0, bytesPerSector = 0, numberOfFreeClusters = 0, totalNumberOfClusters = 0;
            GetDiskFreeSpace(rootPathName, ref sectorsPerCluster, ref bytesPerSector, ref numberOfFreeClusters, ref totalNumberOfClusters);

            diskInfo.SectorsPerCluster = sectorsPerCluster;
            diskInfo.BytesPerSector = bytesPerSector;

            return diskInfo;
        }

        public static long GetClusterSize(DirectoryInfo dir)
        {
            long clusterSize = 0;
            DiskInfo diskInfo = new DiskInfo();
            diskInfo = GetDiskInfo(dir.Root.FullName);
            clusterSize = (diskInfo.BytesPerSector * diskInfo.SectorsPerCluster);
            return clusterSize;
        }

        public static long GetClusterSize(FileInfo file)
        {
            long clusterSize = 0;
            DiskInfo diskInfo = new DiskInfo();
            diskInfo = GetDiskInfo(file.Directory.Root.FullName);
            clusterSize = (diskInfo.BytesPerSector * diskInfo.SectorsPerCluster);
            return clusterSize;
        }

        public static long FileSpace(string filePath)
        {
            long temp = 0;
            FileInfo fileInfo = new FileInfo(filePath);
            long clusterSize = GetClusterSize(fileInfo);
            if (fileInfo.Length % clusterSize != 0)
            {
                decimal res = fileInfo.Length / clusterSize;
                int clu = Convert.ToInt32(Math.Ceiling(res)) + 1;
                temp = clusterSize * clu;
            }
            else
            {
                return fileInfo.Length;
            }
            return temp;
        }
        public static long GetDirectorySpace(string dirPath)
        {
            //return value 
            long len = 0;
            if (!Directory.Exists(dirPath))
            {
                len = FileSpace(dirPath);
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                //The cluster value of this machine
                long clusterSize = GetClusterSize(di);
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Length % clusterSize != 0)
                    {
                        decimal res = fi.Length / clusterSize;
                        //The file size is divided by the cluster, and the integer is incremented by 1. The value of the cluster is occupied for this file
                        int clu = Convert.ToInt32(Math.Ceiling(res)) + 1;
                        long result = clusterSize * clu;
                        len += result;
                    }
                    else
                    {
                        // If the remainder is 0, the occupied space is equal to the file size.
                        len += fi.Length;
                    }
                }
                DirectoryInfo[] dis = di.GetDirectories();
                if (dis.Length > 0)
                {
                    for (int i = 0; i < dis.Length; i++)
                    {
                        len += GetDirectorySpace(dis[i].FullName);
                    }
                }
            }
            return len;
        }
        public static int[] GetNumberFileByMonth(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return new int[] { };
            }
            DirectoryInfo di = new DirectoryInfo(dirPath);
            int nbrFiveMonth = 0, nbrFourMonth = 0, nbrThreeMonth = 0, nbrTwoMonth = 0, nbrOneMonth = 0;
            foreach (var file in di.GetFiles())
            {
                if (file.LastWriteTime.Month == DateTime.Now.AddMonths(-4).Month)
                {
                    nbrFiveMonth++;
                }
                else if (file.LastWriteTime.Month == DateTime.Now.AddMonths(-3).Month)
                {
                    nbrFourMonth++;
                }
                else if (file.LastWriteTime.Month == DateTime.Now.AddMonths(-2).Month)
                {
                    nbrThreeMonth++;
                }
                else if (file.LastWriteTime.Month == DateTime.Now.AddMonths(-1).Month)
                {
                    nbrTwoMonth++;
                }
                else if (file.LastWriteTime.Month == DateTime.Now.Month)
                {
                    nbrOneMonth++;
                }
            }
            int[] allFilesByMonths = new int[5];
            allFilesByMonths[0] = nbrFiveMonth;
            allFilesByMonths[1] = nbrFourMonth;
            allFilesByMonths[2] = nbrThreeMonth;
            allFilesByMonths[3] = nbrTwoMonth;
            allFilesByMonths[4] = nbrOneMonth;

            return allFilesByMonths;

        }

        public static int FileNumbers(string filePath)
        {

            int len = 0;
            if (Directory.Exists(filePath))
            {
                DirectoryInfo di = new DirectoryInfo(filePath);
                len = di.GetFiles().Length;
            }
            return len;
        }
    }
}
