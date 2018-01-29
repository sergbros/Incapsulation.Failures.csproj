using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{

    public class Common
    {
        public static int IsFailureSerious(int failureType)
        {
            if (failureType%2==0) return 1;
            return 0;
        }

        public static int Earlier(object[] v, int day, int month, int year)
        {
            /*int vYear = (int)v[2];
            int vMonth = (int)v[1];
            int vDay = (int)v[0];
            if (vYear < year) return 1;
            if (vYear > year) return 0;
            if (vMonth < month) return 1;
            if (vMonth > month) return 0;
            if (vDay < day) return 1;*/
            DateTime dateObj = new DateTime(year, month, day);
            DateTime dateForComporise = new DateTime((int)v[2], (int)v[1], (int)v[0]);

            if (dateForComporise < dateObj) return 1;
            return 0;
        }
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            DateTime date1 = new DateTime(year, month, day);

            bool[] bEarlier = new bool[4];
            for (int i = 0; i < failureTypes.Length; i++)
            {
                if (Common.Earlier(times[i], day, month, year) == 1) { bEarlier[i] = true; }
                else bEarlier[i] = false;
            }

            return (FindDevicesFailedBeforeDate(failureTypes, deviceId, bEarlier, devices));

            //var problematicDevices = new HashSet<int>();
            //for (int i = 0; i < failureTypes.Length; i++)
            //    if (Common.IsFailureSerious(failureTypes[i])==1 && Common.Earlier(times[i], day, month, year)==1)
            //        problematicDevices.Add(deviceId[i]);

            //var result = new List<string>();
            //foreach (var device in devices)
            //    if (problematicDevices.Contains((int)device["DeviceId"]))
            //        result.Add(device["Name"] as string);

            //return result;
        }

        //DateTime date1 = new DateTime(2015, 7, 20); // год - месяц - день
        public static List<string> FindDevicesFailedBeforeDate(
            int[] failureTypes,
            int[] deviceId,
            bool[] bEarlier,
            List<Dictionary<string, object>> devices)
        {
            var problematicDevices = new HashSet<int>();
            for (int i = 0; i < failureTypes.Length; i++)
                if (Common.IsFailureSerious(failureTypes[i]) == 1 && bEarlier[i])
                    problematicDevices.Add(deviceId[i]);

            var result = new List<string>();
            foreach (var device in devices)
                if (problematicDevices.Contains((int)device["DeviceId"]))
                    result.Add(device["Name"] as string);

            return result;

        }

    }
}
