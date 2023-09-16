namespace csv_compression
{
    using System.Diagnostics;
    using System.IO.Compression;

    class Program
    {
        static void Main()
        {
            string uncompressedFilePath = "uncompressed-output.csv";
            string compressedFilePath = "compressed-output.csv";

            int numberOfRecords = 1000000;

            var records = GenerateRecords(numberOfRecords);

            var headerValues = typeof(Record).GetProperties().Select(p => p.Name);
            var headerCsvRecord = string.Join(" ", headerValues);

            var csvRecords = records
                .Select(r => ConvertToCsvLine(r))
                .Prepend(headerCsvRecord)
                .ToList();

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Starting to write uncompressed csv.");
            stopwatch.Start();
            WriteUncompressedCsvToFile(uncompressedFilePath, csvRecords);
            stopwatch.Stop();
            Console.WriteLine($"Uncompressed CSV writing time: {stopwatch.Elapsed}");

            Console.WriteLine("Starting to write compressed csv.");
            stopwatch.Restart();
            WriteCompressedCsvToFile(compressedFilePath, csvRecords);
            stopwatch.Stop();
            Console.WriteLine($"Compressed CSV writing time: {stopwatch.Elapsed}");

            Console.WriteLine("CSV data written to file.");
        }

        private static IList<Record> GenerateRecords(int nRecords)
        {
            var records = new List<Record>();

            for (int i = 0; i < nRecords; i++)
            {
                records.Add(new Record());
            }

            return records;
        }

        private static string ConvertToCsvLine(Record record)
        {
            if (record == null)
            {
                return "";
            }

            var properties = record
                .GetType()
                .GetProperties()
                .Select(p => p.GetValue(record)?.ToString());

            return string.Join(" ", properties);
        }

        private static void WriteUncompressedCsvToFile(string filePath, List<string> csvRecords)
        {
            using StreamWriter writer = new StreamWriter(filePath);

            WriteCsvLine(csvRecords, writer);
        }

        private static void WriteCompressedCsvToFile(string filePath, List<string> csvRecords)
        {
            using FileStream fileStream = 
                new FileStream(filePath, FileMode.Create);
            
            using GZipStream gzipStream = 
                new GZipStream(fileStream, CompressionMode.Compress);
            
            using StreamWriter writer = 
                new StreamWriter(gzipStream);

            WriteCsvLine(csvRecords, writer);
        }

        private static void WriteCsvLine(List<string> csvRecords, StreamWriter writer)
        {
            int totalRecords = csvRecords.Count;
            int recordsWritten = 0;

            for (int i = 0; i < totalRecords; i++)
            {
                writer.WriteLine(csvRecords[i]);
                recordsWritten++;

                // Check if 10% of records have been written
                if (recordsWritten % (totalRecords / 10) == 0)
                {
                    double progress = (double)recordsWritten / totalRecords * 100;
                    Console.WriteLine($"Progress: {progress}%");
                }
            }
        }
    }
}