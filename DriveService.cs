using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;

namespace DriveService
{
    public class DriveService
    {
        public static readonly DriveInfo SystemDrive = new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory));

        public IEnumerable<DriveInfo> GetDrives()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives().Where(di => di.IsReady && di.GetLetter() != SystemDrive.GetLetter()))
            {
                yield return drive;
            }
        }

        public void PrintDiskProperties(DriveInfo drive)
        {
            if (drive == null)
                throw new ArgumentNullException(nameof(drive));

            using (var logicalDisk = new ManagementObject($"Win32_LogicalDisk.DeviceID='{drive.GetLetter()}'"))
            {
                foreach (PropertyData logicalDiskProperty in logicalDisk.Properties)
                {
                    var value = Convert.ToString(logicalDiskProperty.Value);

                    if (!string.IsNullOrEmpty(value))
                        Console.WriteLine($" • [Win32_LogicalDisk] {logicalDiskProperty.Name} = {value}");
                }

                using (ManagementObjectCollection diskPartitionCollection = logicalDisk.GetRelated("Win32_DiskPartition"))
                using (ManagementObject diskPartition = diskPartitionCollection.OfType<ManagementObject>().FirstOrDefault())
                {
                    if (diskPartition == null)
                        return;

                    using (ManagementObjectCollection diskDriveCollection = diskPartition.GetRelated("Win32_DiskDrive"))
                    using (ManagementObject diskDrive = diskDriveCollection.OfType<ManagementObject>().FirstOrDefault())
                    {
                        if (diskDrive == null)
                            return;

                        foreach (PropertyData diskDriveProperty in diskDrive.Properties)
                        {
                            var value = Convert.ToString(diskDriveProperty.Value);

                            if (!string.IsNullOrEmpty(value))
                                Console.WriteLine($" • [Win32_DiskDrive] {diskDriveProperty.Name} = {value}");
                        }
                    }
                }
            }
        }
    }
}