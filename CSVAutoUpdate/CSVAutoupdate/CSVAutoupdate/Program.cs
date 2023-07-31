using System;
using System.IO;
using System.Threading.Tasks;
class Program
{
    // Create a lock object for file access synchronization
    private static readonly object fileLock = new object();

    static async Task Main()
    {
        // File paths
        string sourceFilePath = @"C:\Program Files\MT5-ACC1\MQL5\Files\MasterEASignal.csv";
        string destinationFolder1 = @"C:\Program Files\MT5-ACC2\MQL5\Files\";
        string destinationFolder2 = @"C:\Program Files\MT5-ACC3\MQL5\Files\";
        string destinationFolder3 = @"C:\Program Files\MT5-ACC4\MQL5\Files\";
        string destinationFolder4 = @"C:\Program Files\MT5-ACC5\MQL5\Files\";
        string destinationFolder5 = @"C:\Program Files\MT5-ACC6\MQL5\Files\";
        string destinationFolder6 = @"C:\Program Files\MT5-ACC7\MQL5\Files\";

        int maxRetries = 10; // Set the maximum number of retry attempts

        // Start a continuous update loop
        while (true)
        {
            int retryCount = 0; // Initialize the retry counter

            while (retryCount <= maxRetries)
            {
                try
                {
                    // Check if the source CSV file has been updated
                    if (File.GetLastWriteTime(sourceFilePath) > GetLastUpdateTime())
                    {
                        // Read data from the source CSV file asynchronously
                        string[][] rows = await ReadCsvFileAsync(sourceFilePath);

                        // Process and transfer data to other CSV files
                        foreach (string[] data in rows)
                        {
                            // Process the data if needed
                            string processedData = ProcessData(data);

                            // Transfer data to the first destination CSV file asynchronously
                            string destinationFile1 = Path.Combine(destinationFolder1, "SlaveEASignal1.csv");
                            await TransferDataAsync(destinationFile1, processedData);

                            // Transfer data to the second destination CSV file asynchronously
                            string destinationFile2 = Path.Combine(destinationFolder2, "SlaveEASignal2.csv");
                            await TransferDataAsync(destinationFile2, processedData);

                            // Transfer data to the third destination CSV file asynchronously
                            string destinationFile3 = Path.Combine(destinationFolder3, "SlaveEASignal3.csv");
                            await TransferDataAsync(destinationFile3, processedData);

                            // Transfer data to the fourth destination CSV file asynchronously
                            string destinationFile4 = Path.Combine(destinationFolder4, "SlaveEASignal4.csv");
                            await TransferDataAsync(destinationFile4, processedData);

                            // Transfer data to the fifth destination CSV file asynchronously
                            string destinationFile5 = Path.Combine(destinationFolder5, "SlaveEASignal5.csv");
                            await TransferDataAsync(destinationFile5, processedData);

                            // Transfer data to the sixth destination CSV file asynchronously
                            string destinationFile6 = Path.Combine(destinationFolder6, "SlaveEASignal6.csv");
                            await TransferDataAsync(destinationFile6, processedData);
                        }
                        // Update the last update time to avoid processing the same data multiple times
                        SetLastUpdateTime(DateTime.Now);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    // Handle the case when the file does not exist
                    Console.WriteLine("File not found: " + ex.Message);
                    //Retry the operation
                    Thread.Sleep(1000);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // Handle the case when you don't have permission to access the file
                    Console.WriteLine("Unauthorized access: " + ex.Message);
                }
                catch (IOException ex)
                {
                    // Handle other IO-related errors (e.g., file in use, disk full, etc.)
                    Console.WriteLine("IO error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    // Handle any other unexpected exceptions
                    Console.WriteLine("Error: " + ex.Message);
                }
                // Wait for a specified time before checking for updates again (e.g., 1 minute)
                await Task.Delay(1000);
            }
            // Wait for a specified time before checking for updates again (e.g., 1 minute)
            await Task.Delay(1000);
        }
    }

// Process data (you can customize this based on your needs)
static string ProcessData(string[] data)
{
    // Ensure that the data array has at least six elements
    if (data.Length < 6)
    {
        // Handle the case where the data is incomplete or incorrect
        // For example, you can log an error or return an empty string
        return string.Empty;
    }

    // Trim each element to remove any whitespaces
    for (int i = 0; i < data.Length; i++)
    {
        data[i] = data[i].Trim();
    }

    // For simplicity, just concatenate the first and second columns
    return data[0] + "," + data[1] + "," + data[2] + "," + data[3] + "," + data[4] + "," + data[5];
}
    // Write data to the destination CSV file asynchronously
    static async Task TransferDataAsync(string destinationFile, string data)
    {
        using (StreamWriter writer = new StreamWriter(destinationFile, false))
        {
            // Write the data asynchronously to the file
            await writer.WriteAsync(data);
        }
    }
    // Store and retrieve the last update time in a file or database
    static DateTime GetLastUpdateTime()
    {
        // Implement your logic to retrieve the last update time
        // For simplicity, we will return DateTime.MinValue for the initial check
        return DateTime.MinValue;
    }
    static void SetLastUpdateTime(DateTime updateTime)
    {
        // Implement your logic to store the last update time
        // For simplicity, we won't store the time in this example
    }
    static async Task<string[][]> ReadCsvFileAsync(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            // Initialize a list to store rows
            List<string[]> rowsList = new List<string[]>();

            // Read each line asynchronously
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();

                // Skip empty lines
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // Split the line into columns based on commas
                    string[] columns = line.Split(',');

                    rowsList.Add(columns);
                }
            }
            return rowsList.ToArray();
        }
    }


}
