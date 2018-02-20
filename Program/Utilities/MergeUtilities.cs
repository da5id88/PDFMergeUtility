using System;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PDF_Merger.Utilities
{
    internal static class MergeUtilities
    {
        public static void MergePdfDocuments(string targetPath, params string[] pdfs)
        {
            using (var targetDoc = new PdfDocument())
            {
                foreach (var item in pdfs)
                {
                    using (var pdfDoc = PdfReader.Open(item, PdfDocumentOpenMode.Import))
                    {
                        for (var i = 0; i < pdfDoc.PageCount; i++)
                        {
                            targetDoc.AddPage(pdfDoc.Pages[i]);
                        }
                    }
                }
                try
                {
                    targetDoc.Save(targetPath);
                }
                catch (System.InvalidOperationException)
                {

                }

            }
        }
    }
}