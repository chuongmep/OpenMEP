using ICSharpCode.SharpZipLib.Zip;

namespace DeployInstaller;

public static class Utils
{
    // https://learn.microsoft.com/en-us/previous-versions/dotnet/articles/ms973825(v=msdn.10)?redirectedfrom=MSDN
    public static string GetDayInYear()
    {
        // Get Day Current Of Year
        DateTime now = DateTime.UtcNow.ToUniversalTime();
        DateTime startOfYear = new DateTime(now.Year, 1, 1);
        return (now - startOfYear).Days.ToString();
        //get date time at london 
        // DateTime now = DateTime.UtcNow;
        // DateTime londonTime = now.ToUniversalTime().AddHours(7);
    }

    public static string GetLastTwoDigitOfYear()
    {
        DateTime now = DateTime.UtcNow.ToUniversalTime();
        return now.Year.ToString().Substring(2, 2);
    }

    public static string GetDay()
    {
        int hour = DateTime.UtcNow.ToUniversalTime().Hour;
        int minute = DateTime.UtcNow.ToUniversalTime().Minute;
        string Sub = hour + minute.ToString();
        return Sub;
    }
    public static void CompressFile(string filePath, string OutputFilePath, int compressLevel = 9)
    {
        try
        {
            using (ZipOutputStream OutputStream = new ZipOutputStream(File.Create(OutputFilePath)))
            {
                // Define the compression level
                // 0 - store only to 9 - means best compression
                OutputStream.SetLevel(compressLevel);
                byte[] buffer = new byte[4096];
                ZipEntry entry = new ZipEntry(Path.GetFileName(filePath));
                entry.DateTime = DateTime.Now;
                OutputStream.PutNextEntry(entry);

                using (FileStream fs = File.OpenRead(filePath))
                {
                    // Using a fixed size buffer here makes no noticeable difference for output
                    // but keeps a lid on memory usage.
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        OutputStream.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }

                OutputStream.Finish();
                OutputStream.Close();
                Console.WriteLine("Zip file has been built: " + OutputFilePath);
            }
        }
        catch (Exception ex)
        {
            // No need to rethrow the exception as for our purposes its handled.
            Console.WriteLine("Exception during processing {0}", ex);
        }
    }

}