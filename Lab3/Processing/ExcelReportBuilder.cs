using OfficeOpenXml;
using System.Runtime.CompilerServices;

namespace Lab3.Processing
{
    internal class ExcelReportBuilder : IDisposable
    {
        public ExcelPackage _excelPackage;
        private bool _disposedValue;

        private int _cnt = 0;
        private int _sampleBlockSize;

        private const int HEADER_SIZE = 2;
        private const int BLOCK_HEADER_SIZE = 2;

        private const int START_COLUMN = 1;
        

        public ExcelReportBuilder(
            string templateFileName,
            string newFileName,
            int sampleBlockSize = 4)
        {
            if (!File.Exists(templateFileName))
            {
                throw new FileNotFoundException(templateFileName);
            }

            _excelPackage = new ExcelPackage(
                new FileInfo(newFileName), 
                new FileInfo(templateFileName));

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            _sampleBlockSize = sampleBlockSize;
        }

        private int GetRowIdx(int count)
        {
            return HEADER_SIZE 
                + BLOCK_HEADER_SIZE * ((count - 1) / _sampleBlockSize + 1) 
                + count;
        }

        public void AppendReportLine(ValuesParameters parameters)
        {
            var row = GetRowIdx(_cnt + 1);
            var column = START_COLUMN;

            var worksheet = _excelPackage.Workbook.Worksheets[0];

            worksheet.Cells[row, column + 1].Value = parameters.Average;
            worksheet.Cells[row, column + 2].Value = parameters.HalfSum;
            worksheet.Cells[row, column + 3].Value = parameters.Median;
            worksheet.Cells[row, column + 4].Value = parameters.SegmentMedian;

            worksheet.Cells[row, column + 5].Value = parameters.AverageError;
            worksheet.Cells[row, column + 6].Value = parameters.HalfSumError;
            worksheet.Cells[row, column + 7].Value = parameters.MedianError;
            worksheet.Cells[row, column + 8].Value = parameters.SegmentMedianError;

            worksheet.Cells[row, column + 1, row, column + 8].Style.Numberformat.Format = "0.000000";

            worksheet.Cells[row, column + 1, row, column + 8].AutoFitColumns();

            _cnt++;
        }

        public void SaveReport()
        {
            _excelPackage.Save();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _excelPackage.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
