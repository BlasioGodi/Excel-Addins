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
                        }

                        // Update the last update time to avoid processing the same data multiple times
                        SetLastUpdateTime(DateTime.Now);
                    }
                }
                catch (Exception ex)
                {
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
