using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbisMonitor.Domain;
using Common.Service;

namespace AbisMonitor.Service.DbServices
{
    public class DeviceSimpleService 
    {

        public List<AbisMonitor.Domain.AbisDeviceSimple> GetAbisDeviceSimpleList()
        {
            string queryString = @"SELECT
	bts.DataNum,
	device.DeviceName,
  device.DeviceNum,
	bts.PortNum,
	device.IPAddress,
	bts.BTS_Name,
	bts.SlotNum
FROM
	mtbtstable bts
Inner JOIN mtabisdevicetable device ON bts.DeviceNum = device.DataNum
GROUP BY
	device.DeviceName,bts.BTS_Name
ORDER BY
  device.DeviceNum,bts.PortNum,bts.SlotNum;";
            var list = SearchDataService.SearchData<AbisDeviceSimple>(queryString);
            return list == null ? null : list.ToList();
        }
    }
}
